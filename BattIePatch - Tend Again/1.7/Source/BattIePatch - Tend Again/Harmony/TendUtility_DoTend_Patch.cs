using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BattIePatch_TendAgain
{
    [HarmonyPatch(typeof(TendUtility), nameof(TendUtility.DoTend))]
    public static class TendUtility_DoTend_Patch
    {
        private static readonly Dictionary<Pawn, List<HediffComp_TendDuration>> previousTends = new Dictionary<Pawn, List<HediffComp_TendDuration>>();

        public static void Prefix(Pawn patient)
        {
            previousTends[patient] = new List<HediffComp_TendDuration>();

            foreach (Hediff hediff in patient.health.hediffSet.hediffs)
            {
                HediffComp_TendDuration comp = hediff.TryGetComp<HediffComp_TendDuration>();
                if (comp != null)
                {
                    if (comp.AllowTend)
                    {
                        previousTends[patient].Add(comp);
                    }
                }
            }
        }

        public static void Postfix(Pawn doctor, Pawn patient, Medicine medicine)
        {
            if (!previousTends.TryGetValue(patient, out List<HediffComp_TendDuration> previous))
            {
                return;
            }

            float potential = TendUtility.CalculateBaseTendQuality(doctor, patient, medicine?.def);

            foreach (HediffComp_TendDuration comp in previous)
            {
                if (!comp.AllowTend)
                {
                    HediffComp_TendTracker tracker = comp.parent.TryGetComp<HediffComp_TendTracker>();

                    if (tracker != null)
                    {
                        tracker.UpdateStoredTendPotential(potential);
                    }
                }
            }

            previousTends.Remove(patient);
        }
    }
}