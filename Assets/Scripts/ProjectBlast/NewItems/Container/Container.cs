using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class Container
    {
        [SerializeField] int width = 2;
        [SerializeField] int height = 2;
        [SerializeField] ContainerContents contents;

        public int Width => width;
        public int Height => height;
        public int Size => Width * Height;
        public Vector2Int Dimensions => new Vector2Int(width, height);
        public ContainerContents Contents => contents;

        public Container(int width, int height)
        {
            this.width = width;
            this.height = height;
            contents = new ContainerContents(width, height);
        }

        public bool AddItem(ItemInstance item)
        {
            Vector2Int size = item.Size;
            for (int y = 0; y < height && y + size.y <= height; y++) {
                for (int x = 0; x < width && x + size.x <= width; x++)
                {
                    bool isClear = true;
                    Vector2Int containerPos = new Vector2Int(x, y);
                    void CheckItem()
                    {
                        for (int itemX = 0; itemX < size.x; itemX++) {
                            for (int itemY = 0; itemY < size.y; itemY++)
                            {
                                Vector2Int itemPos = new Vector2Int(itemX, itemY);
                                if (!contents.IsCoordinatesClear(containerPos + itemPos))
                                {
                                    isClear = false;
                                    return;
                                }
                            }
                        }
                    }
                    CheckItem();
                    if (isClear)
                    {
                        contents.AddItem(item, containerPos);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddItem(ItemInstance item, Vector2Int coordinates)
        {
            return contents.AddItem(item, coordinates);
        }

        public bool RemoveItem(ItemInstance item)
        {
            return contents.RemoveItem(item);
        }
    }
}