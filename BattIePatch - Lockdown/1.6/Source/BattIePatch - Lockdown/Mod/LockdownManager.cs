using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BattIePatch_Lockdown
{
    [StaticConstructorOnStartup]
    public static class LockdownManager
    {
        private static Dictionary<Map, List<CompSealable>> AllSealableOnMap = new Dictionary<Map, List<CompSealable>>();
        private static Dictionary<Map, Dictionary<CompSealable.Sector, bool>> QuerySectorSealed = new Dictionary<Map, Dictionary<CompSealable.Sector, bool>>();

        public static void AddNewSealableComp(Map map, CompSealable comp)
        {
            //If map is not saved, add it to the dictionary
            if (AllSealableOnMap.ContainsKey(map) == false)
            {
                AllSealableOnMap.Add(map, new List<CompSealable>() { comp });
                QuerySectorSealed.Add(map, new Dictionary<CompSealable.Sector, bool>());
                QuerySectorSealed[map].Add(comp.sector, comp.isSealed);
                return;
            }
            if (AllSealableOnMap[map].Contains(comp))
            {
                Log.Error("Trying to add " + comp.parent.def.defName + " twice!");
                return;
            }
            //if map is saved, add the comp to the list
            AllSealableOnMap[map].Add(comp);
            //then check if the sector is saved, if not, add it
            if (QuerySectorSealed[map].ContainsKey(comp.sector) == false)
            {
                QuerySectorSealed[map].Add(comp.sector, comp.isSealed);
                return;
            }
            //if sector is saved, set the comp to the saved value
            if (comp.isSealed != QuerySectorSealed[map][comp.sector])
            {
                comp.isSealed = QuerySectorSealed[map][comp.sector];
            }
        }

        public static void RemoveSealableComp(Map map, CompSealable comp)
        {
            if (AllSealableOnMap.ContainsKey(map) == false)
            {
                Log.Error("Trying to remove " + comp.parent.def.defName + ", but it's map has not been saved!");
                return;
            }
            if (AllSealableOnMap[map].Contains(comp) == false)
            {
                Log.Error("Trying to remove " + comp.parent.def.defName + ", but it was never added!");
                return;
            }
            AllSealableOnMap[map].Remove(comp);
            if (AllSealableOnMap[map].Count == 0)
            {
                AllSealableOnMap.Remove(map);
                QuerySectorSealed.Remove(map);
            }
            else
            {
                for (int i = 0; i < AllSealableOnMap[map].Count; i++)
                {
                    if (AllSealableOnMap[map][i].sector == comp.sector)
                    {
                        return;
                    }
                }
                if (QuerySectorSealed[map].ContainsKey(comp.sector))
                {
                    QuerySectorSealed[map][comp.sector] = false;
                }
            }
        }

        public static void SetCompNewSector(Map map, CompSealable comp, CompSealable.Sector prevSector)
        {
            if (AllSealableOnMap.ContainsKey(map) == false)
            {
                Log.Error("Trying to change sector of " + comp.parent.def.defName + ", but it's map has not been saved!");
                return;
            }
            if (QuerySectorSealed[map].ContainsKey(prevSector) == false)
            {
                Log.Error("Trying to change sector of " + comp.parent.def.defName + ", but it's previous sector was not saved!");
                return;
            }
            if (AllSealableOnMap[map].Contains(comp) == false)
            {
                Log.Error("Trying to change sector of " + comp.parent.def.defName + ", but it was never added!");
                return;
            }
            if (QuerySectorSealed[map].ContainsKey(comp.sector) == false)
            {
                QuerySectorSealed[map].Add(comp.sector, false);
            }
            if (comp.isSealed != QuerySectorSealed[map][comp.sector])
            {
                comp.isSealed = QuerySectorSealed[map][comp.sector];
            }

            for (int i = 0; i < AllSealableOnMap[map].Count; i++)
            {
                if (AllSealableOnMap[map][i].sector == prevSector)
                {
                    return;
                }
            }
            QuerySectorSealed[map][prevSector] = false;

        }

        public static void LockdownSector(Map map, CompSealable.Sector sector, bool locked)
        {
            if (AllSealableOnMap.ContainsKey(map) == false)
            {
                Log.Error("Trying to lockdown " + map.ToString() + ", but it was not saved!");
                return;
            }
            if (QuerySectorSealed[map].ContainsKey(sector) == false)
            {
                Log.Error("Trying to lockdown " + sector.ToString() + ", but it was not saved!");
                return;
            }

            QuerySectorSealed[map][sector] = locked;
            for (int i = 0; i < AllSealableOnMap[map].Count; i++)
            {
                if (AllSealableOnMap[map][i].sector == sector)
                {
                    if (AllSealableOnMap[map][i].isSealed != locked)
                    {
                        AllSealableOnMap[map][i].isSealed = locked;
                    }
                }
            }
        }

        public static void LockdownMap(Map map, bool locked)
        {
            if (AllSealableOnMap.ContainsKey(map) == false)
            {
                Log.Error("Trying to lockdown " + map.ToString() + ", but it was not saved!");
                return;
            }

            if(QuerySectorSealed[map].ContainsKey(CompSealable.Sector.Red))
            {
                QuerySectorSealed[map][CompSealable.Sector.Red] = locked;
            }
            if (QuerySectorSealed[map].ContainsKey(CompSealable.Sector.Blue))
            {
                QuerySectorSealed[map][CompSealable.Sector.Blue] = locked;
            }
            if (QuerySectorSealed[map].ContainsKey(CompSealable.Sector.Yellow))
            {
                QuerySectorSealed[map][CompSealable.Sector.Yellow] = locked;
            }

            for (int i = 0; i < AllSealableOnMap[map].Count; i++)
            {
                if (AllSealableOnMap[map][i].isSealed != locked)
                {
                    AllSealableOnMap[map][i].isSealed = locked;
                }
            }
        }
    }
}
