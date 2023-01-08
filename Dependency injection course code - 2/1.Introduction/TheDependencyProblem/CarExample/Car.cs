namespace TheDependencyProblem.CarExample;

public class Car
{
    private readonly PetrolEngine _carEngine = new();

    public void StartEngine()
    {
        _carEngine.Start();
    }

    //More methods
}

public class PetrolEngine
{
    public void Start()
    {
        //Battery, induction coil, spark plug, fuel
    }
}
