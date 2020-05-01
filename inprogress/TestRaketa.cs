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
 * запуск ракеты
 * запускаем ракету и следим за параметрами
 * варианты управления: ручное/автоматическое
 * База:
 * - отправляет команду
 * - мониторит параметры
 * Ракета:
 * - принимает команду и запускается
 * - передаёт свою скорость, высоту, расстояние до цели
 */
namespace SpaceEngineers.TestRaketa
{
    public sealed class Program : MyGridProgram
    {
        string NAME_MERGE_BLOCK = "Соединитель 2";
        string CMD_START = "start";
        string CMD_START_MANUAL = "startmanual";

        const int TIME_UP = 5;  // 5 секунд на набор высоты

        const int STATUS_IDLE = 0;
        const int STATUS_UP = 1;
        const int STATUS_KILL = 2;

        public IMyRemoteControl remoteControl;
        List<IMyThrust> list = new List<IMyThrust>();
        List<IMyThrust> forwardList = new List<IMyThrust>();
        List<IMyThrust> upList = new List<IMyThrust>();

        int status = STATUS_IDLE;
        DateTime startTime;
        StringBuilder sb = new StringBuilder();
        int tick;
        TargetCapture targetCapture;

        public Program()
        {
            targetCapture = new TargetCapture(this);
            // log();
            initRemoteControl();
            initThrusters();
            updateWeightVector();
            updateThrustValue();
            updateThrustersOverride();
            Me.GetSurface(0).WriteText(sb.ToString());
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument.StartsWith(CMD_START_MANUAL))
            {
                // ручное управление
                mergeUnlock();
                initRemoteControl();
                // включаем ручное управление
                remoteControl.ControlThrusters = true;
                remoteControl.DampenersOverride = true;
                remoteControl.ShowHorizonIndicator = true;
                remoteControl.SetValueBool("ControlGyros", true);
                initThrusters();
                startThrustersForward(); // включаем тягу чтоб подняться наверх
            }
            else if (argument.StartsWith(CMD_START))
            {
                status = STATUS_UP;
                parseGps(argument.Substring(CMD_START.Length + 1));
                targetCapture.initGyros();
                runUp();
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
                return;
            }

            switch (status)
            {
                case STATUS_UP:
                    if (canKill())
                    {
                        status = STATUS_KILL;
                        enableUpThrusters();
                    }
                    break;
                case STATUS_KILL:
                    kill();
                    break;
            }
            //log();
        }

        Vector3D weightVector = new Vector3D();
        double upThrust = 0f;
        double downThrust = 0f;
        double upThrustMax = 0;
        double downThrustMax = 0;

        /// 
        /// обновляем вектор гравитации с учетом массы корабля
        ///
        private void updateWeightVector()
        {
            Vector3D gravityVector = remoteControl.GetNaturalGravity();
            float shipMass = remoteControl.CalculateShipMass().PhysicalMass;
            Vector3D.Multiply(ref gravityVector, shipMass, out weightVector);
        }

        Matrix remoteControlMatrix = new MatrixD();
        Matrix thrusterMatrix = new MatrixD();

        /// распределяем тягу по всем двигателям
        private void updateThrustersOverride()
        {
            remoteControl.DampenersOverride = true;
            // remoteControl.Orientation.GetMatrix(out remoteControlMatrix);

            // // находим максимальную возможную тягу
            // foreach (var item in upList)
            // {
            //     if (thrusterMatrix.Forward == remoteControlMatrix.Up)
            //         upThrustMax += item.MaxEffectiveThrust;
            //     else if (thrusterMatrix.Forward == remoteControlMatrix.Down)
            //         downThrustMax += item.MaxEffectiveThrust;
            // }
            // // распределяем тягу
            // foreach (var item in upList)
            // {
            //     if (thrusterMatrix.Forward == remoteControlMatrix.Up)
            //         item.ThrustOverridePercentage = (float)(upThrust / upThrustMax);
            //     else if (thrusterMatrix.Forward == remoteControlMatrix.Down)
            //         item.ThrustOverridePercentage = (float)(downThrust / downThrustMax);
            // }

            sb.Clear();
            sb.Append("upThrust: " + upThrust);
            sb.Append("\ndownThrust: " + downThrust);
            foreach (var item in upList)
            {
                sb.Append("\n  DisplayNameText: " + item.DisplayNameText);
                sb.Append("\n  proc: " + item.ThrustOverridePercentage);
            }

        }
        private void updateThrustValue()
        {
            // узнаём какую тягу(вниз) нужно подавать
            upThrust = weightVector.Dot(remoteControl.WorldMatrix.Up);
            downThrust = -upThrust;
        }
        private void kill()
        {
            updateWeightVector();
            updateThrustValue();
            // распределяем тягу по всем двигателям
            updateThrustersOverride();

            // смотрим на цель
            targetCapture.capture();
        }


