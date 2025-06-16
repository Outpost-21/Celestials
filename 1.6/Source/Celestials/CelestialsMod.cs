using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using RimWorld;
using Verse;

using TabulaRasa;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace Celestials
{
    public class CelestialsMod : Mod
    {
        public static CelestialsMod mod;

        public static CelestialsSettings settings;

        public Vector2 optionsScrollPosition;
        public float optionsViewRectHeight;

        public Dictionary<string, float> sectionHeights = new Dictionary<string, float>();

        public float GetSectionHeight(string section)
        {
            if (!sectionHeights.ContainsKey(section))
            {
                sectionHeights.Add(section, float.MaxValue);
            }
            return sectionHeights[section];
        }

        public void SetSectionHeight(string section, float value)
        {
            sectionHeights[section] = value;
        }

        public override string SettingsCategory() => "Celestials";

        internal static string VersionDir => Path.Combine(mod.Content.ModMetaData.RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public CelestialsMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<CelestialsSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            Log.Message($":: Celestials :: {CurrentVersion} ::".Colorize(Color.cyan));

            if (Prefs.DevMode && VersionDir != null)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            new Harmony("Neronix17.RimWorld.Celestials").PatchAll();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            bool flag = optionsViewRectHeight > inRect.height;
            Rect viewRect = new Rect(inRect.x, inRect.y, inRect.width - (flag ? 26f : 0f), optionsViewRectHeight);
            Widgets.BeginScrollView(inRect, ref optionsScrollPosition, viewRect);
            Listing_Standard listing = new Listing_Standard();
            Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, 999999f);
            listing.Begin(rect);
            // ============================ CONTENTS ================================
            DoOptionsCategoryContents(listing);
            // ======================================================================
            optionsViewRectHeight = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
        }

        public void DoOptionsCategoryContents(Listing_Standard ls)
        {
            ls.CheckboxEnhanced("Resurrection Side Effects", "If true, resurrection has the same side-effects as resurrector mech serum.", ref settings.resurrectionSideEffects);
            ls.CheckboxEnhanced("EMP Burst", "If true, resurrection generates a large EMP blast.", ref settings.empExplosion);
            ls.GapLine();
            Rect rect = ls.GetRect(18);
            Rect rect2 = rect.LeftHalf().Rounded();
            Rect rect3 = rect.RightHalf().Rounded();
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(rect2, "Chance of Celestial");
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.TextFieldPercent(rect3, ref settings.celestialChance, ref settings.chanceOfCelestial_buffer, 0f, 1f);
            ls.Note("Getting the numbers after the decimal point to be 0 can be finnicky due to RimWorlds own code, you can paste in the value to get around that. \nDefault: 1%", GameFont.Tiny);
            ls.GapLine();
            ls.Label("Resurrection time");
            ls.IntRange(ref settings.resurrectionTicks, 0, 300000);
            ls.Note("This is in ticks, so 60000 = 1 day. \nDefaults: 30000 - 60000", GameFont.Tiny);
            ls.GapLine();
            ls.LabelBacked("Allowed Races", Color.white);
            ls.Note("These toggles allow you to set specific races to have a chance to be a Celestial, by default only Humans are enabled, but this allows you to enable it for any living race. Changing this list requires a game restart to take effect!");
            List<ThingDef> races = DefDatabase<ThingDef>.AllDefsListForReading.Where(td => td.IsLivingCreature()).ToList();
            races.SortBy(rc => rc.label);
            Listing_Standard rls = ls.BeginSection(GetSectionHeight("AllowedRaces"));
            rls.ColumnWidth -= 26f;
            rls.ColumnWidth *= 0.5f;
            for (int i = 0; i < races.Count(); i++)
            {
                if (i == (races.Count + (races.Count % 2f == 0f ? 0f : 1f)) / 2f)
                {
                    rls.NewColumn();
                }
                DrawRaceToggle(rls, races[i]);
            }
            SetSectionHeight("AllowedRaces", rls.MaxColumnHeightSeen);
            ls.EndSection(rls);
        }

        public void DrawRaceToggle(Listing_Standard listing, ThingDef curRace)
        {
            bool tempBool = settings.allowedRaceDefs[curRace.defName];
            string tooltip = curRace.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + curRace.DescriptionDetailed;
            listing.CheckboxLabeled(curRace.LabelCap, ref tempBool, tooltip);
            settings.allowedRaceDefs[curRace.defName] = tempBool;
        }
    }
}
