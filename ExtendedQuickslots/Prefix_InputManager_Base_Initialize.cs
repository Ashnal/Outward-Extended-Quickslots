using Harmony;
using Rewired;
using UnityEngine;
using System.Collections.Generic;

namespace ExtendedQuickslots
{
    //only place we can add actions
    [HarmonyPatch(typeof(InputManager_Base))]
    [HarmonyPatch("Initialize")]
    public static class Prefix_InputManager_Base_Initialize
    {
        [HarmonyPrefix]
        public static bool InitializePrefix(InputManager_Base __instance)
        {
            ExtendedQuickslots.Logger.LogDebug("InitializePrefix()");
            InputAction inputAction;
            List<InputAction> actions_Copy;
            Traverse inputActionTrav;

            for (var x = 0; x < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; ++x)
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

                ExtendedQuickslots.Logger.LogDebug("ExtendedQuickslots - InitializePatch() inputAction:\r\n\tname: " + inputAction.name + "\r\n\tdescriptiveName: " + inputAction.descriptiveName + "\r\n\tuserAssignable: " + inputAction.userAssignable + "\r\n\tbehaviorId: " + inputAction.behaviorId);
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
}
