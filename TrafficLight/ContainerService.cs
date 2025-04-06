namespace Servers;

using Services;

/// <summary>
/// Service
/// </summary>
public class ContainerService : IContainerService
{
    //NOTE: non-singleton service would need logic to be static or injected from a singleton instance
    private readonly ContainerLogic mLogic = new ContainerLogic();

    public void SetMass(double mass)
    {
        mLogic.SetMass(mass);
    }

    public ContainerLimits GetContainerLimits()
    {
        return mLogic.GetContainerLimits();
    }

    public ContainerDesc ContainerInfo()
    {
        return mLogic.ContainerInfo();
    }

    public int ActiveClient()
    {
        return mLogic.ActiveClient();
    }
}