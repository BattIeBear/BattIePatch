using RimWorld;
using Verse;
using Verse.AI;

namespace BattIePatch_TendAgain
{
    public class WorkGiver_DoctorRetendPatient : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (pawn == null || t == null)
            {
                return false;
            }

            if (!(t is Pawn pawn2) || pawn == pawn2 || !WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) || pawn2.playerSettings == null || !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(pawn2) || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.IsMutant && !pawn2.mutant.Def.entitledToMedicalCare) || (pawn2.InAggroMentalState && !pawn2.health.hediffSet.HasHediff(HediffDefOf.Scaria)) || pawn2.health.HasHediffsNeedingTend())
            {
                return false;
            }

            if (!EligableForRetend(pawn2))
            {
                return false;
            }

            if (!RetendUtility.DoctorShouldRetend(pawn, pawn2))
            {
                return false;
            }

            return true;
        }

        public bool EligableForRetend(Pawn pawn)
        {

            if (pawn.IsAnimal && BattIePatchTendAgainSettings.tendAgainAnimals == false)
            {
                return false;
            }

            if (pawn.Faction.IsPlayer == false)
            {
                Pawn_GuestTracker guestTracker = pawn.guest;
                if (guestTracker == null)
                {
                    return false;
                }

                if (guestTracker.HostFaction == null || guestTracker.HostFaction.IsPlayer == false)
                {
                    return false;
                }
            }

            if (pawn.GuestStatus == GuestStatus.Guest && BattIePatchTendAgainSettings.tendAgainGuests == false)
            {
                return false;
            }

            if (pawn.GuestStatus == GuestStatus.Prisoner && BattIePatchTendAgainSettings.tendAgainPrisoners == false)
            {
                return false;
            }

            if (pawn.GuestStatus == GuestStatus.Slave && BattIePatchTendAgainSettings.tendAgainSlaves == false)
            {
                return false;
            }

            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn patient = (Pawn)t;
            Thing medicine = RetendUtility.FindBestMedicine(pawn, patient);

            if (medicine != null && medicine.SpawnedParentOrMe != medicine)
            {
                return JobMaker.MakeJob(JobDefOf.BattIeBear_RetendPatient, patient, medicine, medicine.SpawnedParentOrMe);
            }

            if (medicine != null)
            {
                return JobMaker.MakeJob(JobDefOf.BattIeBear_RetendPatient, patient, medicine);
            }

            return JobMaker.MakeJob(JobDefOf.BattIeBear_RetendPatient, patient);
        }
    }
}