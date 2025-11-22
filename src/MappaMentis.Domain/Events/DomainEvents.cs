namespace MappaMentis.Domain.Events;

/// <summary>
/// Base class for domain events.
/// </summary>
public abstract class DomainEvent
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}

/// <summary>
/// Event raised when a node is added to a mind map.
/// </summary>
public class NodeAdded : DomainEvent
{
    public Guid MindMapId { get; set; }
    public Guid NodeId { get; set; }
    public string Content { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }

    public NodeAdded(Guid mindMapId, Guid nodeId, string content, int positionX = 0, int positionY = 0)
    {
        MindMapId = mindMapId;
        NodeId = nodeId;
        Content = content;
        PositionX = positionX;
        PositionY = positionY;
    }
}

/// <summary>
/// Event raised when an idea is captured in a node.
/// </summary>
public class IdeaCaptured : DomainEvent
{
    public Guid MindMapId { get; set; }
    public Guid NodeId { get; set; }
    public string Idea { get; set; }
    public string Category { get; set; }

    public IdeaCaptured(Guid mindMapId, Guid nodeId, string idea, string category = "General")
    {
        MindMapId = mindMapId;
        NodeId = nodeId;
        Idea = idea;
        Category = category;
    }
}

/// <summary>
/// Event raised when a pomodoro timer is started.
/// </summary>
public class TimerStarted : DomainEvent
{
    public Guid TimerId { get; set; }
    public Guid MindMapId { get; set; }
    public int DurationMinutes { get; set; }
    public int SessionNumber { get; set; }

    public TimerStarted(Guid timerId, Guid mindMapId, int durationMinutes, int sessionNumber)
    {
        TimerId = timerId;
        MindMapId = mindMapId;
        DurationMinutes = durationMinutes;
        SessionNumber = sessionNumber;
    }
}
