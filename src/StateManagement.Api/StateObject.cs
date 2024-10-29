namespace StateManagement.Api;

public class StateObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Property1 { get; set; } = "Property1";
    public string? Property2 { get; set; } = "Property2";
    public string? Property3 { get; set; } = "Property3";
    public string? Property4 { get; set; } = "Property4";
    public string? Property5 { get; set; } = "Property5";
    public string? Property6 { get; set; } = "Property6";
    public string? Property7 { get; set; } = "Property7";
    public string? Property8 { get; set; } = "Property8";
    public string? Property9 { get; set; } = "Property9";
    public List<Event> Events { get; set; } = new List<Event>();

    public void AddEvent()
    {
        Events.Add(new Event());
    }
}

public class Event
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Property1 { get; set; } = "Property1";
    public string? Property2 { get; set; } = "Property2";
}
