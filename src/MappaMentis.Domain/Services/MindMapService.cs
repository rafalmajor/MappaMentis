using MappaMentis.Domain.Entities;
using MappaMentis.Domain.Interfaces;

namespace MappaMentis.Domain.Services;

/// <summary>
/// Domain service for performing pure domain operations on mind maps.
/// This service contains business logic that spans multiple entities or aggregates.
/// </summary>
public class MindMapService
{
    private readonly IMindMapRepository _repository;

    public MindMapService(IMindMapRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Creates a new mind map with an initial root node.
    /// </summary>
    public async Task<MindMap> CreateMindMapAsync(
        string title,
        string description,
        string rootNodeContent,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(rootNodeContent))
            throw new ArgumentException("Root node content cannot be null or empty.", nameof(rootNodeContent));

        var mapId = Guid.NewGuid();
        var mindMap = new MindMap(mapId, title, description);

        // Add root node
        var rootNodeId = Guid.NewGuid();
        var rootNode = new MindNode(rootNodeId, mapId, rootNodeContent);
        mindMap.AddNode(rootNode);

        await _repository.AddAsync(mindMap, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return mindMap;
    }

    /// <summary>
    /// Adds a node to a mind map and establishes a link from a parent node.
    /// </summary>
    public async Task<MindNode> AddNodeWithLinkAsync(
        Guid mindMapId,
        string nodeContent,
        Guid parentNodeId,
        string linkLabel = "",
        CancellationToken cancellationToken = default)
    {
        var mindMap = await _repository.GetByIdAsync(mindMapId, cancellationToken)
            ?? throw new InvalidOperationException($"Mind map with ID {mindMapId} not found.");

        if (string.IsNullOrWhiteSpace(nodeContent))
            throw new ArgumentException("Node content cannot be null or empty.", nameof(nodeContent));

        var parentNode = mindMap.Nodes.FirstOrDefault(n => n.Id == parentNodeId)
            ?? throw new InvalidOperationException($"Parent node with ID {parentNodeId} not found.");

        // Create new node
        var newNodeId = Guid.NewGuid();
        var newNode = new MindNode(newNodeId, mindMapId, nodeContent);
        mindMap.AddNode(newNode);

        // Create link from parent to new node
        var linkId = Guid.NewGuid();
        var link = new MindLink(linkId, mindMapId, parentNodeId, newNodeId, linkLabel);
        mindMap.AddLink(link);

        await _repository.UpdateAsync(mindMap, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return newNode;
    }

    /// <summary>
    /// Removes a node and all its associated links from the mind map.
    /// </summary>
    public async Task RemoveNodeAsync(
        Guid mindMapId,
        Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        var mindMap = await _repository.GetByIdAsync(mindMapId, cancellationToken)
            ?? throw new InvalidOperationException($"Mind map with ID {mindMapId} not found.");

        mindMap.RemoveNode(nodeId);

        await _repository.UpdateAsync(mindMap, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a mind map with all its nodes and links.
    /// </summary>
    public async Task<MindMap?> GetMindMapAsync(Guid mindMapId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(mindMapId, cancellationToken);
    }

    /// <summary>
    /// Searches for nodes by content keyword in a mind map.
    /// </summary>
    public async Task<IEnumerable<MindNode>> SearchNodesByContentAsync(
        Guid mindMapId,
        string keyword,
        CancellationToken cancellationToken = default)
    {
        var mindMap = await _repository.GetByIdAsync(mindMapId, cancellationToken)
            ?? throw new InvalidOperationException($"Mind map with ID {mindMapId} not found.");

        if (string.IsNullOrWhiteSpace(keyword))
            return Enumerable.Empty<MindNode>();

        return mindMap.Nodes
            .Where(n => n.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Gets all child nodes of a given parent node.
    /// </summary>
    public async Task<IEnumerable<MindNode>> GetChildNodesAsync(
        Guid mindMapId,
        Guid parentNodeId,
        CancellationToken cancellationToken = default)
    {
        var mindMap = await _repository.GetByIdAsync(mindMapId, cancellationToken)
            ?? throw new InvalidOperationException($"Mind map with ID {mindMapId} not found.");

        var childNodeIds = mindMap.Links
            .Where(l => l.SourceNodeId == parentNodeId)
            .Select(l => l.TargetNodeId)
            .ToList();

        return mindMap.Nodes.Where(n => childNodeIds.Contains(n.Id)).ToList();
    }

    /// <summary>
    /// Adds a markdown note to a node.
    /// </summary>
    public async Task<MarkdownNote> AddMarkdownNoteAsync(
        Guid mindMapId,
        Guid nodeId,
        string noteTitle,
        string noteContent,
        CancellationToken cancellationToken = default)
    {
        var mindMap = await _repository.GetByIdAsync(mindMapId, cancellationToken)
            ?? throw new InvalidOperationException($"Mind map with ID {mindMapId} not found.");

        var node = mindMap.Nodes.FirstOrDefault(n => n.Id == nodeId)
            ?? throw new InvalidOperationException($"Node with ID {nodeId} not found.");

        var noteId = Guid.NewGuid();
        var note = new MarkdownNote(noteId, nodeId, noteTitle, noteContent);
        node.AddNote(note);

        await _repository.UpdateAsync(mindMap, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return note;
    }
}
