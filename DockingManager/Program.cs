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

namespace DockingManager
{
    public class Program : MyGridProgram
    {
        private const string IniSection = "Deer Industries DockingManager";
        private const string IniRechargeBatteries = "Recharge Batteries";
        private const string IniToggleGenerators = "Toggle O2/H2 Generators";
        private const string IniStockpileHydrogen = "Stockpile Hydrogen";
        private const string IniToggleMotors = "Toggle Motors";
        private const string IniToggleThrusters = "Toggle Thrusters";
        private const string IniToggleGyroscopes = "Toggle Gyroscopes";
        private const string IniMaintainPower = "MaintainPower";

        private const string IniRechargeBatteriesOfConnectedShipsIfOverPercent =
            "RechargeBatteriesOfConnectedShipsIfOverPercent";

        private const string IniRefillHydrogenOfConnectedShipsIfOverPercent =
            "RefillHydrogenOfConnectedShipsIfOverPercent";

        private class Ini
        {
            private static readonly MyIni Parser = new MyIni();

            private static readonly IDictionary<IMyProgrammableBlock, string> IniTextCache =
                new Dictionary<IMyProgrammableBlock, string>();

            private static readonly IDictionary<IMyProgrammableBlock, Ini> IniCache =
                new Dictionary<IMyProgrammableBlock, Ini>();

            private static readonly Ini Default = new Ini(true, true, true, true, true, true, true, 100f, 100f);

            private readonly MyGridProgram program;

            public static Ini FromBlock(MyGridProgram program, IMyProgrammableBlock block)
            {
                return FromBlockInternal(program, block, false);
            }

            public static Ini FromBlockOrSetDefault(MyGridProgram program, IMyProgrammableBlock block)
            {
                return FromBlockInternal(program, block, true);
            }

            private static Ini FromBlockInternal(MyGridProgram program, IMyProgrammableBlock block, bool setDefault)
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
                            Parser.Get(IniSection, IniRechargeBatteries).ToBoolean(Default.RechargeBatteries),
                            Parser.Get(IniSection, IniToggleGenerators).ToBoolean(Default.ToggleGenerators),
                            Parser.Get(IniSection, IniStockpileHydrogen).ToBoolean(Default.StockpileHydrogen),
                            Parser.Get(IniSection, IniToggleMotors).ToBoolean(Default.ToggleMotors),
                            Parser.Get(IniSection, IniToggleThrusters).ToBoolean(Default.ToggleThrusters),
                            Parser.Get(IniSection, IniToggleGyroscopes).ToBoolean(Default.ToggleGyroscopes),
                            Parser.Get(IniSection, IniMaintainPower).ToBoolean(Default.MaintainPower),
                            Parser.Get(IniSection, IniRechargeBatteriesOfConnectedShipsIfOverPercent)
                                .ToSingle(Default.RechargeBatteriesOfConnectedShipsIfOverPercent),
                            Parser.Get(IniSection, IniRefillHydrogenOfConnectedShipsIfOverPercent)
                                .ToSingle(Default.RefillHydrogenOfConnectedShipsIfOverPercent)
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

                Parser.Set(IniSection, IniRechargeBatteries, ini.RechargeBatteries);
                Parser.Set(IniSection, IniToggleGenerators, ini.ToggleGenerators);
                Parser.Set(IniSection, IniStockpileHydrogen, ini.StockpileHydrogen);
                Parser.Set(IniSection, IniToggleMotors, ini.ToggleMotors);
                Parser.Set(IniSection, IniToggleThrusters, ini.ToggleThrusters);
                Parser.Set(IniSection, IniToggleGyroscopes, ini.ToggleGyroscopes);
                Parser.Set(IniSection, IniMaintainPower, ini.MaintainPower);
                Parser.Set(IniSection, IniRechargeBatteriesOfConnectedShipsIfOverPercent,
                    ini.RechargeBatteriesOfConnectedShipsIfOverPercent);
                Parser.Set(IniSection, IniRefillHydrogenOfConnectedShipsIfOverPercent,
                    ini.RefillHydrogenOfConnectedShipsIfOverPercent);
                block.CustomData = Parser.ToString();

                return ini;
            }

            public readonly bool RechargeBatteries;
            public readonly bool ToggleGenerators;
            public readonly bool StockpileHydrogen;
            public readonly bool ToggleMotors;
            public readonly bool ToggleThrusters;
            public readonly bool ToggleGyroscopes;
            public readonly bool MaintainPower;
            public readonly float RechargeBatteriesOfConnectedShipsIfOverPercent;
            public readonly float RefillHydrogenOfConnectedShipsIfOverPercent;

