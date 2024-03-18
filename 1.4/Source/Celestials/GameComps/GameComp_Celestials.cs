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
    public class GameComp_Celestials : GameComponent
    {
        public bool gameLoaded = false;

        public HashSet<Pawn> celestials = new HashSet<Pawn>();

        public CelestialsSettings settings;

        public HashSet<Pawn> storedCelestials = new HashSet<Pawn>();

        public GameComp_Celestials(Game game)
        {
            this.gameLoaded = false;
            this.settings = CelestialsMod.settings;
        }

        public override void StartedNewGame()
        {
            settings = CelestialsMod.settings;
            gameLoaded = true;
            storedCelestials = new HashSet<Pawn>();
        }

        public override void LoadedGame()
        {
            settings = CelestialsMod.settings;
            gameLoaded = true;
        }

        public override void GameComponentUpdate()
        {
            base.GameComponentUpdate();
            int currentTick = Current.Game.tickManager.TicksGame;
            if (currentTick % settings.CelestialTickRate == 0)
            {
                foreach (Pawn p in storedCelestials)
                {
                    if (ShouldResurrect(p, currentTick))
                    {
                        MakeResurrectionEvent(p, Find.AnyPlayerHomeMap);
                    }
                }
            }
        }

        //public override void GameComponentTick()
        //{
        //    int currentTick = Current.Game.tickManager.TicksGame;
        //    if (currentTick % settings.CelestialTickRate == 0)
        //    {
        //        foreach(Pawn p in GetDirectlyHeldThings().Where(p => p is Pawn))
        //        {
        //            if (ShouldResurrect(p, currentTick))
        //            {
        //                MakeResurrectionEvent(p, Find.AnyPlayerHomeMap);
        //            }
        //        }
        //    }
        //    base.GameComponentTick();
        //}

        public void StoreCelestialForResurrection(Pawn pawn)
        {
            if (pawn != null)
            {
                Hediff_Celestial hediff = (Hediff_Celestial)pawn.health.hediffSet.GetFirstHediffOfDef(CelestialsDefOf.CEL_CelestialHediff);
                if(hediff != null)
                {
                    HealCelestialCompletely(pawn);
                    hediff.SetResurrectionTick();
                    storedCelestials.Add(pawn);
                    if (pawn.Spawned)
                    {
                        pawn.DeSpawn();
                    }
                }
            }
        }

        public void HealCelestialCompletely(Pawn pawn)
        {
            if (pawn.Dead)
            {
                ResurrectionUtility.Resurrect(pawn);
            }
            List<Hediff> tmpHediffs = new List<Hediff>();
            tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
            for (int i = 0; i < tmpHediffs.Count; i++)
            {
                if(tmpHediffs[i] is Hediff_Injury injury)
                {
                    pawn.health.RemoveHediff(injury);
                }
                else if(tmpHediffs[i] is Hediff_MissingPart missing)
                {
                    pawn.health.RestorePart(missing.part);
                }
            }
        }

        public void MakeResurrectionEvent(Pawn pawn, Map map)
        {
            IIncidentTarget target = map;
            if(target != null && pawn != null)
            {
                IncidentParms parms = StorytellerUtility.DefaultParmsNow(CelestialsDefOf.CEL_CelestialResurrection.category, target);
                parms.forced = true;
                parms.tmpPawns = new List<Pawn>() { pawn };
                storedCelestials.Remove(pawn);
                CelestialsDefOf.CEL_CelestialResurrection.Worker.TryExecute(parms);
            }
        }

        public bool ShouldResurrect(Pawn pawn, int currTick)
        {
            if(pawn != null)
            {
                Hediff_Celestial hediff = (Hediff_Celestial)pawn.health.hediffSet.GetFirstHediffOfDef(CelestialsDefOf.CEL_CelestialHediff);
                if(hediff != null && currTick == hediff.resurrectionTick)
                {
                    return true;
                }
            }
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref celestials, "celestials");
            Scribe_Collections.Look(ref storedCelestials, "storedCelestials", LookMode.Deep);
        }
    }
}
