using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using RimWorld;
using Verse;

namespace Celestials
{
    [StaticConstructorOnStartup]
    public static class CelestialsStartup
    {
        static CelestialsStartup()
        {
            TrackNewRaceDefs();
            PatchDefs();
        }

        public static void TrackNewRaceDefs()
        {
            if (CelestialsMod.settings.allowedRaceDefs.NullOrEmpty())
            {
                CelestialsMod.settings.allowedRaceDefs = new Dictionary<string, bool>();
            }
            foreach(ThingDef race in DefDatabase<ThingDef>.AllDefs)
            {
                if(race.IsLivingCreature())
                {
                    if (!CelestialsMod.settings.allowedRaceDefs.ContainsKey(race.defName))
                    {
                        CelestialsMod.settings.allowedRaceDefs.Add(race.defName, race.defName == "Human");
                    }
                }
            }
        }

        public static void PatchDefs()
        {
            if (Prefs.DevMode)
            {
                Log.Message($":: Celestials :: Patching... ::");
            }

            StringBuilder patchedRaces = new StringBuilder();
            patchedRaces.AppendLine(":: Celestials :: Patched Races ::");
            foreach(ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (thingDef.IsLivingCreature())
                {
                    if (CelestialsMod.settings.allowedRaceDefs[thingDef.defName])
                    {
                        if (thingDef.comps == null)
                        {
                            thingDef.comps = new List<CompProperties>();
                        }
                        thingDef.comps.Add(new CompProperties_Celestials());

                        if (Prefs.DevMode)
                        {
                            patchedRaces.AppendLine($"=> {thingDef.LabelCap} ({thingDef.defName})");
                        }
                    }
                }
            }
            Log.Message(patchedRaces);
        }
    }
}
