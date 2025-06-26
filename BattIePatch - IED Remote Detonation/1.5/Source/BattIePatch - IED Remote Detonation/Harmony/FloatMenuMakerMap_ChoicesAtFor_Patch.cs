using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;

namespace BattIePatch_IEDRemoteDetonation
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "ChoicesAtFor")]
    public static class FloatMenuMakerMap_ChoicesAtFor_Patch
    {
        static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> __result)
        {
            if (!pawn.Drafted || !BattIePatchIEDRemoteDetonationSettings.DraftedDetonation || pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                return;
            }

            IntVec3 cell = IntVec3.FromVector3(clickPos);
            Map map = pawn.Map;
            foreach (Thing thing in cell.GetThingList(map))
            {
                var remoteTrigger = thing.TryGetComp<CompRemoteTrigger>();

                if (remoteTrigger != null)
                {
                    __result.Add(new FloatMenuOption(
                        "BattIePatch_IEDRemoteDetonation_Detonate".Translate(),
                        () => AssignDetonateJob(pawn, thing, remoteTrigger),
                        MenuOptionPriority.High
                    ));
                }
            }
        }

        static void AssignDetonateJob(Pawn pawn, Thing target, CompRemoteTrigger remoteTrigger)
        {
            Job job = JobMaker.MakeJob(JobDefOf.BattIePatch_DetonateIED, target);
            pawn.jobs.TryTakeOrderedJob(job);
        }
    }
}