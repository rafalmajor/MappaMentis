namespace MappaMentis.Domain.ValueObjects;

/// <summary>
/// Value object representing a title with validation.
/// </summary>
public class Title : IEquatable<Title>
{
    private const int MinLength = 1;
    private const int MaxLength = 500;

    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be null or empty.", nameof(value));

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException($"Title must be between {MinLength} and {MaxLength} characters.", nameof(value));

        Value = value.Trim();
    }

    public override bool Equals(object? obj)
    {
        return obj is Title title && Equals(title);
    }

    public bool Equals(Title? other)
    {
        return other is not null && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(Title? left, Title? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Title? left, Title? right)
    {
        return !(left == right);
    }

    public static implicit operator string(Title title)
    {
        return title.Value;
    }
}
