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
 * дрон для поиска залежей руды
 * летит прямо на расстояние 5 км и сохраняет координаты найденной руды
 * возвращается назад
 * результат: список GPS меток с рудой
 * 
 */
namespace SpaceEngineers.SearchOres
{
    public sealed class Program : MyGridProgram
    {

        public Program()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            
        }
    }
}