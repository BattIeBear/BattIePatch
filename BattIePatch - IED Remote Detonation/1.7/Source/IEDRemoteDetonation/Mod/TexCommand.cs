using UnityEngine;
using Verse;

namespace BattIePatch_IEDRemoteDetonation
{
    [StaticConstructorOnStartup]
    public static class TexCommand
    {
        public static readonly Texture2D Detonate = ContentFinder<Texture2D>.Get("UI/Commands/Detonate");
    }
}
