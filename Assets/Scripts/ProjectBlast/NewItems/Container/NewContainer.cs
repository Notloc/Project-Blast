using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class NewContainer
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Size => Width * Height;
        public NewContainerContents contents { get; private set; }


        public NewContainer(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            contents = new NewContainerContents(width, height);
        }


    }
}