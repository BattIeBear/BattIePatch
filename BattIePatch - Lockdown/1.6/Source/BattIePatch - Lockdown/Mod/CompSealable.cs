using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BattIePatch_Lockdown
{
    public class CompSealable : ThingComp
    {
        private bool sealIsActive = false;
        private bool wasHeldOpen = false;
        public CompProperties_Sealable Props => (CompProperties_Sealable)props;

        public enum Sector
        {
            Red, Yellow, Blue
        }

        private Sector cachedSector = Sector.Red;
        public Sector sector
        {
            get
            {
                return cachedSector;
            }
            set
            {
                if (value == cachedSector)
                {
                    return;
                }
                cachedSector = value;
                cachedOverlayGraphic = null;
                cachedLightGraphic = null;
                if (cachedGlowEffector != null)
                {
                    cachedGlowEffector.Cleanup();
                    cachedGlowEffector = null;
                }
                if (cachedTwinkleEffector != null)
                {
                    cachedTwinkleEffector.Cleanup();
                    cachedTwinkleEffector = null;
                }
            }
        }

        public bool isSealed
        {
            get
            {
                return sealIsActive;
            }
            set
            {
                if (value == sealIsActive)
                {
                    return;
                }
                sealIsActive = value;
                if(!value)
                {
                    if (cachedGlowEffector != null)
                    {
                        cachedGlowEffector.Cleanup();
                        cachedGlowEffector = null;
                    }
                    if (cachedTwinkleEffector != null)
                    {
                        cachedTwinkleEffector.Cleanup();
                        cachedTwinkleEffector = null;
                    }
                }
                if (BattIePatchLockdownSettings.OverridesHoldOpen)
                {
                    if (value)
                    {
                        // Check if parent is a door and is held open
                        var door = parent as Building_Door;
                        if (door != null)
                        {
                            if (door.Open)
                            {
                                // Use reflection to set the private 'holdOpenInt' field
                                var holdOpenField = typeof(Building_Door).GetField("holdOpenInt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                if (holdOpenField != null)
                                {
                                    if ((bool)holdOpenField.GetValue(door))
                                    {
                                        holdOpenField.SetValue(door, false);
                                        wasHeldOpen = true;
                                    }
                                }

                                // Use reflection to invoke the private 'DoorTryClose' method
                                var tryCloseMethod = typeof(Building_Door).GetMethod("DoorTryClose", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                if (tryCloseMethod != null)
                                {
                                    tryCloseMethod.Invoke(door, null);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (wasHeldOpen)
                        {
                            // Check if parent is a door and was held open
                            var door = parent as Building_Door;
                            if (door != null)
                            {
                                // Use reflection to set the private 'holdOpenInt' field
                                var holdOpenField = typeof(Building_Door).GetField("holdOpenInt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                if (holdOpenField != null)
                                {
                                    holdOpenField.SetValue(door, true);
                                    wasHeldOpen = false;
                                }
                            }
                        }
                    }
                }
                if (parent.Spawned)
                {
                    parent.Map.reachability.ClearCache();
                }
            }
        }

        public Texture2D gizmoGraphic;

        private Graphic cachedOverlayGraphic = null;
        public Graphic OverlayGraphic
        {
            get
            {
                if (cachedOverlayGraphic == null)
                {
                    switch (sector)
                    {
                        case Sector.Red:
                            cachedOverlayGraphic = GraphicDatabase.Get(typeof(Graphic_Single), "UI/Overlays/WarningOverlayRed", ShaderDatabase.MetaOverlay, new Vector2(1f, 1f), Color.white, Color.white, (string)null);
                            break;
                        case Sector.Yellow:
                            cachedOverlayGraphic = GraphicDatabase.Get(typeof(Graphic_Single), "UI/Overlays/WarningOverlayYellow", ShaderDatabase.MetaOverlay, new Vector2(1f, 1f), Color.white, Color.white, (string)null);
                            break;
                        case Sector.Blue:
                            cachedOverlayGraphic = GraphicDatabase.Get(typeof(Graphic_Single), "UI/Overlays/WarningOverlayBlue", ShaderDatabase.MetaOverlay, new Vector2(1f, 1f), Color.white, Color.white, (string)null);
                            break;
                    }
                }

                return cachedOverlayGraphic;
            }
        }

        private Graphic cachedLightGraphic = null;
        public Graphic LightGraphic
        {
            get
            {
                if (cachedLightGraphic == null)
                {
                    switch (sector)
                    {
                        case Sector.Red:
                            cachedLightGraphic = GraphicDatabase.Get(typeof(Graphic_Single), "Things/Building/SecurityDoor/RedLight", ShaderDatabase.Transparent, new Vector2(.5f, .5f), Color.white, Color.white, (string)null);
                            break;
                        case Sector.Yellow:
                            cachedLightGraphic = GraphicDatabase.Get(typeof(Graphic_Single), "Things/Building/SecurityDoor/YellowLight", ShaderDatabase.Transparent, new Vector2(.5f, .5f), Color.white, Color.white, (string)null);
                            break;
                        case Sector.Blue:
                            cachedLightGraphic = GraphicDatabase.Get(typeof(Graphic_Single), "Things/Building/SecurityDoor/BlueLight", ShaderDatabase.Transparent, new Vector2(.5f, .5f), Color.white, Color.white, (string)null);
                            break;
                    }
                }

                return cachedLightGraphic;
            }
        }
        TickManager tickManager = Find.TickManager;

        private Effecter cachedTwinkleEffector = null;
        public Effecter TwinkleEffector
        {
            get
            {
                if (cachedTwinkleEffector == null && tickManager.TicksGame % 180 == 0)
                {
                    if (parent.Rotation.AsInt == 0 || parent.Rotation.AsInt == 2)
                    {
                        cachedTwinkleEffector = EffecterDefOf.battiepatch_TwinklePulseH.SpawnAttached(parent, parent.Map);
                    }
                    else
                    {
                        cachedTwinkleEffector = EffecterDefOf.battiepatch_TwinklePulseV.SpawnAttached(parent, parent.Map);
                    }
                }

                return cachedTwinkleEffector;
            }
        }
        private Effecter cachedGlowEffector = null;
        public Effecter GlowEffector
        {
            get
            {
                if (cachedGlowEffector == null)
                {
                    if (parent.Rotation.AsInt == 0 || parent.Rotation.AsInt == 2)
                    {
                        switch (sector)
                        {
                            case CompSealable.Sector.Red:
                                cachedGlowEffector = EffecterDefOf.battiepatch_RedAlertPulseH.SpawnAttached(parent, parent.Map);
                                break;
                            case CompSealable.Sector.Blue:
                                cachedGlowEffector = EffecterDefOf.battiepatch_BlueAlertPulseH.SpawnAttached(parent, parent.Map);
                                break;
                            case CompSealable.Sector.Yellow:
                                cachedGlowEffector = EffecterDefOf.battiepatch_YellowAlertPulseH.SpawnAttached(parent, parent.Map);
                                break;
                        }
                    }
                    else
                    {
                        switch (sector)
                        {
                            case CompSealable.Sector.Red:
                                cachedGlowEffector = EffecterDefOf.battiepatch_RedAlertPulseV.SpawnAttached(parent, parent.Map);
                                break;
                            case CompSealable.Sector.Blue:
                                cachedGlowEffector = EffecterDefOf.battiepatch_BlueAlertPulseV.SpawnAttached(parent, parent.Map);
                                break;
                            case CompSealable.Sector.Yellow:
                                cachedGlowEffector = EffecterDefOf.battiepatch_YellowAlertPulseV.SpawnAttached(parent, parent.Map);
                                break;
                        }
                    }
                }

                return cachedGlowEffector;
            }
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            sector = cachedSector;
            gizmoGraphic = TexCommand.BlankGizmo;
            DelayAndRegister();
        }

        async void DelayAndRegister()
        {
            await Task.Delay(100);
            LockdownManager.AddNewSealableComp(parent.Map, this);
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref sealIsActive, "sealIsActive", defaultValue: false);
            Scribe_Values.Look(ref wasHeldOpen, "wasHeldOpen", defaultValue: false);
            Scribe_Values.Look(ref cachedSector, "chachedSector", defaultValue: Sector.Red);
        }

        public override void PostSplitOff(Thing piece)
        {
            piece.SetForbidden(sealIsActive);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (parent.Faction == Faction.OfPlayer)
            {
                //Switch Sector Gizmos
                Command_Action command_SetSector = new Command_Action();
                command_SetSector.defaultLabel = "BattIePatch_Lockdown_SetSector".Translate() + "...";
                command_SetSector.defaultDesc = "BattIePatch_Lockdown_SetSectorDesc".Translate();
                if (sector == Sector.Red)
                {
                    command_SetSector.icon = TexCommand.CurSectorRed;
                }
                else if (sector == Sector.Yellow)
                {
                    command_SetSector.icon = TexCommand.CurSectorYellow;
                }
                else if (sector == Sector.Blue)
                {
                    command_SetSector.icon = TexCommand.CurSectorBlue;
                }
                command_SetSector.action = delegate
                {
                    List<FloatMenuOption> sectorlist = new List<FloatMenuOption>();
                    foreach (Sector newSector in Enum.GetValues(typeof(Sector)).Cast<Sector>())
                    {
                        Color GizmoColor = Color.black;
                        switch (newSector)
                        {
                            case Sector.Red:
                                GizmoColor = new Color(.573f, .106f, .106f, .85f);
                                break;
                            case Sector.Blue:
                                GizmoColor = new Color(.302f, .482f, .671f, .85f);
                                break;
                            case Sector.Yellow:
                                GizmoColor = new Color(.839f, .784f, .180f, .85f);
                                break;
                        }

                        sectorlist.Add(new FloatMenuOption(newSector.ToString() + " Sector", delegate
                        {
                            // Apply to all selected things with CompSealable
                            foreach (var obj in Find.Selector.SelectedObjects)
                            {
                                if (obj is Thing thing)
                                {
                                    var comp = thing.TryGetComp<CompSealable>();
                                    if (comp != null)
                                    {
                                        comp.SetSector(newSector);
                                    }
                                }
                            }
                        }, gizmoGraphic, GizmoColor));

                    }
                    Find.WindowStack.Add(new FloatMenu(sectorlist));
                };

                yield return command_SetSector;

                //Sealable Sector Gizmos
                Command_Toggle seal_Toggle = new Command_Toggle();
                seal_Toggle.isActive = () => !isSealed;
                if(!sealIsActive)
                {
                    if (sector == Sector.Red)
                    {
                        seal_Toggle.icon = TexCommand.SealRedSector;
                    }
                    else if (sector == Sector.Yellow)
                    {
                        seal_Toggle.icon = TexCommand.SealYellowSector;
                    }
                    else if (sector == Sector.Blue)
                    {
                        seal_Toggle.icon = TexCommand.SealBlueSector;
                    }
                    seal_Toggle.defaultLabel = "BattIePatch_Lockdown_SealSector".Translate();
                    seal_Toggle.defaultDesc = "BattIePatch_Lockdown_SealSectorDesc".Translate();
                }
                else
                {
                    if (sector == Sector.Red)
                    {
                        seal_Toggle.icon = TexCommand.UnsealRedSector;
                    }
                    else if (sector == Sector.Yellow)
                    {
                        seal_Toggle.icon = TexCommand.UnsealYellowSector;
                    }
                    else if (sector == Sector.Blue)
                    {
                        seal_Toggle.icon = TexCommand.UnsealBlueSector;
                    }
                    seal_Toggle.defaultLabel = "BattIePatch_Lockdown_UnsealSector".Translate();
                    seal_Toggle.defaultDesc = "BattIePatch_Lockdown_UnsealSectorDesc".Translate();
                }
                seal_Toggle.activateIfAmbiguous = false;
                bool localSealLock = sealIsActive;
                seal_Toggle.toggleAction = delegate
                {
                    if (sealIsActive == localSealLock)
                    {
                        LockdownManager.LockdownSector(parent.Map, sector, !sealIsActive);
                    }
                };
                yield return seal_Toggle;

                //Seal Map Gizmos
                Command_Toggle lockdown_Toggle = new Command_Toggle();
                lockdown_Toggle.isActive = () => !isSealed;
                if (!sealIsActive)
                {
                    lockdown_Toggle.icon = TexCommand.SealMap;
                    lockdown_Toggle.defaultLabel = "BattIePatch_Lockdown_SealMap".Translate();
                    lockdown_Toggle.defaultDesc = "BattIePatch_Lockdown_SealMapDesc".Translate();
                }
                else
                {
                    lockdown_Toggle.icon = TexCommand.UnsealMap;
                    lockdown_Toggle.defaultLabel = "BattIePatch_Lockdown_UnsealMap".Translate();
                    lockdown_Toggle.defaultDesc = "BattIePatch_Lockdown_UnsealMapDesc".Translate();
                }
                lockdown_Toggle.activateIfAmbiguous = false;
                bool mapSealLock = sealIsActive;
                lockdown_Toggle.toggleAction = delegate
                {
                    if (mapSealLock == sealIsActive)
                    {
                        LockdownManager.LockdownMap(parent.Map, !sealIsActive);
                    }
                };
                yield return lockdown_Toggle;
            }
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);
            if (cachedGlowEffector != null)
            {
                cachedGlowEffector.Cleanup();
                cachedGlowEffector = null;
            }

            if (cachedTwinkleEffector != null)
            {
                cachedTwinkleEffector.Cleanup();
                cachedTwinkleEffector = null;
            }
            LockdownManager.RemoveSealableComp(map, this);
        }


        public void SetSector(Sector newSector)
        {
            if (newSector == sector)
            {
                return;
            }
            Sector prevSector = sector;
            sector = newSector;
            LockdownManager.SetCompNewSector(parent.Map, this, prevSector);
        }

        ///Not Needed? Should be handled by PostDeSpawn.
        //public override void PostDestroy(DestroyMode mode, Map previousMap)
        //{
        //    base.PostDestroy(mode, previousMap);
        //    LockdownManager.RemoveSealableComp(previousMap, this);
        //}
    }
}