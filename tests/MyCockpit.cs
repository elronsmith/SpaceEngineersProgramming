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
 * Кабина
 */
namespace SpaceEngineers.MyCockpit
{
    public sealed class Program : MyGridProgram
    {
        string NAME_COCKPIT = "Кокпит";

        IMyCockpit cockpit;
        IMyTextSurface lcd;

        Vector3D position = new Vector3D();
        StringBuilder stringBuilder = new StringBuilder();

        public Program()
        {
            cockpit = GridTerminalSystem.GetBlockWithName(NAME_COCKPIT) as IMyCockpit;
            if (cockpit != null)
            {
                Echo("init OK");
                Echo("SurfaceCount: " + cockpit.SurfaceCount);

                lcd = cockpit.GetSurface(0); // or 1
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;

                Echo("OxygenCapacity: " + cockpit.OxygenCapacity);
                Echo("OxygenFilledRatio: " + cockpit.OxygenFilledRatio);

                // гравитация, вектор указывает на центр планеты
                Vector3D gravityVector = cockpit.GetNaturalGravity();
                stringBuilder.Append("\ngravityVector: ").Append(gravityVector.Length())
                         .Append("\n  X: ").Append(gravityVector.X)
                         .Append("\n  Y:").Append(gravityVector.Y)
                         .Append("\n  Z:").Append(gravityVector.Z);
                Echo(stringBuilder.ToString());

                // масса корабля
                float PhysicalMass = cockpit.CalculateShipMass().PhysicalMass;
                Echo("PhysicalMass: " + PhysicalMass);

                Runtime.UpdateFrequency = UpdateFrequency.Update1;
            }
            else
                Echo("init ERROR");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // скорость
            Vector3D currentPosition = cockpit.GetPosition();
            double speed = ((currentPosition - position) * 60).Length();
            position = currentPosition;

            stringBuilder.Clear();
            stringBuilder.Append(string.Format("speed: {0:0.#}", speed))
                         .Append("\nposition: " + position.Length())
                         .Append("\n  X: " + position.X)
                         .Append("\n  Y: " + position.Y)
                         .Append("\n  Z: " + position.Z);

            // высота над землей
            double elevation;
            cockpit.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation);
            stringBuilder.Append("\nelevation: ").Append(elevation);
            
            // единичный вектор вперёд
            var forward2 = cockpit.WorldMatrix.GetOrientation().Forward;
            var forward = cockpit.WorldMatrix.Forward;
            stringBuilder.Append("\nforvard: ").Append(forward.Length())
                         .Append("\n  X: ").Append(forward.X)
                         .Append("\n  Y:").Append(forward.Y)
                         .Append("\n  Z:").Append(forward.Z);
            stringBuilder.Append("\n").Append(forward == forward2); // true

            lcd.WriteText(stringBuilder.ToString());
        }
    }
}
