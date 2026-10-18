using RimWorld;
using Verse;

namespace BattIePatch_IEDRemoteDetonation
{
    [DefOf]
    public static class JobDefOf
    {
        public static JobDef BattIePatch_DetonateIED;

        static JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
        }
    }
}