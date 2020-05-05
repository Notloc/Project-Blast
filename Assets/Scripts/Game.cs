using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public FactoryStruct Factories;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public struct FactoryStruct
    {
        public ItemFactory ItemFactory;
        public ItemEntityFactory ItemEntityFactory;
    }
}

