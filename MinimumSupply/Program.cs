using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;
using System.Collections.Immutable;

namespace MinimumSupply
{
    public class Program : MyGridProgram
    {
        private class Ini
        {
            private const string IniSection = "Deer Industries MinimumSupply";
            private const string IniBulletproofGlass = "Bulletproof Glass";
            private const string IniComputers = "Computers";
            private const string IniConstructionComponents = "Construction Components";
            private const string IniDetectorComponents = "Detector Components";
            private const string IniDisplays = "Displays";
            private const string IniExplosives = "Explosives";
            private const string IniGirders = "Girders";
            private const string IniGravityComponents = "Gravity Components";
            private const string IniInteriorPlates = "Interior Plates";
            private const string IniLargeSteelTubes = "Large Steel Tubes";
            private const string IniMedicalComponents = "Medical Components";
            private const string IniMetalGrids = "Metal Grids";
            private const string IniMotors = "Motors";
            private const string IniPowerCells = "Power Cells";
            private const string IniRadioCommunicationComponents = "Radio Communication Components";
            private const string IniReactorComponents = "Reactor Components";
            private const string IniSmallSteelTubes = "Small Steel Tubes";
            private const string IniSolarCells = "Solar Cells";
            private const string IniSteelPlates = "Steel Plates";
            private const string IniSuperconductors = "Superconductors";
            private const string IniThrusterComponents = "Thruster Components";
            private const string IniPro1s = "PRO-1s";
            private const string IniGrinders = "Grinders";
            private const string IniEnhancedGrinders = "Enhanced Grinders";
            private const string IniProficientGrinders = "Proficient Grinders";
            private const string IniEliteGrinders = "Elite Grinders";
            private const string IniMr20s = "MR-20s";
            private const string IniRo1s = "RO-1s";
            private const string IniDatapads = "Datapads";
            private const string IniS10es = "S-10Es";
            private const string IniS20as = "S-20As";
            private const string IniHandDrills = "Hand Drills";
            private const string IniEnhancedHandDrills = "Enhanced Hand Drills";
            private const string IniProficientHandDrills = "Proficient Hand Drills";
            private const string IniEliteHandDrills = "Elite Hand Drills";
            private const string IniHydrogenBottles = "Hydrogen Bottles";
            private const string IniOxygenBottles = "Oxygen Bottles";
            private const string IniMr8ps = "MR-8Ps";
            private const string IniMr50as = "MR-50As";
            private const string IniS10s = "S-10s";
            private const string IniMr30es = "MR-30Es";
            private const string IniWelders = "Welders";
            private const string IniEnhancedWelders = "Enhanced Welders";
            private const string IniProficientWelders = "Proficient Welders";
            private const string IniEliteWelders = "Elite Welders";
            private const string IniAutocannonMagazines = "Autocannon Magazines";
            private const string IniMr20Magazines = "MR-20 Magazines";
            private const string IniCanvases = "Canvases";
            private const string IniS10eMagazines = "S-10E Magazines";
            private const string IniS20aMagazines = "S-20A Magazines";
            private const string IniArtilleryShells = "Artillery Shells";
            private const string IniLargeRailgunSabots = "Large Railgun Sabots";
            private const string IniAssaultCannonShells = "Assault Cannon Shells";
            private const string IniMissiles = "Missiles";
            private const string IniGatlingAmmoBoxes = "Gatling Ammo Boxes";
            private const string IniMr8pMagazines = "MR-8P Magazines";
            private const string IniMr50aMagazines = "MR-50A Magazines";
            private const string IniS10Magazines = "S-10 Magazines";
            private const string IniSmallRailgunSabots = "Small Railgun Sabots";
            private const string IniMr30eMagazines = "MR-30E Magazines";

            private static readonly MyIni Parser = new MyIni();

            private static readonly IDictionary<IMyProgrammableBlock, string> IniTextCache =
                new Dictionary<IMyProgrammableBlock, string>();

            private static readonly IDictionary<IMyProgrammableBlock, Ini> IniCache =
                new Dictionary<IMyProgrammableBlock, Ini>();

            private static readonly Ini Default = new Ini(
                100,
                100,
                100,
                20,
                50,
                20,
                100,
                20,
                500,
                100,
                20,
                100,
                100,
                20,
                20,
                20,
                100,
                100,
                500,
                20,
                20,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                1,
                1,
                1,
                1,
                1,
                1,
                6,
                6,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                200,
                200,
                0,
                200,
                200,
                200,
                200,
                200,
                200,
                200,
                200,
                200,
                200,
                200,
                200
            );

            public static Ini FromBlock(MyGridProgram program, IMyProgrammableBlock block)
            {
                return FromBlockInternal(block, false);
            }

            public static Ini FromBlockOrSetDefault(IMyProgrammableBlock block)
            {
                return FromBlockInternal(block, true);
            }

