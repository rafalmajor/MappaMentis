namespace MappaMentis.Domain.Entities;

/// <summary>
/// Represents a single node in a mind map.
/// A node can contain text content and markdown notes.
/// </summary>
public class MindNode
{
    public Guid Id { get; private set; }
    public Guid MindMapId { get; private set; }
    public string Content { get; private set; }
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public string Color { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<MarkdownNote> _notes = new();
    public IReadOnlyList<MarkdownNote> Notes => _notes.AsReadOnly();

    public MindNode(Guid id, Guid mindMapId, string content, int positionX = 0, int positionY = 0, string color = "#FFFFFF")
    {
        Id = id;
        MindMapId = mindMapId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        PositionX = positionX;
        PositionY = positionY;
        Color = color ?? "#FFFFFF";
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the node's content and metadata.
    /// </summary>
    public void Update(string content, int positionX, int positionY, string color)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        PositionX = positionX;
        PositionY = positionY;
        Color = color ?? "#FFFFFF";
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a markdown note to the node.
    /// </summary>
    public void AddNote(MarkdownNote note)
    {
        if (note == null)
            throw new ArgumentNullException(nameof(note));

        if (_notes.Any(n => n.Id == note.Id))
            throw new InvalidOperationException("Note with this ID already exists.");

        _notes.Add(note);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a markdown note from the node.
    /// </summary>
    public void RemoveNote(Guid noteId)
    {
        var note = _notes.FirstOrDefault(n => n.Id == noteId);
        if (note != null)
        {
            _notes.Remove(note);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Clears all notes from the node.
    /// </summary>
    public void ClearNotes()
    {
        if (_notes.Count > 0)
        {
            _notes.Clear();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
