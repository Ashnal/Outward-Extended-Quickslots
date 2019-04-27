using Rewired;
using UnityEngine;

namespace ExtendedQuickslots
{
    //to make sure the mappings are initialized properly
    public static partial class Hooks
    {
        public static void ControlMappingPanel_InitSections(On.ControlMappingPanel.orig_InitSections orig, ControlMappingPanel self, Rewired.ControllerMap _controllerMap)
        {

            ExtendedQuickslots.Logger.LogDebug("ControllerMap type: " + _controllerMap.controllerType + " Category: " + _controllerMap.categoryId);
            if ((_controllerMap is KeyboardMap || _controllerMap is MouseMap) && _controllerMap.categoryId == 4)
            {
                ExtendedQuickslots.Logger.LogInfo("Mapping Quickslots ...");
                // Loop through our 8 actions we added via ReWired and create the mapping objects for them.
                for (int i = 0; i < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; i++)
                {
                    ExtendedQuickslots.Logger.LogDebug("\tMapping " + string.Format("QS_Instant{0}", i + 12));
                    var aid = ReInput.mapping.GetActionId(string.Format("QS_Instant{0}", i + 12));
                    _controllerMap.CreateElementMap(aid, Pole.Positive, KeyCode.None, ModifierKeyFlags.None);
                }

            }
            orig(self, _controllerMap);
        }
    }
}
