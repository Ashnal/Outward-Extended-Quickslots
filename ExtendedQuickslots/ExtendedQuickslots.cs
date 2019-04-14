using System;
using System.IO;
using System.Reflection;
using Partiality.Modloader;
using Harmony;
using UnityEngine;

namespace ExtendedQuickslots
{
    public class ExtendedQuickslots : PartialityMod
    {
        public static int numSlots = 8;
        public static bool centerBar = false;

        public ExtendedQuickslots()
        {
            ModID = "Extended Quickslots";
            Version = "1.0.1";
            author = "Stimmedcow, Ashnal";
        }

        public override void Init()
        {
            string[] configFileContents = File.ReadAllLines(@"Mods/ExtendedQuickslots_Config.txt");

            if (!Int32.TryParse(configFileContents[0].Substring(9), out numSlots))
            {
                Debug.Log("Mods\\ExtendedQuickslots_Config.txt numSlots= line was unable to be read. The first line of this file should contain numSlots= followed by an integer. Example: numSlots=12. Defaulting to 8.");
            }
            if (!bool.TryParse(configFileContents[1].Substring(10), out centerBar))
            {
                Debug.Log("Mods\\ExtendedQuickslots_Config.txt centerBar= line was unable to be read. The second line of this file should contain centerBar= followed by either true or false. Example: numSlots=true. Defaulting to false.");
            }

            var harmony = HarmonyInstance.Create("com.ashnal.outward.extendedquickslots");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}