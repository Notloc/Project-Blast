using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponInstance WeaponInstance { get; private set; }
    [SerializeField] WeaponInstanceHolder weaponInstanceHolder = null;

    private List<GameObject> attachmentModels = new List<GameObject>();
    private Dictionary<ItemInstance, GameObject> attachmentModelDict = new Dictionary<ItemInstance, GameObject>();
    private Dictionary<GameObject, ItemInstance> attachmentInstanceDict = new Dictionary<GameObject, ItemInstance>();
    private Dictionary<Transform, GameObject> attachmentPointToAttachmentModelDict = new Dictionary<Transform, GameObject>();

    private void Awake()
    {
        if (weaponInstanceHolder != null)
        {
            SetWeaponInstance(weaponInstanceHolder.GetWeaponInstance());
        }
    }

    public void SetWeaponInstance(WeaponInstance weaponInstance)
    {
        if (this.WeaponInstance != null)
        {
            this.WeaponInstance.OnModAdded -= OnAddAttachment;
            this.WeaponInstance.OnModRemoved -= OnRemoveAttachment;
        }

        this.WeaponInstance = weaponInstance;
        this.WeaponInstance.OnModAdded += OnAddAttachment;
        this.WeaponInstance.OnModRemoved += OnRemoveAttachment;

        CreateModel();
    }

    private void CreateModel()
    {
        GameObject gunModel = Instantiate(WeaponInstance.WeaponBase.ModelPrefab, transform);
        ItemModelPositionData positionData = gunModel.GetComponent<ItemModelPositionData>();

        attachmentModelDict.Add(WeaponInstance, gunModel);
        attachmentInstanceDict.Add(gunModel, WeaponInstance);

        IList<ItemModData> installedMods = WeaponInstance.InstalledMods;
        foreach(ItemModData data in installedMods)
        {
            CreateAttachmentModel(data, positionData);
        }
    }

    private void CreateAttachmentModel(ItemModData attachmentData, ItemModelPositionData positionData)
    {
        ItemInstance attachment = attachmentData.itemInstance;
        string slotName = attachmentData.modSlotName;

        Assert.IsTrue(positionData.ModSlotPositionsByName.ContainsKey(slotName), "Attachment Slot [" + slotName + "] not found in the weapons position data.");

        Transform attachPoint = positionData.ModSlotPositionsByName[slotName].AttachmentPoint;
        GameObject attachmentModel = Instantiate(attachment.ItemBase.ModelPrefab, attachPoint);

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

            IList<ItemModData> subMods = moddableAttachment.InstalledMods;
            foreach (ItemModData subMod in subMods)
            {
                CreateAttachmentModel(subMod, moddableAttachmentPositionData);
            }
        }
    }

    public void OnRemoveAttachment(ItemInstance itemInstance)
    {
        GameObject attachmentModel = attachmentModelDict[itemInstance];

        List<GameObject> modelsToDestroy = new List<GameObject>();
        GetModelsToDestroy(attachmentModel, modelsToDestroy);
        

        foreach (GameObject model in modelsToDestroy)
        {
            ItemInstance item = attachmentInstanceDict[model];
            attachmentModelDict.Remove(item);
            attachmentInstanceDict.Remove(model);
            attachmentModels.Remove(attachmentModel);
            attachmentPointToAttachmentModelDict.Remove(model.transform.parent);
            Destroy(model);

            ModdableItemInstance moddableAttachment = item as ModdableItemInstance;
            if (moddableAttachment != null)
            {
                moddableAttachment.OnModAdded -= OnAddAttachment;
                moddableAttachment.OnModRemoved -= OnRemoveAttachment;
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

    public void OnAddAttachment(ItemModData itemModData, ItemInstance parent)
    {
        GameObject attachmentModel = attachmentModelDict[parent];
        ItemModelPositionData positionData = attachmentModel.GetComponent<ItemModelPositionData>();

        CreateAttachmentModel(itemModData, positionData);
    }
}
