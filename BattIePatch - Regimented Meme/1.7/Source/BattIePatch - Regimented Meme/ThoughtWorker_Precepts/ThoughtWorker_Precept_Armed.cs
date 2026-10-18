using RimWorld;
using System;
using Verse;

namespace BattIePatch_RegimentedMeme
{
    public class ThoughtWorker_Precept_Armed : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p == null || HasWeapon(p) || p.ageTracker.AgeBiologicalYears < 16)
            {
                return false;
            }

            return ThoughtState.ActiveAtStage(Convert.ToInt32(p.WorkTagIsDisabled(WorkTags.Violent)));
        }

        public static bool HasWeapon(Pawn p)
        {
            if (p == null || p.equipment == null)
            {
                return false;
            }

            var primary = p.equipment.Primary;
            return primary != null && primary.def.IsWeapon;
        }
    }
}
