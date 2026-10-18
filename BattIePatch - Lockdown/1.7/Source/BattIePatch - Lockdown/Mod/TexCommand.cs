using UnityEngine;
using Verse;

namespace BattIePatch_Lockdown
{
    [StaticConstructorOnStartup]
    public static class TexCommand
    {
        public static readonly Texture2D CurSectorRed = ContentFinder<Texture2D>.Get("UI/Commands/CurSectorRed");
        public static readonly Texture2D CurSectorYellow = ContentFinder<Texture2D>.Get("UI/Commands/CurSectorYellow");
        public static readonly Texture2D CurSectorBlue = ContentFinder<Texture2D>.Get("UI/Commands/CurSectorBlue");
        public static readonly Texture2D SealRedSector = ContentFinder<Texture2D>.Get("UI/Commands/SealRedSector");
        public static readonly Texture2D SealYellowSector = ContentFinder<Texture2D>.Get("UI/Commands/SealYellowSector");
        public static readonly Texture2D SealBlueSector = ContentFinder<Texture2D>.Get("UI/Commands/SealBlueSector");
        public static readonly Texture2D UnsealRedSector = ContentFinder<Texture2D>.Get("UI/Commands/UnsealRedSector");
        public static readonly Texture2D UnsealYellowSector = ContentFinder<Texture2D>.Get("UI/Commands/UnsealYellowSector");
        public static readonly Texture2D UnsealBlueSector = ContentFinder<Texture2D>.Get("UI/Commands/UnsealBlueSector");
        public static readonly Texture2D SealMap = ContentFinder<Texture2D>.Get("UI/Commands/SealMap");
        public static readonly Texture2D UnsealMap = ContentFinder<Texture2D>.Get("UI/Commands/UnsealMap");
        public static readonly Texture2D BlankGizmo = ContentFinder<Texture2D>.Get("UI/Icons/BlankGizmo");
    }
}
