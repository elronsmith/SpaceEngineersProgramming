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
 * пример работы с LCD
 * находим LCD и выводим сообщение
 */
namespace SpaceEngineers.MyTextPanel
{
    public sealed class Program : MyGridProgram
    {
        // имя панели в игре
        string NAME_LCD = "Test LCD 1";
        IMyTextPanel lcd;

        public Program()
        {
            lcd = GridTerminalSystem.GetBlockWithName(NAME_LCD) as IMyTextPanel;
            if (lcd != null)
            {
                // настраиваем
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                lcd.BackgroundColor = Color.Black;
                lcd.FontColor = Color.Green;
                Echo("init OK");
            }
            else
            {
                Echo("init ERROR");
            }
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (lcd != null)
            {
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                lcd.WriteText("Hello MyTextPanel\n");
                lcd.WriteText("Status: OK", true);
                lcd.WritePublicTitle("my public title");
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
 * пример работы с LCD вместе с StringBuilder
 * находим LCD и выводим сообщение
 */
namespace SpaceEngineers.MyTextPanel2
{
    public sealed class Program : MyGridProgram
    {
        // имя панели в игре
        string NAME_LCD = "Test LCD 1";
        Color backgroundColor = new Color(0, 0, 255);
        Color textColor = Color.White;
        StringBuilder stringBuilder = new StringBuilder();
        IMyTextPanel lcd;

        public Program()
        {
            lcd = GridTerminalSystem.GetBlockWithName(NAME_LCD) as IMyTextPanel;
            if (lcd != null)
            {
                // настраиваем
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                lcd.BackgroundColor = backgroundColor;
                lcd.FontColor = textColor;
                Echo("init OK");
            }
            else
            {
                Echo("init ERROR");
            }
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (lcd != null)
            {
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                stringBuilder.Clear();
                stringBuilder.AppendLine("Hello MyTextPanel")
                             .AppendLine("Status: OK");
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
 * пример работы с LCD вместе с StringBuilder
 * находим LCD и выводим сообщение
 * имя LCD передаём в аргументе
 */
namespace SpaceEngineers.MyTextPanel3
{
    public sealed class Program : MyGridProgram
    {
        Color backgroundColor = new Color(0, 0, 255);
        Color textColor = Color.White;
        StringBuilder stringBuilder = new StringBuilder();
        IMyTextPanel lcd;

        public void Main(string argument, UpdateType updateSource)
        {
            if (lcd == null)
                initLCD(argument);

            if (lcd != null)
            {
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                stringBuilder.Clear();
                stringBuilder.AppendLine("Hello MyTextPanel")
                             .AppendLine("Status: OK");
                Echo("run OK");
            }
            else
            {
                Echo("run ERROR");
            }
        }

        private void initLCD(string argument)
        {
            lcd = GridTerminalSystem.GetBlockWithName(argument) as IMyTextPanel;
            if (lcd != null)
            {
                // настраиваем
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                lcd.BackgroundColor = backgroundColor;
                lcd.FontColor = textColor;
                Echo("init OK");
            }
            else
            {
                Echo("init ERROR");
            }
        }
    }
}