            private static Ini FromBlockInternal(IMyProgrammableBlock block, bool setDefault)
            {
                string text;
                if (IniTextCache.TryGetValue(block, out text) && text == block.CustomData)
                {
                    return IniCache[block];
                }

                IniTextCache[block] = block.CustomData;

                Parser.Clear();
                Ini ini = null;
                if (Parser.TryParse(block.CustomData))
                {
                    if (Parser.ContainsSection(IniSection))
                    {
                        ini = new Ini(
                            Parser.Get(IniSection, IniBulletproofGlass).ToInt32(Default.BulletproofGlass),
                            Parser.Get(IniSection, IniComputers).ToInt32(Default.Computers),
                            Parser.Get(IniSection, IniConstructionComponents).ToInt32(Default.ConstructionComponents),
                            Parser.Get(IniSection, IniDetectorComponents).ToInt32(Default.DetectorComponents),
                            Parser.Get(IniSection, IniDisplays).ToInt32(Default.Displays),
                            Parser.Get(IniSection, IniExplosives).ToInt32(Default.Explosives),
                            Parser.Get(IniSection, IniGirders).ToInt32(Default.Girders),
                            Parser.Get(IniSection, IniGravityComponents).ToInt32(Default.GravityComponents),
                            Parser.Get(IniSection, IniInteriorPlates).ToInt32(Default.InteriorPlates),
                            Parser.Get(IniSection, IniLargeSteelTubes).ToInt32(Default.LargeSteelTubes),
                            Parser.Get(IniSection, IniMedicalComponents).ToInt32(Default.MedicalComponents),
                            Parser.Get(IniSection, IniMetalGrids).ToInt32(Default.MetalGrids),
                            Parser.Get(IniSection, IniMotors).ToInt32(Default.Motors),
                            Parser.Get(IniSection, IniPowerCells).ToInt32(Default.PowerCells),
                            Parser.Get(IniSection, IniRadioCommunicationComponents)
                                .ToInt32(Default.RadioCommunicationComponents),
                            Parser.Get(IniSection, IniReactorComponents).ToInt32(Default.ReactorComponents),
                            Parser.Get(IniSection, IniSmallSteelTubes).ToInt32(Default.SmallSteelTubes),
                            Parser.Get(IniSection, IniSolarCells).ToInt32(Default.SolarCells),
                            Parser.Get(IniSection, IniSteelPlates).ToInt32(Default.SteelPlates),
                            Parser.Get(IniSection, IniSuperconductors).ToInt32(Default.Superconductors),
                            Parser.Get(IniSection, IniThrusterComponents).ToInt32(Default.ThrusterComponents),
                            Parser.Get(IniSection, IniPro1s).ToInt32(Default.Pro1s),
                            Parser.Get(IniSection, IniGrinders).ToInt32(Default.Grinders),
                            Parser.Get(IniSection, IniEnhancedGrinders).ToInt32(Default.EnhancedGrinders),
                            Parser.Get(IniSection, IniProficientGrinders).ToInt32(Default.ProficientGrinders),
                            Parser.Get(IniSection, IniEliteGrinders).ToInt32(Default.EliteGrinders),
                            Parser.Get(IniSection, IniMr20s).ToInt32(Default.Mr20s),
                            Parser.Get(IniSection, IniRo1s).ToInt32(Default.Ro1s),
                            Parser.Get(IniSection, IniDatapads).ToInt32(Default.Datapads),
                            Parser.Get(IniSection, IniS10es).ToInt32(Default.S10es),
                            Parser.Get(IniSection, IniS20as).ToInt32(Default.S20as),
                            Parser.Get(IniSection, IniHandDrills).ToInt32(Default.HandDrills),
                            Parser.Get(IniSection, IniEnhancedHandDrills).ToInt32(Default.EnhancedHandDrills),
                            Parser.Get(IniSection, IniProficientHandDrills).ToInt32(Default.ProficientHandDrills),
                            Parser.Get(IniSection, IniEliteHandDrills).ToInt32(Default.EliteHandDrills),
                            Parser.Get(IniSection, IniHydrogenBottles).ToInt32(Default.HydrogenBottles),
                            Parser.Get(IniSection, IniOxygenBottles).ToInt32(Default.OxygenBottles),
                            Parser.Get(IniSection, IniMr8ps).ToInt32(Default.Mr8ps),
                            Parser.Get(IniSection, IniMr50as).ToInt32(Default.Mr50as),
                            Parser.Get(IniSection, IniS10s).ToInt32(Default.S10s),
                            Parser.Get(IniSection, IniMr30es).ToInt32(Default.Mr30es),
                            Parser.Get(IniSection, IniWelders).ToInt32(Default.Welders),
                            Parser.Get(IniSection, IniEnhancedWelders).ToInt32(Default.EnhancedWelders),
                            Parser.Get(IniSection, IniProficientWelders).ToInt32(Default.ProficientWelders),
                            Parser.Get(IniSection, IniEliteWelders).ToInt32(Default.EliteWelders),
                            Parser.Get(IniSection, IniAutocannonMagazines).ToInt32(Default.AutocannonMagazines),
                            Parser.Get(IniSection, IniMr20Magazines).ToInt32(Default.Mr20Magazines),
                            Parser.Get(IniSection, IniCanvases).ToInt32(Default.Canvases),
                            Parser.Get(IniSection, IniS10eMagazines).ToInt32(Default.S10eMagazines),
                            Parser.Get(IniSection, IniS20aMagazines).ToInt32(Default.S20aMagazines),
                            Parser.Get(IniSection, IniArtilleryShells).ToInt32(Default.ArtilleryShells),
                            Parser.Get(IniSection, IniLargeRailgunSabots).ToInt32(Default.LargeRailgunSabots),
                            Parser.Get(IniSection, IniAssaultCannonShells).ToInt32(Default.AssaultCannonShells),
                            Parser.Get(IniSection, IniMissiles).ToInt32(Default.Missiles),
                            Parser.Get(IniSection, IniGatlingAmmoBoxes).ToInt32(Default.GatlingAmmoBoxes),
                            Parser.Get(IniSection, IniMr8pMagazines).ToInt32(Default.Mr8pMagazines),
                            Parser.Get(IniSection, IniMr50aMagazines).ToInt32(Default.Mr50AMagazines),
                            Parser.Get(IniSection, IniS10Magazines).ToInt32(Default.S10Magazines),
                            Parser.Get(IniSection, IniSmallRailgunSabots).ToInt32(Default.SmallRailgunSabots),
                            Parser.Get(IniSection, IniMr30eMagazines).ToInt32(Default.Mr30eMagazines)
                        );
                    }
                }
                else
                {
                    Parser.EndContent = block.CustomData;
                }

                IniCache[block] = ini;

                if (!setDefault)
                {
                    return ini;
                }

                if (ini == null)
                {
                    ini = Default;
                }

                Parser.Set(IniSection, IniBulletproofGlass, ini.BulletproofGlass);
                Parser.Set(IniSection, IniComputers, ini.Computers);
                Parser.Set(IniSection, IniConstructionComponents, ini.ConstructionComponents);
                Parser.Set(IniSection, IniDetectorComponents, ini.DetectorComponents);
                Parser.Set(IniSection, IniDisplays, ini.Displays);
                Parser.Set(IniSection, IniExplosives, ini.Explosives);
                Parser.Set(IniSection, IniGirders, ini.Girders);
                Parser.Set(IniSection, IniGravityComponents, ini.GravityComponents);
                Parser.Set(IniSection, IniInteriorPlates, ini.InteriorPlates);
                Parser.Set(IniSection, IniLargeSteelTubes, ini.LargeSteelTubes);
                Parser.Set(IniSection, IniMedicalComponents, ini.MedicalComponents);
                Parser.Set(IniSection, IniMetalGrids, ini.MetalGrids);
                Parser.Set(IniSection, IniMotors, ini.Motors);
                Parser.Set(IniSection, IniPowerCells, ini.PowerCells);
                Parser.Set(IniSection, IniRadioCommunicationComponents, ini.RadioCommunicationComponents);
                Parser.Set(IniSection, IniReactorComponents, ini.ReactorComponents);
                Parser.Set(IniSection, IniSmallSteelTubes, ini.SmallSteelTubes);
                Parser.Set(IniSection, IniSolarCells, ini.SolarCells);
                Parser.Set(IniSection, IniSteelPlates, ini.SteelPlates);
                Parser.Set(IniSection, IniSuperconductors, ini.Superconductors);
                Parser.Set(IniSection, IniThrusterComponents, ini.ThrusterComponents);
                Parser.Set(IniSection, IniPro1s, ini.Pro1s);
                Parser.Set(IniSection, IniGrinders, ini.Grinders);
                Parser.Set(IniSection, IniEnhancedGrinders, ini.EnhancedGrinders);
                Parser.Set(IniSection, IniProficientGrinders, ini.ProficientGrinders);
                Parser.Set(IniSection, IniEliteGrinders, ini.EliteGrinders);
                Parser.Set(IniSection, IniMr20s, ini.Mr20s);
                Parser.Set(IniSection, IniRo1s, ini.Ro1s);
                Parser.Set(IniSection, IniDatapads, ini.Datapads);
                Parser.Set(IniSection, IniS10es, ini.S10es);
                Parser.Set(IniSection, IniS20as, ini.S20as);
                Parser.Set(IniSection, IniHandDrills, ini.HandDrills);
                Parser.Set(IniSection, IniEnhancedHandDrills, ini.EnhancedHandDrills);
                Parser.Set(IniSection, IniProficientHandDrills, ini.ProficientHandDrills);
                Parser.Set(IniSection, IniEliteHandDrills, ini.EliteHandDrills);
                Parser.Set(IniSection, IniHydrogenBottles, ini.HydrogenBottles);
                Parser.Set(IniSection, IniOxygenBottles, ini.OxygenBottles);
                Parser.Set(IniSection, IniMr8ps, ini.Mr8ps);
                Parser.Set(IniSection, IniMr50as, ini.Mr50as);
                Parser.Set(IniSection, IniS10s, ini.S10s);
                Parser.Set(IniSection, IniMr30es, ini.Mr30es);
                Parser.Set(IniSection, IniWelders, ini.Welders);
                Parser.Set(IniSection, IniEnhancedWelders, ini.EnhancedWelders);
                Parser.Set(IniSection, IniProficientWelders, ini.ProficientWelders);
                Parser.Set(IniSection, IniEliteWelders, ini.EliteWelders);
                Parser.Set(IniSection, IniAutocannonMagazines, ini.AutocannonMagazines);
                Parser.Set(IniSection, IniMr20Magazines, ini.Mr20Magazines);
                Parser.Set(IniSection, IniCanvases, ini.Canvases);
                Parser.Set(IniSection, IniS10eMagazines, ini.S10eMagazines);
                Parser.Set(IniSection, IniS20aMagazines, ini.S20aMagazines);
                Parser.Set(IniSection, IniArtilleryShells, ini.ArtilleryShells);
                Parser.Set(IniSection, IniLargeRailgunSabots, ini.LargeRailgunSabots);
                Parser.Set(IniSection, IniAssaultCannonShells, ini.AssaultCannonShells);
                Parser.Set(IniSection, IniMissiles, ini.Missiles);
                Parser.Set(IniSection, IniGatlingAmmoBoxes, ini.GatlingAmmoBoxes);
                Parser.Set(IniSection, IniMr8pMagazines, ini.Mr8pMagazines);
                Parser.Set(IniSection, IniMr50aMagazines, ini.Mr50AMagazines);
                Parser.Set(IniSection, IniS10Magazines, ini.S10Magazines);
                Parser.Set(IniSection, IniSmallRailgunSabots, ini.SmallRailgunSabots);
                Parser.Set(IniSection, IniMr30eMagazines, ini.Mr30eMagazines);
                block.CustomData = Parser.ToString();

                return ini;
            }

