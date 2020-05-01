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
 * шаблон
 */
namespace SpaceEngineers.MyTemplate
{
    public sealed class Program : MyGridProgram
    {

        public Program()
        {
            Echo("init OK");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            var block = GridTerminalSystem.GetBlockWithName(argument);
            Echo(block.ToString());
        }
    }
}

/**
 * шаблон 2
 * отображает все свойства объекта и как с ним взаимодействовать
 */
namespace SpaceEngineers.MyTemplate2
{
    public sealed class Program : MyGridProgram
    {
        string NAME_BLOCK = "Дистанционное управление 2";

        IMyTerminalBlock block;

        public Program()
        {
            block = GridTerminalSystem.GetBlockWithName(NAME_BLOCK);
            if (block != null)
            {
                Echo("init OK");
            }
            else
            {
                Echo("init ERROR");
            }
        }

        public void Main(string argument, UpdateType updateSource)
        {
            var actions = new List<ITerminalAction>();
            var properties = new List<ITerminalProperty>();

            block.GetProperties(properties);
            block.GetActions(actions);

            StringBuilder sb = new StringBuilder();
            sb.Append("--- properties");
            foreach (var item in properties)
            {
                sb.Append("\n").Append(item.Id);
            }
            sb.Append("\n--- actions");
            foreach (var item in actions)
            {
                sb.Append("\n").Append(item.Id);
            }

            Me.GetSurface(0).WriteText(sb.ToString());
            Echo(block.ToString());
        }
    }
}
