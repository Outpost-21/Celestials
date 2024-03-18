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
    public class Hediff_Celestial : HediffWithComps
    {
        public int resurrectionTick = -1;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref resurrectionTick, "resurrectionTick", -1);
        }

        public override void Tick()
        {
            base.Tick();
            if(resurrectionTick > Find.TickManager.TicksGame)
            {
                resurrectionTick = -1;
            }
        }

        public void SetResurrectionTick()
        {
            resurrectionTick = Find.TickManager.TicksGame + CelestialsMod.settings.resurrectionTicks.RandomInRange;
        }
    }
}
