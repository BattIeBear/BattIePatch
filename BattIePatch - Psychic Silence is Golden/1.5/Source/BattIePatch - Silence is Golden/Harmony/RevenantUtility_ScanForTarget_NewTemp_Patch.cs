using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(RevenantUtility), nameof(RevenantUtility.ScanForTarget_NewTemp))]
    class RevenantUtility_ScanForTarget_NewTemp_Patch
    {
        public static bool Prefix(Pawn pawn, ref Pawn __result, ref bool forced)
        {
            if (!BattIePatchSilenceIsGoldenSettings.WeightedMode)
            {
                return true;
            }

            float totalSensitivity = 0;
            float currentSensitivity = 0;
            HashSet<Pawn> tmpTargets = new HashSet<Pawn>();
            TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors);
            RegionTraverser.BreadthFirstTraverse(
                pawn.Position, pawn.Map,
                (Region from, Region to) => to.Allows(traverseParms, isDestination: true),
                delegate (Region x)
                {
                    List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Pawn pawn2 = (Pawn)list[i];
                        if (RevenantUtility.ValidTarget(pawn2))
                        {
                            totalSensitivity += pawn2.GetStatValue(StatDefOf.PsychicSensitivity) > 0f ? pawn2.GetStatValue(StatDefOf.PsychicSensitivity) : 1f;
                            tmpTargets.Add(pawn2);
                        }
                    }
                    return false;
                }
            );

            float targetSensitivity = Random.Range(0f, totalSensitivity);
            if (tmpTargets.Count > 0)
            {
                for (int i = 0; i < tmpTargets.Count; i++)
                {
                    Pawn p = tmpTargets.ElementAt(i);
                    currentSensitivity += p.GetStatValue(StatDefOf.PsychicSensitivity) > 0f ? p.GetStatValue(StatDefOf.PsychicSensitivity) : 1f;
                    if (currentSensitivity >= targetSensitivity)
                    {
                        if (p != null && (forced || RevenantUtility.NearbyHumanlikePawnCount(p.Position, p.Map, 12f) < 5))
                        {
                            __result = p;
                            return false;
                        }
                    }
                }
            }

            __result = null;
            return false;
        }
    }
}
