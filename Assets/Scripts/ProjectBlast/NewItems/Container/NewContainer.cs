using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewContainer : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    



    public int size => width * height;
    public int Width => width;
    public int Height => height;
    

}
