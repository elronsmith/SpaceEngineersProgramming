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
 * камера
 */
namespace SpaceEngineers.MyCameraBlock
{
    public sealed class Program : MyGridProgram
    {
        string NAME_CAMERA = "Test cockpit";

        IMyCameraBlock camera;

        public Program()
        {
            camera = GridTerminalSystem.GetBlockWithName(NAME_CAMERA) as IMyCameraBlock;
            if (camera != null)
            {
                Echo("init OK");


            }
            else
                Echo("init ERROR");
        }

        public void Main(string argument, UpdateType updateSource)
        {

        }
    }
}
