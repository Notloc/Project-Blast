using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponGraphics
{
    private List<GameObject> attachmentModels = new List<GameObject>();
    private Dictionary<ItemInstance, GameObject> attachmentModelDict = new Dictionary<ItemInstance, GameObject>();
    private Dictionary<GameObject, ItemInstance> attachmentInstanceDict = new Dictionary<GameObject, ItemInstance>();
    private Dictionary<Transform, GameObject> attachmentPointToAttachmentModelDict = new Dictionary<Transform, GameObject>();


    public void CreateModel(WeaponInstance weaponInstance, Transform transform)
    {
        GameObject gunModel = GameObject.Instantiate(weaponInstance.WeaponBase.ModelPrefab, transform);
        ItemModelPositionData positionData = gunModel.GetComponent<ItemModelPositionData>();

        attachmentModelDict.Add(weaponInstance, gunModel);
        attachmentInstanceDict.Add(gunModel, weaponInstance);

        IList<ItemModSlotInstance> modSlots = weaponInstance.ModSlots;
        foreach (ItemModSlotInstance modSlot in modSlots)
        {
            CreateModelForModSlot(modSlot, positionData);
        }
    }

    private void CreateModelForModSlot(ItemModSlotInstance modSlot, ItemModelPositionData positionData)
    {
        ItemInstance attachment = modSlot.Mod;
        string slotName = modSlot.ModSlotData.SlotName;
        if (attachment == null)
        {
            return;
        }

        Assert.IsTrue(positionData.ModSlotPositionsByName.ContainsKey(slotName), "Attachment Slot [" + slotName + "] not found in the weapons position data.");

        Transform attachPoint = positionData.ModSlotPositionsByName[slotName].AttachmentPoint;
        GameObject attachmentModel = GameObject.Instantiate(attachment.ItemBase.ModelPrefab, attachPoint);

        attachmentModels.Add(attachmentModel);
        attachmentModelDict.Add(attachment, attachmentModel);
        attachmentInstanceDict.Add(attachmentModel, attachment);
        attachmentPointToAttachmentModelDict.Add(attachPoint, attachmentModel);

        ModdableItemInstance moddableAttachment = attachment as ModdableItemInstance;
        if (moddableAttachment != null)
        {
            moddableAttachment.OnModAdded += OnAddAttachment;
            moddableAttachment.OnModRemoved += OnRemoveAttachment;

            ItemModelPositionData moddableAttachmentPositionData = attachmentModel.GetComponent<ItemModelPositionData>();

            IList<ItemModSlotInstance> subModSlots = moddableAttachment.ModSlots;
            foreach (ItemModSlotInstance subModSlot in subModSlots)
            {
                if (subModSlot.Mod != null)
                {
                    CreateModelForModSlot(subModSlot, moddableAttachmentPositionData);
                }
            }
        }
    }


    private void GetModelsToDestroy(GameObject model, List<GameObject> outList)
    {
        outList.Add(model);
        ItemModelPositionData positionData = model.GetComponent<ItemModelPositionData>();
        if (positionData)
        {
            foreach (ItemModSlotPositionData modSlotPosition in positionData.ModSlotPositions)
            {
                if (attachmentPointToAttachmentModelDict.ContainsKey(modSlotPosition.AttachmentPoint))
                {
                    GameObject attachment = attachmentPointToAttachmentModelDict[modSlotPosition.AttachmentPoint];
                    GetModelsToDestroy(attachment, outList);
                }
            }
        }

    }

    public void OnRemoveAttachment(ItemModSlotInstance modSlot, ItemInstance removedItem)
    {
        GameObject attachmentModel = attachmentModelDict[removedItem];

        List<GameObject> modelsToDestroy = new List<GameObject>();
        GetModelsToDestroy(attachmentModel, modelsToDestroy);


        foreach (GameObject model in modelsToDestroy)
        {
            ItemInstance item = attachmentInstanceDict[model];
            attachmentModelDict.Remove(item);
            attachmentInstanceDict.Remove(model);
            attachmentModels.Remove(model);
            attachmentPointToAttachmentModelDict.Remove(model.transform.parent);
            GameObject.Destroy(model);

            ModdableItemInstance moddableAttachment = item as ModdableItemInstance;
            if (moddableAttachment != null)
            {
                moddableAttachment.OnModAdded -= OnAddAttachment;
                moddableAttachment.OnModRemoved -= OnRemoveAttachment;
            }
        }
    }



    public void OnAddAttachment(ItemModSlotInstance modSlot)
    {
        GameObject parentModel = attachmentModelDict[modSlot.parentModdableItem];
        ItemModelPositionData positionData = parentModel.GetComponent<ItemModelPositionData>();

        CreateModelForModSlot(modSlot, positionData);
    }
}
