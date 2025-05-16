using Mono.Cecil;

[System.Serializable]
public class ResourceRequirement
{
    public ResourceData resource;  // Este es tu ScriptableObject del recurso
    public int quantity;

    public int MultiplyByLevel(int level)
    {
        return quantity * level;
    }

}