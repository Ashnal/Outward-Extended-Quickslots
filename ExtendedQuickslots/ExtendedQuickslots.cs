
using System;
using System.Collections.Generic;
using System.Reflection;
using Partiality.Modloader;
using UnityEngine;
using Rewired;
using Harmony;

public class MyMod : PartialityMod
{
    public MyMod()
    {
        this.ModID = "Extended Quickslots";
        this.Version = "1.0.0";
        this.author = "Stimmedcow, Ashnal";
    }

    public override void Init()
    {
        var harmony = HarmonyInstance.Create("com.ashnal.outward.extendedquickslots");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}

//only place we can add actions
[HarmonyPatch(typeof(InputManager_Base))]
[HarmonyPatch("Initialize")]
public static class Patch_InputManager_Base_Initialize
{
    [HarmonyPrefix]
    public static bool InitializePrefix(InputManager_Base __instance)
    {
        Debug.Log("Extended Quickslots - Prefix()");
        InputAction inputAction;
        List<InputAction> actions_Copy;
        Traverse inputActionTrav;

        for (var x = 0; x < 8; ++x)
        {
            // Add an action to the QuickSlot category (see 'Rewired_actions.txt')
            __instance.userData.AddAction(2);

            // This is how we have to get the action we just added
            actions_Copy = __instance.userData.GetActions_Copy();
            inputAction = actions_Copy[actions_Copy.Count - 1];

            //Debug.Log("ExtendedQuickslots - InitializePatch() before edit inputAction:\r\n\tname: " + inputAction.name + "\r\n\tdescriptiveName: " + inputAction.descriptiveName + "\r\n\tuserAssignable: " + inputAction.userAssignable + "\r\n\tbehaviorId: " + inputAction.behaviorId);
            // Set the action specific data
            // We use 12 as the base index to avoid conflicts with other game data.
            // This is done to code around an issue with how there's more quickslots
            // due to controllers, so we start with 12 rather than 9 to avoid issues.
            // We'll just change what the options menu shows though, so this internal name
            // doesn't really matter.
            inputActionTrav = Traverse.Create(inputAction);
            string name = string.Format("QS_Instant{0}", x + 12);
            inputActionTrav.Property("name").SetValue(name);
            inputActionTrav.Property("descriptiveName").SetValue("Action0");
            inputActionTrav.Property("userAssignable").SetValue(true);
            inputActionTrav.Property("behaviorId").SetValue(0);

            Debug.Log("ExtendedQuickslots - InitializePatch() inputAction:\r\n\tname: " + inputAction.name + "\r\n\tdescriptiveName: " + inputAction.descriptiveName + "\r\n\tuserAssignable: " + inputAction.userAssignable + "\r\n\tbehaviorId: " + inputAction.behaviorId);
        }

        __instance.userData.AddAction(4); //Sit emote
        __instance.userData.AddAction(4); //alternate idle
        actions_Copy = __instance.userData.GetActions_Copy();

        inputAction = actions_Copy[actions_Copy.Count - 2];
        inputActionTrav = Traverse.Create(inputAction);
        inputActionTrav.Property("name").SetValue("Sit_Emote");
        inputActionTrav.Property("descriptiveName").SetValue("Sit emote");
        inputActionTrav.Property("userAssignable").SetValue(true);
        inputActionTrav.Property("behaviorId").SetValue(0);

        inputAction = actions_Copy[actions_Copy.Count - 1];
        inputActionTrav = Traverse.Create(inputAction);
        inputActionTrav.Property("name").SetValue("Alternate_Idle_Emote");
        inputActionTrav.Property("descriptiveName").SetValue("Arms Crossed emote");
        inputActionTrav.Property("userAssignable").SetValue(true);
        inputActionTrav.Property("behaviorId").SetValue(0);

        return true;
    }
}

//this is where we listen for input then act on it
[HarmonyPatch(typeof(LocalCharacterControl))]
[HarmonyPatch("UpdateQuickSlots")]
public static class Patch_LocalCharacterControl_UpdateQuickSlots
{
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

            if (ControlsInput.m_playerInputManager[playerID].GetButtonDown("Sit_Emote"))
            {
                __instance.Character.CastSpell(Character.SpellCastType.Sit, __instance.Character.gameObject, Character.SpellCastModifier.Immobilized, 1, -1f);
            }
            else if (ControlsInput.m_playerInputManager[playerID].GetButtonDown("Alternate_Idle_Emote"))
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
                for (var x = 0; x < 8; ++x)
                {
                    bool inputRecieved = ControlsInput.m_playerInputManager[playerID].GetButtonDown(string.Format("QS_Instant{0}", x + 12));
                    //Debug.Log(string.Format("Checking QS_Instant{0}: {1}", x+12, inputRecieved));
                    if (inputRecieved)
                    {
                        Debug.Log("ExtendedQuickslots - UpdateQuickSlotsPatch() QS_Instant" + (x + 12));
                        __instance.Character.QuickSlotMngr.QuickSlotInput(x + 11);
                        break;
                    }
                }
            }
        }
        return false;
    }
}

//we need this to setup the extra quickslots
[HarmonyPatch(typeof(CharacterQuickSlotManager))]
[HarmonyPatch("Awake")]
public static class Patch_CharacterQuickSlotManager_Awake
{
    [HarmonyPrefix]
    public static bool AwakePrefix(CharacterQuickSlotManager __instance)
    {
        __instance.m_character = __instance.GetComponent<Character>();
        __instance.m_quickslotTrans = __instance.transform.Find("QuickSlots");
        // Add our 8 QuickSlots
        for (var x = 0; x < 8; ++x)
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

//to make sure the mappings are initialized properly
[HarmonyPatch(typeof(ControlMappingPanel))]
[HarmonyPatch("InitMappings")]
public static class Patch_ControlMappingPanel_InitMappings
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

//sets the display order of the new quickslots
[HarmonyPatch(typeof(KeyboardQuickSlotPanel))]
[HarmonyPatch("InitializeQuickSlotDisplays")]
public static class Patch_KeyboardQuickSlotPanel_InitializeQuickSlotDisplays
{
    [HarmonyPrefix]
    public static bool InitializeQuickSlotDisplaysPatch(KeyboardQuickSlotPanel __instance)
    {
        // Add 8 new quickslot ids to the array and assign them the new ids we added.
        Array.Resize(ref __instance.DisplayOrder, __instance.DisplayOrder.Length + 8);
        int s = 12;
        for (int x = 8; x >= 1; x--)
        {
            __instance.DisplayOrder[__instance.DisplayOrder.Length - x] = (QuickSlot.QuickSlotIDs)s++;
        }
        return true;
    }
}

//adds user friendly names for our binds
[HarmonyPatch(typeof(LocalizationManager))]
[HarmonyPatch("StartLoading")]
public static class Patch_LocalizationManager_StartLoading
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
