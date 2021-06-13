using Notloc.Utility;
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
        public bool IsRotated => isRotated;
        public Vector2Int Size => isRotated ? item.BaseSize.Swap() : item.BaseSize;

        public ContainerItemInstance(ItemInstance item, Vector2Int coordinates, bool isRotated = false)
        {
            this.item = item;
            this.coordinates = coordinates;
            this.isRotated = isRotated;
        }

        public void Rotate()
        {
            isRotated = !isRotated;
        }
    }
}