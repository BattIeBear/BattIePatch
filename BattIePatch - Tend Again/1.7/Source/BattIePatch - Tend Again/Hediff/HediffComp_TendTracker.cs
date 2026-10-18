using Verse;

namespace BattIePatch_TendAgain
{
    public class HediffComp_TendTracker : HediffComp
    {
        public float StoredTendPotential = -1f;
        public bool HasBeenTended { get { return StoredTendPotential >= 0f; } }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref StoredTendPotential, "StoredTendPotential", -1f);
        }

        public bool CanImprove(float currentPotential)
        {
            return currentPotential > StoredTendPotential;
        }

        public void UpdateStoredTendPotential(float newPotential)
        {
            StoredTendPotential = newPotential;
        }
    }
}
