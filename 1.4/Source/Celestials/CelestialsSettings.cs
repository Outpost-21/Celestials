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
    public class CelestialsSettings : ModSettings
    {
        public bool empOnDeath = true;

        public float celestialTickRate = 60f;

        public bool resurrectionSideEffects = true;

        public IntRange resurrectionTicks = new IntRange(30000, 60000);

        public float celestialChance = 0.01f;

        public float chanceOfCelestial = 1f;

        public float chanceOfMeteor = 0.01f;

        public bool allowOnHumanlikes = true;

        public bool allowOnAnimals = false;

        public List<string> disallowedFleshTypes = new List<string>() { "Mechanoid", "Asimov_Artificial" };

        public int CelestialTickRate => Mathf.RoundToInt(CelestialTickRate);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref empOnDeath, "empOnDeath", true);
            Scribe_Values.Look(ref celestialTickRate, "celestialTickRate", 60f);
            Scribe_Values.Look(ref resurrectionSideEffects, "resurrectionSideEffects", true);
            Scribe_Values.Look(ref resurrectionTicks, "resurrectionTicks", new IntRange(30000, 60000));
            Scribe_Values.Look(ref celestialChance, "celestialChance", 0.01f);
            Scribe_Values.Look(ref chanceOfMeteor, "chanceOfMeteor", 0.01f);
            Scribe_Values.Look(ref allowOnHumanlikes, "allowOnHumanlikes", true);
            Scribe_Values.Look(ref allowOnAnimals, "allowOnAnimals", false);
        }
    }
}
