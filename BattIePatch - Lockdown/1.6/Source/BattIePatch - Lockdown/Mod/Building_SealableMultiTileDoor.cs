﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace BattIePatch_Lockdown
{
    public class Building_SealableMultiTileDoor : Building_MultiTileDoor
    {
        public CompSealable sealableComp;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            sealableComp = GetComp<CompSealable>();
        }

        public override bool PawnCanOpen(Pawn p)
        {
            if (sealableComp.isSealed && powerComp.PowerOn)
            {
                return false;
            }
            return base.PawnCanOpen(p);
        }

        private Vector3 cachedLightDrawPos = Vector3.zero;
        private Vector3 LightDrawPos
        {
            get
            {
                if (cachedLightDrawPos != Vector3.zero)
                {
                    return cachedLightDrawPos;
                }

                if(sealableComp.parent.def.size == new IntVec2(3, 1))
                {
                    switch (Rotation.AsInt)
                    {
                        case 0:
                            cachedLightDrawPos = new Vector3(1.1f, 6f, .65f);
                            break;
                        case 1:
                            cachedLightDrawPos = new Vector3(0f, 6f, 1.2f);
                            break;
                        case 2:
                            cachedLightDrawPos = new Vector3(1.1f, 6f, .65f);
                            break;
                        case 3:
                            cachedLightDrawPos = new Vector3(0f, 6f, 1.2f);
                            break;
                    }
                }
                else
                {
                    switch (Rotation.AsInt)
                    {
                        case 0:
                            cachedLightDrawPos = new Vector3(.6f, 6f, .65f);
                            break;
                        case 1:
                            cachedLightDrawPos = new Vector3(0f, 6f, 1.2f);
                            break;
                        case 2:
                            cachedLightDrawPos = new Vector3(.6f, 6f, .65f);
                            break;
                        case 3:
                            cachedLightDrawPos = new Vector3(0f, 6f, 1.2f);
                            break;
                    }
                }
                return cachedLightDrawPos;
            }
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            sealableComp.LightGraphic.Draw(((Thing)this).DrawPos + LightDrawPos, Rot4.North, (Thing)(object)this, 0f);
            if (sealableComp.isSealed && powerComp.PowerOn)
            {
                sealableComp.OverlayGraphic.Draw(((Thing)this).DrawPos + new Vector3(0f, 6f, 0f), Rotation, (Thing)(object)this, 0f);
            }

        }

        protected override void Tick()
        {
            base.Tick();
            if (sealableComp.isSealed && powerComp.PowerOn && sealableComp.GlowEffector != null)
            {
                //play the light effect
                sealableComp.GlowEffector.EffectTick(this, this);
            }
            if (sealableComp.isSealed && powerComp.PowerOn && sealableComp.TwinkleEffector != null)
            {
                //play the light effect
                sealableComp.TwinkleEffector.EffectTick(this, this);
            }
        }
    }
}