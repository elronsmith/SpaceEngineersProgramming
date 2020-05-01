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
 * пример Ротора
 * принимает команды в аргументе:
 * - остановиться
 * - стать на [0..360] градус
 * - повернуться на [-360..360] градусов
 */
namespace SpaceEngineers.MyMotorStator
{
    public sealed class Program : MyGridProgram
    {
        string NAME_ROTOR = "Test Rotor";
        string CMD_STOP = "stop";
        string CMD_POSITION = "P";
        string CMD_ROTATE = "R";
        float defaultRadSpeed = toRad(5);  // скорость поворота

        IMyMotorStator rotor;

        // private bool rotatingRight;
        private float angleRadLength;
        private float lastAngle;
        bool needSlowdown = true;
        int counter = 0;

        public Program()
        {
            rotor = GridTerminalSystem.GetBlockWithName(NAME_ROTOR) as IMyMotorStator;
            if (rotor != null)
            {
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
                Echo("init OK");
                // rotor.TargetVelocityRad = defaultRadSpeed;
                log();
            }
            else
                Echo("init ERROR");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (rotor != null)
            {
                Echo("updateSource: " + updateSource);
                Echo("counter: " + counter++);

                if ((updateSource & UpdateType.Terminal) > 0 && argument.Length > 0)
                {
                    if (CMD_STOP.Equals(argument))
                    {
                        stop();
                    }
                    else if (argument.StartsWith(CMD_POSITION))
                    {
                        // нужно переместиться на определенный угол
                        float angleTo = parse(argument);
                        angleRadLength = angleTo - rotor.Angle;
                        bool toRight = true;
                        if (angleRadLength < 0f)
                            angleRadLength = (float)(2f * Math.PI) - Math.Abs(angleRadLength);
                        toRight = angleRadLength <= Math.PI;

                        lastAngle = rotor.Angle;
                        if (toRight)
                        {
                            if (argument.Contains('-'))
                            {
                                // по длинному пути
                                rotor.TargetVelocityRad = -defaultRadSpeed;
                                angleRadLength = (float)(2f * Math.PI) - angleRadLength;
                            }
                            else
                            {
                                // по короткому пути
                                rotor.TargetVelocityRad = defaultRadSpeed;
                            }
                        }
                        else
                        {
                            if (argument.Contains('-'))
                            {
                                // по длинному пути
                                rotor.TargetVelocityRad = defaultRadSpeed;
                            }
                            else
                            {
                                // по короткому пути
                                rotor.TargetVelocityRad = -defaultRadSpeed;
                                angleRadLength = (float)(2f * Math.PI) - angleRadLength;
                            }
                        }
                        needSlowdown = true;
                        rotor.RotorLock = false;
                        Runtime.UpdateFrequency = UpdateFrequency.Update1;
                    }
                    else if (argument.StartsWith(CMD_ROTATE))
                    {
                        // нужно повернуться
                        angleRadLength = parse(argument);
                        lastAngle = rotor.Angle;
                        if (argument.Contains('-'))
                            rotor.TargetVelocityRad = -defaultRadSpeed;
                        else
                            rotor.TargetVelocityRad = defaultRadSpeed;
                        needSlowdown = true;
                        rotor.RotorLock = false;
                        Runtime.UpdateFrequency = UpdateFrequency.Update1;
                    }
                }
                else
                {
                    // поворачиваемся
                    if (angleRadLength > 0f)
                    {
                        float dif = Math.Abs(rotor.Angle - lastAngle);
                        if (dif > Math.PI)
                            dif = (float)(2f * Math.PI) - dif;
                        angleRadLength -= dif;
                        lastAngle = rotor.Angle;

                        // замедляем скорость
                        if (needSlowdown)
                        {
                            if (angleRadLength - defaultRadSpeed < 0f)
                            {
                                rotor.TargetVelocityRad /= 2f;
                                needSlowdown = false;
                            }
                        }
                    }

                    if (angleRadLength == 0f)
                        Runtime.UpdateFrequency = UpdateFrequency.None;
                        
                    if (angleRadLength < 0f)
                        stop(); // останавливаемся
                }

                log();
            }
            else
                Echo("run ERROR");
        }

        private void stop()
        {
            rotor.RotorLock = true;
            rotor.TargetVelocityRPM = 0;
            lastAngle = rotor.Angle;
            angleRadLength = 0f;
            Runtime.UpdateFrequency = UpdateFrequency.None;
        }
        private float parse(string degString)
        {
            float result = Math.Abs(float.Parse(degString.Substring(1)));
            result = toRad(result);
            return result;
        }
        private void log()
        {
            Echo("Angle rad: " + rotor.Angle);       // rotor.Angle это угол в радианах
            Echo("Angle deg: " + toDeg(rotor.Angle));
            Echo("Torque: " + rotor.Torque);
            Echo("TargetVelocityRad: " + rotor.TargetVelocityRad);
            Echo("TargetVelocityRPM: " + rotor.TargetVelocityRPM);
            Echo("LowerLimitRad: " + rotor.LowerLimitRad);
            Echo("LowerLimitDeg: " + rotor.LowerLimitDeg);
            Echo("UpperLimitRad: " + rotor.UpperLimitRad);
            Echo("UpperLimitDeg: " + rotor.UpperLimitDeg);
            Echo("Displacement: " + rotor.Displacement);
            Echo("RotorLock: " + rotor.RotorLock);
            Echo("angleRadLength: " + angleRadLength);
        }

        private static float toRad(float deg)
        {
            float result = deg % 360;
            result = (float)(result * Math.PI / 180D);
            return result;
        }
        private static float toDeg(float rad)
        {
            return (float)(180D * rad / Math.PI);
        }
    }
}
