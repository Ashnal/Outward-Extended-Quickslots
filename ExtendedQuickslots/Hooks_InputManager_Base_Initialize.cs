using Harmony;
using Rewired;
using UnityEngine;
using System.Collections.Generic;

namespace ExtendedQuickslots
{
    //only place we can add actions
    static partial class Hooks
    {
        public static void InputManager_Base_Initialize(On.Rewired.InputManager_Base.orig_Initialize orig, Rewired.InputManager_Base self)
        {
            ExtendedQuickslots.Logger.LogDebug("InitializePrefix()");
            InputAction inputAction;
            List<InputAction> actions_Copy;
            Traverse inputActionTrav;

            for (var x = 0; x < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; ++x)
            {
                // Add an action to the QuickSlot category (see 'Rewired_actions.txt')
                self.userData.AddAction(2);

                // Get a reference to the added action
                actions_Copy = self.userData.GetActions_Copy();
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

            self.userData.AddAction(4); //Sit emote
            self.userData.AddAction(4); //alternate idle
            actions_Copy = self.userData.GetActions_Copy();

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

            orig(self);
        }
    }
}
