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
 * пример работы с сенсором
 * пример координат GPS: "GPS:elron #1:88671.87:-54368.51:17368.41:"
 */
namespace SpaceEngineers.MySensorBlock
{
    public sealed class Program : MyGridProgram
    {
        string NAME_SENSOR = "Sensor";
        string NAME_PB = "PB";

        IMySensorBlock sensor;
        IMyTextSurface pbLCD;
        List<MyDetectedEntityInfo> entityList = new List<MyDetectedEntityInfo>();
        StringBuilder stringBuilder = new StringBuilder();

        private string gpsToString(string name, Vector3D position) {
            return string.Format("GPS:{0}:{1}:{2}:{3}:", name, position.X, position.Y, position.Z);
        }

        public Program()
        {
            sensor = GridTerminalSystem.GetBlockWithName(NAME_SENSOR) as IMySensorBlock;
            IMyProgrammableBlock pb = GridTerminalSystem.GetBlockWithName(NAME_PB) as IMyProgrammableBlock;
            if (pb != null)
            {
                Echo("LCD count: " + pb.SurfaceCount);
                pbLCD = pb.GetSurface(0); // or 1
                pbLCD.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                pbLCD.WriteText("init OK");
            }
            else
                Echo("init ERROR PB");

            if (sensor != null)
                Echo("init SENSOR OK");
            else
                Echo("init SENSOR ERROR");

            if (sensor != null && pbLCD != null)
                Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (sensor != null)
            {
                Echo("updateSource: " + updateSource);
                Echo("IsActive: " + sensor.IsActive);
                
                entityList.Clear();
                sensor.DetectedEntities(entityList);

                stringBuilder.Clear();
                foreach (var item in entityList)
                {
                    Echo("" + item.Type + ": " + item.Name);
                    stringBuilder.Append(gpsToString(item.Name, item.Position)).Append("\n");
                }
                pbLCD.WriteText(stringBuilder.ToString());
            }
            else
            {
                Echo("run ERROR");
            }
        }
    }
}