            public readonly int BulletproofGlass;
            public readonly int Computers;
            public readonly int ConstructionComponents;
            public readonly int DetectorComponents;
            public readonly int Displays;
            public readonly int Explosives;
            public readonly int Girders;
            public readonly int GravityComponents;
            public readonly int InteriorPlates;
            public readonly int LargeSteelTubes;
            public readonly int MedicalComponents;
            public readonly int MetalGrids;
            public readonly int Motors;
            public readonly int PowerCells;
            public readonly int RadioCommunicationComponents;
            public readonly int ReactorComponents;
            public readonly int SmallSteelTubes;
            public readonly int SolarCells;
            public readonly int SteelPlates;
            public readonly int Superconductors;
            public readonly int ThrusterComponents;
            public readonly int Pro1s;
            public readonly int Grinders;
            public readonly int EnhancedGrinders;
            public readonly int ProficientGrinders;
            public readonly int EliteGrinders;
            public readonly int Mr20s;
            public readonly int Ro1s;
            public readonly int Datapads;
            public readonly int S10es;
            public readonly int S20as;
            public readonly int HandDrills;
            public readonly int EnhancedHandDrills;
            public readonly int ProficientHandDrills;
            public readonly int EliteHandDrills;
            public readonly int HydrogenBottles;
            public readonly int OxygenBottles;
            public readonly int Mr8ps;
            public readonly int Mr50as;
            public readonly int S10s;
            public readonly int Mr30es;
            public readonly int Welders;
            public readonly int EnhancedWelders;
            public readonly int ProficientWelders;
            public readonly int EliteWelders;
            public readonly int AutocannonMagazines;
            public readonly int Mr20Magazines;
            public readonly int Canvases;
            public readonly int S10eMagazines;
            public readonly int S20aMagazines;
            public readonly int ArtilleryShells;
            public readonly int LargeRailgunSabots;
            public readonly int AssaultCannonShells;
            public readonly int Missiles;
            public readonly int GatlingAmmoBoxes;
            public readonly int Mr8pMagazines;
            public readonly int Mr50AMagazines;
            public readonly int S10Magazines;
            public readonly int SmallRailgunSabots;
            public readonly int Mr30eMagazines;

