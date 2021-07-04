using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public struct ContainerEntry
    {
        public IContainer Container => container;
        [SerializeField] IContainer container;
        
        public ItemInstance Item => item;
        [SerializeField] ItemInstance item;
        
        public Vector2Int Coordinates => coordinates;
        [SerializeField] Vector2Int coordinates;
        
        public bool IsRotated => isRotated;
        [SerializeField] bool isRotated;

        public Vector2Int Size => size;
        private Vector2Int size;

        public ContainerEntry(ItemInstance item, IContainer container, Vector2Int coordinates, bool isRotated = false)
        {
            this.item = item;
            this.container = container;
            this.coordinates = coordinates;
            this.isRotated = isRotated;
            this.size = isRotated ? item.Size.Swap() : item.Size;
        }

        public ContainerEntry(ContainerEntry other, bool isRotated)
        {
            this.item = other.item;
            this.container = other.container;
            this.coordinates = other.coordinates;
            this.isRotated = isRotated;

            this.size = isRotated ? item.Size.Swap() : item.Size;
        }
    }
}