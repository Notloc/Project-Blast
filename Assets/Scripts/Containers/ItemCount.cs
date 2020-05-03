[System.Serializable]
public struct ItemCount
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
