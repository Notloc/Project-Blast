using UnityEngine;

public abstract class ScriptableItem : ScriptableObject
{
    public abstract bool IsUnique { get; }
    public abstract ItemBase GetBase();
}
