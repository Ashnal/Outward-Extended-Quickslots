using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedQuickslots
{
    //this is where we listen for input then act on it
    [HarmonyPatch(typeof(LocalCharacterControl))]
    [HarmonyPatch("UpdateQuickSlots")]
    public static class Prefix_LocalCharacterControl_UpdateQuickSlots
    {
        static Dictionary<int, RewiredInputs> m_playerInputManager;
        static void Prepare()
        {
            m_playerInputManager = (Dictionary<int, RewiredInputs>)AccessTools.Field(typeof(ControlsInput), "m_playerInputManager").GetValue(null);
        }

        [HarmonyPrefix]
        public static bool UpdateQuickSlotsPrefix(LocalCharacterControl __instance)
        {
            if (__instance.Character != null && __instance.Character.QuickSlotMngr != null)
            {
                int playerID = __instance.Character.OwnerPlayerSys.PlayerID;
                if (!__instance.Character.CharacterUI.IsMenuFocused)
                {
                    __instance.Character.QuickSlotMngr.ShowQuickSlotSection1 = ControlsInput.QuickSlotToggle1(playerID);
                    __instance.Character.QuickSlotMngr.ShowQuickSlotSection2 = ControlsInput.QuickSlotToggle2(playerID);
                }

                if (m_playerInputManager[playerID].GetButtonDown("Sit_Emote"))
                {
                    __instance.Character.CastSpell(Character.SpellCastType.Sit, __instance.Character.gameObject, Character.SpellCastModifier.Immobilized, 1, -1f);
                }
                else if (m_playerInputManager[playerID].GetButtonDown("Alternate_Idle_Emote"))
                {
                    __instance.Character.CastSpell(Character.SpellCastType.IdleAlternate, __instance.Character.gameObject, Character.SpellCastModifier.Immobilized, 1, -1f);
                }
                else if (ControlsInput.QuickSlotInstant1(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(0);
                }
                else if (ControlsInput.QuickSlotInstant2(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(1);
                }
                else if (ControlsInput.QuickSlotInstant3(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(2);
                }
                else if (ControlsInput.QuickSlotInstant4(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(3);
                }
                else if (ControlsInput.QuickSlotInstant5(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(4);
                }
                else if (ControlsInput.QuickSlotInstant6(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(5);
                }
                else if (ControlsInput.QuickSlotInstant7(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(6);
                }
                else if (ControlsInput.QuickSlotInstant8(playerID))
                {
                    __instance.Character.QuickSlotMngr.QuickSlotInput(7);
                }
                else
                {
                    //Debug.Log("ExtendedQuickslots - UpdateQuickSlotsPatch() else");
                    for (var x = 0; x < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; ++x)
                    {
                        bool inputRecieved = m_playerInputManager[playerID].GetButtonDown(string.Format("QS_Instant{0}", x + 12));
                        //Debug.Log(string.Format("Checking QS_Instant{0}: {1}", x+12, inputRecieved));
                        if (inputRecieved)
                        {
                            ExtendedQuickslots.Logger.LogDebug("UpdateQuickSlotsPatch() QS_Instant" + (x + 12));
                            __instance.Character.QuickSlotMngr.QuickSlotInput(x + 11);
                            break;
                        }
                    }
                }
            }
            return false;
        }
    }
}
