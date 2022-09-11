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
        private static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> pairs)
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
                new[]
                {
                    new ItemDefinition("Bulletproof Glass", MyItemType.MakeComponent("BulletproofGlass"),
                        MyDefinitionId.Parse("BlueprintDefinition/BulletproofGlass")),
                    new ItemDefinition("Computers", MyItemType.MakeComponent("Computer"),
                        MyDefinitionId.Parse("BlueprintDefinition/ComputerComponent")),
                    new ItemDefinition("Construction Components", MyItemType.MakeComponent("Construction"),
                        MyDefinitionId.Parse("BlueprintDefinition/ConstructionComponent")),
                    new ItemDefinition("Detector Components", MyItemType.MakeComponent("Detector"),
                        MyDefinitionId.Parse("BlueprintDefinition/DetectorComponent")),
                    new ItemDefinition("Displays", MyItemType.MakeComponent("Display"),
                        MyDefinitionId.Parse("BlueprintDefinition/Display")),
                    new ItemDefinition("Explosives", MyItemType.MakeComponent("Explosives"),
                        MyDefinitionId.Parse("BlueprintDefinition/ExplosivesComponent")),
                    new ItemDefinition("Girders", MyItemType.MakeComponent("Girder"),
                        MyDefinitionId.Parse("BlueprintDefinition/GirderComponent")),
                    new ItemDefinition("Gravity Components", MyItemType.MakeComponent("GravityGenerator"),
                        MyDefinitionId.Parse("BlueprintDefinition/GravityGeneratorComponent")),
                    new ItemDefinition("Interior Plates", MyItemType.MakeComponent("InteriorPlate"),
                        MyDefinitionId.Parse("BlueprintDefinition/InteriorPlate")),
                    new ItemDefinition("Large Steel Tubes", MyItemType.MakeComponent("LargeTube"),
                        MyDefinitionId.Parse("BlueprintDefinition/LargeTube")),
                    new ItemDefinition("Medical Components", MyItemType.MakeComponent("Medical"),
                        MyDefinitionId.Parse("BlueprintDefinition/MedicalComponent")),
                    new ItemDefinition("Metal Grids", MyItemType.MakeComponent("MetalGrid"),
                        MyDefinitionId.Parse("BlueprintDefinition/MetalGrid")),
                    new ItemDefinition("Motors", MyItemType.MakeComponent("Motor"),
                        MyDefinitionId.Parse("BlueprintDefinition/MotorComponent")),
                    new ItemDefinition("Power Cells", MyItemType.MakeComponent("PowerCell"),
                        MyDefinitionId.Parse("BlueprintDefinition/PowerCell")),
                    new ItemDefinition("Radio Communication Components",
                        MyItemType.MakeComponent("RadioCommunication"),
                        MyDefinitionId.Parse("BlueprintDefinition/RadioCommunicationComponent")),
                    new ItemDefinition("Reactor Components", MyItemType.MakeComponent("Reactor"),
                        MyDefinitionId.Parse("BlueprintDefinition/ReactorComponent")),
                    new ItemDefinition("Small Steel Tubes", MyItemType.MakeComponent("SmallTube"),
                        MyDefinitionId.Parse("BlueprintDefinition/SmallTube")),
                    new ItemDefinition("Solar Cells", MyItemType.MakeComponent("SolarCell"),
                        MyDefinitionId.Parse("BlueprintDefinition/SolarCell")),
                    new ItemDefinition("Steel Plates", MyItemType.MakeComponent("SteelPlate"),
                        MyDefinitionId.Parse("BlueprintDefinition/SteelPlate")),
                    new ItemDefinition("Superconductors", MyItemType.MakeComponent("Superconductor"),
                        MyDefinitionId.Parse("BlueprintDefinition/Superconductor")),
                    new ItemDefinition("Thruster Components", MyItemType.MakeComponent("Thrust"),
                        MyDefinitionId.Parse("BlueprintDefinition/ThrustComponent")),
                    new ItemDefinition("PRO-1s", MyItemType.MakeTool("AdvancedHandHeldLauncherItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/AdvancedHandHeldLauncher")),
                    new ItemDefinition("Grinders", MyItemType.MakeTool("AngleGrinderItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/AngleGrinder")),
                    new ItemDefinition("Enhanced Grinders", MyItemType.MakeTool("AngleGrinder2Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/AngleGrinde2")),
                    new ItemDefinition("Proficient Grinders", MyItemType.MakeTool("AngleGrinder3Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/AngleGrinder3")),
                    new ItemDefinition("Elite Grinders", MyItemType.MakeTool("AngleGrinder4Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/AngleGrinder4")),
                    new ItemDefinition("MR-20s", MyItemType.MakeTool("AutomaticRifleItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/AutomaticRifle")),
                    new ItemDefinition("RO-1s", MyItemType.MakeTool("BasicHandHeldLauncherItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/BasicHandHeldLauncher")),
                    new ItemDefinition("Datapads", new MyItemType("MyObjectBuilder_Datapad", "Datapad"),
                        MyDefinitionId.Parse("BlueprintDefinition/Datapad")),
                    new ItemDefinition("S-10Es", MyItemType.MakeTool("ElitePistolItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/EliteAutoPistol")),
                    new ItemDefinition("S-20As", MyItemType.MakeTool("FullAutoPistolItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/FullAutoPistol")),
                    new ItemDefinition("Hand Drills", MyItemType.MakeTool("HandDrillItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/HandDrill")),
                    new ItemDefinition("Enhanced Hand Drills", MyItemType.MakeTool("HandDrill2Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/HandDrill2")),
                    new ItemDefinition("Proficient Hand Drills", MyItemType.MakeTool("HandDrill3Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/HandDrill3")),
                    new ItemDefinition("Elite Hand Drills", MyItemType.MakeTool("HandDrill4Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/HandDrill4")),
                    // new ItemDefinition("Hydrogen Bottles",
                    //     new MyItemType("MyObjectBuilder_GasContainerObject", "HydrogenBottle"),
                    //     MyDefinitionId.Parse("BlueprintDefinition/HydrogenBottle")),
                    // new ItemDefinition("Oxygen Bottles",
                    //     new MyItemType("MyObjectBuilder_GasContainerObject", "OxygenBottle"),
                    //     MyDefinitionId.Parse("BlueprintDefinition/OxygenBottle")),
                    new ItemDefinition("MR-8Ps", MyItemType.MakeTool("PreciseAutomaticRifleItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/PreciseAutomaticRifle")),
                    new ItemDefinition("MR-50As", MyItemType.MakeTool("RapidFireAutomaticRifleItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/RapidFireAutomaticRifle")),
                    new ItemDefinition("S-10s", MyItemType.MakeTool("SemiAutoPistolItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/SemiAutoPistol")),
                    new ItemDefinition("MR-30Es", MyItemType.MakeTool("UltimateAutomaticRifleItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/UltimateAutomaticRifle")),
                    new ItemDefinition("Welders", MyItemType.MakeTool("WelderItem"),
                        MyDefinitionId.Parse("BlueprintDefinition/Welder")),
                    new ItemDefinition("Enhanced Welders", MyItemType.MakeTool("Welder2Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/Welder2")),
                    new ItemDefinition("Proficient Welders", MyItemType.MakeTool("Welder3Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/Welder3")),
                    new ItemDefinition("Elite Welders", MyItemType.MakeTool("Welder4Item"),
                        MyDefinitionId.Parse("BlueprintDefinition/Welder4")),
                    new ItemDefinition("Autocannon Magazines", MyItemType.MakeAmmo("AutocannonClip"),
                        MyDefinitionId.Parse("BlueprintDefinition/AutocannonClip")),
                    new ItemDefinition("MR-20 Magazines", MyItemType.MakeAmmo("AutomaticRifleGun_Mag_20rd"),
                        MyDefinitionId.Parse("BlueprintDefinition/AutomaticRifleGun_Mag_20rd")),
                    new ItemDefinition("Canvases", MyItemType.MakeComponent("Canvas"),
                        MyDefinitionId.Parse("BlueprintDefinition/Canvas")),
                    new ItemDefinition("S-10E Magazines", MyItemType.MakeAmmo("ElitePistolMagazine"),
                        MyDefinitionId.Parse("BlueprintDefinition/ElitePistolMagazine")),
                    new ItemDefinition("S-20A Magazines", MyItemType.MakeAmmo("FullAutoPistolMagazine"),
                        MyDefinitionId.Parse("BlueprintDefinition/FullAutoPistolMagazine")),
                    new ItemDefinition("Artillery Shells", MyItemType.MakeAmmo("LargeCalibreAmmo"),
                        MyDefinitionId.Parse("BlueprintDefinition/LargeCalibreAmmo")),
                    new ItemDefinition("Large Railgun Sabots", MyItemType.MakeAmmo("LargeRailgunAmmo"),
                        MyDefinitionId.Parse("BlueprintDefinition/LargeRailgunAmmo")),
                    new ItemDefinition("Assault Cannon Shells", MyItemType.MakeAmmo("MediumCalibreAmmo"),
                        MyDefinitionId.Parse("BlueprintDefinition/MediumCalibreAmmo")),
                    new ItemDefinition("Missiles", MyItemType.MakeAmmo("Missile200mm"),
                        MyDefinitionId.Parse("BlueprintDefinition/Missile200mm")),
                    new ItemDefinition("Gatling Ammo Boxes", MyItemType.MakeAmmo("NATO_25x184mm"),
                        MyDefinitionId.Parse("BlueprintDefinition/NATO_25x184mmMagazine")),
                    new ItemDefinition("MR-8P Magazines", MyItemType.MakeAmmo("PreciseAutomaticRifleGun_Mag_5rd"),
                        MyDefinitionId.Parse("BlueprintDefinition/PreciseAutomaticRifleGun_Mag_5rd")),
                    new ItemDefinition("MR-50A Magazines", MyItemType.MakeAmmo("RapidFireAutomaticRifleGun_Mag_50rd"),
                        MyDefinitionId.Parse("BlueprintDefinition/RapidFireAutomaticRifleGun_Mag_50rd")),
                    new ItemDefinition("S-10 Magazines", MyItemType.MakeAmmo("SemiAutoPistolMagazine"),
                        MyDefinitionId.Parse("BlueprintDefinition/SemiAutoPistolMagazine")),
                    new ItemDefinition("Small Railgun Sabots", MyItemType.MakeAmmo("SmallRailgunAmmo"),
                        MyDefinitionId.Parse("BlueprintDefinition/SmallRailgunAmmo")),
                    new ItemDefinition("MR-30E Magazines", MyItemType.MakeAmmo("UltimateAutomaticRifleGun_Mag_30rd"),
                        MyDefinitionId.Parse("BlueprintDefinition/UltimateAutomaticRifleGun_Mag_30rd"))
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

        private static readonly IDictionary<MyItemType, int> EmptyInventory = ToDictionary(
            ItemDefinitionsByItemType.Keys
                .Select(key => new KeyValuePair<MyItemType, int>(key, 0))
        );

        private struct ItemDefinition
        {
            public readonly string IniName;
            public readonly MyItemType ItemType;
            public readonly MyDefinitionId Blueprint;

            public ItemDefinition(string iniName, MyItemType itemType, MyDefinitionId blueprint)
            {
                IniName = iniName;
                ItemType = itemType;
                Blueprint = blueprint;
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
                            // new KeyValuePair<string, int>("Hydrogen Bottles", 6),
                            // new KeyValuePair<string, int>("Oxygen Bottles", 6),
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
                            Default.TargetQuantities.Select(pair => new KeyValuePair<MyItemType, int>(
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

                foreach (var pair in ini.TargetQuantities)
                {
                    Parser.Set(IniSection, ItemDefinitionsByItemType[pair.Key].IniName, pair.Value);
                }

                block.CustomData = Parser.ToString();

                return ini;
            }

            public readonly IDictionary<MyItemType, int> TargetQuantities;

            public Ini(IDictionary<MyItemType, int> targetQuantities)
            {
                this.TargetQuantities = targetQuantities;
            }
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

            GridTerminalSystem.GetBlocksOfType(_assemblers,
                assembler => assembler.CubeGrid == Me.CubeGrid
                             && assembler.Enabled
                             && assembler.IsFunctional
                             && !assembler.CooperativeMode);
            if (_assemblers.Count == 0)
            {
                return;
            }

            GridTerminalSystem.GetBlocksOfType(_blocksWithInventory,
                block => block.CubeGrid == Me.CubeGrid && block.HasInventory);
            var inventory = _blocksWithInventory
                .SelectMany(block => Enumerable.Range(0, block.InventoryCount).Select(block.GetInventory))
                .Aggregate(EmptyInventory, (accumulator, block) => ToDictionary(
                    accumulator.Select(pair => new KeyValuePair<MyItemType, int>(
                        pair.Key,
                        pair.Value + block.GetItemAmount(pair.Key).ToIntSafe())
                    ))
                );

            var mostNeeded = inventory
                .Select(pair => new
                {
                    ItemType = pair.Key,
                    Ratio = (float)pair.Value / ini.TargetQuantities[pair.Key]
                })
                .MinBy(item => item.Ratio)
                .ItemType;

            var neededQuantity = ini.TargetQuantities[mostNeeded] - inventory[mostNeeded];
            var quantityToProduce = Math.Ceiling(neededQuantity * 0.1f);

            Echo($"Most Needed: {ItemDefinitionsByItemType[mostNeeded].IniName}");
            Echo($"Needed Quantity: {neededQuantity}");
            Echo($"Quantity To Produce: {quantityToProduce}");

            if (quantityToProduce < 1d)
            {
                return;
            }

            var firstAssembler = _assemblers.First();
            if (firstAssembler.IsQueueEmpty)
            {
                firstAssembler.Mode = MyAssemblerMode.Assembly;
                firstAssembler.AddQueueItem(ItemDefinitionsByItemType[mostNeeded].Blueprint, quantityToProduce);
            }
        }
    }
}