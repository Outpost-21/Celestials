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
    public static class CelestialsUtil
    {
        public static bool IsLivingCreature(this ThingDef thing)
        {
            return !thing.IsCorpse && thing.race != null && thing.race.IsFlesh;
        }
    }
}
