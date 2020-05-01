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
 * пример работы с программируемым блоком
 * отображаем текст на LCD
 */
namespace SpaceEngineers.MyProgrammableBlock
{
    public sealed class Program : MyGridProgram
    {
        string NAME_PB = "PB dron";

        IMyProgrammableBlock pb;
        IMyTextSurface pbLcd;

        public Program()
        {
            pb = GridTerminalSystem.GetBlockWithName(NAME_PB) as IMyProgrammableBlock;
            if (pb != null)
            {
                Echo("init OK");
                Echo("EntityId: " + pb.EntityId);   // блок который мы нашли
                Echo("EntityId: " + Me.EntityId);   // блок, на котором исполняется этот скрипт
                Echo("Surface count: " + pb.SurfaceCount);
                pbLcd = pb.GetSurface(0); // or 1
                pbLcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
            }
            else
                Echo("init ERROR");

        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (pbLcd != null)
            {
                pbLcd.WriteText("Hello MyProgrammableBlock");
                Echo("run OK");
            }
            else
            {
                Echo("run ERROR");
            }
        }
    }
}

/**
 * пример работы с программируемым блоком
 * - находим все программируемые
 * - находим наш программруемый блок
 * - передаём нужному блоку команду в параметре
 */
namespace SpaceEngineers.MyProgrammableBlock2
{
    public sealed class Program : MyGridProgram
    {
        string NAME_LCD = "Test LCD 1";

        IMyTextPanel lcd;
        List<IMyProgrammableBlock> list = new List<IMyProgrammableBlock>();

        StringBuilder stringBuilder = new StringBuilder();

        public Program()
        {
            lcd = GridTerminalSystem.GetBlockWithName(NAME_LCD) as IMyTextPanel;
            GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(list);

            if (lcd != null)
                Echo("init LCD OK");
            Echo("list: " + list.Count());
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (lcd != null)
            {
                stringBuilder.Clear();
                stringBuilder.Append("count: " + list.Count());
                foreach (var item in list)
                {
                    stringBuilder.Append("\n" + item.CustomName); // Программируемый блок 5
                    // stringBuilder.Append("\n  IsRunning: " + item.IsRunning); // true
                    // stringBuilder.Append("\n  TerminalRunArgument: " + item.TerminalRunArgument); // это аргумент по-умолчанию
                    // stringBuilder.Append("\n  Position: " + item.Position.X + ", " + item.Position.Y + ", " + item.Position.Z); // 1, -4, 0
                    // stringBuilder.Append("\n" + item.GetId()); // 96243392444855564
                    // stringBuilder.Append("\n" + item.EntityId); // 96243392444855564
                    // stringBuilder.Append("\n" + item.DisplayNameText); // Программируемый блок 5
                    // stringBuilder.Append("\n" + item.DisplayName); // пусто
                    // stringBuilder.Append("\n" + item.DetailedInfo); // пусто
                    // stringBuilder.Append("\n" + item.Name); // 96243392444855564
                }
                lcd.WriteText(stringBuilder.ToString());

                // отправляем команду
                var pb = list[1];
                var result = pb.TryRun("hello PB");
                Echo("TryRun result: " + result);

                Echo("run OK");
            }
            else
            {
                Echo("run ERROR");
            }
        }
    }
}

/**
 * программируемый блок приёмник
 * - получает команду от другого программируемого блока и выводит её в консоли
 */
namespace SpaceEngineers.MyProgrammableBlock3
{
    public sealed class Program : MyGridProgram
    {
        int counter;

        public Program()
        {
            Echo("init OK");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Echo("argument: " + argument);          // hello PB
            Echo("updateSource: " + updateSource);  // Script - этот скрипт был вызван другим скриптом
            Echo("counter: " + counter++);
        }
    }
}
