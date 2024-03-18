using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Celestials
{
    [DefOf]
    public static class CelestialsDefOf
    {
        static CelestialsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(CelestialsDefOf));
        }

        public static HediffDef CEL_CelestialHediff;

        public static IncidentDef CEL_CelestialResurrection;
        public static IncidentDef CEL_CelestialMeteor;

        public static ThingSetMakerDef CEL_MeteorRocks;

        public static ThingDef CEL_CelestialOre;
    }
}
