using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

/**
 * пример работы с освещением
 * включаем/выключаем лампу зеленого света
 */
namespace SpaceEngineers.MyInteriorLight
{
    public sealed class Program : MyGridProgram
    {
        string NAME_LIGHT = "Test Lamp 1";
        Color color = Color.Green;
        IMyInteriorLight light;

        public Program()
        {
            light = GridTerminalSystem.GetBlockWithName(NAME_LIGHT) as IMyInteriorLight;
            light.SetValueColor("Color", color);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            light.Enabled = !light.Enabled;
        }
    }
}
