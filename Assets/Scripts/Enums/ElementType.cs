public enum ElementType
{
    None,
    Metal,
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
