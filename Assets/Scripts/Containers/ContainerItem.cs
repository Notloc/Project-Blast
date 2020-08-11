[System.Serializable]
public struct ContainerItem
{
    public Item item;
    public uint count;

    public ScriptableItem Key
    {
        get
        {
            if (item.IsUnique)
                return item;
            else
                return item.GetBase();
        }
    }
}
