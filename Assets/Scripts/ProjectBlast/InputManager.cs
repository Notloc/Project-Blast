using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Engine
{
    public class InputManager : MonoBehaviour
    {
        public static MainInput mainInput;
        private void Awake()
        {
            mainInput = new MainInput();
            mainInput.Enable();
        }
    }
}