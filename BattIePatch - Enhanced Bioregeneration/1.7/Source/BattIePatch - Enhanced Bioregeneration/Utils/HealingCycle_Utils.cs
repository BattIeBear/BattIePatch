using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BattIePatch_EnhancedBioregeneration
{
    public static class HealingCycle_Utils
    {
        /// runs before the cycle starts, telling the player which parts CAN be healed
        public static string GetHealingDescriptionForPawn(CompBiosculpterPod_HealingCycle __instance, Pawn pawn, List<string> ___tmpWillHealHediffs, List<string> ___tmpCanHealHediffs)
        {
            List<string> tmpCanRegenerateHediffs = new List<string>();
            try
            {
                string text = "";
                if (pawn != null)
                {
                    // goes over each of the pawns hediffs
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                    {
                        // runs the "WillHeal" function for that hediff
                        if (WillHeal(pawn, hediff, __instance.Regenerate, __instance.Props))
                        {
                            // adds that hediff to the "will heal" list as a lable (see below)
                            ___tmpWillHealHediffs.Add(HediffLabel(hediff));
                        }
                        // runs the "CanHeal" function for that hediff
                        else if (CanPotentiallyHeal(pawn, hediff, __instance.Regenerate, __instance.Props) && !___tmpCanHealHediffs.Contains(HediffLabel(hediff)))
                        {
                            // adds that hediff to the "can heal" list as a lable (see below)
                            ___tmpCanHealHediffs.Add(HediffLabel(hediff));
                        }
                        else if(BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration)
                        {
                            if(hediff is Hediff_MissingPart hediff_MissingPart)
                            {
                                if (!__instance.Props.bodyPartsToRestore.Contains(hediff.Part.def))
                                {
                                    if (!ParentIsMissing(pawn, hediff))
                                    {
                                        tmpCanRegenerateHediffs.Add(HediffLabel(hediff));
                                    }
                                }
                            }
                        }
                    }

                    // if there is only one "can heal" list
                    if (___tmpCanHealHediffs.Count == 1)
                    {
                        // add "can heal" to "will heal" as there is only one option to heal
                        ___tmpWillHealHediffs.AddRange(___tmpCanHealHediffs);
                        // clear "can heal" to avoid confusion below
                        ___tmpCanHealHediffs.Clear();
                    }

                    string text2 = string.Empty;
                    // check if we are healing all conditions, or just one (default)
                    if (BattIePatchEnhancedBioregenerationSettings.HealAllConditions == true)
                    {
                        // if "will heal" has values, print "will heal" with list  values
                        if (___tmpWillHealHediffs.Any())
                        {
                            text2 += "HealingCycleWillHeal".Translate() + ":\n" + ___tmpWillHealHediffs.ToLineList("  - ", capitalizeItems: true);
                        }
                        // if "can heal" has values
                        if (___tmpCanHealHediffs.Any())
                        {
                            // if there is not already text
                            if (text2.NullOrEmpty())
                            {
                                // add intro text
                                text2 += "HealingCycleWillHeal".Translate() + ":";
                            }
                            // print "will heal" with list  values
                            text2 += "\n" + ___tmpCanHealHediffs.ToLineList("  - ", capitalizeItems: true);
                        }

                        /// run lesser restoration check
                        if (BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration == true && tmpCanRegenerateHediffs.Count > 0)
                        {
                            // if there is already text
                            if (!text2.NullOrEmpty())
                            {
                                //add seperator
                                text2 += "\n\n";
                            }
                            // print "can regenerate" text with "can regenerate" list
                            if (tmpCanRegenerateHediffs.Count > 1)
                            {
                                text2 += "One of the following parts will be regenerated:\n" + tmpCanRegenerateHediffs.ToLineList("  - ", capitalizeItems: true);
                            }
                            else
                            {
                                text2 += "The following part will be regenerated:\n" + tmpCanRegenerateHediffs.ToLineList("  - ", capitalizeItems: true);
                            }
                        }
                    }
                    else
                    {
                        // if "will heal" contains values
                        if (___tmpWillHealHediffs.Any())
                        {
                            // print "will heal" text with "will heal" list
                            text2 += "HealingCycleWillHeal".Translate() + ":\n" + ___tmpWillHealHediffs.ToLineList("  - ", capitalizeItems: true);
                        }

                        // if "can heal" contains values
                        if (___tmpCanHealHediffs.Any())
                        {
                            // if there is already text
                            if (!text2.NullOrEmpty())
                            {
                                //add seperator
                                text2 += "\n\n";
                            }
                            // print "can heal" text with "can heal" list
                            text2 += "HealingCycleOneWillHeal".Translate() + ":\n" + ___tmpCanHealHediffs.ToLineList("  - ", capitalizeItems: true);
                        }

                        /// run lesser restoration check
                        if (BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration == true && tmpCanRegenerateHediffs.Count > 0)
                        {
                            // if there is already text
                            if (!text2.NullOrEmpty())
                            {
                                //add seperator
                                text2 += "\n\n";
                            }
                            // print "can regenerate" text with "can regenerate" list
                            text2 += "One of the following parts will be regenerated:\n" + tmpCanRegenerateHediffs.ToLineList("  - ", capitalizeItems: true);
                        }
                    }

                    // if there is text
                    if (!text2.NullOrEmpty())
                    {
                        // set text
                        text += text2;
                    }
                }

                return text;
            }
            finally
            {
                // clear temp lists
                ___tmpWillHealHediffs.Clear();
                ___tmpCanHealHediffs.Clear();
                tmpCanRegenerateHediffs.Clear();
            }

            /// create a lable (used above)
            static string HediffLabel(Hediff hediff)
            {
                // first make sure the hediff has a part (not sure about last part, see base function)
                if (hediff.Part != null && !hediff.def.cureAllAtOnceIfCuredByItem)
                {
                    // return the lable including the part
                    return hediff.Part.Label + " (" + hediff.Label + ")";
                }

                // if the hediff doesn't have a part
                // return the label
                return hediff.Label;
            }
        }

        /// runs after the cycle is completed, determines which conditions to heal
        public static void CycleCompleted(CompBiosculpterPod_HealingCycle __instance, Pawn pawn, List<Hediff> ___tmpHediffs)
        {
            if (pawn.health == null)
            {
                return;
            }

            ___tmpHediffs.Clear();
            ___tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
            try
            {
                // goes over each heddif
                foreach (Hediff tmpHediff in ___tmpHediffs)
                {
                    // runs the "WillHeal" function for that hediff
                    if (WillHeal(pawn, tmpHediff, __instance.Regenerate, __instance.Props))
                    {
                        // cures that hediff if true
                        HealthUtility.Cure(tmpHediff);
                    }
                    // checks if that hediff is a fresh missing part
                    else if (tmpHediff is Hediff_MissingPart hediff_MissingPart && hediff_MissingPart.IsFresh)
                    {
                        // sets fresh to false
                        hediff_MissingPart.IsFresh = false;
                        // notifies pawn to redraw
                        pawn.health.Notify_HediffChanged(hediff_MissingPart);
                    }

                    // if internal setting of HealAllConditions
                    if (BattIePatchEnhancedBioregenerationSettings.HealAllConditions == true)
                    {
                        // also runs the "CanHeal" function for that hediff
                        if (CanPotentiallyHeal(pawn, tmpHediff, __instance.Regenerate, __instance.Props))
                        {
                            // cures that hediff if true
                            HealthUtility.Cure(tmpHediff);
                        }
                    }
                }

                // clears all tmp values
                ___tmpHediffs.Clear();

                // if internal setting of HealAllConditions == false
                if (BattIePatchEnhancedBioregenerationSettings.HealAllConditions == false)
                {
                    // go over all hediffs
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                    {
                        // run the "CanHeal" function for that hediff
                        if (CanPotentiallyHeal(pawn, hediff, __instance.Regenerate, __instance.Props))
                        {
                            // add that hediff to temp list if true
                            ___tmpHediffs.Add(hediff);
                        }
                    }

                    // selects one random element from the list (if there are any)
                    if (___tmpHediffs.TryRandomElement(out var result))
                    {
                        // cures ones condition
                        HealthUtility.Cure(result);
                    }
                }
                // clears all tmp values
                ___tmpHediffs.Clear();
                // if internal setting of LessenedRegeneration == false
                if (BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration == true)
                {
                    // go over all hediffs
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                    {
                        // if hediff is a missing part
                        if (hediff is Hediff_MissingPart hediff_MissingPart)
                        {
                            // if it is NOT a part that can usually be restored
                            if (!__instance.Props.bodyPartsToRestore.Contains(hediff.Part.def))
                            {
                                // AND if it's parent exists
                                if (!ParentIsMissing(pawn, hediff))
                                {
                                    // add that hediff to temp list
                                    ___tmpHediffs.Add(hediff);
                                }
                            }
                        }
                    }

                    // selects one random element from the list (if there are any)
                    if (___tmpHediffs.TryRandomElement(out var result))
                    {
                        // create an empty list that will hold a reference to cured parts
                        HashSet<BodyPartRecord> cured = new HashSet<BodyPartRecord>();
                        // creates a queue of hediffs that need to be cured
                        Queue<Hediff> toCure = new Queue<Hediff>();
                        // adds the random result to the queue
                        toCure.Enqueue(result);

                        // while there is anything in queue
                        while (toCure.Count > 0)
                        {
                            // dequeue from toCure but keep a local reference
                            Hediff current = toCure.Dequeue();
                            // cure reference
                            HealthUtility.Cure(current);
                            // if it's a missing part (which it should alwys be)
                            if (current is Hediff_MissingPart current_MissingPart)
                            {
                                // if it's fresh
                                if (current_MissingPart.IsFresh)
                                {

                                    // sets fresh to false
                                    current_MissingPart.IsFresh = false;
                                    // notifies pawn to redraw
                                    pawn.health.Notify_HediffChanged(current_MissingPart);
                                }
                            }
                            // add reference to the cured part to the HashSet
                            cured.Add(current.Part);

                            //goes over each hediff
                            foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                            {
                                // if that hediff is a missing part
                                if (hediff is Hediff_MissingPart hediff_MissingPart)
                                {
                                    // if HashSet contains reference to the parent of that hediff
                                    if (cured.Contains(hediff.Part.parent))
                                    {
                                        // enqueue that hediff to be cured
                                        toCure.Enqueue(hediff);
                                    }
                                }
                            }
                        }
                    }
                }
                // print that cycle was completed
                Messages.Message("BiosculpterHealCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            finally
            {
                // clear temp list
                ___tmpHediffs.Clear();
            }
        }

        /// determines if a specific hediff will always be healed by the cycle
        static bool WillHeal(Pawn pawn, Hediff hediff, bool regenerate, CompProperties_BiosculpterPod_HealingCycle props)
        {
            // if hediff can't be cured
            if (!hediff.def.everCurableByItem)
            {
                // don't heal
                return false;
            }

            // if hediff is chronic
            if (hediff.def.chronic)
            {
                // don't heal
                return false;
            }

            // if part is artificial or otherwise added
            if (hediff.def.countsAsAddedPartOrImplant)
            {
                // don't heal
                return false;
            }

            // if hediff is an injury that is bleeding
            if (hediff.def == HediffDefOf.BloodLoss)
            {
                // heal
                return true;
            }

            // Never remove psylink
            if(hediff.def.defName == "PsychicAmplifier" || hediff.def.defName == "VPE_PsycastAbilityImplant")
            {
                // heal
                return false;
            }

            // if hedif is a ritual scarification
            // and the pawn's ideology likes scarifications
            // and KeepAllScars or KeepMinorScars is ticked
            if (hediff.def.defName == "Scarification" && pawn.ideo.Ideo.RequiredScars > 0)
            {
                // don't heal
                return false;
            }

            // if hediff is a permenant injury
            // and LessenedRegeneration is ticked
            // and the props doesn't specifically contain this hediff
            if (hediff is Hediff_Injury && hediff.IsPermanent() && BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration && props.conditionsToPossiblyCure.Contains(hediff.def) == false)
            {
                // that means it's a scar
                // if KeepAllScars is ticked
                // and the pawn's ideology likes scarifications
                if (BattIePatchEnhancedBioregenerationSettings.KeepAllScars && pawn.ideo.Ideo.RequiredScars > 0)
                {
                    // don't heal
                    return false;
                }
                // if KeepMinorScars is ticked
                // and the pawn's ideology likes scarifications
                if (BattIePatchEnhancedBioregenerationSettings.KeepMinorScars && pawn.ideo.Ideo.RequiredScars > 0)
                {
                    // if scar is not too damaging (severe)
                    // and either or both:
                    //  pawn is a masochist
                    //  pain for scar is minor
                    if (hediff.Severity <= 3 && (pawn.story.traits.GetTrait(TraitDef.Named("Masochist")) != null || hediff.PainOffset == 0))
                    {
                        // don't heal
                        return false;
                    }
                }
                // otherwise scar is not needed/wanted
                // heal
                return true;
            }

            // if regenerate is true
            // and hediff has part
            // and bodyPartsToRestore exists and contains the part
            // and that part is not an artificial
            if (regenerate && hediff.Part != null && props.bodyPartsToRestore != null && props.bodyPartsToRestore.Contains(hediff.Part.def) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff.Part))
            {
                // if that part has a parent
                // and that parent is missing
                if (hediff.Part.parent != null && ParentIsMissing(pawn, hediff))
                {
                    // if bodyPartsToRestore doesn't contain the parnet part
                    if (props.bodyPartsToRestore.Contains(hediff.Part.parent.def) == false)
                    {
                        // don't heal
                        return false;
                    }
                }

                // if part is an eye
                // if pawn has an ideology
                // if ideology likes blindness
                if (hediff.Part.def == BodyPartDefOf.Eye && pawn.Ideo != null && pawn.Ideo.IdeoApprovesOfBlindness())
                {
                    // don't heal
                    return false;
                }

                // heal
                return true;
            }

            // if hediff is an injury
            // and not permanent
            if (hediff is Hediff_Injury && !hediff.IsPermanent())
            {
                // heal
                return true;
            }

            // if conditionsToPossiblyCure exists
            // and contains hediff
            if (props.conditionsToPossiblyCure != null && props.conditionsToPossiblyCure.Contains(hediff.def))
            {
                // heal
                return true;
            }

            // if none of the above
            // don't heal
            return false;
        }

        /// determines if the hediff CAN be healed by the cycle, though only one will be picked
        static bool CanPotentiallyHeal(Pawn pawn, Hediff hediff, bool regenerate, CompProperties_BiosculpterPod_HealingCycle props)
        {
            // if hediff can't be cured
            if (!hediff.def.everCurableByItem)
            {
                // don't heal
                return false;
            }

            // if part is artificial or otherwise added
            if (hediff.def.countsAsAddedPartOrImplant)
            {
                // don't heal
                return false;
            }


            // if hediff is a permenant injury
            // and LessenedRegeneration is NOT ticked
            // and the props doesn't specifically contain this hediff
            if (regenerate && hediff is Hediff_Injury && hediff.IsPermanent() && !BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration && props.conditionsToPossiblyCure.Contains(hediff.def) == false)
            {
                // that means it's a scar
                // if KeepAllScars is ticked
                // and the pawn's ideology likes scarifications
                if (BattIePatchEnhancedBioregenerationSettings.KeepAllScars && pawn.ideo.Ideo.RequiredScars > 0)
                {
                    // don't heal
                    return false;
                }
                // if KeepMinorScars is ticked
                // and the pawn's ideology likes scarifications
                if (BattIePatchEnhancedBioregenerationSettings.KeepMinorScars && pawn.ideo.Ideo.RequiredScars > 0)
                {
                    // if scar is not too damaging (severe)
                    // and either or both:
                    //  pawn is a masochist
                    //  pain for scar is minor
                    if (hediff.Severity <= 3 && (pawn.story.traits.GetTrait(TraitDef.Named("Masochist")) != null || hediff.PainOffset == 0))
                    {
                        // don't heal
                        return false;
                    }
                }
                // otherwise scar is not needed/wanted, so heal
                return true;
            }


            // if conditionsToPossiblyCure exists
            // and contains hediff
            if (props.conditionsToPossiblyCure != null && props.conditionsToPossiblyCure.Contains(hediff.def))
            {
                // heal
                return true;
            }

            // if none of the above
            // don't heal
            return false;
        }

        /// static check for determining if a part has a missing parent part
        static bool ParentIsMissing(Pawn pawn, Hediff hediff)
        {
            // go over each hediff
            for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                // if that hediff is missing
                // and the parent of the part
                if (pawn.health.hediffSet.hediffs[i] is Hediff_MissingPart hediff_MissingPart && hediff_MissingPart.Part == hediff.Part.parent)
                {
                    // parent is missing
                    return true;
                }
            }
            // parent is not missing
            return false;
        }
    }
}