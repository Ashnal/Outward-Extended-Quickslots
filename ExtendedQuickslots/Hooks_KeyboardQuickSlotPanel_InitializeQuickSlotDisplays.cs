using System;
using Harmony;

namespace ExtendedQuickslots
{
    //sets the display order of the new quickslots
    static partial class Hooks
    {
        public static void KeyboardQuickSlotPanel_InitializeQuickSlotDisplays(On.KeyboardQuickSlotPanel.orig_InitializeQuickSlotDisplays orig, KeyboardQuickSlotPanel self)
        {
            // Add 8 new quickslot ids to the array and assign them the new ids we added.
            Array.Resize(ref self.DisplayOrder, self.DisplayOrder.Length + ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value);
            int s = 12;
            for (int x = ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; x >= 1; x--)
            {
                self.DisplayOrder[self.DisplayOrder.Length - x] = (QuickSlot.QuickSlotIDs)s++;
            }
            orig(self);
        }
    }
}
