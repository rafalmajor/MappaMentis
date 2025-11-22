namespace MappaMentis.Domain.ValueObjects;

/// <summary>
/// Value object representing a unique node identifier.
/// </summary>
public class NodeId : IEquatable<NodeId>
{
    public Guid Value { get; }

    public NodeId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("NodeId cannot be empty.", nameof(value));

        Value = value;
    }

    public static NodeId New() => new(Guid.NewGuid());

    public static NodeId Create(Guid value) => new(value);

    public override bool Equals(object? obj)
    {
        return obj is NodeId nodeId && Equals(nodeId);
    }

    public bool Equals(NodeId? other)
    {
        return other is not null && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(NodeId? left, NodeId? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(NodeId? left, NodeId? right)
    {
        return !(left == right);
    }
}
