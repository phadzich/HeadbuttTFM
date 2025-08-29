public enum ElementType
{
    None,
    Neutral,
    Fire,
    Water,
    Grass,
    Electric,
    Antinezur
}

public interface IElemental
{
    ElementType Element { get; }
}
