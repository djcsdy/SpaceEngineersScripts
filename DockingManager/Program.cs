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

        private readonly MyIni _ini = new MyIni();
        private readonly List<IMyCockpit> _cockpits = new List<IMyCockpit>();
        private readonly List<IMyRemoteControl> _remoteControls = new List<IMyRemoteControl>();
        private readonly List<IMyShipConnector> _connectors = new List<IMyShipConnector>();
        private readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>();
        private readonly List<IMyGasGenerator> _generators = new List<IMyGasGenerator>();
        private readonly List<IMyGasTank> _hydrogenTanks = new List<IMyGasTank>();
        private readonly List<IMyMotorSuspension> _motors = new List<IMyMotorSuspension>();
        private readonly List<IMyThrust> _thrusters = new List<IMyThrust>();
        private readonly List<IMyGyro> _gyroscopes = new List<IMyGyro>();
        private bool _rechargeBatteries = true;
        private bool _toggleGenerators = true;
        private bool _stockpileHydrogen = true;
        private bool _toggleMotors = true;
        private bool _toggleThrusters = true;
        private bool _toggleGyroscopes = true;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        private void Main(string argument)
        {
            _ini.Clear();
            if (_ini.TryParse(Me.CustomData))
            {
                _rechargeBatteries = _ini.Get(IniSection, IniRechargeBatteries).ToBoolean(true);
                _toggleGenerators = _ini.Get(IniSection, IniToggleGenerators).ToBoolean(true);
                _stockpileHydrogen = _ini.Get(IniSection, IniStockpileHydrogen).ToBoolean(true);
                _toggleMotors = _ini.Get(IniSection, IniToggleMotors).ToBoolean(true);
                _toggleThrusters = _ini.Get(IniSection, IniToggleThrusters).ToBoolean(true);
                _toggleGyroscopes = _ini.Get(IniSection, IniToggleGyroscopes).ToBoolean(true);
            }
            else if (!string.IsNullOrWhiteSpace(Me.CustomData))
            {
                _ini.EndContent = Me.CustomData;
            }

            _ini.Set(IniSection, IniRechargeBatteries, _rechargeBatteries);
            _ini.Set(IniSection, IniToggleGenerators, _toggleGenerators);
            _ini.Set(IniSection, IniStockpileHydrogen, _stockpileHydrogen);
            _ini.Set(IniSection, IniToggleMotors, _toggleMotors);
            _ini.Set(IniSection, IniToggleThrusters, _toggleThrusters);
            _ini.Set(IniSection, IniToggleGyroscopes, _toggleGyroscopes);
            Me.CustomData = _ini.ToString();
            
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
                if (_rechargeBatteries)
                {
                    _batteries.ForEach(battery => battery.ChargeMode = ChargeMode.Auto);
                }
                
                if (_toggleGenerators)
                {
                    _generators.ForEach(generator => generator.Enabled = true);
                }
                
                if (_stockpileHydrogen)
                {
                    _hydrogenTanks.ForEach(tank => tank.Stockpile = false);
                }
                
                if (_toggleMotors)
                {
                    _motors.ForEach(motor => motor.Enabled = true);
                    _motors.ForEach(motor => motor.Brake = false);
                }
                
                if (_toggleThrusters)
                {
                    _thrusters.ForEach(thruster => thruster.Enabled = true);
                }

                if (_toggleGyroscopes)
                {
                    _gyroscopes.ForEach(gyroscope => gyroscope.Enabled = true);
                }
                
                _connectors.ForEach(connector => connector.Disconnect());
            }
            else
            {
                if (_rechargeBatteries)
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
                
                if (_toggleGenerators)
                {
                    _generators.ForEach(generator => generator.Enabled = false);
                }
                
                if (_stockpileHydrogen)
                {
                    _hydrogenTanks.ForEach(tank => tank.Stockpile = true);
                }
                
                if (_toggleMotors)
                {
                    _motors.ForEach(motor => motor.Enabled = false);
                }
                
                if (_toggleThrusters)
                {
                    _thrusters.ForEach(thruster => thruster.Enabled = false);
                }

                if (_toggleGyroscopes)
                {
                    _gyroscopes.ForEach(gyroscope => gyroscope.Enabled = false);
                }
            }
        }
    }
}