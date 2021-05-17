using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class NewContainerContents
    {
        public int width { get; private set; }
        public int height { get; private set; }

        [SerializeField] private List<NewContainerItemEntry> itemEntries;
        [SerializeField] private ContainerItemGrid itemGrid;

        public NewContainerContents(int width, int height)
        {
            this.width = width;
            this.height = height;
            itemGrid = new ContainerItemGrid(width, height);
        }


        public bool AddItem(NewItemData itemData, List<int> indexes)
        {
            if (!itemGrid.IsSlotsClear(indexes))
                return false;

            itemEntries.Add(new NewContainerItemEntry(itemData, indexes));
            itemGrid.FillSlots(indexes);
            return true;
        }

        public bool RemoveItem(NewItemData itemData)
        {


            return true;
        }

    }
}