﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BattIePatch_Lockdown
{
    public class CompProperties_Sealable : CompProperties
    {
        public CompProperties_Sealable()
        {
            compClass = typeof(CompSealable);
        }
    }
}
