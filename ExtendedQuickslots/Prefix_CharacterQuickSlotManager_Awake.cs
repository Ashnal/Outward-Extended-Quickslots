using Harmony;
using UnityEngine;

namespace ExtendedQuickslots
{
    //we need this to setup the extra quickslots
    [HarmonyPatch(typeof(CharacterQuickSlotManager))]
    [HarmonyPatch("Awake")]
    public static class Prefix_CharacterQuickSlotManager_Awake
    {
        [HarmonyPrefix]
        public static bool AwakePrefix(CharacterQuickSlotManager __instance)
        {
            __instance.m_character = __instance.GetComponent<Character>();
            __instance.m_quickslotTrans = __instance.transform.Find("QuickSlots");
            // Add our 8 QuickSlots
            for (var x = 0; x < ExtendedQuickslots.numSlots; ++x)
            {
                // Create a GameObject with a name that shouldn't overlap with anything else
                GameObject gameObject = new GameObject(string.Format("EXQS_{0}", x));
                // Add a QuickSlot object and set the name based on how the client will process it
                QuickSlot qs = gameObject.AddComponent<QuickSlot>();
                qs.name = string.Format("{0}", x + 12);
                // Set the parent so the code below will find the new objects and treat them like the originals
                gameObject.transform.SetParent(__instance.m_quickslotTrans);
            }
            QuickSlot[] componentsInChildren = __instance.m_quickslotTrans.GetComponentsInChildren<QuickSlot>();
            __instance.m_quickSlots = new QuickSlot[componentsInChildren.Length];
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                int num = int.Parse(componentsInChildren[i].name);
                __instance.m_quickSlots[num - 1] = componentsInChildren[i];
                __instance.m_quickSlots[num - 1].Index = num - 1;
            }
            for (int j = 0; j < __instance.m_quickSlots.Length; j++)
            {
                __instance.m_quickSlots[j].SetOwner(__instance.m_character);
            }
            return false;
        }
    }
}
