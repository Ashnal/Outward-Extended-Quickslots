namespace ExtendedQuickslots
{
    //adds user friendly names for our binds
    partial class Hooks
    {

        public static void LocalizationManager_StartLoading(On.LocalizationManager.orig_StartLoading orig, LocalizationManager self)
        {
            orig(self);
            for (int x = 0; x < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; x++)
            {
                // Our actions start at 12, but we want them to be displayed starting from 9
                self.m_generalLocalization.Add(string.Format("InputAction_QS_Instant{0}", x + 12), string.Format("Quick Slot {0}", x + 9));
            }
            self.m_generalLocalization.Add("InputAction_Sit_Emote", "Sit Emote");
            self.m_generalLocalization.Add("InputAction_Alternate_Idle_Emote", "Arms crossed idle Emote");
        }
    }
}
