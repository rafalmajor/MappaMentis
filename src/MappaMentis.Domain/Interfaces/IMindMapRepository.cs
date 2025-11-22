using MappaMentis.Domain.Entities;

namespace MappaMentis.Domain.Interfaces;

/// <summary>
/// Repository interface for persisting and retrieving mind maps.
/// </summary>
public interface IMindMapRepository
{
    /// <summary>
    /// Gets a mind map by its identifier.
    /// </summary>
    Task<MindMap?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all mind maps.
    /// </summary>
    Task<IEnumerable<MindMap>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new mind map to the repository.
    /// </summary>
    Task AddAsync(MindMap mindMap, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing mind map.
    /// </summary>
    Task UpdateAsync(MindMap mindMap, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a mind map by its identifier.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a mind map exists by its identifier.
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all pending changes to the persistence store.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
