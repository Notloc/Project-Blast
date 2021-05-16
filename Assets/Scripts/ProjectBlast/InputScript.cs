using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputScript : MonoBehaviour
{
    public abstract float GetAxis(string name);
    public abstract bool GetButton(string name);
    public abstract bool GetButtonUp(string name);
    public abstract bool GetButtonDown(string name);
}