            public Ini(bool rechargeBatteries, bool toggleGenerators, bool stockpileHydrogen, bool toggleMotors,
                bool toggleThrusters, bool toggleGyroscopes, bool maintainPower,
                float rechargeBatteriesOfConnectedShipsIfOverPercent, float refillHydrogenOfConnectedShipsIfOverPercent)
            {
                RechargeBatteries = rechargeBatteries;
                ToggleGenerators = toggleGenerators;
                StockpileHydrogen = stockpileHydrogen;
                ToggleMotors = toggleMotors;
                ToggleThrusters = toggleThrusters;
                ToggleGyroscopes = toggleGyroscopes;
                MaintainPower = maintainPower;
                RechargeBatteriesOfConnectedShipsIfOverPercent = rechargeBatteriesOfConnectedShipsIfOverPercent;
                RefillHydrogenOfConnectedShipsIfOverPercent = refillHydrogenOfConnectedShipsIfOverPercent;
            }
        }

        private readonly List<IMyCockpit> _cockpits = new List<IMyCockpit>();
        private readonly List<IMyRemoteControl> _remoteControls = new List<IMyRemoteControl>();
        private readonly List<IMyShipConnector> _connectors = new List<IMyShipConnector>();
        private readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>();
        private readonly List<IMyGasGenerator> _generators = new List<IMyGasGenerator>();
        private readonly List<IMyGasTank> _hydrogenTanks = new List<IMyGasTank>();
        private readonly List<IMyMotorSuspension> _motors = new List<IMyMotorSuspension>();
        private readonly List<IMyThrust> _thrusters = new List<IMyThrust>();
        private readonly List<IMyGyro> _gyroscopes = new List<IMyGyro>();
        private readonly List<IMyProgrammableBlock> _connectedDockingControllers = new List<IMyProgrammableBlock>();
        private readonly List<IMyBatteryBlock> _connectedBatteries = new List<IMyBatteryBlock>();
        private readonly List<IMyGasTank> _connectedHydrogenTanks = new List<IMyGasTank>();

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        private void Main(string argument)
        {
            var ini = Ini.FromBlockOrSetDefault(this, Me);

            GridTerminalSystem.GetBlocksOfType(_cockpits, cockpit => cockpit.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_remoteControls, remoteControl => remoteControl.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_connectors, connector => connector.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_batteries, battery => battery.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_generators, generator => generator.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_hydrogenTanks,
                tank => tank.CubeGrid == Me.CubeGrid && tank.BlockDefinition.SubtypeId.Contains("Hydrogen"));
            GridTerminalSystem.GetBlocksOfType(_motors, motor => motor.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_thrusters, thruster => thruster.CubeGrid == Me.CubeGrid);
            GridTerminalSystem.GetBlocksOfType(_gyroscopes, gyroscope => gyroscope.CubeGrid == Me.CubeGrid);

            if (argument.Equals("dock", StringComparison.InvariantCultureIgnoreCase))
            {
                _connectors.ForEach(connector => connector.Connect());
            }

            var connected = _connectors.Any(connector => connector.Status == MyShipConnectorStatus.Connected);

            if (argument.Equals("undock", StringComparison.InvariantCultureIgnoreCase) || !connected)
            {
                if (ini.RechargeBatteries || ini.RechargeBatteriesOfConnectedShipsIfOverPercent < 100f)
                {
                    _batteries.ForEach(battery => battery.ChargeMode = ChargeMode.Auto);
                }

                if (ini.ToggleGenerators)
                {
                    _generators.ForEach(generator => generator.Enabled = true);
                }

                if (ini.StockpileHydrogen)
                {
                    _hydrogenTanks.ForEach(tank => tank.Stockpile = false);
                }

                if (ini.ToggleMotors)
                {
                    _motors.ForEach(motor => motor.Enabled = true);
                    _motors.ForEach(motor => motor.Brake = false);
                }

                if (ini.ToggleThrusters)
                {
                    _thrusters.ForEach(thruster => thruster.Enabled = true);
                }

                if (ini.ToggleGyroscopes)
                {
                    _gyroscopes.ForEach(gyroscope => gyroscope.Enabled = true);
                }

                _connectors.ForEach(connector => connector.Disconnect());
            }
            else
            {
                var rechargeConnectedBatteries = false;
                if (ini.RechargeBatteriesOfConnectedShipsIfOverPercent < 100f)
                {
                    if (_batteries.Select(battery => battery.CurrentStoredPower / battery.MaxStoredPower).Average() *
                        100f > ini.RechargeBatteriesOfConnectedShipsIfOverPercent)
                    {
                        var directlyConnectedGrids = _connectors.Select(connector => connector.OtherConnector.CubeGrid);
                        GridTerminalSystem.GetBlocksOfType(_connectedDockingControllers,
                            block => directlyConnectedGrids.Any(grid => block.CubeGrid == grid) &&
                                     Ini.FromBlock(this, block) != null);
                        rechargeConnectedBatteries = _connectedDockingControllers.Any(controller =>
                        {
                            if (Ini.FromBlock(this, controller).RechargeBatteriesOfConnectedShipsIfOverPercent >
                                ini.RechargeBatteriesOfConnectedShipsIfOverPercent)
                            {
                                GridTerminalSystem.GetBlocksOfType(_connectedBatteries,
                                    battery => battery.CubeGrid == controller.CubeGrid);
                                return _connectedBatteries.Any(battery =>
                                    battery.CurrentStoredPower / battery.MaxStoredPower < 1f);
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                }

                var refillConnectedHydrogen = false;
                if (ini.RefillHydrogenOfConnectedShipsIfOverPercent < 100f)
                {
                    if (_hydrogenTanks.Select(tank => tank.FilledRatio).Average() * 100f >
                        ini.RefillHydrogenOfConnectedShipsIfOverPercent)
                    {
                        var directlyConnectedGrids = _connectors.Select(connector => connector.OtherConnector.CubeGrid);
                        GridTerminalSystem.GetBlocksOfType(_connectedDockingControllers,
                            block => directlyConnectedGrids.Any(grid => block.CubeGrid == grid) &&
                                     Ini.FromBlock(this, block) != null);
                        refillConnectedHydrogen = _connectedDockingControllers.Any(controller =>
                        {
                            if (Ini.FromBlock(this, controller).RefillHydrogenOfConnectedShipsIfOverPercent >
                                ini.RefillHydrogenOfConnectedShipsIfOverPercent)
                            {
                                GridTerminalSystem.GetBlocksOfType(_connectedHydrogenTanks,
                                    tank => tank.CubeGrid == controller.CubeGrid &&
                                            tank.BlockDefinition.SubtypeId.Contains("Hydrogen"));
                                return _connectedHydrogenTanks.Any(tank => tank.FilledRatio < 1d);
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                }

                if (rechargeConnectedBatteries)
                {
                    _batteries.ForEach(battery => battery.ChargeMode = ChargeMode.Discharge);
                }
                else if (ini.RechargeBatteries)
                {
                    if (_batteries.Count == 1 && ini.MaintainPower)
                    {
                        _batteries.ForEach(battery =>
                        {
                            if (battery.CurrentStoredPower < 0.99)
                            {
                                battery.ChargeMode = ChargeMode.Recharge;
                            }
                            else if (battery.CurrentStoredPower >= 1)
                            {
                                battery.ChargeMode = ChargeMode.Auto;
                            }
                        });
                    }
                    else
                    {
                        var backupBattery = ini.MaintainPower
                            ? _batteries.MaxBy(battery => battery.CurrentStoredPower / battery.MaxStoredPower)
                            : null;

                        _batteries.ForEach(battery => battery.ChargeMode = battery == backupBattery
                            ? ChargeMode.Auto
                            : ChargeMode.Recharge);
                    }
                }

                if (ini.ToggleGenerators)
                {
                    _generators.ForEach(generator => generator.Enabled = false);
                }

                if (refillConnectedHydrogen)
                {
                    _hydrogenTanks.ForEach(tank => tank.Stockpile = false);
                }
                else if (ini.StockpileHydrogen)
                {
                    _hydrogenTanks.ForEach(tank => tank.Stockpile = true);
                }

                if (ini.ToggleMotors)
                {
                    _motors.ForEach(motor => motor.Enabled = false);
                }

                if (ini.ToggleThrusters)
                {
                    _thrusters.ForEach(thruster => thruster.Enabled = false);
                }

                if (ini.ToggleGyroscopes)
                {
                    _gyroscopes.ForEach(gyroscope => gyroscope.Enabled = false);
                }
            }
        }
    }
}