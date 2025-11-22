namespace MappaMentis.Domain.Entities;

/// <summary>
/// Represents a markdown note attached to a mind map node.
/// Allows rich text formatting for node details.
/// </summary>
public class MarkdownNote
{
    public Guid Id { get; private set; }
    public Guid NodeId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public MarkdownNote(Guid id, Guid nodeId, string title, string content)
    {
        Id = id;
        NodeId = nodeId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the markdown note content and title.
    /// </summary>
    public void Update(string title, string content)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        UpdatedAt = DateTime.UtcNow;
    }
}
