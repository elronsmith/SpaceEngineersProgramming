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

/*
--- properties
OnOff
ShowInTerminal
ShowInInventory
ShowInToolbarConfig
Name
ShowOnHUD
Range
BroadcastUsingAntennas
--- actions
OnOff
OnOff_On
OnOff_Off
ShowOnHUD
ShowOnHUD_On
ShowOnHUD_Off
BroadcastUsingAntennas
*/
/**
 * детектор руды
 * на данный момент нет возможности получать координаты GPS c рудой
 */
namespace SpaceEngineers.MyOreDetector
{
    public sealed class Program : MyGridProgram
    {

        public Program()
        {
            Echo("init OK");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            List<IMyOreDetector> oresList = new List<IMyOreDetector>();
            GridTerminalSystem.GetBlocksOfType<IMyOreDetector>(oresList);
            if (oresList.Count() > 0)
            {
                IMyOreDetector detector = oresList[0];
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Range: ").Append(detector.Range);
                stringBuilder.Append("\nBroadcastUsingAntennas: ").Append(detector.BroadcastUsingAntennas);

                // Range: 50
                // BroadcastUsingAntennas: True
                string data = stringBuilder.ToString();
                detector.CustomData = data;
                Me.GetSurface(0).WriteText(data);
            } else {
                Me.GetSurface(0).WriteText("ore ZERO");
            }
        }
    }
}