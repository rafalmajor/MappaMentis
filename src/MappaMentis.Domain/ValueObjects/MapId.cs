namespace MappaMentis.Domain.ValueObjects;

/// <summary>
/// Value object representing a unique mind map identifier.
/// </summary>
public class MapId : IEquatable<MapId>
{
    public Guid Value { get; }

    public MapId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("MapId cannot be empty.", nameof(value));

        Value = value;
    }

    public static MapId New() => new(Guid.NewGuid());

    public static MapId Create(Guid value) => new(value);

    public override bool Equals(object? obj)
    {
        return obj is MapId mapId && Equals(mapId);
    }

    public bool Equals(MapId? other)
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

    public static bool operator ==(MapId? left, MapId? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(MapId? left, MapId? right)
    {
        return !(left == right);
    }
}
