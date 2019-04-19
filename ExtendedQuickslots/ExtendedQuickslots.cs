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

            var harmony = HarmonyInstance.Create("com.ashnal.outward.extendedquickslots");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}