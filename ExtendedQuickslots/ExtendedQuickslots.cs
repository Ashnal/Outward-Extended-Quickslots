using System.Reflection;
using Partiality.Modloader;
using Harmony;

namespace ExtendedQuickslots
{
    public class ExtendedQuickslots : PartialityMod
    {
        public ExtendedQuickslots()
        {
            ModID = "Extended Quickslots";
            Version = "1.0.0";
            author = "Stimmedcow, Ashnal";
        }

        public override void Init()
        {
            var harmony = HarmonyInstance.Create("com.ashnal.outward.extendedquickslots");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}