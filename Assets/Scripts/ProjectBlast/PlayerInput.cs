using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

public class PlayerInput : InputScript
{
    [SerializeField] protected InputNameMap inputs = new InputNameMap();
    protected string GetInputName(string name)
    {
        if (!inputs.ContainsKey(name))
            throw new NotImplementedException("The requested player input [" + name + "] is not setup in the InputScript.");
        return inputs[name];
    }

    public override float GetAxis(string name)
    {
        string axisName = GetInputName(name);
        return Input.GetAxis(axisName);
    }

    public override bool GetButton(string name)
    {
        string buttonName = GetInputName(name);
        return Input.GetButton(buttonName);
    }

    public override bool GetButtonUp(string name)
    {
        string buttonName = GetInputName(name);
        return Input.GetButtonUp(buttonName);
    }

    public override bool GetButtonDown(string name)
    {
        string buttonName = GetInputName(name);
        return Input.GetButtonDown(buttonName);
    }
}

[System.Serializable]
public class InputNameMap : SerializableDictionaryBase<string, string> { }