            public Ini(int bulletproofGlass, int computers, int constructionComponents, int detectorComponents,
                int displays, int explosives, int girders, int gravityComponents, int interiorPlates,
                int largeSteelTubes, int medicalComponents, int metalGrids, int motors, int powerCells,
                int radioCommunicationComponents, int reactorComponents, int smallSteelTubes, int solarCells,
                int steelPlates, int superconductors, int thrusterComponents, int pro1s, int grinders,
                int enhancedGrinders, int proficientGrinders, int eliteGrinders, int mr20s, int ro1s, int datapads,
                int s10es, int s20as, int handDrills, int enhancedHandDrills, int proficientHandDrills,
                int eliteHandDrills, int hydrogenBottles, int oxygenBottles, int mr8ps, int mr50as, int s10s, int mr30es,
                int welders, int enhancedWelders, int proficientWelders, int eliteWelders, int autocannonMagazines,
                int mr20Magazines, int canvases, int s10eMagazines, int s20aMagazines, int artilleryShells,
                int largeRailgunSabots, int assaultCannonShells, int missiles, int gatlingAmmoBoxes, int mr8pMagazines,
                int mr50aMagazines, int s10Magazines, int smallRailgunSabots, int mr30eMagazines)
            {
                BulletproofGlass = bulletproofGlass;
                Computers = computers;
                ConstructionComponents = constructionComponents;
                DetectorComponents = detectorComponents;
                Displays = displays;
                Explosives = explosives;
                Girders = girders;
                GravityComponents = gravityComponents;
                InteriorPlates = interiorPlates;
                LargeSteelTubes = largeSteelTubes;
                MedicalComponents = medicalComponents;
                MetalGrids = metalGrids;
                Motors = motors;
                PowerCells = powerCells;
                RadioCommunicationComponents = radioCommunicationComponents;
                ReactorComponents = reactorComponents;
                SmallSteelTubes = smallSteelTubes;
                SolarCells = solarCells;
                SteelPlates = steelPlates;
                Superconductors = superconductors;
                ThrusterComponents = thrusterComponents;
                Pro1s = pro1s;
                Grinders = grinders;
                EnhancedGrinders = enhancedGrinders;
                ProficientGrinders = proficientGrinders;
                EliteGrinders = eliteGrinders;
                Mr20s = mr20s;
                Ro1s = ro1s;
                Datapads = datapads;
                S10es = s10es;
                S20as = s20as;
                HandDrills = handDrills;
                EnhancedHandDrills = enhancedHandDrills;
                ProficientHandDrills = proficientHandDrills;
                EliteHandDrills = eliteHandDrills;
                HydrogenBottles = hydrogenBottles;
                OxygenBottles = oxygenBottles;
                Mr8ps = mr8ps;
                Mr50as = mr50as;
                S10s = s10s;
                Mr30es = mr30es;
                Welders = welders;
                EnhancedWelders = enhancedWelders;
                ProficientWelders = proficientWelders;
                EliteWelders = eliteWelders;
                AutocannonMagazines = autocannonMagazines;
                Mr20Magazines = mr20Magazines;
                Canvases = canvases;
                S10eMagazines = s10eMagazines;
                S20aMagazines = s20aMagazines;
                ArtilleryShells = artilleryShells;
                LargeRailgunSabots = largeRailgunSabots;
                AssaultCannonShells = assaultCannonShells;
                Missiles = missiles;
                GatlingAmmoBoxes = gatlingAmmoBoxes;
                Mr8pMagazines = mr8pMagazines;
                Mr50AMagazines = mr50aMagazines;
                S10Magazines = s10Magazines;
                SmallRailgunSabots = smallRailgunSabots;
                Mr30eMagazines = mr30eMagazines;
            }
        }

