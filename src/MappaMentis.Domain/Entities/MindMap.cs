namespace MappaMentis.Domain.Entities;

/// <summary>
/// Represents the root aggregate of a mind map.
/// Contains nodes and connections between them.
/// </summary>
public class MindMap
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private readonly List<MindNode> _nodes = new();
    private readonly List<MindLink> _links = new();

    public IReadOnlyList<MindNode> Nodes => _nodes.AsReadOnly();
    public IReadOnlyList<MindLink> Links => _links.AsReadOnly();

    public MindMap(Guid id, string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a node to the mind map.
    /// </summary>
    public void AddNode(MindNode node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));

        if (_nodes.Any(n => n.Id == node.Id))
            throw new InvalidOperationException("Node with this ID already exists.");

        _nodes.Add(node);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a node from the mind map.
    /// </summary>
    public void RemoveNode(Guid nodeId)
    {
        var node = _nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node != null)
        {
            _nodes.Remove(node);
            // Remove associated links
            var associatedLinks = _links
                .Where(l => l.SourceNodeId == nodeId || l.TargetNodeId == nodeId)
                .ToList();
            
            foreach (var link in associatedLinks)
            {
                _links.Remove(link);
            }

            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Adds a link between two nodes.
    /// </summary>
    public void AddLink(MindLink link)
    {
        if (link == null)
            throw new ArgumentNullException(nameof(link));

        var sourceExists = _nodes.Any(n => n.Id == link.SourceNodeId);
        var targetExists = _nodes.Any(n => n.Id == link.TargetNodeId);

        if (!sourceExists || !targetExists)
            throw new InvalidOperationException("One or both nodes do not exist.");

        if (_links.Any(l => l.Id == link.Id))
            throw new InvalidOperationException("Link with this ID already exists.");

        _links.Add(link);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a link between two nodes.
    /// </summary>
    public void RemoveLink(Guid linkId)
    {
        var link = _links.FirstOrDefault(l => l.Id == linkId);
        if (link != null)
        {
            _links.Remove(link);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Updates the title and description of the mind map.
    /// </summary>
    public void Update(string title, string description)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UpdatedAt = DateTime.UtcNow;
    }
}
