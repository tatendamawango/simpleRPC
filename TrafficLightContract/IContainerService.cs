namespace Services;


public class ContainerDesc
{
    public double Mass { get; set; }

    public double Temperature { get; set; }
    public double Pressure { get; set; }

    public double GasConstant => 8.314;

    public double Volume => 1;
}

public class ContainerLimits
{
    public double lowerLimit => 80000;
    public double upperLimit => 98000;
    public double implosionLimit => 70000;
    public double explosionLimit => 100000;
}

/// <summary>
/// Service contract.
/// </summary>
public interface IContainerService
{
    ContainerLimits GetContainerLimits();

    ContainerDesc ContainerInfo();

    void SetMass(double mass);

    int ActiveClient();
}