        private struct Inventory
        {
            public static readonly Inventory Empty = new Inventory();

            public readonly int BulletproofGlass;
            public readonly int Computers;
            public readonly int ConstructionComponents;
            public readonly int DetectorComponents;
            public readonly int Displays;
            public readonly int Explosives;
            public readonly int Girders;
            public readonly int GravityComponents;
            public readonly int InteriorPlates;
            public readonly int LargeSteelTubes;
            public readonly int MedicalComponents;
            public readonly int MetalGrids;
            public readonly int Motors;
            public readonly int PowerCells;
            public readonly int RadioCommunicationComponents;
            public readonly int ReactorComponents;
            public readonly int SmallSteelTubes;
            public readonly int SolarCells;
            public readonly int SteelPlates;
            public readonly int Superconductors;
            public readonly int ThrusterComponents;
            public readonly int Pro1s;
            public readonly int Grinders;
            public readonly int EnhancedGrinders;
            public readonly int ProficientGrinders;
            public readonly int EliteGrinders;
            public readonly int Mr20s;
            public readonly int Ro1s;
            public readonly int Datapads;
            public readonly int S10es;
            public readonly int S20as;
            public readonly int HandDrills;
            public readonly int EnhancedHandDrills;
            public readonly int ProficientHandDrills;
            public readonly int EliteHandDrills;
            public readonly int HydrogenBottles;
            public readonly int OxygenBottles;
            public readonly int Mr8ps;
            public readonly int Mr50as;
            public readonly int S10s;
            public readonly int Mr30es;
            public readonly int Welders;
            public readonly int EnhancedWelders;
            public readonly int ProficientWelders;
            public readonly int EliteWelders;
            public readonly int AutocannonMagazines;
            public readonly int Mr20Magazines;
            public readonly int Canvases;
            public readonly int S10eMagazines;
            public readonly int S20aMagazines;
            public readonly int ArtilleryShells;
            public readonly int LargeRailgunSabots;
            public readonly int AssaultCannonShells;
            public readonly int Missiles;
            public readonly int GatlingAmmoBoxes;
            public readonly int Mr8pMagazines;
            public readonly int Mr50aMagazines;
            public readonly int S10Magazines;
            public readonly int SmallRailgunSabots;
            public readonly int Mr30eMagazines;

            public Inventory(int bulletproofGlass, int computers, int constructionComponents, int detectorComponents,
                int displays, int explosives, int girders, int gravityComponents, int interiorPlates,
                int largeSteelTubes, int medicalComponents, int metalGrids, int motors, int powerCells,
                int radioCommunicationComponents, int reactorComponents, int smallSteelTubes, int solarCells,
                int steelPlates, int superconductors, int thrusterComponents, int pro1S, int grinders,
                int enhancedGrinders, int proficientGrinders, int eliteGrinders, int mr20s, int ro1s, int datapads,
                int s10es, int s20as, int handDrills, int enhancedHandDrills, int proficientHandDrills,
                int eliteHandDrills, int hydrogenBottles, int oxygenBottles, int mr8ps, int mr50as, int s10s, int mr30es,
                int welders, int enhancedWelders, int proficientWelders, int eliteWelders, int autocannonMagazines,
                int mr20Magazines, int canvases, int s10eMagazines, int s20aMagazines, int artilleryShells,
                int largeRailgunSabots, int assaultCannonShells, int missiles, int gatlingAmmoBoxes, int mr8pMagazines,
                int mr50aMagazines, int s10Magazines, int smallRailgunSabots, int mr30eMagazines)
            {
                BulletproofGlass = bulletproofGlass;
                Computers = computers;
                ConstructionComponents = constructionComponents;
                DetectorComponents = detectorComponents;
                Displays = displays;
                Explosives = explosives;
                Girders = girders;
                GravityComponents = gravityComponents;
                InteriorPlates = interiorPlates;
                LargeSteelTubes = largeSteelTubes;
                MedicalComponents = medicalComponents;
                MetalGrids = metalGrids;
                Motors = motors;
                PowerCells = powerCells;
                RadioCommunicationComponents = radioCommunicationComponents;
                ReactorComponents = reactorComponents;
                SmallSteelTubes = smallSteelTubes;
                SolarCells = solarCells;
                SteelPlates = steelPlates;
                Superconductors = superconductors;
                ThrusterComponents = thrusterComponents;
                Pro1s = pro1S;
                Grinders = grinders;
                EnhancedGrinders = enhancedGrinders;
                ProficientGrinders = proficientGrinders;
                EliteGrinders = eliteGrinders;
                Mr20s = mr20s;
                Ro1s = ro1s;
                Datapads = datapads;
                S10es = s10es;
                S20as = s20as;
                HandDrills = handDrills;
                EnhancedHandDrills = enhancedHandDrills;
                ProficientHandDrills = proficientHandDrills;
                EliteHandDrills = eliteHandDrills;
                HydrogenBottles = hydrogenBottles;
                OxygenBottles = oxygenBottles;
                Mr8ps = mr8ps;
                Mr50as = mr50as;
                S10s = s10s;
                Mr30es = mr30es;
                Welders = welders;
                EnhancedWelders = enhancedWelders;
                ProficientWelders = proficientWelders;
                EliteWelders = eliteWelders;
                AutocannonMagazines = autocannonMagazines;
                Mr20Magazines = mr20Magazines;
                Canvases = canvases;
                S10eMagazines = s10eMagazines;
                S20aMagazines = s20aMagazines;
                ArtilleryShells = artilleryShells;
                LargeRailgunSabots = largeRailgunSabots;
                AssaultCannonShells = assaultCannonShells;
                Missiles = missiles;
                GatlingAmmoBoxes = gatlingAmmoBoxes;
                Mr8pMagazines = mr8pMagazines;
                Mr50aMagazines = mr50aMagazines;
                S10Magazines = s10Magazines;
                SmallRailgunSabots = smallRailgunSabots;
                Mr30eMagazines = mr30eMagazines;
            }
        }

