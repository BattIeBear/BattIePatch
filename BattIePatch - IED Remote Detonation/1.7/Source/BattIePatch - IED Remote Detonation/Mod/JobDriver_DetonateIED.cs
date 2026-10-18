using Verse.AI;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace BattIePatch_IEDRemoteDetonation
{
    public class JobDriver_DetonateIED : JobDriver
    {
        public Thing TargetIED => job.targetA.Thing;
        public CompRemoteTrigger RemoteTrigger => TargetIED.TryGetComp<CompRemoteTrigger>();

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            // Reserve the IED for the pawn
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (RemoteTrigger == null)
            {
                Log.Error("No CompRemoteTrigger found on: " + TargetIED.def.defName);
                yield break;
            }
            if (RemoteTrigger.ExplosiveComp == null)
            {
                Log.Error("No ExplosiveComp found on: " + TargetIED.def.defName + ", even though it has a CompRemoteTrigger!");
                yield break;
            }

            float blastRadius = RemoteTrigger.ExplosiveComp.Props.explosiveRadius;

            bool pawnInsideBlast = pawn.Position.DistanceTo(TargetIED.Position) <= blastRadius;

            IntVec3 detonationCell;

            if (pawnInsideBlast && BattIePatchIEDRemoteDetonationSettings.MoveToSafeDistance)
            {
                // Find a cell OUTSIDE the blast radius, closest to pawn
                detonationCell = GenRadial.RadialCellsAround(TargetIED.Position, blastRadius + 2, true)
                    .Where(cell => cell.Standable(Map) && cell.DistanceTo(TargetIED.Position) > blastRadius + 1)
                    .OrderBy(cell => cell.DistanceTo(pawn.Position))
                    .FirstOrDefault();
            }
            else
            {
                float detonationDistance = blastRadius * BattIePatchIEDRemoteDetonationSettings.DraftedDetonationMaxRange;
                // Find a cell INSIDE the detonation range, closest to pawn
                detonationCell = GenRadial.RadialCellsAround(TargetIED.Position, detonationDistance, true)
                    .Where(cell => cell.Standable(Map))
                    .OrderBy(cell => cell.DistanceTo(pawn.Position))
                    .FirstOrDefault();
            }

            yield return Toils_Goto.GotoCell(detonationCell, PathEndMode.OnCell);

            var waitToil = Toils_General.Wait(180).WithProgressBar(TargetIndex.C, () => 1f - (float)this.ticksLeftThisToil / 180f, false);

            waitToil.defaultCompleteMode = ToilCompleteMode.Delay;

            yield return waitToil;

            yield return new Toil
            {
                initAction = delegate
                {
                    RemoteTrigger.ExplosiveComp.StartWick();
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}