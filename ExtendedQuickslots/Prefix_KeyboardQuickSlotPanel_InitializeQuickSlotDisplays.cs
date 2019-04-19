using System;
using Harmony;

namespace ExtendedQuickslots
{
    //sets the display order of the new quickslots
    [HarmonyPatch(typeof(KeyboardQuickSlotPanel))]
    [HarmonyPatch("InitializeQuickSlotDisplays")]
    public static class Prefix_KeyboardQuickSlotPanel_InitializeQuickSlotDisplays
    {
        [HarmonyPrefix]
        public static bool InitializeQuickSlotDisplaysPatch(KeyboardQuickSlotPanel __instance)
        {
            // Add 8 new quickslot ids to the array and assign them the new ids we added.
            Array.Resize(ref __instance.DisplayOrder, __instance.DisplayOrder.Length + ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value);
            int s = 12;
            for (int x = ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; x >= 1; x--)
            {
                __instance.DisplayOrder[__instance.DisplayOrder.Length - x] = (QuickSlot.QuickSlotIDs)s++;
            }
            return true;
        }
    }
}
