using HarmonyLib;
using RimWorld;

namespace BattIePatch_RegimentedMeme
{
    [HarmonyPatch(typeof(Thought_IdeoRoleEmpty), "CurrentStateInternal")]
    public class Thought_IdeoRoleEmpty_CurrentStateInternal_Patch
    {
        public static ThoughtState Postfix(ThoughtState __result, Thought_IdeoRoleEmpty __instance)
        {
            if (__result.Active)
            {
                if (__instance.Role.ideo.GetPrecept(PreceptDefOf.BattIePatch_ChainOfCommand_Mandatory) != null)
                {
                    __result = ThoughtState.Inactive;
                }
            }

            return __result;
        }
    }
}