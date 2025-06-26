using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace BattIePatch_IEDRemoteDetonation
{
    public class FloatMenuOptionProvider_IEDRemoteDetonation : FloatMenuOptionProvider
    {
        protected override bool Drafted => true;
        protected override bool Undrafted => false;
        protected override bool Multiselect => false;

        public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
        {
            if (!(clickedThing is Thing thing)) yield break;
            var pawn = context.FirstSelectedPawn;
            if (pawn == null || !pawn.Drafted || !BattIePatchIEDRemoteDetonationSettings.DraftedDetonation || pawn.WorkTagIsDisabled(WorkTags.Violent))
                yield break;

            var remoteTrigger = thing.TryGetComp<CompRemoteTrigger>();
            if (remoteTrigger != null)
            {
                yield return new FloatMenuOption(
                    "BattIePatch_IEDRemoteDetonation_Detonate".Translate(),
                    () => AssignDetonateJob(pawn, thing, remoteTrigger),
                    TexCommand.Detonate,
                    Color.white,
                    MenuOptionPriority.High
                );
            }
        }

        private void AssignDetonateJob(Pawn pawn, Thing target, CompRemoteTrigger remoteTrigger)
        {
            Job job = JobMaker.MakeJob(JobDefOf.BattIePatch_DetonateIED, target);
            pawn.jobs.TryTakeOrderedJob(job);
        }
    }
}