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
	public class CompProperties_UseEffectMakeCelestial : CompProperties_Usable
	{
		public CompProperties_UseEffectMakeCelestial()
		{
			compClass = typeof(Comp_UseEffectMakeCelestial);
		}
	}
}
