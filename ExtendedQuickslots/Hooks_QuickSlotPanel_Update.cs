using Harmony;
using UnityEngine;

namespace ExtendedQuickslots
{
    public static partial class Hooks
    {
        public static void QuickSlotPanel_Update(On.QuickSlotPanel.orig_Update orig, QuickSlotPanel self)
        {
            UIElement instanceBase = self as UIElement;
            instanceBase.Update();

            if ((instanceBase.LocalCharacter == null || self.m_lastCharacter != instanceBase.LocalCharacter) && self.m_initialized)
            {
                self.m_initialized = false;
            }
            if (self.m_initialized)
            {
                if (self.UpdateInputVisibility)
                {
                    for (int i = 0; i < self.m_quickSlotDisplays.Length; i++)
                    {
                        self.m_quickSlotDisplays[i].SetInputTargetAlpha((float)((!self.m_active) ? 0 : 1));
                    }
                }
            }
            else if (instanceBase.LocalCharacter != null)
            {
                for (int j = 0; j < self.m_quickSlotDisplays.Length; j++)
                {
                    int refSlotID = self.m_quickSlotDisplays[j].RefSlotID;
                    self.m_quickSlotDisplays[j].SetQuickSlot(instanceBase.LocalCharacter.QuickSlotMngr.GetQuickSlot(refSlotID));
                }
                self.m_lastCharacter = instanceBase.LocalCharacter;
                self.m_initialized = true;
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
                    if (ExtendedQuickslots.CenteredQuickslotUI.Value)
                    {
                        Vector3[] v0 = new Vector3[4];
                        Vector3[] v1 = new Vector3[4];
                        self.m_quickSlotDisplays[0].RectTransform.GetWorldCorners(v0);
                        self.m_quickSlotDisplays[1].RectTransform.GetWorldCorners(v1);
                        // The width of each icon
                        var qsdWidth = v0[2].x - v0[1].x;
                        // The width of the space between each icon
                        var qsdSpacer = v1[0].x - v0[2].x;
                        // Total space per icon/spacer pair
                        var elemWidth = qsdWidth + qsdSpacer;
                        // How long our bar really is
                        var realWidth = elemWidth * self.m_quickSlotDisplays.Length;
                        // Re-center it based on actual content
                        instanceBase.transform.parent.position = new Vector3(realWidth / 2.0f + elemWidth / 2.0f, instanceBase.transform.parent.position.y, instanceBase.transform.parent.position.z);
                        // Scale based on the config
                        //instanceBase.transform.parent.localScale += new Vector3(ExtendedQuickslots.QuickslotUIScale.Value, ExtendedQuickslots.QuickslotUIScale.Value);
                    }
                }
            }
            orig(self);
        }
    }   
}