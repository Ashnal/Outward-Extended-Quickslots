using Harmony;
using UnityEngine;
using System.Reflection;

namespace ExtendedQuickslots
{
    //Here we fix the stability bar display
    [HarmonyPatch(typeof(QuickSlotPanel))]
    [HarmonyPatch("Update")]
    public static class Prefix_QuickSlotPanel_Update
    {
        public static FastInvokeHandler UIElementUpdate = null;
        static void Prepare()
        {
            UIElementUpdate = MethodInvoker.GetHandler(
                AccessTools.Method(typeof(UIElement), "Update")
            );
        }

        [HarmonyPrefix]
        public static bool UpdatePrefix(QuickSlotPanel __instance, ref Character ___m_lastCharacter, ref bool ___m_initialized, ref QuickSlotDisplay[] ___m_quickSlotDisplays, ref bool ___m_active)
        {
            UIElement instanceBase = __instance as UIElement;
            UIElementUpdate(__instance, new object[] { });

            if ((instanceBase.LocalCharacter == null || ___m_lastCharacter != instanceBase.LocalCharacter) && ___m_initialized)
            {
                ___m_initialized = false;
            }
            if (___m_initialized)
            {
                if (__instance.UpdateInputVisibility)
                {
                    for (int i = 0; i < ___m_quickSlotDisplays.Length; i++)
                    {
                        ___m_quickSlotDisplays[i].SetInputTargetAlpha((float)((!___m_active) ? 0 : 1));
                    }
                }
            }
            else if (instanceBase.LocalCharacter != null)
            {
                for (int j = 0; j < ___m_quickSlotDisplays.Length; j++)
                {
                    int refSlotID = ___m_quickSlotDisplays[j].RefSlotID;
                    ___m_quickSlotDisplays[j].SetQuickSlot(instanceBase.LocalCharacter.QuickSlotMngr.GetQuickSlot(refSlotID));
                }
                ___m_lastCharacter = instanceBase.LocalCharacter;
                ___m_initialized = true;
                // We want to find a specific QuickSlotPanel instance, since there are multiple
                if (instanceBase.name == "Keyboard" && instanceBase.transform.parent.name == "QuickSlot")
                {
                    // Find our default StabilityDisplay_Simple object
                    StabilityDisplay_Simple stabilityDisplay = UnityEngine.Object.FindObjectOfType<StabilityDisplay_Simple>();
                    // Streamline the stability display so it's not so far from the bottom of the screen.
                    // This also means the hotbar gets placed closer to the bottom of the screen, but
                    // still with neat spacing.
                    stabilityDisplay.transform.position = new Vector3(stabilityDisplay.transform.position.x, stabilityDisplay.transform.position.y / 3f, stabilityDisplay.transform.position.z);
                    // Get the screen coords of its corners
                    Vector3[] stabilityDisplayCorners = new Vector3[4];
                    stabilityDisplay.RectTransform.GetWorldCorners(stabilityDisplayCorners);
                    // We want to set the QuickSlotPanel to be above the stability display, with equal space above the 
                    // stability display as there is below it, to make it look nice and neat.
                    float newY = stabilityDisplayCorners[1].y + stabilityDisplayCorners[0].y;
                    instanceBase.transform.parent.position = new Vector3(instanceBase.transform.parent.position.x, newY, instanceBase.transform.parent.position.z);
                    // Logic to center the panel so the look and feel is more consistent with normal games
                    if (ExtendedQuickslots.centerBar)
                    {
                        Vector3[] v0 = new Vector3[4];
                        Vector3[] v1 = new Vector3[4];
                        ___m_quickSlotDisplays[0].RectTransform.GetWorldCorners(v0);
                        ___m_quickSlotDisplays[1].RectTransform.GetWorldCorners(v1);
                        // The width of each icon
                        var qsdWidth = v0[2].x - v0[1].x;
                        // The width of the space between each icon
                        var qsdSpacer = v1[0].x - v0[2].x;
                        // Total space per icon/spacer pair
                        var elemWidth = qsdWidth + qsdSpacer;
                        // How long our bar really is
                        var realWidth = elemWidth * ___m_quickSlotDisplays.Length;
                        // Re-center it based on actual content
                        instanceBase.transform.parent.position = new Vector3(realWidth / 2.0f + elemWidth / 2.0f, instanceBase.transform.parent.position.y, instanceBase.transform.parent.position.z);
                    }
                }
            }
            return false;
        }
    }   
}