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
    public class HediffCompProperties_MakeCelestial : HediffCompProperties
    {
        public HediffCompProperties_MakeCelestial()
        {
            compClass = typeof(HediffComp_MakeCelestial);
        }
    }
}
