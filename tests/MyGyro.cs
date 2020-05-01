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

// !!! ВАЖНО !!!
//  все гироскопы желательно устанавливать в том же направлении
//    куда смотрит кокпит или контроль управления

/**
 * гироскоп
 * команды:
 * - поворачивается так чтобы смотреть лицом на цель
 * - можно обходить цель и двигаться вокруг неё
 * значения оборотов в минуту: [-60..60]
 * команда Y1 - направо - 9.55 об/мин, максимум 60 об/мин
 * команда R1 - направо
 * команда P1 - вниз
 */
namespace SpaceEngineers.MyGyro3
{
    public sealed class Program : MyGridProgram
    {
        const string NAME_COCKPIT = "Кокпит";
        const int STATUS_IDLE = 0;
        const int STATUS_FIND2 = 1;
        const int STATUS_FIND3 = 2;
        const int STATUS_FIND4 = 3;
        const string CMD_FIND2 = "find2";
        const string CMD_FIND3 = "find3";
        const string CMD_FIND4 = "find4";
        const string CMD_IDLE = "idle";

        // координаты точки
        // GPS:Test GPS:53606.95:-26682.71:11812.02:
        Vector3D Target = new Vector3D(53606.95, -26682.71, 11812.02);

        IMyCockpit cockpit;
        IMyTextSurface lcd;

        List<IMyGyro> gyroList = new List<IMyGyro>();
        int status = 0;

        public Program()
        {
            cockpit = GridTerminalSystem.GetBlockWithName(NAME_COCKPIT) as IMyCockpit;
            GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);

            if (cockpit != null)
            {
                Echo("cockpit OK");
                lcd = cockpit.GetSurface(0); // or 1
                lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
            }
            else
                Echo("cockpit ERROR");

            if (gyroList.Count() > 0)
                Echo("gyro OK");
            else
                Echo("gyro ERROR");

            if (gyroList.Count() > 0 && cockpit != null)
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
            else
                Runtime.UpdateFrequency = UpdateFrequency.None;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            handleCommand(argument);

            switch (status)
            {
                case STATUS_FIND2:
                    findV2();
                    break;
                case STATUS_FIND3:
                    findV3();
                    break;
                case STATUS_FIND4:
                    findV4();
                    break;
                default:
                    break;
            }
            // showInfo();
        }

        private void showInfo()
        {
            Vector3D GravityVector = cockpit.GetNaturalGravity();
            Vector3D GravNorm = Vector3D.Normalize(GravityVector);

            //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
            double gF = GravNorm.Dot(cockpit.WorldMatrix.Forward);
            double gU = GravNorm.Dot(cockpit.WorldMatrix.Up);
            float PitchInput = -(float)Math.Atan2(gF, -gU);

            lcd.WriteText(gF.ToString());
            lcd.WriteText("\n", true);
            lcd.WriteText(gU.ToString(), true);
            lcd.WriteText("\n", true);
            lcd.WriteText(PitchInput.ToString(), true);
        }

        private void handleCommand(string argument)
        {
            if (argument.Length == 0)
                return;
            else if (argument.StartsWith("P"))
            {
                string v = argument.Substring(1);
                if (v.Equals("0"))
                    allGyroPitch();
                else
                    allGyroPitch(float.Parse(v));
            }
            else if (argument.StartsWith("R"))
            {
                string v = argument.Substring(1);
                if (v.Equals("0"))
                    allGyroRoll();
                else
                    allGyroRoll(float.Parse(v));
            }
            else if (argument.StartsWith("Y"))
            {
                string v = argument.Substring(1);
                if (v.Equals("0"))
                    allGyroYaw();
                else
                    allGyroYaw(float.Parse(v));
            }
            else if (CMD_FIND2.Equals(argument))
            {
                status = STATUS_FIND2;
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
            }
            else if (CMD_FIND3.Equals(argument))
            {
                status = STATUS_FIND3;
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
            }
            else if (CMD_FIND4.Equals(argument))
            {
                status = STATUS_FIND4;
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
            }
            else if (CMD_IDLE.Equals(argument))
            {
                status = STATUS_IDLE;
                allGyroOff();
                Runtime.UpdateFrequency = UpdateFrequency.None;
            }
        }

