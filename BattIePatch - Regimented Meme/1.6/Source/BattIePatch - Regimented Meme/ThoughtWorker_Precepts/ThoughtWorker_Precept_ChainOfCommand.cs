using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BattIePatch_RegimentedMeme
{
    public class ThoughtWorker_Precept_ChainOfCommand : ThoughtWorker_Precept
    {
        private int storedMaxRoles = -1;

        private int maxRoles(Pawn p)
        {
            if (storedMaxRoles == -1)
            {
                storedMaxRoles = 1;
                storedIsPrimaryIdeo = p.ideo.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo;
                foreach (Precept precept in p.ideo.Ideo.PreceptsListForReading)
                {
                    if (precept is Precept_Role precept_Role && precept_Role.Active && precept_Role.def.leaderRole == false)
                    {
                        storedMaxRoles++;
                    }
                }
            }
            return storedMaxRoles;

        }

        int savedStage = -1;
        int iterations = 1;

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

            if (savedStage != -1 && GenTicks.TicksGame % 2500 != 0)
            {
                return ThoughtState.ActiveAtStage(savedStage);
            }

            if (!RolesFilled(p, out int filledRoles))
            {
                savedStage = 0;
                return ThoughtState.ActiveAtStage(0);
            }

            int stage = 0;
            float filledPercent = (float)filledRoles / (float)maxRoles(p);

            // Get the number of free colonists in the colony
            if (filledPercent >= 1.0f)
            {
                // All roles filled
                stage = 0;
            }
            else if (filledPercent >= 0.660f)
            {
                // ~2/3 of roles filled
                stage = 1;
            }
            else if (filledPercent >= 0.330f)
            {
                // ~1/3 of roles filled
                stage = 2;
            }
            else if (filledPercent > 0.000f)
            {
                // At least one role filled
                stage = 3;
            }
            else
            {
                // No roles filled
                stage = 4;
            }

            if (stage > 0 && stage < 4 && filledRoles >= p.Map.mapPawns.ColonistCount / 3.0f)
            {
                // At least 1/3 of colonists have a role, but not all roles are filled
                stage = 5;
            }
            savedStage = stage;
            return ThoughtState.ActiveAtStage(stage);
        }

        bool storedIsPrimaryIdeo = false;
        List<Precept_Role> totalRoles = new List<Precept_Role>();
        public bool RolesFilled(Pawn p, out int filledRolesCount)
        {
            List<Precept_Role> filledRoles = new List<Precept_Role>();

            if (storedIsPrimaryIdeo != (p.ideo.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo))
            {
                storedIsPrimaryIdeo = p.ideo.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo;
                totalRoles.Clear();
            }

            if (totalRoles.Count == 0)
            {
                switch (storedIsPrimaryIdeo)
                {
                    case true:
                        foreach (Precept precept in p.ideo.Ideo.PreceptsListForReading)
                        {
                            if (precept is Precept_Role precept_Role && precept_Role.Active)
                            {
                                totalRoles.Add(precept_Role);
                            }
                        }
                        break;
                    case false:

                        foreach (Precept precept in p.ideo.Ideo.PreceptsListForReading)
                        {
                            if (precept is Precept_Role precept_Role && precept_Role.def.leaderRole == false && precept_Role.Active)
                            {
                                totalRoles.Add(precept_Role);
                            }
                        }

                        foreach (Precept precept in Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading)
                        {
                            if (precept is Precept_Role precept_Role && precept_Role.def.leaderRole == true && precept_Role.Active)
                            {
                                totalRoles.Add(precept_Role);
                            }
                        }
                        break;
                }
            }

            for (int i = 0; i < totalRoles.Count; i++)
            {
                Precept_Role precept_Role = totalRoles[i];
                if (precept_Role.ChosenPawns().Any())
                {
                    filledRoles.Add(precept_Role);
                }
            }

            filledRolesCount = filledRoles.Count;
            return filledRoles.Any();
        }
    }
}
