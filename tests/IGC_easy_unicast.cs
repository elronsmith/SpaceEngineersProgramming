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
namespace SpaceEngineers.IGC.Easy.Listener2
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
                        // отправляем ответное сообщение
                        IGC.SendUnicastMessage<string>(message.Source, TAG, "result message");
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
namespace SpaceEngineers.IGC.Easy.Sender2
{
    public sealed class Program : MyGridProgram
    {
        string TAG = "elron.test";
        string ARG_CALLBACK = "elron.callback";
        int count = 0;

        IMyUnicastListener listener;

        public Program()
        {
            listener = IGC.UnicastListener;
            listener.SetMessageCallback(ARG_CALLBACK);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & UpdateType.Terminal) > 0)
            {
                // отправляем сообщение
                count++;
                string message = "test message " + count;
                IGC.SendBroadcastMessage<string>(TAG, message, TransmissionDistance.TransmissionDistanceMax);
                Echo("send: " + message);
            }
            else if ((updateSource & UpdateType.IGC) > 0)
            {
                // получаем ответ
                Echo("argument: " + argument);
                if (listener.HasPendingMessage)
                {
                    MyIGCMessage message = listener.AcceptMessage();
                    if (message.Data is string)
                    {
                        Echo("result: " + message.Data.ToString());
                    }
                }
            }
        }
    }
}