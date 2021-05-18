using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class ContainerItemInstance
    {
        [SerializeField] ItemInstance item;
        [SerializeField] Vector2Int coordinates;
        [SerializeField] bool isRotated = false;

        public ItemInstance Item => item;
        public Vector2Int Coordinates => coordinates;

        public ContainerItemInstance(ItemInstance item, Vector2Int coordinates)
        {
            this.item = item;
            this.coordinates = coordinates;
        }
    }
}