        private void enableUpThrusters()
        {
            foreach (var item in upList)
            {
                item.Enabled = true;
                item.ThrustOverridePercentage = 0f;
            }
        }
        private bool canKill()
        {
            if (System.DateTime.Now >= startTime)
                return true;
            return false;
        }

        /// отключаем соединитель
        private void mergeUnlock()
        {
            IMyShipMergeBlock merge = GridTerminalSystem.GetBlockWithName(NAME_MERGE_BLOCK) as IMyShipMergeBlock;
            if (!merge.Enabled)
                merge.Enabled = true;
            merge.Enabled = false;
        }
        /// находим блок удаленного управления
        private void initRemoteControl()
        {
            List<IMyRemoteControl> remoteControlList = new List<IMyRemoteControl>();
            GridTerminalSystem.GetBlocksOfType<IMyRemoteControl>(remoteControlList);
            if (remoteControlList.Count() > 0)
                remoteControl = remoteControlList[0];
        }

        /// находим трастеры и добавляем как тягу вперед и вверх
        private void initThrusters()
        {
            list.Clear();
            forwardList.Clear();
            upList.Clear();
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(list);
            foreach (var item in list)
            {
                // sb.Clear();
                // sb.Append(item.DisplayNameText).Append(":\n")
                //     .Append("  ").Append(item.Orientation.Forward).Append("\n");
                // Echo(sb.ToString());
                if (item.Orientation.Forward == Base6Directions.Direction.Backward)
                    upList.Add(item);
                else if (item.Orientation.Forward == Base6Directions.Direction.Down)
                    forwardList.Add(item);
            }
        }
        /// включаем задние трастеры
        private void startThrustersForward()
        {
            foreach (var item in forwardList)
            {
                item.Enabled = true;
                item.ThrustOverridePercentage = 1f;
            }
        }
        void runUp()
        {
            mergeUnlock();
            initRemoteControl();
            remoteControl.ControlThrusters = false;
            remoteControl.DampenersOverride = false;
            remoteControl.ShowHorizonIndicator = false;
            remoteControl.SetValueBool("ControlGyros", false);
            initThrusters();
            startThrustersForward(); // включаем тягу чтоб подняться наверх

            startTime = System.DateTime.Now.AddSeconds(TIME_UP);
        }

        private void parseGps(string gps)
        {
            var array = gps.Split(':');
            targetCapture.target.X = double.Parse(array[2]);
            targetCapture.target.Y = double.Parse(array[3]);
            targetCapture.target.Z = double.Parse(array[4]);
        }

        double elevation = 0;
        private void log()
        {
            sb.Clear();
            tick++;
            sb.Append("status: ").Append(status);
            sb.Append("\ntick: ").Append(tick);
            sb.Append("\nthrusters: ").Append(forwardList.Count());
            sb.Append("\ngyros: ").Append(targetCapture.getGyrosCount());
            sb.Append("\nRollInput: ").Append(targetCapture.RollInput);

            if (remoteControl != null)
            {
                remoteControl.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation);
                sb.Append(string.Format("\nelevation: {0:#.#}m", elevation));
            }

            Me.GetSurface(0).WriteText(sb.ToString());
        }
    }


    public class TargetCapture
    {
        Program program;
        // GPS:тестовая цель 3:54052.38:-26985.24:8506.02:
        public Vector3D target = new Vector3D();
        List<IMyGyro> gyroList = new List<IMyGyro>();
        float maxRpM = 3f;

        public TargetCapture(Program _program)
        {
            program = _program;
        }

        // значения [-1;1]
        float PitchInput = 0;
        float YawInput = 0;
        public float RollInput = 0; // не вращаем
        /// захват цели
        public void capture()
        {
            // вектор на точку
            Vector3D targetNormVector = Vector3D.Normalize(target - program.remoteControl.GetPosition());

            double F = targetNormVector.Dot(program.remoteControl.WorldMatrix.Forward);
            double L = targetNormVector.Dot(program.remoteControl.WorldMatrix.Left);
            double U = targetNormVector.Dot(program.remoteControl.WorldMatrix.Up);

            YawInput = getRPM((float)F, (float)L);
            PitchInput = getRPM((float)F, (float)U);

            // доворачиваем крен чтоб было паралельно горизонту
            // double D = targetNormVector.Dot(program.remoteControl.WorldMatrix.Down);
            Vector3D gravNormVector = Vector3D.Normalize(program.remoteControl.GetNaturalGravity());
            double G = gravNormVector.Dot(program.remoteControl.WorldMatrix.Left);

            RollInput = (float)G;

            setGyro(PitchInput, RollInput, YawInput);
        }

        public void initGyros()
        {
            gyroList.Clear();
            program.GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);
        }
        private float getRPM(float F, float omega)
        {
            if (Math.Abs(omega) <= 0.001)
                return 0f;

            float result = F + omega - 1; // (-2;0)
            result = Math.Abs(result);
            result *= maxRpM;
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

        public int getGyrosCount()
        {
            return gyroList.Count();
        }
    }
}
