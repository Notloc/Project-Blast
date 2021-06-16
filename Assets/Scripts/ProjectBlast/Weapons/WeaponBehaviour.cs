using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponInstance WeaponInstance { get; private set; }
    [SerializeField] WeaponInstanceHolder weaponInstanceHolder = null;

    private Dictionary<ItemInstance, GameObject> attachmentModelDict = new Dictionary<ItemInstance, GameObject>();
    private Dictionary<ItemBase, WeaponAttachmentInstance> itemBaseToAttachmentInstanceDict;

    private void Awake()
    {
        if (weaponInstanceHolder != null)
        {
            SetWeaponInstance(weaponInstanceHolder.GetWeaponInstance());
        }
    }

    public void SetWeaponInstance(WeaponInstance weaponInstance)
    {
        this.WeaponInstance = weaponInstance;
        itemBaseToAttachmentInstanceDict = Util.Dictionary(weaponInstance.InstalledMods, data => data.itemInstance.ItemBase, data => data.itemInstance as WeaponAttachmentInstance);
        CreateModel();
    }

    private void CreateModel()
    {
        GameObject gunModel = Instantiate(WeaponInstance.WeaponBase.ModelPrefab, transform);
        ItemModelPositionData positionData = gunModel.GetComponent<ItemModelPositionData>();

        IList<ItemModData> installedMods = WeaponInstance.InstalledMods;
        foreach(ItemModData data in installedMods)
        {
            CreateAttachmentModel(data, positionData);
            data.itemInstance.ItemBase.OnDataUpdated += UpdateAttachmentModel; // TODO: This is basically debug code
        }
    }

    private void CreateAttachmentModel(ItemModData attachmentData, ItemModelPositionData positionData)
    {
        ItemInstance attachment = attachmentData.itemInstance;
        string slotName = attachmentData.modSlotName;

        Assert.IsTrue(positionData.ModSlotPositionsByName.ContainsKey(slotName), "Attachment Slot [" + slotName + "] not found in the weapons position data.");

        Transform attachPoint = positionData.ModSlotPositionsByName[slotName].AttachmentPoint;
        GameObject attachmentModel = Instantiate(attachment.ItemBase.ModelPrefab, attachPoint);

        attachmentModelDict.Add(attachment, attachmentModel);

        ModdableItemInstance moddableAttachment = attachment as ModdableItemInstance;
        if (moddableAttachment != null)
        {
            ItemModelPositionData moddableAttachmentPositionData = attachmentModel.GetComponent<ItemModelPositionData>();

            IList<ItemModData> subMods = moddableAttachment.InstalledMods;
            foreach (ItemModData subMod in subMods)
            {
                CreateAttachmentModel(subMod, moddableAttachmentPositionData);
                attachment.ItemBase.OnDataUpdated += UpdateAttachmentModel; // TODO: This is basically debug code
            }
        }
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


    public void RemoveAttachment()
    {

    }

    public void AddAttachment()
    {

    }
}
