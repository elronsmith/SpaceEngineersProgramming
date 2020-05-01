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
 * пример работы с Поршнем
 * 
 */
namespace SpaceEngineers.MyExtendedPistonBase
{
    public sealed class Program : MyGridProgram
    {
        string NAME_PISTON = "Test Piston";
        string ARG_EXTEND = "extend";
        string ARG_RETRACT = "retract";
        string ARG_REVERSE = "reverse";

        IMyExtendedPistonBase piston;

        public Program()
        {
            piston = GridTerminalSystem.GetBlockWithName(NAME_PISTON) as IMyExtendedPistonBase;
            if (piston != null)
            {
                Echo("Status: " + piston.Status);
                Echo("Velocity: " + piston.Velocity);
                Echo("MaxVelocity: " + piston.MaxVelocity);
                Echo("MinLimit: " + piston.MinLimit);
                Echo("MaxLimit: " + piston.MaxLimit);   // максимальная длина указанная в блоке
                Echo("LowestPosition: " + piston.LowestPosition);
                Echo("HighestPosition: " + piston.HighestPosition);
                Echo("CurrentPosition: " + piston.CurrentPosition);
            }
            else
                Echo("init ERROR");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (piston == null)
            {
                Echo("Error");
                return;
            }

            if (ARG_EXTEND.Equals(argument))
            {
                piston.Extend();
            } else if (ARG_RETRACT.Equals(argument))
            {
                piston.Retract();
            } else if (ARG_REVERSE.Equals(argument))
            {
                piston.Reverse();
            }
        }
    }
}
