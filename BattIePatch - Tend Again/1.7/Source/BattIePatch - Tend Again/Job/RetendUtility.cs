using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BattIePatch_TendAgain
{
    public static class RetendUtility
    {
        public static void Retend(Pawn doctor, Pawn patient, Medicine medicine)
        {
            if (medicine != null && medicine.Destroyed)
            {
                Log.Error("Tried to use destroyed medicine.");
                medicine = null;
            }
            float quality = TendUtility.CalculateBaseTendQuality(doctor, patient, medicine?.def);
            float maxQuality = medicine?.def.GetStatValueAbstract(StatDefOf.MedicalQualityMax) ?? 0.7f;


            Hediff hediff = GetOptimalHediffToRetend(patient, quality);
            
            if (hediff != null)
            {
                HediffComp_TendDuration duration = hediff.TryGetComp<HediffComp_TendDuration>();
                float storedQuality = duration.tendQuality;
                hediff.Tended(quality, maxQuality);
                HediffComp_TendTracker tracker = hediff.TryGetComp<HediffComp_TendTracker>();
                tracker.UpdateStoredTendPotential(quality);
                if (duration.tendQuality < storedQuality)
                {
                    duration.tendQuality = storedQuality;
                }
            }
            else
            {
                Log.Error($"No hediff found to retend for {patient.LabelShort}");
                return;
            }


            if (doctor != null && doctor.Faction == Faction.OfPlayer && patient.Faction != doctor.Faction && !patient.IsPrisoner && patient.Faction != null)
            {
                patient.mindState.timesGuestTendedToByPlayer++;
            }
            if (doctor != null && doctor.RaceProps.Humanlike && patient.RaceProps.Animal && patient.RaceProps.playerCanChangeMaster && RelationsUtility.TryDevelopBondRelation(doctor, patient, 0.004f) && doctor.Faction != null && doctor.Faction != patient.Faction)
            {
                InteractionWorker_RecruitAttempt.DoRecruit(doctor, patient, useAudiovisualEffects: false);
            }
            patient.records.Increment(RecordDefOf.TimesTendedTo);
            doctor?.records.Increment(RecordDefOf.TimesTendedOther);
            if (doctor == patient && !doctor.Dead)
            {
                doctor.mindState.Notify_SelfTended();
            }
            if (medicine != null)
            {
                if ((patient.Spawned || (doctor != null && doctor.Spawned)) && medicine.GetStatValue(StatDefOf.MedicalPotency) > ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency))
                {
                    SoundDefOf.TechMedicineUsed.PlayOneShot(new TargetInfo(patient.Position, patient.Map));
                }
                if (medicine.stackCount > 1)
                {
                    medicine.stackCount--;
                }
                else if (!medicine.Destroyed)
                {
                    medicine.Destroy();
                }
            }
            if (ModsConfig.IdeologyActive && doctor?.Ideo != null)
            {
                Precept_Role role = doctor.Ideo.GetRole(doctor);
                if (role?.def.roleEffects != null)
                {
                    foreach (RoleEffect roleEffect in role.def.roleEffects)
                    {
                        roleEffect.Notify_Tended(doctor, patient);
                    }
                }
            }
            if (doctor != null && doctor.Faction == Faction.OfPlayer && doctor != patient)
            {
                QuestUtility.SendQuestTargetSignals(patient.questTags, "PlayerTended", patient.Named("SUBJECT"));
            }
        }

        private static Hediff GetOptimalHediffToRetend(Pawn patient, float quality)
        {
            int priority = 0;
            Hediff best = null;

            foreach (Hediff hediff in patient.health.hediffSet.hediffs)
            {
                if (hediff is Hediff_Injury && priority > 1)
                {
                    continue;
                }
                if (priority > 2)
                {
                    if (hediff.def != HediffDefOf.ScariaInfection && hediff.def != HediffDefOf.WoundInfection)
                    {
                        continue;
                    }
                }
                HediffComp_TendDuration tendDuration = hediff.TryGetComp<HediffComp_TendDuration>();

                if (!CanRetendHediff(hediff, tendDuration, quality))
                {
                    continue;
                }

                if (hediff is Hediff_Injury)
                {
                    if(best == null || priority == 0)
                    {
                        priority = 1;
                    }
                }
                else if (hediff.def == HediffDefOf.ScariaInfection || hediff.def == HediffDefOf.WoundInfection)
                {
                    if (best == null || priority < 3)
                    {
                        priority = 3;
                    }
                }
                else
                {
                    if (best == null || priority < 2)
                    {
                        priority = 2;
                    }
                }

                if (best == null || tendDuration.tendQuality < best.TryGetComp<HediffComp_TendDuration>().tendQuality)
                {
                    best = hediff;
                }
            }

            return best;
        }
        public static bool CanRetendHediff(Hediff hediff, HediffComp_TendDuration tendDuration, float currentPotential)
        {
            HediffComp_TendTracker tracker = hediff.TryGetComp<HediffComp_TendTracker>();

            if (tracker == null || tendDuration == null)
            {
                return false;
            }

            if (!CheckThresholds(new List<Hediff>() {hediff}, out float storedPotential))
            {
                return false;
            }

            return currentPotential > storedPotential + BattIePatchTendAgainSettings.tendAgain_Buffer;
        }

        public static bool DoctorShouldRetend(Pawn doctor, Pawn patient)
        {
            float currentPotential = TendUtility.CalculateBaseTendQuality(doctor, patient, FindBestMedicine(doctor, patient)?.def);
            
            if (!CheckThresholds(patient.health.hediffSet.hediffs, out float storedPotential))
            {
                return false;
            }

            return currentPotential > storedPotential + BattIePatchTendAgainSettings.tendAgain_Buffer;
        }

        private static bool CheckThresholds(List<Hediff> hediffs, out float storedPotential)
        {
            float injuryThreshold = Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain * 100) / 100f;
            float infectionThreshold = Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain_Infection * 100) / 100f;
            float diseaseThreshold = Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain_Disease * 100) / 100f;
            bool flag = false;
            float storedQuality = -1f;

            foreach (Hediff hediff in hediffs)
            {
                HediffComp_TendTracker tracker = hediff.TryGetComp<HediffComp_TendTracker>();
                HediffComp_TendDuration tendDuration = hediff.TryGetComp<HediffComp_TendDuration>();
                if (tracker == null || tendDuration == null)
                {
                    continue;
                }


                if (hediff.IsPermanent())
                {
                    continue;
                }

                if (hediff is Hediff_Injury)
                {
                    if (BattIePatchTendAgainSettings.tendAgainCheck == false || tendDuration.tendQuality >= injuryThreshold)
                    {
                        continue;
                    }
                }
                else if (hediff.def == HediffDefOf.ScariaInfection || hediff.def == HediffDefOf.WoundInfection)
                {
                    if (BattIePatchTendAgainSettings.tendAgainCheck_Infection == false || tendDuration.tendQuality >= infectionThreshold)
                    {
                        continue;
                    }
                }
                else
                {
                    if (BattIePatchTendAgainSettings.tendAgainCheck_Disease == false || tendDuration.tendQuality >= diseaseThreshold)
                    {
                        continue;
                    }
                }
                storedQuality = tracker.StoredTendPotential;
                flag = true;
            }
            storedPotential = storedQuality;
            return flag;
        }

        public static Thing FindBestMedicine(Pawn healer, Pawn patient)
        {
            if (patient.playerSettings != null && (int)patient.playerSettings.medCare <= 1)
            {
                return null;
            }

            Predicate<Thing> validator = delegate (Thing m)
            {
                bool flag = ((patient.playerSettings == null) ? MedicalCareCategory.NoMeds : patient.playerSettings.medCare).AllowsMedicine(m.def);

                return (!m.IsForbidden(healer) && flag && healer.CanReserve(m, 10, 1)) ? true : false;
            };
            Thing thing = GetBestMedInInventory(healer.inventory.innerContainer);

            Thing thing2 = GenClosest.ClosestThing_Global_Reachable(patient.PositionHeld, patient.MapHeld, patient.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Medicine), PathEndMode.ClosestTouch, TraverseParms.For(healer), 9999f, validator, PriorityOf);
            if (thing != null && thing2 != null)
            {
                if (!(PriorityOf(thing) >= PriorityOf(thing2)))
                {
                    return thing2;
                }

                return thing;
            }

            if (thing == null && thing2 == null && healer.IsColonist && healer.Map != null)
            {
                Thing thing3 = null;
                foreach (Pawn spawnedColonyAnimal in healer.Map.mapPawns.SpawnedColonyAnimals)
                {
                    thing3 = GetBestMedInInventory(spawnedColonyAnimal.inventory.innerContainer);
                    if (thing3 != null && (thing2 == null || PriorityOf(thing2) < PriorityOf(thing3)) && !spawnedColonyAnimal.IsForbidden(healer) && healer.CanReach(spawnedColonyAnimal, PathEndMode.OnCell, Danger.Some))
                    {
                        thing2 = thing3;
                    }
                }
            }

            return thing ?? thing2;
            Thing GetBestMedInInventory(ThingOwner inventory)
            {
                if (inventory.Count == 0)
                {
                    return null;
                }

                return inventory.Where((Thing t) => t.def.IsMedicine && validator(t)).OrderByDescending(PriorityOf).FirstOrDefault();
            }

            static float PriorityOf(Thing t)
            {
                return t.def.GetStatValueAbstract(StatDefOf.MedicalPotency);
            }
        }
    }
}
