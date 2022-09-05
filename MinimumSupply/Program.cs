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
        private static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var pair in pairs)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
            return dictionary;
        }
        
        private static readonly IDictionary<MyItemType, ItemDefinition> ItemDefinitionsByItemType =
            ToDictionary(
                new []
                {
                    new ItemDefinition("Bulletproof Glass", MyItemType.MakeComponent("BulletproofGlass")),
                    new ItemDefinition("Computers", MyItemType.MakeComponent("Computer")),
                    new ItemDefinition("Construction Components", MyItemType.MakeComponent("ConstructionComponent")),
                    new ItemDefinition("Detector Components", MyItemType.MakeComponent("Detector")),
                    new ItemDefinition("Displays", MyItemType.MakeComponent("Display")),
                    new ItemDefinition("Explosives", MyItemType.MakeComponent("Explosives")),
                    new ItemDefinition("Girders", MyItemType.MakeComponent("Girder")),
                    new ItemDefinition("Gravity Components", MyItemType.MakeComponent("GravityGenerator")),
                    new ItemDefinition("Interior Plates", MyItemType.MakeComponent("InteriorPlate")),
                    new ItemDefinition("Large Steel Tubes", MyItemType.MakeComponent("LargeTube")),
                    new ItemDefinition("Medical Components", MyItemType.MakeComponent("Medical")),
                    new ItemDefinition("Metal Grids", MyItemType.MakeComponent("MetalGrid")),
                    new ItemDefinition("Motors", MyItemType.MakeComponent("Motor")),
                    new ItemDefinition("Power Cells", MyItemType.MakeComponent("PowerCell")),
                    new ItemDefinition("Radio Communication Components",
                        MyItemType.MakeComponent("RadioCommunication")),
                    new ItemDefinition("Reactor Components", MyItemType.MakeComponent("Reactor")),
                    new ItemDefinition("Small Steel Tubes", MyItemType.MakeComponent("SmallTube")),
                    new ItemDefinition("Solar Cells", MyItemType.MakeComponent("SolarCell")),
                    new ItemDefinition("Steel Plates", MyItemType.MakeComponent("SteelPlate")),
                    new ItemDefinition("Superconductors", MyItemType.MakeComponent("Superconductor")),
                    new ItemDefinition("Thruster Components", MyItemType.MakeComponent("Thrust")),
                    new ItemDefinition("PRO-1s", MyItemType.MakeTool("AdvancedHandHeldLauncherItem")),
                    new ItemDefinition("Grinders", MyItemType.MakeTool("AngleGrinderItem")),
                    new ItemDefinition("Enhanced Grinders", MyItemType.MakeTool("AngleGrinder2Item")),
                    new ItemDefinition("Proficient Grinders", MyItemType.MakeTool("AngleGrinder3Item")),
                    new ItemDefinition("Elite Grinders", MyItemType.MakeTool("AngleGrinder4Item")),
                    new ItemDefinition("MR-20s", MyItemType.MakeTool("AutomaticRifleItem")),
                    new ItemDefinition("RO-1s", MyItemType.MakeTool("BasicHandHeldLauncherItem")),
                    new ItemDefinition("Datapads", new MyItemType("MyObjectBuilder_Datapad", "Datapad")),
                    new ItemDefinition("S-10Es", MyItemType.MakeTool("ElitePistolItem")),
                    new ItemDefinition("S-20As", MyItemType.MakeTool("FullAutoPistolItem")),
                    new ItemDefinition("Hand Drills", MyItemType.MakeTool("HandDrillItem")),
                    new ItemDefinition("Enhanced Hand Drills", MyItemType.MakeTool("HandDrill2Item")),
                    new ItemDefinition("Proficient Hand Drills", MyItemType.MakeTool("HandDrill3Item")),
                    new ItemDefinition("Elite Hand Drills", MyItemType.MakeTool("HandDrill4Item")),
                    new ItemDefinition("Hydrogen Bottles",
                        new MyItemType("MyObjectBuilder_GasContainerObject", "HydrogenBottle")),
                    new ItemDefinition("Oxygen Bottles",
                        new MyItemType("MyObjectBuilder_GasContainerObject", "OxygenBottle")),
                    new ItemDefinition("MR-8Ps", MyItemType.MakeTool("PreciseAutomaticRifleItem")),
                    new ItemDefinition("MR-50As", MyItemType.MakeTool("RapidFireAutomaticRifleItem")),
                    new ItemDefinition("S-10s", MyItemType.MakeTool("SemiAutoPistolItem")),
                    new ItemDefinition("MR-30Es", MyItemType.MakeTool("UltimateAutomaticRifleItem")),
                    new ItemDefinition("Welders", MyItemType.MakeTool("WelderItem")),
                    new ItemDefinition("Enhanced Welders", MyItemType.MakeTool("Welder2Item")),
                    new ItemDefinition("Proficient Welders", MyItemType.MakeTool("Welder3Item")),
                    new ItemDefinition("Elite Welders", MyItemType.MakeTool("Welder4Item")),
                    new ItemDefinition("Autocannon Magazines", MyItemType.MakeAmmo("AutocannonClip")),
                    new ItemDefinition("MR-20 Magazines", MyItemType.MakeAmmo("AutomaticRifleGun_Mag_20rd")),
                    new ItemDefinition("Canvases", MyItemType.MakeComponent("Canvas")),
                    new ItemDefinition("S-10E Magazines", MyItemType.MakeAmmo("ElitePistolMagazine")),
                    new ItemDefinition("S-20A Magazines", MyItemType.MakeAmmo("FullAutoPistolMagazine")),
                    new ItemDefinition("Artillery Shells", MyItemType.MakeAmmo("LargeCalibreAmmo")),
                    new ItemDefinition("Large Railgun Sabots", MyItemType.MakeAmmo("LargeRailgunAmmo")),
                    new ItemDefinition("Assault Cannon Shells", MyItemType.MakeAmmo("MediumCalibreAmmo")),
                    new ItemDefinition("Missiles", MyItemType.MakeAmmo("Missile200mm")),
                    new ItemDefinition("Gatling Ammo Boxes", MyItemType.MakeAmmo("NATO_25x184mm")),
                    new ItemDefinition("MR-8P Magazines", MyItemType.MakeAmmo("PreciseAutomaticRifleGun_Mag_5rd")),
                    new ItemDefinition("MR-50A Magazines", MyItemType.MakeAmmo("RapidFireAutomaticRifleGun_Mag_50rd")),
                    new ItemDefinition("S-10 Magazines", MyItemType.MakeAmmo("SemiAutoPistolMagazine")),
                    new ItemDefinition("Small Railgun Sabots", MyItemType.MakeAmmo("SmallRailgunAmmo")),
                    new ItemDefinition("MR-30E Magazines", MyItemType.MakeAmmo("UltimateAutomaticRifleGun_Mag_30rd"))
                }.Select(definition => new KeyValuePair<MyItemType, ItemDefinition>(
                    definition.ItemType,
                    definition
                ))
            );

        private static readonly IDictionary<string, ItemDefinition> ItemDefinitionsByIniName =
            ToDictionary(
                ItemDefinitionsByItemType
                    .Values
                    .Select(definition => new KeyValuePair<string, ItemDefinition>(definition.IniName, definition))
            );

        private static IDictionary<MyItemType, int> EmptyInventory = ToDictionary(
            ItemDefinitionsByItemType.Keys
                .Select(key => new KeyValuePair<MyItemType, int>(key, 0))
        );

        private struct ItemDefinition
        {
            public readonly string IniName;
            public readonly MyItemType ItemType;

            public ItemDefinition(string iniName, MyItemType itemType)
            {
                IniName = iniName;
                ItemType = itemType;
            }
        }

        private class Ini
        {
            private const string IniSection = "Deer Industries MinimumSupply";

            private static readonly MyIni Parser = new MyIni();

            private static readonly IDictionary<IMyProgrammableBlock, string> IniTextCache =
                new Dictionary<IMyProgrammableBlock, string>();

            private static readonly IDictionary<IMyProgrammableBlock, Ini> IniCache =
                new Dictionary<IMyProgrammableBlock, Ini>();

            private static readonly Ini Default = new Ini(
                ToDictionary(
                    new[]
                        {
                            new KeyValuePair<string, int>("Bulletproof Glass", 100),
                            new KeyValuePair<string, int>("Computers", 100),
                            new KeyValuePair<string, int>("Construction Components", 100),
                            new KeyValuePair<string, int>("Detector Components", 20),
                            new KeyValuePair<string, int>("Displays", 50),
                            new KeyValuePair<string, int>("Explosives", 20),
                            new KeyValuePair<string, int>("Girders", 100),
                            new KeyValuePair<string, int>("Gravity Components", 20),
                            new KeyValuePair<string, int>("Interior Plates", 500),
                            new KeyValuePair<string, int>("Large Steel Tubes", 100),
                            new KeyValuePair<string, int>("Medical Components", 20),
                            new KeyValuePair<string, int>("Metal Grids", 100),
                            new KeyValuePair<string, int>("Motors", 100),
                            new KeyValuePair<string, int>("Power Cells", 20),
                            new KeyValuePair<string, int>("Radio Communication Components", 20),
                            new KeyValuePair<string, int>("Reactor Components", 20),
                            new KeyValuePair<string, int>("Small Steel Tubes", 100),
                            new KeyValuePair<string, int>("Solar Cells", 100),
                            new KeyValuePair<string, int>("Steel Plates", 500),
                            new KeyValuePair<string, int>("Superconductors", 20),
                            new KeyValuePair<string, int>("Thruster Components", 20),
                            new KeyValuePair<string, int>("PRO-1s", 1),
                            new KeyValuePair<string, int>("Grinders", 1),
                            new KeyValuePair<string, int>("Enhanced Grinders", 1),
                            new KeyValuePair<string, int>("Proficient Grinders", 1),
                            new KeyValuePair<string, int>("Elite Grinders", 1),
                            new KeyValuePair<string, int>("MR-20s", 1),
                            new KeyValuePair<string, int>("RO-1s", 1),
                            new KeyValuePair<string, int>("Datapads", 0),
                            new KeyValuePair<string, int>("S-10Es", 1),
                            new KeyValuePair<string, int>("S-20As", 1),
                            new KeyValuePair<string, int>("Hand Drills", 1),
                            new KeyValuePair<string, int>("Enhanced Hand Drills", 1),
                            new KeyValuePair<string, int>("Proficient Hand Drills", 1),
                            new KeyValuePair<string, int>("Elite Hand Drills", 1),
                            new KeyValuePair<string, int>("Hydrogen Bottles", 6),
                            new KeyValuePair<string, int>("Oxygen Bottles", 6),
                            new KeyValuePair<string, int>("MR-8Ps", 1),
                            new KeyValuePair<string, int>("MR-50As", 1),
                            new KeyValuePair<string, int>("S-10s", 1),
                            new KeyValuePair<string, int>("MR-30Es", 1),
                            new KeyValuePair<string, int>("Welders", 1),
                            new KeyValuePair<string, int>("Enhanced Welders", 1),
                            new KeyValuePair<string, int>("Proficient Welders", 1),
                            new KeyValuePair<string, int>("Elite Welders", 1),
                            new KeyValuePair<string, int>("Autocannon Magazines", 200),
                            new KeyValuePair<string, int>("MR-20 Magazines", 200),
                            new KeyValuePair<string, int>("Canvases", 0),
                            new KeyValuePair<string, int>("S-10E Magazines", 200),
                            new KeyValuePair<string, int>("S-20A Magazines", 200),
                            new KeyValuePair<string, int>("Artillery Shells", 200),
                            new KeyValuePair<string, int>("Large Railgun Sabots", 200),
                            new KeyValuePair<string, int>("Assault Cannon Shells", 200),
                            new KeyValuePair<string, int>("Missiles", 200),
                            new KeyValuePair<string, int>("Gatling Ammo Boxes", 200),
                            new KeyValuePair<string, int>("MR-8P Magazines", 200),
                            new KeyValuePair<string, int>("MR-50A Magazines", 200),
                            new KeyValuePair<string, int>("S-10 Magazines", 200),
                            new KeyValuePair<string, int>("Small Railgun Sabots", 200),
                            new KeyValuePair<string, int>("MR-30E Magazines", 200)
                        }
                        .Select(pair =>
                            new KeyValuePair<MyItemType, int>(ItemDefinitionsByIniName[pair.Key].ItemType, pair.Value)))
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
                        ini = new Ini(ToDictionary(
                            Default.targetQuantities.Select(pair => new KeyValuePair<MyItemType, int>(
                                pair.Key,
                                Parser.Get(IniSection, ItemDefinitionsByItemType[pair.Key].IniName).ToInt32(pair.Value)
                            ))
                        ));
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

                foreach (var pair in ini.targetQuantities)
                {
                    Parser.Set(IniSection, ItemDefinitionsByItemType[pair.Key].IniName, pair.Value);
                }

                block.CustomData = Parser.ToString();

                return ini;
            }

            private readonly IDictionary<MyItemType, int> targetQuantities;

            public Ini(IDictionary<MyItemType, int> targetQuantities)
            {
                this.targetQuantities = targetQuantities;
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

            public static readonly MyItemType RadioCommunicationComponent =
                MyItemType.MakeComponent("RadioCommunication");

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

            public static readonly MyItemType HydrogenBottle =
                new MyItemType("MyObjectBuilder_GasContainerObject", "HydrogenBottle");

            public static readonly MyItemType OxygenBottle =
                new MyItemType("MyObjectBuilder_GasContainerObject", "OxygenBottle");

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

            public static readonly MyItemType
                Mr50aMagazine = MyItemType.MakeAmmo("RapidFireAutomaticRifleGun_Mag_50rd");

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
                .Aggregate(EmptyInventory, (accumulator, block) => ToDictionary(
                    accumulator.Select(pair => new KeyValuePair<MyItemType, int>(
                        pair.Key,
                        pair.Value + block.GetItemAmount(pair.Key).ToIntSafe())
                    ))
                );

            foreach (var pair in inventory)
            {
                Echo($"{ItemDefinitionsByItemType[pair.Key].IniName}: {pair.Value}");
            }
        }
    }
}