using RimWorld;
using Verse;

namespace BattIePatch_RegimentedMeme
{
    public class ThoughtWorker_Precept_Armed_Social : ThoughtWorker_Precept_Social
    {
        protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
        {
            if(otherPawn.ageTracker.AgeBiologicalYears < 16 || p.Map != otherPawn.Map || p.Faction != otherPawn.Faction)
            {
                return false;
            }
            return !ThoughtWorker_Precept_Armed.HasWeapon(otherPawn);
        }
    }
}
