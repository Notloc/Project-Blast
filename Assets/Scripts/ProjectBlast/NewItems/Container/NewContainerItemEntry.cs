using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class NewContainerItemEntry
    {
        public NewItemData itemData { get; private set; }
        public List<int> indexes { get; private set; }

        public NewContainerItemEntry(NewItemData itemData, List<int> indexes)
        {
            this.itemData = itemData;
            this.indexes = indexes;
        }
    }
}