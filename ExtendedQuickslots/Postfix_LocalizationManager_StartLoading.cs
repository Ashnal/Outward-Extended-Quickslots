using Harmony;

namespace ExtendedQuickslots
{
    //adds user friendly names for our binds
    [HarmonyPatch(typeof(LocalizationManager))]
    [HarmonyPatch("StartLoading")]
    public static class Postfix_LocalizationManager_StartLoading
    {
        [HarmonyPostfix]
        public static void StartLoadingPostfix(LocalizationManager __instance)
        {
            for (int x = 0; x < 8; x++)
            {
                // Our actions start at 12, but we want them to be displayed starting from 9
                __instance.m_generalLocalization.Add(string.Format("InputAction_QS_Instant{0}", x + 12), string.Format("Quick Slot {0}", x + 9));
            }
            __instance.m_generalLocalization.Add("InputAction_Sit_Emote", "Sit Emote");
            __instance.m_generalLocalization.Add("InputAction_Alternate_Idle_Emote", "Arms crossed idle Emote");
        }
    }
}
