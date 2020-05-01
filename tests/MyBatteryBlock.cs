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
 * пример работы с группой аккумуляторов, выводим общий заряд на LCD в процентах
 * группу батарей и LCD передаём в аргументы
 * формат: <имя группы батарей>,<имя LCD>
 */
namespace SpaceEngineers.MyBatteryBlock
{
    public sealed class Program : MyGridProgram
    {
        string NAME_BATTERY_GROUP = "BatteryGroup";
        string NAME_LCD = "Test LCD 1";

        IMyTextPanel lcd = null;
        List<IMyBatteryBlock> batteryList = new List<IMyBatteryBlock>();

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            initBlock(NAME_LCD);
            initBlock(NAME_BATTERY_GROUP);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // Echo("argument: " + argument);
            // Echo("updateSource: " + updateSource);

            if (lcd == null)
            {
                Echo("ERROR: need LCD");
            }
            else if (batteryList.Count == 0)
            {
                lcd.WriteText("ERROR: need BATTERY");
            }
            else
            {
                // отображаем заряд
                float CurrentStoredPowerSum = 0f;
                float MaxStoredPowerSum = 0f;
                foreach (var battery in batteryList)
                {
                    CurrentStoredPowerSum += battery.CurrentStoredPower;
                    MaxStoredPowerSum += battery.MaxStoredPower;
                }
                float value = (CurrentStoredPowerSum / MaxStoredPowerSum) * 100;
                lcd.WriteText(String.Format("{0:#}%", value));
            }
        }

        private void initBlock(string name)
        {
            Echo("initBlock() " + name);
            var block = GridTerminalSystem.GetBlockWithName(name);
            if (block != null)
            {
                if (block is IMyTextPanel)
                    lcd = block as IMyTextPanel;
            }
            else
            {
                var group = GridTerminalSystem.GetBlockGroupWithName(name);
                if (group != null)
                {
                    if (group is IMyBlockGroup)
                    {
                        batteryList.Clear();
                        (group as IMyBlockGroup).GetBlocksOfType<IMyBatteryBlock>(batteryList);
                    }
                }
            }
        }
    }
}
