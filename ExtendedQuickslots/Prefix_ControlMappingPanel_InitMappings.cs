using Harmony;
using Rewired;
using UnityEngine;

namespace ExtendedQuickslots
{
    //to make sure the mappings are initialized properly
    [HarmonyPatch(typeof(ControlMappingPanel))]
    [HarmonyPatch("InitMappings")]
    public static class Prefix_ControlMappingPanel_InitMappings
    {
        [HarmonyPrefix]
        public static bool InitMappingsPrefix(ControlMappingPanel __instance)
        {
            if (__instance.m_sectionTemplate)
            {
                __instance.m_sectionTemplate.gameObject.SetActive(true);
                foreach (InputMapCategory inputMapCategory in ReInput.mapping.UserAssignableMapCategories)
                {
                    if (__instance.ControllerType == ControlMappingPanel.ControlType.Keyboard)
                    {
                        KeyboardMap keyboardMapInstance = ReInput.mapping.GetKeyboardMapInstance(inputMapCategory.id, 0);
                        MouseMap mouseMapInstance = ReInput.mapping.GetMouseMapInstance(inputMapCategory.id, 0);
                        // We know this name from debugging, or we can dump it.
                        Debug.Log("Extended Quickslots - InitMappingsPrefix() inputMapCategory = " + inputMapCategory.name);
                        if (inputMapCategory.name == "QuickSlot")
                        {
                            Debug.Log("ExtendedQuickslots - Mapping Quickslots ...");
                            // Loop through our 8 actions we added via ReWired and create the mapping objects for them.
                            for (int i = 0; i < 8; i++)
                            {
                                Debug.Log("\tMapping " + string.Format("QS_Instant{0}", i + 12));
                                var aid = ReInput.mapping.GetActionId(string.Format("QS_Instant{0}", i + 12));
                                ElementAssignment elementAssignment = new ElementAssignment(KeyCode.None, ModifierKeyFlags.None, aid, Pole.Positive);
                                ActionElementMap actionElementMap;
                                keyboardMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                                mouseMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                            }
                        }
                        if (inputMapCategory.name == "Actions")
                        {
                            ElementAssignment elementAssignment;
                            ActionElementMap actionElementMap;
                            int aid;

                            Debug.Log("\tMapping Sit emote");
                            aid = ReInput.mapping.GetActionId("Sit_Emote");
                            elementAssignment = new ElementAssignment(KeyCode.None, ModifierKeyFlags.None, aid, Pole.Positive);
                            keyboardMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                            mouseMapInstance.CreateElementMap(elementAssignment, out actionElementMap);

                            Debug.Log("\tMapping Alternate Idle Emote");
                            aid = ReInput.mapping.GetActionId("Alternate_Idle_Emote");
                            elementAssignment = new ElementAssignment(KeyCode.None, ModifierKeyFlags.None, aid, Pole.Positive);
                            keyboardMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                            mouseMapInstance.CreateElementMap(elementAssignment, out actionElementMap);
                        }

                        __instance.InitSections(keyboardMapInstance);
                        __instance.InitSections(mouseMapInstance);
                    }
                    else if (__instance.m_lastJoystickController != null)
                    {
                        JoystickMap joystickMapInstance = ReInput.mapping.GetJoystickMapInstance((Joystick)__instance.m_lastJoystickController, inputMapCategory.id, 0);
                        __instance.InitSections(joystickMapInstance);
                    }
                    else
                    {
                        __instance.m_mappingInitialized = false;
                    }
                }
                __instance.m_sectionTemplate.gameObject.SetActive(false);
            }
            return false;
        }
    }
}
