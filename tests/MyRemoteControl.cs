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
ShowInTerminal
ShowInInventory
ShowInToolbarConfig
Name
ShowOnHUD
ControlThrusters
ControlWheels
ControlGyros
HandBrake
DampenersOverride
HorizonIndicator
MainCockpit
MainRemoteControl
AutoPilot
CollisionAvoidance
DockingMode
CameraList
FlightMode
Direction
SpeedLimit
--- actions
ShowOnHUD
ShowOnHUD_On
ShowOnHUD_Off
ControlThrusters
ControlWheels
ControlGyros
HandBrake
DampenersOverride
HorizonIndicator
MainCockpit
MainRemoteControl
AutoPilot
AutoPilot_On
AutoPilot_Off
CollisionAvoidance
CollisionAvoidance_On
CollisionAvoidance_Off
DockingMode
DockingMode_On
DockingMode_Off
IncreaseSpeedLimit
DecreaseSpeedLimit
Вперед
Назад
Влево
Вправо
Вверх
Вниз
*/
/**
 * Дистанционное управление
 */
namespace SpaceEngineers.MyRemoteControl
{
    public sealed class Program : MyGridProgram
    {
        string NAME_REMOTE = "Remote dron";

        IMyRemoteControl remoteControl;
        List<ITerminalAction> resultList = new List<ITerminalAction>();

        public Program()
        {
            remoteControl = GridTerminalSystem.GetBlockWithName(NAME_REMOTE) as IMyRemoteControl;

            if (remoteControl != null)
            {
                Echo("init OK");
                Echo("IsAutoPilotEnabled: " + remoteControl.IsAutoPilotEnabled);
                Echo("SpeedLimit: " + remoteControl.SpeedLimit);
                Echo("FlightMode: " + remoteControl.FlightMode);    // Patrol, Circle, OneWay
                remoteControl.GetActions(resultList);
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
            
        }
    }
}
