using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Harmony;

namespace ExtendedQuickslots
{
    [BepInPlugin("com.Ashnal.Outward.ExtendedQuickslots", "Extended Quickslots", "1.1.0")]
    public class ExtendedQuickslots : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger;
        internal static ConfigWrapper<int> NumberOfExtraSlotsToAdd;
        internal static ConfigWrapper<bool> CenteredQuickslotUI;
        internal static ConfigWrapper<float> QuickslotUIScale;

        public ExtendedQuickslots()
        {
            Logger = base.Logger;
        }

        public void Awake()
        {
            Logger.LogInfo("Grabbing config from " + Config.ConfigFilePath);

            NumberOfExtraSlotsToAdd = Config.Wrap(
                "Quickslots",
                "NumberOfExtraSlotsToAdd",
                "The number of EXTRA slots to add to the existing 8. So 8 here would result in 16 total slots.",
                8);
            Logger.LogInfo("\tNumberOfExtraSlotsToAdd = " + NumberOfExtraSlotsToAdd.Value);

            CenteredQuickslotUI = Config.Wrap(
                "Quickslots",
                "CenteredQuickslotUI",
                "If true, this will horizontally center the quickslot UI.",
                false);
            Logger.LogInfo("\tCenteredQuickslotUI = " + CenteredQuickslotUI.Value);

            //Waiting for float implementation
            /*
            QuickslotUIScale = Config.Wrap(
                "Quickslots",
                "QuickslotUIScale",
                "This can scale the quickslot UI smaller or larger. The scale is multiplied by this value, so 0.75 for example would shrink it, and 1.5 would enlarge it.",
                1f);
            Logger.LogInfo("\tCenteredQuickslotUI = " + CenteredQuickslotUI);
            */

            On.QuickSlotPanel.Update += new On.QuickSlotPanel.hook_Update(Hooks.QuickSlotPanel_Update);
            On.KeyboardQuickSlotPanel.InitializeQuickSlotDisplays += new On.KeyboardQuickSlotPanel.hook_InitializeQuickSlotDisplays(Hooks.KeyboardQuickSlotPanel_InitializeQuickSlotDisplays);
            On.CharacterQuickSlotManager.Awake += new On.CharacterQuickSlotManager.hook_Awake(Hooks.CharacterQuickSlotManager_Awake);
            On.LocalCharacterControl.UpdateQuickSlots += new On.LocalCharacterControl.hook_UpdateQuickSlots(Hooks.LocalCharacterControl_UpdateQuickSlots);
            On.ControlMappingPanel.InitSections += new On.ControlMappingPanel.hook_InitSections(Hooks.ControlMappingPanel_InitSections);
            On.Rewired.InputManager_Base.Initialize += new On.Rewired.InputManager_Base.hook_Initialize(Hooks.InputManager_Base_Initialize);
            On.LocalizationManager.StartLoading += new On.LocalizationManager.hook_StartLoading(Hooks.LocalizationManager_StartLoading);
        }
    }
}