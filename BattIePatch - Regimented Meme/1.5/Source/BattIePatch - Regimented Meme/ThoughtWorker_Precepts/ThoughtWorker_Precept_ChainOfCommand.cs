using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BattIePatch_RegimentedMeme
{

    public class ThoughtWorker_Precept_ChainOfCommand : ThoughtWorker_Precept
    {

        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (GenDate.DaysPassed < 10)
            {
                return false;
            }

            if (p == null)
            {
                return false;
            }

            if (p.IsPrisoner || p.IsSlave)
            {
                return false;
            }

            bool leader_role_filled = false;
            foreach (Precept_Role role in p.Ideo.cachedPossibleRoles)
            {
                if (role.def.leaderRole)
                {
                    leader_role_filled = role.Active && p.Ideo == role.ideo && role.ChosenPawnSingle() == null;
                }
            }

            return ThoughtState.ActiveAtStage(Convert.ToInt32(leader_role_filled));
        }
    }
}
