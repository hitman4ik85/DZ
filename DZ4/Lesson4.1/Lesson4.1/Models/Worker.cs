namespace Lesson4.Models;

public class Worker
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public IInstrument Instrument { get; set; } = new Hummer();
}

/*public abstract class Instrument
{
    public abstract void DoWork();
}*/

public interface IInstrument
{
    string DoWork();
}

public class Hummer : IInstrument
{
    public string DoWork()
    {
        return "Work with Hummer";
    }
}

public class Shovel : IInstrument
{
    public string DoWork()
    {
        return "Dig with Shovel";
    }
}