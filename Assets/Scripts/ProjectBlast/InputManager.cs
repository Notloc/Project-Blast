using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static MainInput mainInput;
    private void Awake()
    {
        mainInput = new MainInput();
        mainInput.Enable();
    }
}