        private static class ItemTypes
        {
            public static readonly MyItemType BulletproofGlass = MyItemType.MakeComponent("BulletproofGlass");
            public static readonly MyItemType Computer = MyItemType.MakeComponent("Computer");
            public static readonly MyItemType ConstructionComponent = MyItemType.MakeComponent("Construction");
            public static readonly MyItemType DetectorComponent = MyItemType.MakeComponent("Detector");
            public static readonly MyItemType Display = MyItemType.MakeComponent("Display");
            public static readonly MyItemType Explosives = MyItemType.MakeComponent("Explosives");
            public static readonly MyItemType Girder = MyItemType.MakeComponent("Girder");
            public static readonly MyItemType GravityComponent = MyItemType.MakeComponent("GravityGenerator");
            public static readonly MyItemType InteriorPlate = MyItemType.MakeComponent("InteriorPlate");
            public static readonly MyItemType LargeSteelTube = MyItemType.MakeComponent("LargeTube");
            public static readonly MyItemType MedicalComponent = MyItemType.MakeComponent("Medical");
            public static readonly MyItemType MetalGrid = MyItemType.MakeComponent("MetalGrid");
            public static readonly MyItemType Motor = MyItemType.MakeComponent("Motor");
            public static readonly MyItemType PowerCell = MyItemType.MakeComponent("PowerCell");
            public static readonly MyItemType RadioCommunicationComponent = MyItemType.MakeComponent("RadioCommunication");
            public static readonly MyItemType ReactorComponent = MyItemType.MakeComponent("Reactor");
            public static readonly MyItemType SmallSteelTube = MyItemType.MakeComponent("SmallTube");
            public static readonly MyItemType SolarCell = MyItemType.MakeComponent("SolarCell");
            public static readonly MyItemType SteelPlate = MyItemType.MakeComponent("SteelPlate");
            public static readonly MyItemType Superconductor = MyItemType.MakeComponent("Superconductor");
            public static readonly MyItemType ThrusterComponent = MyItemType.MakeComponent("Thrust");
            public static readonly MyItemType Pro1 = MyItemType.MakeTool("AdvancedHandHeldLauncherItem");
            public static readonly MyItemType Grinder = MyItemType.MakeTool("AngleGrinderItem");
            public static readonly MyItemType EnhancedGrinder = MyItemType.MakeTool("AnglerGrinder2Item");
            public static readonly MyItemType ProficientGrinder = MyItemType.MakeTool("AngleGrinder3Item");
            public static readonly MyItemType EliteGrinder = MyItemType.MakeTool("AngleGrinder4Item");
            public static readonly MyItemType Mr20 = MyItemType.MakeTool("AutomaticRifleItem");
            public static readonly MyItemType Ro1 = MyItemType.MakeTool("BasicHandHeldLauncherItem");
            public static readonly MyItemType Datapad = new MyItemType("MyObjectBuilder_Datapad", "Datapad");
            public static readonly MyItemType S10e = MyItemType.MakeTool("ElitePistolItem");
            public static readonly MyItemType S20a = MyItemType.MakeTool("FullAutoPistolItem");
            public static readonly MyItemType HandDrill = MyItemType.MakeTool("HandDrillItem");
            public static readonly MyItemType EnhancedHandDrill = MyItemType.MakeTool("HandDrill2Item");
            public static readonly MyItemType ProficientHandDrill = MyItemType.MakeTool("HandDrill3Item");
            public static readonly MyItemType EliteHandDrill = MyItemType.MakeTool("HandDrill4Item");
            public static readonly MyItemType HydrogenBottle = new MyItemType("MyObjectBuilder_GasContainerObject", "HydrogenBottle");
            public static readonly MyItemType OxygenBottle = new MyItemType("MyObjectBuilder_GasContainerObject", "OxygenBottle");
            public static readonly MyItemType Mr8p = MyItemType.MakeTool("PreciseAutomaticRifleItem");
            public static readonly MyItemType Mr50a = MyItemType.MakeTool("RapidFireAutomaticRifleItem");
            public static readonly MyItemType S10 = MyItemType.MakeTool("SemiAutoPistolItem");
            public static readonly MyItemType Mr30e = MyItemType.MakeTool("UltimateAutomaticRifleItem");
            public static readonly MyItemType Welder = MyItemType.MakeTool("WelderItem");
            public static readonly MyItemType EnhancedWelder = MyItemType.MakeTool("Welder2Item");
            public static readonly MyItemType ProficientWelder = MyItemType.MakeTool("Welder3Item");
            public static readonly MyItemType EliteWelder = MyItemType.MakeTool("Welder4Item");
            public static readonly MyItemType AutocannonMagazine = MyItemType.MakeAmmo("AutocannonClip");
            public static readonly MyItemType Mr20Magazine = MyItemType.MakeAmmo("AutomaticRifleGun_Mag_20rd");
            public static readonly MyItemType Canvas = MyItemType.MakeComponent("Canvas");
            public static readonly MyItemType S10eMagazine = MyItemType.MakeAmmo("ElitePistolMagazine");
            public static readonly MyItemType S20aMagazine = MyItemType.MakeAmmo("FullAutoPistolMagazine");
            public static readonly MyItemType ArtilleryShell = MyItemType.MakeAmmo("LargeCalibreAmmo");
            public static readonly MyItemType LargeRailgunSabot = MyItemType.MakeAmmo("LargeRailgunAmmo");
            public static readonly MyItemType AssaultCannonShell = MyItemType.MakeAmmo("MediumCalibreAmmo");
            public static readonly MyItemType Missile = MyItemType.MakeAmmo("Missile200mm");
            public static readonly MyItemType GatlingAmmoBox = MyItemType.MakeAmmo("NATO_25x184mm");
            public static readonly MyItemType Mr8pMagazine = MyItemType.MakeAmmo("PreciseAutomaticRifleGun_Mag_5rd");
            public static readonly MyItemType Mr50aMagazine = MyItemType.MakeAmmo("RapidFireAutomaticRifleGun_Mag_50rd");
            public static readonly MyItemType S10Magazine = MyItemType.MakeAmmo("SemiAutoPistolMagazine");
            public static readonly MyItemType SmallRailgunSabot = MyItemType.MakeAmmo("SmallRailgunAmmo");
            public static readonly MyItemType Mr30eMagazine = MyItemType.MakeAmmo("UltimateAutomaticRifleGun_Mag_30rd");
        }
        
