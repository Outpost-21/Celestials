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
	public class Comp_UseEffectMakeCelestial : CompUseEffect
	{
		public new CompProperties_UseEffectMakeCelestial Props => (CompProperties_UseEffectMakeCelestial)props;

		public override void DoEffect(Pawn user)
		{
			user.health.AddHediff(CelestialsDefOf.CEL_CelestialHediff);
		}

		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if ((!p.IsFreeColonist || p.HasExtraHomeFaction()))
			{
				failReason = "Celestials.NotAllowedForNonColonists".Translate();
				return false;
			}
			if (CelestialsMod.settings.disallowedFleshTypes.Contains(p.RaceProps.FleshType.defName))
			{
				failReason = "Celestials.NotAllowedForFleshType".Translate(p.RaceProps.fleshType.LabelCap);
				return false;
			}
			failReason = null;
			return true;
		}
	}
}
