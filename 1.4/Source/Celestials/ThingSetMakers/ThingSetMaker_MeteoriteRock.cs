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
    public class ThingSetMaker_MeteoriteRock : ThingSetMaker
	{
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		public static void Reset()
		{
			nonSmoothedMineables.Clear();
			nonSmoothedMineables.AddRange(DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef x) => x.mineable && x != ThingDefOf.CollapsedRocks && x != ThingDefOf.RaisedRocks && !x.IsSmoothed));
		}

		public override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int randomInRange = (parms.countRange ?? MineablesCountRange).RandomInRange;
			ThingDef def = FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		public ThingDef FindRandomMineableDef()
		{
			return nonSmoothedMineables.Where((ThingDef x) => x.building.isResourceRock && x.building.mineableThing.BaseMarketValue <= 1.5f).RandomElement();
		}

		public override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return nonSmoothedMineables;
		}
	}
}
