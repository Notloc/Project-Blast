using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponInstance WeaponInstance => weaponInstance;
    [SerializeField] WeaponInstance weaponInstance = null;

    [SerializeField] List<WeaponAttachmentInstance> attachments;

    private Dictionary<WeaponAttachmentInstance, GameObject> attachmentModelDict = new Dictionary<WeaponAttachmentInstance, GameObject>();
    private Dictionary<ItemBase, WeaponAttachmentInstance> itemBaseToAttachmentInstanceDict;

    private void Awake()
    {
        weaponInstance.SetAttachments(attachments);
        InitializeGun();
    }

    private void InitializeGun()
    {
        itemBaseToAttachmentInstanceDict = Util.Dictionary(weaponInstance.InstalledMods, item => item.ItemBase, item => item as WeaponAttachmentInstance);


        CreateModel();
    }

    private void CreateModel()
    {
        GameObject gunModel = Instantiate(weaponInstance.WeaponBase.ModelPrefab, transform);
        ItemModelPositionData positionData = gunModel.GetComponent<ItemModelPositionData>();

        IList<ItemInstance> installedMods = weaponInstance.InstalledMods;
        foreach(ItemInstance mod in installedMods)
        {
            WeaponAttachmentInstance attachment = mod as WeaponAttachmentInstance;
            if (attachment == null)
                continue;

            AddAttachmentModel(attachment, positionData);
            attachment.ItemBase.OnDataUpdated += UpdateAttachmentModel;
        }
    }

    private void AddAttachmentModel(WeaponAttachmentInstance attachment, ItemModelPositionData positionData)
    {
        Assert.IsTrue(positionData.ModSlotPositionsByName.ContainsKey(attachment.SlotName), "Attachment Slot [" + attachment.SlotName + "] not found in the weapons position data.");

        Transform attachPoint = positionData.ModSlotPositionsByName[attachment.SlotName].AttachmentPoint;
        GameObject attachmentModel = Instantiate(attachment.WeaponAttachmentBase.ModelPrefab, attachPoint);

        attachmentModelDict.Add(attachment, attachmentModel);
    }

    private void UpdateAttachmentModel(UpdatableData caller)
    {
        ItemBase item = (ItemBase)caller;
        WeaponAttachmentInstance attachment = itemBaseToAttachmentInstanceDict[item];

        GameObject oldModel = attachmentModelDict[attachment];
        Transform attachPoint = oldModel.transform.parent;
        
        GameObject newModel = Instantiate(attachment.WeaponAttachmentBase.ModelPrefab, attachPoint);

        ItemModelPositionData oldPositionData = oldModel.GetComponent<ItemModelPositionData>();
        ItemModelPositionData newPositionData = newModel.GetComponent<ItemModelPositionData>();
        if (oldPositionData && newPositionData)
        {
            foreach (var slotData in newPositionData.ModSlotPositions)
            {
                string name = slotData.SlotName;
                ItemModSlotPositionData prevData, newData;
                oldPositionData.ModSlotPositionsByName.TryGetValue(name, out prevData);
                newPositionData.ModSlotPositionsByName.TryGetValue(name, out newData);

                if (prevData != null && newData != null)
                {
                    while (prevData.AttachmentPoint.childCount > 0)
                    {
                        Transform child = prevData.AttachmentPoint.GetChild(0);
                        child.SetParent(null, false);
                        child.SetParent(newData.AttachmentPoint, false);
                    }
                }
            }
        }

        attachmentModelDict[attachment] = newModel;
        Destroy(oldModel);
    }
}
