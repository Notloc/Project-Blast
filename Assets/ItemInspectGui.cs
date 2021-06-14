using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ItemInspectGui : MonoBehaviour
{
    GameObject activeInspect;
    [SerializeField] GenericItemInspectGui genericItemInspect = null;

    private void Awake()
    {
        genericItemInspect.gameObject.SetActive(false);
    }


    private void Show(ItemInstance item)
    {
        genericItemInspect.SetItem(item);
        activeInspect = genericItemInspect.gameObject;
        activeInspect.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        activeInspect.gameObject.SetActive(false);
    }

}