        private void allGyroOff()
        {
            foreach (IMyGyro gyro in gyroList)
            {
                gyro.GyroOverride = false;
                gyro.Yaw = 0;
                gyro.Roll = 0;
                gyro.Pitch = 0;
            }
        }
        private void allGyroPitch(float value = 0f)
        {
            if (value == 0f)
            {
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.Pitch = 0;
                    gyro.GyroOverride = false;
                }
            }
            else
            {
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.GyroOverride = true;
                    gyro.Pitch = value;
                }
            }
        }
        private void allGyroRoll(float value = 0f)
        {
            if (value == 0f)
            {
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.Roll = 0;
                    gyro.GyroOverride = false;
                }
            }
            else
            {
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.GyroOverride = true;
                    gyro.Roll = value;
                }
            }
        }
        private void allGyroYaw(float value = 0f)
        {
            if (value == 0f)
            {
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.Yaw = 0;
                    gyro.GyroOverride = false;
                }
            }
            else
            {
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.GyroOverride = true;
                    gyro.Yaw = value;
                }
            }
        }

        /* всегда смотрит на горизонт */
        private void findV2()
        {
            //Получаем и нормализуем вектор гравитации. Это наше направление "вниз" на планете.
            Vector3D GravityVector = cockpit.GetNaturalGravity();
            Vector3D GravNorm = Vector3D.Normalize(GravityVector);

            //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
            double gF = GravNorm.Dot(cockpit.WorldMatrix.Forward);
            double gL = GravNorm.Dot(cockpit.WorldMatrix.Left);
            double gU = GravNorm.Dot(cockpit.WorldMatrix.Up);

            //Получаем сигналы по тангажу и крены операцией atan2
            float RollInput = (float)Math.Atan2(gL, -gU);
            float PitchInput = -(float)Math.Atan2(gF, -gU);

            //На рыскание просто отправляем сигнал рыскания с контроллера. Им мы будем управлять вручную.
            float YawInput = cockpit.RotationIndicator.Y;

            //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
            foreach (IMyGyro gyro in gyroList)
            {
                gyro.GyroOverride = true;
                gyro.Yaw = YawInput;
                gyro.Roll = RollInput;
                gyro.Pitch = PitchInput;
            }
        }

        /* 
         * всегда смотрит на горизонт
         * наводится на цель
         */
        private void findV3()
        {
            //Получаем и нормализуем вектор гравитации. Это наше направление "вниз" на планете.
            Vector3D GravityVector = cockpit.GetNaturalGravity();
            Vector3D GravNorm = Vector3D.Normalize(GravityVector);

            //вектор на точку
            Vector3D T = Vector3D.Normalize(Target - cockpit.GetPosition());

            //Рысканием прицеливаемся на точку Target.
            double tF = T.Dot(cockpit.WorldMatrix.Forward);
            double tL = T.Dot(cockpit.WorldMatrix.Left);
            float YawInput = -(float)Math.Atan2(tL, tF);

            // lcd.WriteText(YawInput.ToString());

            //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
            double gF = GravNorm.Dot(cockpit.WorldMatrix.Forward);
            double gL = GravNorm.Dot(cockpit.WorldMatrix.Left);
            double gU = GravNorm.Dot(cockpit.WorldMatrix.Up);

            //Получаем сигналы по тангажу и крены операцией atan2
            float RollInput = (float)Math.Atan2(gL, -gU);
            float PitchInput = -(float)Math.Atan2(gF, -gU);

            //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
            setGyro(PitchInput, RollInput, YawInput);

            // это почти один и тот же вектор
            logEcho("G", GravNorm);
            logEcho("D", cockpit.WorldMatrix.Down);
        }

        float maxRpM = 3f;
        /* 
         * используем тангаж + рыскание
         * 
         */
        private void findV4()
        {
            // вектор на точку
            Vector3D targetNormVector = Vector3D.Normalize(Target - cockpit.GetPosition());

            double F = targetNormVector.Dot(cockpit.WorldMatrix.Forward);
            double L = targetNormVector.Dot(cockpit.WorldMatrix.Left);
            double U = targetNormVector.Dot(cockpit.WorldMatrix.Up);

            // значения [-1;1]
            float PitchInput = 0;
            float YawInput = 0;
            float RollInput = 0; // не вращаем

            YawInput = getRPM((float)F, (float)L);
            PitchInput = getRPM((float)F, (float)U);

            setGyro(PitchInput, RollInput, YawInput);
        }

        private float getRPM(float F, float omega)
        {
            if (Math.Abs(omega) <= 0.001)
                return 0f;

            float result = F + omega - 1; // (-2;0)
            result = Math.Abs(result);
            result *= maxRpM;
            // if (result <= 0.1)
            //     result *= 0.7f;
            result = (float)Math.Sqrt(result);
            if (omega > 0)
                result *= -1f;

            return result;
        }

        private void setGyro(float PitchInput, float RollInput, float YawInput)
        {
            foreach (IMyGyro gyro in gyroList)
            {
                gyro.GyroOverride = true;
                gyro.Yaw = YawInput;
                gyro.Roll = RollInput;
                gyro.Pitch = PitchInput;
            }
        }

        StringBuilder stringBuilder = new StringBuilder();
        private void logEcho(string name, Vector3D vector)
        {
            Echo(print(name, vector));
        }
        private void logLCD(string name, Vector3D vector)
        {
            lcd.WriteText(print(name, vector));
        }

        private string print(string name, Vector3D vector)
        {
            stringBuilder.Clear();
            stringBuilder.Append(name).Append(": ").Append(vector.Length())
                     .Append("\n  X: ").Append(vector.X)
                     .Append("\n  Y:").Append(vector.Y)
                     .Append("\n  Z:").Append(vector.Z);
            return stringBuilder.ToString();
        }
    }
}

