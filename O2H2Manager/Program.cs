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

namespace SpaceEngineersScripts
{
    public class Program : MyGridProgram
    {
        private const string IniSection = "Deer Industries O2H2Manager";
        private const string IniMinOxygenPercent = "Minimum Oxygen Percent";
        private const string IniMinHydrogenPercent = "Minimum Hydrogen Percent";

        private readonly MyIni _ini = new MyIni();
        private readonly List<IMyGasTank> _oxygenTanks = new List<IMyGasTank>();
        private readonly List<IMyGasTank> _hydrogenTanks = new List<IMyGasTank>();
        private readonly List<IMyGasGenerator> _gasGenerators = new List<IMyGasGenerator>();
        private readonly List<MyInventoryItem> _items = new List<MyInventoryItem>();
        private readonly IMyTextSurface _textSurface;
        private double _minOxygenPercent = 20d;
        private double _minHydrogenPercent = 20d;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            _textSurface = Me.GetSurface(0);
        }

        private void Main(string argument)
        {
            _ini.Clear();
            if (_ini.TryParse(Me.CustomData))
            {
                _minOxygenPercent = _ini.Get(IniSection, IniMinOxygenPercent).ToDouble(20d);
                _minHydrogenPercent = _ini.Get(IniSection, IniMinHydrogenPercent).ToDouble(20d);
            }
            else if (!string.IsNullOrWhiteSpace(Me.CustomData))
            {
                _ini.EndContent = Me.CustomData;
            }

            _ini.Set(IniSection, IniMinOxygenPercent, _minOxygenPercent);
            _ini.Set(IniSection, IniMinHydrogenPercent, _minHydrogenPercent);
            Me.CustomData = _ini.ToString();

            GridTerminalSystem.GetBlocksOfType(_oxygenTanks,
                tank => tank.CubeGrid == Me.CubeGrid && !tank.BlockDefinition.SubtypeId.Contains("Hydrogen"));
            GridTerminalSystem.GetBlocksOfType(_hydrogenTanks,
                tank => tank.CubeGrid == Me.CubeGrid && tank.BlockDefinition.SubtypeId.Contains("Hydrogen"));
            GridTerminalSystem.GetBlocksOfType(_gasGenerators, generator => generator.CubeGrid == Me.CubeGrid);

            var oxygenPercent =
                (_oxygenTanks.Count == 0 ? 1d : _oxygenTanks.Select(tank => tank.FilledRatio).Average()) * 100;
            var hydrogenPercent = (_hydrogenTanks.Count == 0
                ? 1d
                : _hydrogenTanks.Select(tank => tank.FilledRatio).Average()) * 100;

            if (oxygenPercent < _minOxygenPercent || hydrogenPercent < _minHydrogenPercent)
            {
                _gasGenerators.ForEach(generator => generator.Enabled = true);
            }
            else
            {
                _gasGenerators.ForEach(generator =>
                {
                    generator.GetInventory().GetItems(_items,
                        item => item.Type.TypeId == "MyObjectBuilder_Ore" && item.Type.SubtypeId == "Ice");
                    generator.Enabled = _items.All(item => item.Amount == MyFixedPoint.Zero);
                });
            }

            Echo($"Oxygen: {RoundSignificantFigures(oxygenPercent, 2)}% of {_oxygenTanks.Count} tanks\n" +
                 $"Hydrogen: {RoundSignificantFigures(hydrogenPercent, 2)}% of {_hydrogenTanks.Count} tanks");
        }

        private static double RoundSignificantFigures(double d, int digits)
        {
            if (d == 0)
            {
                return 0;
            }

            var scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }
    }
}