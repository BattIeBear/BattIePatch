using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;


namespace BattIePatch_IEDRemoteDetonation
{
    public class CompRemoteTrigger : ThingComp
    {
        public CompProperties_RemoteTrigger Props => (CompProperties_RemoteTrigger)props;

        public CompExplosive ExplosiveComp
        {
            get
            {
                if (parent.TryGetComp<CompExplosive>() is CompExplosive comp)
                {
                    return comp;
                }
                Log.Error(parent.def.defName + " has CompRemoteTrigger but no CompExplosive.");
                return null;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (ExplosiveComp != null && BattIePatchIEDRemoteDetonationSettings.DraftedDetonation == false)
            {
                yield return new Command_Action
                {
                    defaultLabel = "BattIePatch_IEDRemoteDetonation_Detonate".Translate(),
                    defaultDesc = "BattIePatch_IEDRemoteDetonation_DetonateDesc".Translate(),
                    icon = TexCommand.Detonate,
                    action = delegate
                    {
                        ExplosiveComp.StartWick();
                    }
                };
            }
            yield break;
        }

    }
}
