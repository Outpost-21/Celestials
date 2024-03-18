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
    [HarmonyPatch(typeof(PawnGenerator), "GenerateInitialHediffs")]
    public static class Patch_PawnGenerator_GenerateInitialHediffs
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, PawnGenerationRequest request)
        {
            if(IsApplicablePawn(pawn))
            {
                pawn.health.AddHediff(CelestialsDefOf.CEL_CelestialHediff);
            }
        }

        public static bool IsApplicablePawn(Pawn pawn)
        {
            //if(ModsConfig.BiotechActive && HasCelestialParent(pawn))
            //{
            //    return true;
            //}
            if(pawn.DevelopmentalStage == DevelopmentalStage.Adult && !CelestialsMod.settings.disallowedFleshTypes.Contains(pawn.RaceProps.FleshType.defName))
            {
                if (Rand.Chance(CelestialsMod.settings.celestialChance))
                {
                    if (pawn.RaceProps.Animal && CelestialsMod.settings.allowOnAnimals)
                    {
                        return true;
                    }
                    if (pawn.RaceProps.Humanlike && CelestialsMod.settings.allowOnHumanlikes)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public static bool HasCelestialParent(Pawn pawn)
        //{
        //    IEnumerable<Pawn> parents = pawn.relations.DirectRelations.Where(r => r.def == PawnRelationDefOf.ParentBirth && (r.otherPawn?.health?.hediffSet?.HasHediff(CelestialsDefOf.CEL_CelestialHediff) ?? false)).Select(s => s.otherPawn);
        //    if (!parents.EnumerableNullOrEmpty())
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
