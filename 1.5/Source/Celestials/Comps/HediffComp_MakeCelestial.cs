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
    public class HediffComp_MakeCelestial : HediffComp
    {
        public HediffCompProperties_MakeCelestial Props => (HediffCompProperties_MakeCelestial)props;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            if(parent.pawn != null)
            {
                Comp_Celestials comp = parent.pawn.TryGetComp<Comp_Celestials>();
                if(comp != null)
                {
                    comp.isCelestial = true;
                }
            }
        }
    }
}
