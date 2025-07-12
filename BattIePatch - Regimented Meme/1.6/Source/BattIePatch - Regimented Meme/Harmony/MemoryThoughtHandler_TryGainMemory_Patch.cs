using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace BattIePatch_RegimentedMeme
{
    [HarmonyPatch(typeof(MemoryThoughtHandler))]
    [HarmonyPatch("TryGainMemory")]
    [HarmonyPatch(new Type[] { typeof(Thought_Memory), typeof(Pawn) })]
    public static class MemoryThoughtHandler_TryGainMemory_Patch
    {
        [HarmonyPostfix]
        private static void TryGainMemory_Patch(Thought_Memory newThought, Pawn otherPawn, MemoryThoughtHandler __instance)
        {
            if (newThought.def.defName != "SleptInBarracks")
            {
                return;
            }
            if (__instance.pawn.ideo == null || __instance.pawn.ideo.Ideo == null)
            {
                return;
            }

            switch (newThought.def.defName)
            {
                case "SleptInBarracks":
                    SleptInBarracks(newThought, otherPawn, __instance);
                    break;
            }
        }

        static void SleptInBarracks(Thought_Memory newThought, Pawn otherPawn, MemoryThoughtHandler __instance)
        {
            Precept precept = __instance.pawn.ideo.Ideo.GetPrecept(PreceptDefOf.BattIePatch_Barracks_Tolerable);
            if (precept == null)
            {
                return;
            }

            if (__instance.OldestMemoryOfDef(RimWorld.ThoughtDefOf.SleptInBarracks) != null)
            {
                __instance.RemoveMemory(__instance.OldestMemoryOfDef(RimWorld.ThoughtDefOf.SleptInBarracks));
                newThought = ThoughtMaker.MakeThought(ThoughtDefOf.BattIePatch_SleptInBarracks, newThought.CurStageIndex);
                newThought.sourcePrecept = precept;
                __instance.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought);
            }
        }
    }
}