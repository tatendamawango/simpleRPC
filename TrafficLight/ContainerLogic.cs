namespace Servers;

using NLog;
using Services;
/// <summary>
/// Container state descritor.
/// </summary>
public class ContainerStuff
{
    /// <summary>
    /// Access lock.
    /// </summary>
    public readonly object AccessLock = new object();
    /// <summary>
    /// Access to Container limits
    /// </summary>
	public ContainerLimits ContainerLimits = new ContainerLimits();
    /// <summary>
    /// Intial mass
    /// </summary>
    public double mass = 40.5;
    /// <summary>
    /// Initial temperature
    /// </summary>
    public double temp = 273.15;
    /// <summary>
    /// mass changed tracker
    /// </summary>
    public double massChange = 0;
}


/// <summary>
/// <para>Container logic.</para>
/// <para>Thread safe.</para>
/// </summary>
class ContainerLogic
{
    /// <summary>
    /// Logger for this class.
    /// </summary>
    private Logger mLog = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// Background task thread.
    /// </summary>
    private Thread mBgTaskThread;
    /// <summary>
    /// Container State Descriptor
    /// </summary>
    private ContainerStuff cState = new ContainerStuff();
    /// <summary>
    /// Container Description
    /// </summary>
    public ContainerDesc container = new ContainerDesc() { Mass = 40.5, Temperature = 273.15 };
    /// <summary>
    /// Constructor.
    /// </summary>
    public ContainerLogic()
    {
        //start the background task
        mBgTaskThread = new Thread(BackgroundTask);
        mBgTaskThread.Start();
    }
    /// <summary>
    /// Updates the container's mass.
    /// </summary>
    /// <param name="mass">The amount of mass to add.</param>
    public void SetMass(double mass)
    {
        lock (cState.AccessLock)
        {
            //Input components are not allowed to add gas if pressure is above lower limit.
            //Output components are not allowed to remove mass if pressure is below upper limit.
            if (mass < 0 && (container.Pressure <= cState.ContainerLimits.upperLimit))
            {
                mLog.Info($"Output components are not allowed to remove mass if pressure is below upper limit.");
                cState.massChange = 0;
            }
            else if (mass > 0 && (container.Pressure >= cState.ContainerLimits.lowerLimit))
            {
                mLog.Info($"Input components are not allowed to add gas if pressure is above lower limit.");
                cState.massChange = 0;
            }
            else
            {
                container.Mass += mass;
                cState.massChange = mass;
            }

        }
    }


    /// <summary>
    /// Gets the container's limits.
    /// </summary>
    /// <returns>The container's limits.</returns>
    public ContainerLimits GetContainerLimits()
    {
        lock (cState.AccessLock)
        {
            return cState.ContainerLimits;
        }
    }

    /// <summary>
    /// Resets the simulation.
    /// </summary>
    public void ResetSimulation()
    {
        lock (cState.AccessLock)
        {
            if (container.Pressure >= cState.ContainerLimits.explosionLimit)
            {
                container.Mass = cState.mass;
                cState.massChange = 0;
                container.Temperature = cState.temp;
                Console.Clear();
                mLog.Info($"Container Exploded. Simulation is Reset");
                mLog.Info("Server is about to start\n");
                CalculationPrint();
            }
            else if (container.Pressure <= cState.ContainerLimits.implosionLimit)
            {
                container.Mass = 40.5;
                cState.massChange = 0;
                container.Temperature = 273.15;
                Console.Clear();
                mLog.Info($"Container Imploded. Simulation is Reset");
                mLog.Info("Server is about to start\n");
                CalculationPrint();
            }
        }
    }

    /// <summary>
    /// Gets the container's details.
    /// </summary>
    /// <returns>The container's details.</returns>
    public ContainerDesc ContainerInfo()
    {
        lock (cState.AccessLock)
        {
            return container;
        }
    }

    /// <summary>
    /// Gets the container's control state.
    /// </summary>
    /// <returns>returns 1 for input component to work and 2 for output component to work</returns>
    public int ActiveClient()
    {
        lock (cState.AccessLock)
        {

            if (container.Pressure < cState.ContainerLimits.lowerLimit)
            {
                return 1;
            }
            else if (container.Pressure > cState.ContainerLimits.upperLimit)
            {
                return 2;
            }
            cState.massChange = 0;
            return 0;
        }
    }

    /// <summary>
    /// Calculates new pressure and prints temperature, pressure, mass and mass change
    /// </summary>
    public void CalculationPrint()
    {
        var random = new Random();
        double rnd = random.NextDouble() * 18 - 9;
        container.Temperature += rnd;
        mLog.Info($"New temperature is {container.Temperature:F2}\t\tChange: {rnd:F2}");
        container.Pressure = container.Mass * container.Temperature * container.GasConstant / container.Volume;
        mLog.Info($"New Mass is {container.Mass:F2}    Mass change: {cState.massChange:F2}");
        mLog.Info($"New Pressure is {container.Pressure:F2}\n");
    }

    /// <summary>
    /// Background task for the container
    /// </summary>
    public void BackgroundTask()
    {
        while (true)
        {
            Thread.Sleep(2000);
            lock (cState.AccessLock)
            {
                CalculationPrint();
                ResetSimulation();
            }

        }
    }
}