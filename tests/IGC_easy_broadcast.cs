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

// 
// Принимает сообщение
// Заметки:
// - на базе может быть несколько блоков которые будут принимать сообщение
// 
namespace SpaceEngineers.IGC.Easy.Listener
{
    public sealed class Program : MyGridProgram
    {
        string TAG = "elron.test";

        IMyBroadcastListener listener;

        public Program()
        {
            // подписываемся на канал
            listener = IGC.RegisterBroadcastListener(TAG);
            // указываем какой будет приходить argument в метод Main()
            listener.SetMessageCallback(TAG);
            Echo("Status: init ok");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Echo(updateSource.ToString() + " arg:" + argument);
            if ((updateSource & UpdateType.IGC) > 0)
            {
                if (listener.HasPendingMessage)
                {
                    MyIGCMessage message = listener.AcceptMessage();
                    if (message.Data is string)
                    {
                        Echo(message.Data.ToString());
                    }
                }
            }
        }
    }
}

//
// Отправляет сообщение
// TransmissionDistance.CurrentConstruct
// - все блоки на корабле
// TransmissionDistance.ConnectedConstructs
// - все блоки на корабле, а также пристыкованные корабли
// TransmissionDistance.TransmissionDistanceMax
// - все блоки
//
namespace SpaceEngineers.IGC.Easy.Sender
{
    public sealed class Program : MyGridProgram
    {
        string TAG = "elron.test";
        int count = 0;

        public Program()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
            count++;
            string message = "test message " + count;
            IGC.SendBroadcastMessage<string>(TAG, message, TransmissionDistance.TransmissionDistanceMax);
            Echo("send: " + message);
        }
    }
}