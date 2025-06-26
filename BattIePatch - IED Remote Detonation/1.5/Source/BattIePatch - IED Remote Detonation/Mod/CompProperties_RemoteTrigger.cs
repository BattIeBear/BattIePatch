using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BattIePatch_IEDRemoteDetonation
{
    public class CompProperties_RemoteTrigger : CompProperties
    {
        public CompProperties_RemoteTrigger()
        {
            compClass = typeof(CompRemoteTrigger);
        }
    }
}
