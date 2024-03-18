using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;

namespace Celestials
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "PostApplyDamage")]
    public static class Patch_Pawn_HealthTracker_PostApplyDamage
    {
        [HarmonyPrefix]
        public static bool Prefix(DamageInfo dinfo, float totalDamageDealt, Pawn_HealthTracker __instance)
        {
            Pawn pawn = __instance.pawn;
            if (__instance.ShouldBeDead() && pawn.health.hediffSet.HasHediff(CelestialsDefOf.CEL_CelestialHediff))
            {
                GameComp_Celestials comp = Current.Game.GetComponent<GameComp_Celestials>();
                if (dinfo.Def != DamageDefOf.ExecutionCut)
                {
                    if(pawn.Faction != null && pawn.Faction == Faction.OfPlayer)
                    {
                        PlayerCelestialDeath(pawn, comp);
                        return false;
                    }
                }
                CelestialDeath(pawn);
            }
            return true;
        }

        public static void CelestialDeath(Pawn pawn)
        {
            SpawnAsh(pawn);
            pawn.DestroyOrPassToWorld(DestroyMode.Vanish);
        }

        public static void PlayerCelestialDeath(Pawn pawn, GameComp_Celestials comp)
        {
            SpawnAsh(pawn);
            comp.StoreCelestialForResurrection(pawn);
        }

        public static void SpawnAsh(Pawn pawn)
        {
            if (pawn.Spawned)
            {
                FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_Ash, 18, FilthSourceFlags.Pawn, true);
            }
        }
    }
}
