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
                return item.GetItemBase();
        }
    }

    public static readonly ContainerItem None = new ContainerItem() { item = null, count = 0 };

    public override bool Equals(object obj)
    {
        if (!(obj is ContainerItem))
            return false;

        var other = (ContainerItem)obj;
        if (this.count != other.count)
            return false;

        if (!item && !other.item)
            return true;

        if (!item || !other.item)
            return false;

        if (item.IsUnique != other.item.IsUnique)
            return false;

        if (item.IsUnique)
            return item.GetItemBase() == other.item.GetItemBase();
        else
            return item == other.item;
    }
}
