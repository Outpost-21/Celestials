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
    public class IncidentWorker_CelestialResurrection : IncidentWorker
	{
		public override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 cell;
			return TryFindCell(out cell, map);
		}

		public override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!TryFindCell(out var cell, map))
			{
				return false;
			}
			List<Thing> list = ThingSetMakerDefOf.Meteorite.root.Generate();
			list.AddRange(parms.tmpPawns);
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, list, cell, map);
			string text = string.Format(def.letterText, list[0].def.label).CapitalizeFirst();
			SendStandardLetter(def.letterLabel + ": " + list[0].def.LabelCap, text, LetterDefOf.PositiveEvent, parms, new TargetInfo(cell, map));
			return true;
		}
		
		public bool TryFindCell(out IntVec3 cell, Map map)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, out cell, alwaysAvoidColonists: true, allowCellsWithBuildings: false, allowCellsWithItems: false, allowRoofedCells: false);
		}
	}
}