        private readonly List<IMyAssembler> _assemblers = new List<IMyAssembler>();
        private readonly List<IMyTerminalBlock> _blocksWithInventory = new List<IMyTerminalBlock>();

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        private void Main(string argument)
        {
            var ini = Ini.FromBlockOrSetDefault(Me);

            GridTerminalSystem.GetBlocksOfType(_assemblers, assembler => assembler.CubeGrid == Me.CubeGrid);
            if (_assemblers.Count == 0)
            {
                return;
            }

            GridTerminalSystem.GetBlocksOfType(_blocksWithInventory, block => block.HasInventory);
            var inventory = _blocksWithInventory
                .SelectMany(block => Enumerable.Range(0, block.InventoryCount).Select(block.GetInventory))
                .Aggregate(Inventory.Empty, (accumulator, block) => new Inventory(
                    accumulator.BulletproofGlass + block.GetItemAmount(ItemTypes.BulletproofGlass).ToIntSafe(),
                    accumulator.Computers + block.GetItemAmount(ItemTypes.Computer).ToIntSafe(),
                    accumulator.ConstructionComponents +
                    block.GetItemAmount(ItemTypes.ConstructionComponent).ToIntSafe(),
                    accumulator.DetectorComponents + block.GetItemAmount(ItemTypes.DetectorComponent).ToIntSafe(),
                    accumulator.Displays + block.GetItemAmount(ItemTypes.Display).ToIntSafe(),
                    accumulator.Explosives + block.GetItemAmount(ItemTypes.Explosives).ToIntSafe(),
                    accumulator.Girders + block.GetItemAmount(ItemTypes.Girder).ToIntSafe(),
                    accumulator.GravityComponents + block.GetItemAmount(ItemTypes.GravityComponent).ToIntSafe(),
                    accumulator.InteriorPlates + block.GetItemAmount(ItemTypes.InteriorPlate).ToIntSafe(),
                    accumulator.LargeSteelTubes + block.GetItemAmount(ItemTypes.LargeSteelTube).ToIntSafe(),
                    accumulator.MedicalComponents + block.GetItemAmount(ItemTypes.MedicalComponent).ToIntSafe(),
                    accumulator.MetalGrids + block.GetItemAmount(ItemTypes.MetalGrid).ToIntSafe(),
                    accumulator.Motors + block.GetItemAmount(ItemTypes.Motor).ToIntSafe(),
                    accumulator.PowerCells + block.GetItemAmount(ItemTypes.PowerCell).ToIntSafe(),
                    accumulator.RadioCommunicationComponents +
                    block.GetItemAmount(ItemTypes.RadioCommunicationComponent).ToIntSafe(),
                    accumulator.ReactorComponents + block.GetItemAmount(ItemTypes.ReactorComponent).ToIntSafe(),
                    accumulator.SmallSteelTubes + block.GetItemAmount(ItemTypes.SmallSteelTube).ToIntSafe(),
                    accumulator.SolarCells + block.GetItemAmount(ItemTypes.SolarCell).ToIntSafe(),
                    accumulator.SteelPlates + block.GetItemAmount(ItemTypes.SteelPlate).ToIntSafe(),
                    accumulator.Superconductors + block.GetItemAmount(ItemTypes.Superconductor).ToIntSafe(),
                    accumulator.ThrusterComponents + block.GetItemAmount(ItemTypes.ThrusterComponent).ToIntSafe(),
                    accumulator.Pro1s + block.GetItemAmount(ItemTypes.Pro1).ToIntSafe(),
                    accumulator.Grinders + block.GetItemAmount(ItemTypes.Grinder).ToIntSafe(),
                    accumulator.EnhancedGrinders + block.GetItemAmount(ItemTypes.EnhancedGrinder).ToIntSafe(),
                    accumulator.ProficientGrinders + block.GetItemAmount(ItemTypes.ProficientGrinder).ToIntSafe(),
                    accumulator.EliteGrinders + block.GetItemAmount(ItemTypes.EliteGrinder).ToIntSafe(),
                    accumulator.Mr20s + block.GetItemAmount(ItemTypes.Mr20).ToIntSafe(),
                    accumulator.Ro1s + block.GetItemAmount(ItemTypes.Ro1).ToIntSafe(),
                    accumulator.Datapads + block.GetItemAmount(ItemTypes.Datapad).ToIntSafe(),
                    accumulator.S10es + block.GetItemAmount(ItemTypes.S10e).ToIntSafe(),
                    accumulator.S20as + block.GetItemAmount(ItemTypes.S20a).ToIntSafe(),
                    accumulator.HandDrills + block.GetItemAmount(ItemTypes.HandDrill).ToIntSafe(),
                    accumulator.EnhancedHandDrills + block.GetItemAmount(ItemTypes.EnhancedHandDrill).ToIntSafe(),
                    accumulator.ProficientHandDrills + block.GetItemAmount(ItemTypes.ProficientHandDrill).ToIntSafe(),
                    accumulator.EliteHandDrills + block.GetItemAmount(ItemTypes.EliteHandDrill).ToIntSafe(),
                    accumulator.HydrogenBottles + block.GetItemAmount(ItemTypes.HydrogenBottle).ToIntSafe(),
                    accumulator.OxygenBottles + block.GetItemAmount(ItemTypes.OxygenBottle).ToIntSafe(),
                    accumulator.Mr8ps + block.GetItemAmount(ItemTypes.Mr8p).ToIntSafe(),
                    accumulator.Mr50as + block.GetItemAmount(ItemTypes.Mr50a).ToIntSafe(),
                    accumulator.S10s + block.GetItemAmount(ItemTypes.S10).ToIntSafe(),
                    accumulator.Mr30es + block.GetItemAmount(ItemTypes.Mr30e).ToIntSafe(),
                    accumulator.Welders + block.GetItemAmount(ItemTypes.Welder).ToIntSafe(),
                    accumulator.EnhancedWelders + block.GetItemAmount(ItemTypes.EnhancedWelder).ToIntSafe(),
                    accumulator.ProficientWelders + block.GetItemAmount(ItemTypes.ProficientWelder).ToIntSafe(),
                    accumulator.EliteWelders + block.GetItemAmount(ItemTypes.EliteWelder).ToIntSafe(),
                    accumulator.AutocannonMagazines + block.GetItemAmount(ItemTypes.AutocannonMagazine).ToIntSafe(),
                    accumulator.Mr20Magazines + block.GetItemAmount(ItemTypes.Mr20Magazine).ToIntSafe(),
                    accumulator.Canvases + block.GetItemAmount(ItemTypes.Canvas).ToIntSafe(),
                    accumulator.S10eMagazines + block.GetItemAmount(ItemTypes.S10eMagazine).ToIntSafe(),
                    accumulator.S20aMagazines + block.GetItemAmount(ItemTypes.S20aMagazine).ToIntSafe(),
                    accumulator.ArtilleryShells + block.GetItemAmount(ItemTypes.ArtilleryShell).ToIntSafe(),
                    accumulator.LargeRailgunSabots + block.GetItemAmount(ItemTypes.LargeRailgunSabot).ToIntSafe(),
                    accumulator.AssaultCannonShells + block.GetItemAmount(ItemTypes.AssaultCannonShell).ToIntSafe(),
                    accumulator.Missiles + block.GetItemAmount(ItemTypes.Missile).ToIntSafe(),
                    accumulator.GatlingAmmoBoxes + block.GetItemAmount(ItemTypes.GatlingAmmoBox).ToIntSafe(),
                    accumulator.Mr8pMagazines + block.GetItemAmount(ItemTypes.Mr8pMagazine).ToIntSafe(),
                    accumulator.Mr50aMagazines + block.GetItemAmount(ItemTypes.Mr50aMagazine).ToIntSafe(),
                    accumulator.S10Magazines + block.GetItemAmount(ItemTypes.S10Magazine).ToIntSafe(),
                    accumulator.SmallRailgunSabots + block.GetItemAmount(ItemTypes.SmallRailgunSabot).ToIntSafe(),
                    accumulator.Mr30eMagazines + block.GetItemAmount(ItemTypes.Mr30eMagazine).ToIntSafe()
                ));

            Echo($"Bulletproof Glass: {inventory.BulletproofGlass}");
            Echo($"Computers: {inventory.Computers}");
            Echo($"Construction Components: {inventory.ConstructionComponents}");
            Echo($"Detector Components: {inventory.DetectorComponents}");
            Echo($"Displays: {inventory.Displays}");
            Echo($"Explosives: {inventory.Explosives}");
            Echo($"Girders: {inventory.Girders}");
            Echo($"Datapads: {inventory.Datapads}");
            Echo($"Hydrogen Bottles: {inventory.HydrogenBottles}");
            Echo($"Oxygen Bottles: {inventory.OxygenBottles}");
        }
    }
}