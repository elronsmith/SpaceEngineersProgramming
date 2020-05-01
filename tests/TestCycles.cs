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
 * пример с циклами, периодическое выполнение метода Main()
 * показываем счетчик на LCD
 * что используется:
 * - MyTextPanel
 * - MyProgrammableBlock
 * - кнопочная панель c кнопками "start" и "stop"
 */
namespace SpaceEngineers.TestCycles
{
    public sealed class Program : MyGridProgram
    {
        string NAME_PB = "Test PC";
        string NAME_LCD = "Test LCD 1";
        string CMD_START = "start";
        string CMD_STOP = "stop";

        IMyProgrammableBlock pb;
        IMyTextPanel lcd;

        int counter;

        public Program()
        {
            pb = GridTerminalSystem.GetBlockWithName(NAME_PB) as IMyProgrammableBlock;
            lcd = GridTerminalSystem.GetBlockWithName(NAME_LCD) as IMyTextPanel;

            if (pb != null && lcd != null)
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
            Echo("updateSource:" + updateSource.ToString());
            Echo("argument:" + argument);
            if (pb != null && lcd != null)
            {
                if ((updateSource & UpdateType.Trigger) > 0)
                {
                    // выполнение из кнопки
                    if (CMD_START.Equals(argument))
                    {
                        Runtime.UpdateFrequency = UpdateFrequency.Update100; // каждые 10 тиков
                        counter = 0;
                    }
                    else if (CMD_STOP.Equals(argument))
                    {
                        Runtime.UpdateFrequency = UpdateFrequency.None;
                        Echo("status STOPPED");
                    }
                }
                else
                {
                    // выполнение из терминала
                    lcd.WriteText("count: " + counter);
                    counter++;
                    
                    if ((updateSource & UpdateType.Terminal) > 0)
                        Echo("status TERMINAL");
                    else
                        Echo("status WORKING");
                }
            }
            else
            {
                Echo("status ERROR");
            }
        }
    }
}
