using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemInspectorGuiFactory : ScriptableObject
{
    [SerializeField] ItemInspectGui itemInspectGuiPrefab = null;

    private MainGui mainGui;

    public void CreateItemInspectorGui(ItemInstance item, IContainer container)
    {
        if (!mainGui)
            mainGui = GameObject.FindGameObjectWithTag(Tags.MAIN_GUI).GetComponent<MainGui>();

        ItemInspectGui itemInspect = Instantiate(itemInspectGuiPrefab, mainGui.transform);
        itemInspect.Open(item, container);
        // return itemInspect; // This does not work by default as itemInspect.Open can decide to Destroy itself and simply bring an existing window to the front
    }
}
