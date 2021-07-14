using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public static int EQUIPMENT = LayerMask.NameToLayer("Equipment");

    /// <summary>
    /// The ghost layer colliders with nothing. Not even itself.
    /// </summary>
    public static int GHOST = LayerMask.NameToLayer("Ghost");
}
