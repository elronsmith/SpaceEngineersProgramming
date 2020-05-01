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
 * соединитель (который не передаёт компоненты)
 */
namespace SpaceEngineers.MyShipMergeBlock
{
    public sealed class Program : MyGridProgram
    {
        string NAME_MERGE_BLOCK = "Соединитель dron";
        string CMD_ON = "on";
        string CMD_OFF = "off";

        IMyShipMergeBlock mergeBlock;
        List<ITerminalAction> resultList = new List<ITerminalAction>();

        public Program()
        {
            mergeBlock = GridTerminalSystem.GetBlockWithName(NAME_MERGE_BLOCK) as IMyShipMergeBlock;

            if (mergeBlock != null)
            {
                Echo("init OK");
                Echo("CustomName: " + mergeBlock.CustomName);
                Echo("IsConnected: " + mergeBlock.IsConnected);
                mergeBlock.GetActions(resultList);
                foreach (var item in resultList)
                {
                    Echo("  action: " + item.Name.ToString());
                }
            }
            else
                Echo("init ERROR");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (mergeBlock != null && CMD_OFF.Equals(argument))
            {
                mergeBlock.ApplyAction("OnOff_Off");
            }
        }
    }
}
