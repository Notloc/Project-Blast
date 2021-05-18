using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class ContainerItemData
    {
        [SerializeField] Item item;
        [SerializeField] Vector2Int coordinates;

        public Item Item => item;
        public Vector2Int Coordinates => coordinates;

        public ContainerItemData(Item item, Vector2Int coordinates)
        {
            this.item = item;
            this.coordinates = coordinates;
        }
    }
}