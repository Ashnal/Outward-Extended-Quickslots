using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Harmony;
using UnityEngine;

namespace ExtendedQuickslots
{
    [BepInPlugin("com.ashnal.outward.extendedquickslots", "Extended Quickslots", "1.1.0")]
    public class ExtendedQuickslots : BaseUnityPlugin
    {
        public static int numSlots = 8;
        public static bool centerBar = false;
        public new static ManualLogSource Logger;

        public ExtendedQuickslots()
        {
            Logger = base.Logger;
            string[] configFileContents = File.ReadAllLines(@"BepInEx/config/ExtendedQuickslots_Config.txt");

            if (!Int32.TryParse(configFileContents[0].Substring(9), out numSlots))
            {
                Logger.LogWarning("BepinEx\\config\\ExtendedQuickslots_Config.txt numSlots= line was unable to be read. The first line of this file should contain numSlots= followed by an integer. Example: numSlots=12. Defaulting to 8.");
            }
            if (!bool.TryParse(configFileContents[1].Substring(10), out centerBar))
            {
                Logger.LogWarning("BepinEx\\config\\ExtendedQuickslots_Config.txt centerBar= line was unable to be read. The second line of this file should contain centerBar= followed by either true or false. Example: numSlots=true. Defaulting to false.");
            }

            var harmony = HarmonyInstance.Create("com.ashnal.outward.extendedquickslots");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}