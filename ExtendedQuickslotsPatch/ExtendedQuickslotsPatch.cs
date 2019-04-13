using System;
using System.Collections.Generic;
using Mono.Cecil;
using MonoMod.InlineRT;
using System.Linq;

namespace MonoMod
{
    internal static class MonoModRules
    {
        [MonoModPatch("ExtendedQuickslotsPatch")]
        public class TriggerRulesProcessing
        {
            //Empty class required to trigger MonoMod's rules processing.
        }

        static MonoModRules()
        {
            MonoModder modder = MonoModRule.Modder;
            modder.PostProcessors = (PostProcessor)Delegate.Combine(modder.PostProcessors, new PostProcessor(MonoModRules.PostProcessor));
        }

        public static void PostProcessor(MonoModder modder)
        {
            List<string> typesToPatch = new List<string>() {
                "RewiredInputs",
                "LocalizationManager",
                "ControlMappingPanel",
                "CharacterQuickSlotManager",
                "ControlsInput"
            };

            /*
            foreach (TypeDefinition t in modder.Module.Types)
            {
                Console.WriteLine("Found type: " + t.Name);
            }
            */

            var typeDefinitions = from TypeDefinition t in modder.Module.Types
                                  where typesToPatch.Contains(t.Name)
                                  select t;

            foreach (TypeDefinition typeDefinition in typeDefinitions)
            {
                MonoModRules.PostProcessType(modder, typeDefinition);
            }
        }

        private static void PostProcessType(MonoModder modder, TypeDefinition type)
        {
            foreach (FieldDefinition field in type.Fields)
            {
                MonoModRules.PostProcessField(modder, field);
            }
            foreach (MethodDefinition mDef in type.Methods)
            {
                MonoModRules.PostProcessMethod(modder, mDef);
            }
            foreach (TypeDefinition type2 in type.NestedTypes)
            {
                MonoModRules.PostProcessType(modder, type2);
            }
            if (type.IsNested)
            {
                type.IsNestedPublic = true;
                return;
            }
            type.IsPublic = true;
        }

        private static void PostProcessField(MonoModder modder, FieldDefinition field)
        {
            field.IsPublic = true;
        }

        private static void PostProcessMethod(MonoModder modder, MethodDefinition mDef)
        {
            mDef.IsPublic = true;
        }
    }
}
