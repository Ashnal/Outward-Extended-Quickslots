using Harmony;
using UnityEngine;

namespace ExtendedQuickslots
{
     static partial class Hooks
    {
        public static void CharacterQuickSlotManager_Awake(On.CharacterQuickSlotManager.orig_Awake orig, CharacterQuickSlotManager self)
        {
            self.m_quickslotTrans = self.transform.Find("QuickSlots");
            // Add our 8 QuickSlots
            for (var x = 0; x < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; ++x)
            {
                // Create a GameObject with a name that shouldn't overlap with anything else
                GameObject gameObject = new GameObject(string.Format("EXQS_{0}", x));
                // Add a QuickSlot object and set the name based on how the client will process it
                QuickSlot qs = gameObject.AddComponent<QuickSlot>();
                qs.name = string.Format("{0}", x + 12);
                // Set the parent so the code below will find the new objects and treat them like the originals
                gameObject.transform.SetParent(self.m_quickslotTrans);
            }
            orig(self);
        }
    }
}
