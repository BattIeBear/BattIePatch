using System.Collections.Generic;
using Verse;

namespace XenotypeBodyPuristFix
{
    [StaticConstructorOnStartup]
    public static class XenotypeUtils
    {
        public static HashSet<string> customXenotypesLoaded = new HashSet<string>();
        static XenotypeUtils()
        {
            foreach(var file in GenFilePaths.AllCustomXenotypeFiles)
            {
                string xenoName = file.Name;
                xenoName = xenoName.Substring(0, xenoName.Length - 4);
                customXenotypesLoaded.Add(xenoName);
            }
        }
    }
}
