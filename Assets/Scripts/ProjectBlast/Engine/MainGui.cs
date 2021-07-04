using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Engine
{
    public class MainGui : MonoBehaviour
    {
        [SerializeField] RectTransform topParent = null;
        [SerializeField] RectTransform windowParent = null;

        public RectTransform TopParent => topParent;
        public RectTransform WindowParent => windowParent;
    }
}