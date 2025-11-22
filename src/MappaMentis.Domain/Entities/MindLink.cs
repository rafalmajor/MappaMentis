namespace MappaMentis.Domain.Entities;

/// <summary>
/// Represents a connection/relationship between two nodes in a mind map.
/// </summary>
public class MindLink
{
    public Guid Id { get; private set; }
    public Guid MindMapId { get; private set; }
    public Guid SourceNodeId { get; private set; }
    public Guid TargetNodeId { get; private set; }
    public string Label { get; private set; }
    public string LineStyle { get; private set; } // e.g., "solid", "dashed", "dotted"
    public string Color { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public MindLink(
        Guid id,
        Guid mindMapId,
        Guid sourceNodeId,
        Guid targetNodeId,
        string label = "",
        string lineStyle = "solid",
        string color = "#000000")
    {
        if (sourceNodeId == targetNodeId)
            throw new InvalidOperationException("A link cannot connect a node to itself.");

        Id = id;
        MindMapId = mindMapId;
        SourceNodeId = sourceNodeId;
        TargetNodeId = targetNodeId;
        Label = label ?? "";
        LineStyle = lineStyle ?? "solid";
        Color = color ?? "#000000";
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the link's metadata.
    /// </summary>
    public void Update(string label, string lineStyle, string color)
    {
        Label = label ?? "";
        LineStyle = lineStyle ?? "solid";
        Color = color ?? "#000000";
        UpdatedAt = DateTime.UtcNow;
    }
}
