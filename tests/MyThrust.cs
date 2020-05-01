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
Override
--- actions
OnOff
OnOff_On
OnOff_Off
ShowOnHUD
ShowOnHUD_On
ShowOnHUD_Off
IncreaseOverride
DecreaseOverride
*/
/**
 * пример работы с двигателями
 * !! движкам нельзя передавать отрицательное значение
 */
namespace SpaceEngineers.MyThrust
{
    public sealed class Program : MyGridProgram
    {
        string NAME_THRUST = "Атмосферный ускоритель";

        IMyThrust thrust;

        public Program()
        {
            thrust = GridTerminalSystem.GetBlockWithName(NAME_THRUST) as IMyThrust;

            if (thrust != null)
            {
                Echo("init OK");
                Echo("ThrustOverride: " + thrust.ThrustOverride);
                Echo("ThrustOverridePercentage: " + thrust.ThrustOverridePercentage);
                Echo("MaxThrust: " + thrust.MaxThrust);
                Echo("MaxEffectiveThrust: " + thrust.MaxEffectiveThrust);
                Echo("CurrentThrust: " + thrust.CurrentThrust);
                Echo(string.Format("GridThrustDirection: {0}, {1}, {2}", thrust.GridThrustDirection.X, thrust.GridThrustDirection.Y, thrust.GridThrustDirection.Z));
                Echo("Position.X: " + thrust.Position.X);
                Echo("Position.Y: " + thrust.Position.Y);
                Echo("Position.Z: " + thrust.Orientation);

            }
            else
                Echo("init ERROR");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (thrust != null)
            {

            }
            else
                Echo("run ERROR");
        }
    }
}

/**
 * отображает список двигателей
 */
namespace SpaceEngineers.MyThrust2
{
    public sealed class Program : MyGridProgram
    {
        List<IMyThrust> list = new List<IMyThrust>();
        List<IMyThrust> backwardList = new List<IMyThrust>();
        List<IMyThrust> downList = new List<IMyThrust>();

        public Program()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
            list.Clear();
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(list);

            backwardList.Clear();
            downList.Clear();
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                // Backward
                // Down
                sb.Append(item.DisplayNameText).Append(":\n")
                    .Append("  ").Append(item.Orientation.Forward).Append("\n");
                
                if (item.Orientation.Forward == Base6Directions.Direction.Backward)
                    backwardList.Add(item);
                if (item.Orientation.Forward == Base6Directions.Direction.Down)
                    downList.Add(item);
            }

            Me.GetSurface(0).WriteText(sb.ToString());

            foreach (var item in backwardList)
            {
                item.ThrustOverridePercentage = 0.5f;
            }
        }
    }
}
