using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    public static class ContainerService
    {
        public static bool WillModdedWeaponFit(ItemInstance itemBeingModded, ItemInstance mod, IContainer topLevelContainer)
        {
            if (topLevelContainer == null)
            {
                return true;
            }

            WeaponAttachmentInstance attachment = mod as WeaponAttachmentInstance;
            if (attachment != null)
            {
                return topLevelContainer.WillResizedItemFit(itemBeingModded, attachment, attachment.SizeMod);
            }
            return true;
        }
    }
}