using Harmony;
using Rewired;
using UnityEngine;
using BepInEx;

namespace ExtendedQuickslots
{
    //to make sure the mappings are initialized properly
    [HarmonyPatch(typeof(ControlMappingPanel))]
    [HarmonyPatch("InitMappings")]
    public static class Prefix_ControlMappingPanel_InitMappings
    {
        public static FastInvokeHandler ControlMappingPanel_InitSections = null;
        static void Prepare()
        {
            ControlMappingPanel_InitSections = MethodInvoker.GetHandler(
                AccessTools.Method(typeof(ControlMappingPanel), "InitSections")
            );
        }

        [HarmonyPrefix]
        public static bool InitMappingsPrefix(ControlMappingPanel __instance, ref ControlMappingSection ___m_sectionTemplate, ref bool ___m_mappingInitialized, ref Controller ___m_lastJoystickController)
        {
            if (___m_sectionTemplate)
            {
                ___m_sectionTemplate.gameObject.SetActive(true);
                foreach (InputMapCategory inputMapCategory in ReInput.mapping.UserAssignableMapCategories)
                {
                    if (__instance.ControllerType == ControlMappingPanel.ControlType.Keyboard)
                    {
                        KeyboardMap keyboardMapInstance = ReInput.mapping.GetKeyboardMapInstance(inputMapCategory.id, 0);
                        MouseMap mouseMapInstance = ReInput.mapping.GetMouseMapInstance(inputMapCategory.id, 0);
                        // We know this name from debugging, or we can dump it.
                        ExtendedQuickslots.Logger.LogDebug("InitMappingsPrefix() inputMapCategory = " + inputMapCategory.name);
                        if (inputMapCategory.name == "QuickSlot")
                        {
                            ExtendedQuickslots.Logger.LogInfo("Mapping Quickslots ...");
                            // Loop through our 8 actions we added via ReWired and create the mapping objects for them.
                            for (int i = 0; i < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; i++)
                            {
                                ExtendedQuickslots.Logger.LogDebug("\tMapping " + string.Format("QS_Instant{0}", i + 12));
                                var aid = ReInput.mapping.GetActionId(string.Format("QS_Instant{0}", i + 12));
                                ElementAssignment elementAssignment = new ElementAssignment(KeyCode.None, ModifierKeyFlags.None, aid, Pole.Positive);
                                keyboardMapInstance.CreateElementMap(elementAssignment, out ActionElementMap actionElementMap);
                                mouseMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                            }
                        }
                        if (inputMapCategory.name == "Actions")
                        {
                            ElementAssignment elementAssignment;
                            int aid;

                            ExtendedQuickslots.Logger.LogInfo("Mapping Sit emote");
                            aid = ReInput.mapping.GetActionId("Sit_Emote");
                            elementAssignment = new ElementAssignment(KeyCode.None, ModifierKeyFlags.None, aid, Pole.Positive);
                            keyboardMapInstance.CreateElementMap(elementAssignment, out ActionElementMap actionElementMap);
                            mouseMapInstance.CreateElementMap(elementAssignment, out actionElementMap);

                            ExtendedQuickslots.Logger.LogInfo("Mapping Alternate Idle Emote");
                            aid = ReInput.mapping.GetActionId("Alternate_Idle_Emote");
                            elementAssignment = new ElementAssignment(KeyCode.None, ModifierKeyFlags.None, aid, Pole.Positive);
                            keyboardMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                            mouseMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                        }

                        ControlMappingPanel_InitSections(__instance, new object[] { keyboardMapInstance });
                        ControlMappingPanel_InitSections(__instance, new object[] { mouseMapInstance });
                    }
                    else if (___m_lastJoystickController != null)
                    {
                        JoystickMap joystickMapInstance = ReInput.mapping.GetJoystickMapInstance((Joystick)___m_lastJoystickController, inputMapCategory.id, 0);
                        ControlMappingPanel_InitSections(__instance, new object[] { joystickMapInstance });
                    }
                    else
                    {
                        ___m_mappingInitialized = false;
                    }
                }
                ___m_sectionTemplate.gameObject.SetActive(false);
            }
            return false;
        }
    }
}
