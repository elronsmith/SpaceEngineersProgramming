    // // гравитационная торпеда летит, а корабль указывает точку цели через камеры

    // Radar MyRadar;
    // IMyTextPanel TP1, TP2;
    // int Tick = 0;
    // bool RadarActive;
    // float maneuverK = 4;
    // float gyroK = 3;

    // Torpedo Torp1; //, Torp2; 

    // void Main(string argument)
    // {
    //     Tick++;
    //     // создаем необходимые объекты, в т.ч. объект нашего собственного класса Radar. 
    //     if (TP1 == null)
    //         TP1 = GridTerminalSystem.GetBlockWithName("TP1") as IMyTextPanel;
    //     if (TP2 == null)
    //         TP2 = GridTerminalSystem.GetBlockWithName("TP2") as IMyTextPanel;
    //     if (MyRadar == null)
    //         MyRadar = new Radar(this);

    //     if (Torp1 == null)
    //         Torp1 = new Torpedo(this, "Torpedo1");

    //     // разбираем аргументы, с которыми скрипт был запущен	 
    //     if (argument == "TryLock")
    //     {
    //         MyRadar.Lock(true, 15000);
    //         if (MyRadar.CurrentTarget.EntityId != 0)
    //             RadarActive = true;
    //         else
    //             RadarActive = false;
    //     }
    //     else if (argument == "Stop")
    //     {
    //         Runtime.UpdateFrequency = UpdateFrequency.None;
    //         MyRadar.StopLock();
    //         RadarActive = false;
    //     }
    //     if (argument == "Launch")
    //     {
    //         Torp1.Launch();
    //         //			Torp2.Launch(); 
    //     }
    //     else
    //     {
    //         MyRadar.Update();
    //         Torp1.Update();
    //         //Torp2.Update(); 
    //     }
    //     // если в захвате находится какой-то объект, то выполнение скрипта зацикливается		 
    //     if (RadarActive)
    //         Runtime.UpdateFrequency = UpdateFrequency.Update1;
    // }

    // public class Torpedo
    // {
    //     Program ParentProgram;
    //     public string Prefix;
    //     public Vector3D MyPos;
    //     public Vector3D MyPrevPos;
    //     public Vector3D MyVelocity;
    //     public Vector3D InterceptVector;
    //     public Vector3D CorrectionVector;
    //     public double TargetDistance;
    //     public int Status; //0-destroyed or doesn't exist, 1-ready to launch, 2-launched.  
    //     public int LaunchDelay = 150;
    //     private IMyGyro TorpGyro;
    //     private IMyTerminalBlock TorpMerge;

    //     public Torpedo(Program MyProg, string TorpedoPrefix)
    //     {
    //         ParentProgram = MyProg;
    //         Prefix = TorpedoPrefix;
    //         TorpGyro = ParentProgram.GridTerminalSystem.GetBlockWithName(Prefix + "Gyro") as IMyGyro;
    //         TorpMerge = ParentProgram.GridTerminalSystem.GetBlockWithName(Prefix + "MergeBlock") as IMyTerminalBlock;

    //         if ((TorpGyro != null) && (TorpMerge != null))
    //         {
    //             Status = 1;
    //         }
    //         else
    //         {
    //             Status = 0;
    //         }
    //     }

    //     public void Update()
    //     {
    //         if (TorpGyro == null)
    //             Status = 0;
    //         if (Status == 2)
    //         {
    //             LaunchDelay--;
    //             MyPos = TorpGyro.GetPosition();
    //             MyVelocity = (MyPos - MyPrevPos) * 60;
    //             MyPrevPos = MyPos;
    //             TargetDistance = (ParentProgram.MyRadar.correctTargetLocation - MyPos).Length();

    //             InterceptVector = FindInterceptVector(MyPos, MyVelocity.Length(), ParentProgram.MyRadar.correctTargetLocation, ParentProgram.MyRadar.CurrentTarget.Velocity);
    //             CorrectionVector = CustomReflect(MyVelocity, InterceptVector, ParentProgram.maneuverK);
    //             if (LaunchDelay < 0)
    //                 SetGyroOverride(GetNavAngles(CorrectionVector) * ParentProgram.gyroK);
    //         }
    //         UpdateTorpedoInfo();
    //     }

    //     public void UpdateTorpedoInfo()
    //     {
    //         ParentProgram.TP1.WritePublicText("\n Torpedo Status:" + Status.ToString() + " \n", true);
    //     }

    //     public void SetGyroOverride(Vector3D settings)
    //     {
    //         if (TorpGyro != null)
    //         {
    //             if (!TorpGyro.GyroOverride)
    //                 TorpGyro.ApplyAction("Override");
    //             TorpGyro.Yaw = (float)settings.GetDim(0);
    //             TorpGyro.Pitch = (float)settings.GetDim(1);
    //             TorpGyro.Roll = (float)settings.GetDim(2);
    //         }
    //     }

    //     public void Launch()
    //     {
    //         if ((Status == 1) && (ParentProgram.MyRadar.CurrentTarget.EntityId != 0))
    //         {
    //             List<IMyTerminalBlock> TorpedoBlocks = new List<IMyTerminalBlock>();
    //             ParentProgram.GridTerminalSystem.SearchBlocksOfName(Prefix, TorpedoBlocks);
    //             foreach (IMyTerminalBlock torpedoBlock in TorpedoBlocks)
    //             {
    //                 torpedoBlock.GetActionWithName("OnOff_On").Apply(torpedoBlock);
    //             }
    //             TorpMerge.GetActionWithName("OnOff_Off").Apply(TorpMerge);
    //             Status = 2;
    //         }
    //         UpdateTorpedoInfo();
    //     }

    //     private Vector3D CustomReflect(Vector3D vec, Vector3D dir, double K = 2)
    //     {
    //         Vector3D rej = Vector3D.Reject(vec, Vector3D.Normalize(dir));
    //         return vec - (rej * K);
    //     }

    //     private Vector3D GetNavAngles(Vector3D Dir)
    //     {
    //         double TargetPitch = -Vector3D.Dot(TorpGyro.WorldMatrix.Up, Vector3D.Normalize(Dir));
    //         double TargetYaw = -Vector3D.Dot(TorpGyro.WorldMatrix.Left, Vector3D.Normalize(Dir));
    //         double TargetRoll = 0;
    //         return new Vector3D(TargetYaw, TargetPitch, TargetRoll);
    //     }

	// 	//функция расчета вектора перехвата
	// 	//принимает: координаты снаряда, скалярную скорость снаряда, координаты цели, вектор скорости цели
    //     private Vector3D FindInterceptVector(Vector3D shotOrigin, double shotSpeed, Vector3D targetOrigin, Vector3D targetVel)
    //     {
    //         Vector3D dirToTarget = Vector3D.Normalize(targetOrigin - shotOrigin); //направление от снаряда на цель
    //         Vector3D targetVelOrth = Vector3D.Dot(targetVel, dirToTarget) * dirToTarget; //ортогональная скорость цели
	// 		Vector3D targetVelTang = Vector3D.Reject(targetVel, dirToTarget);//тангенциальная скорость цели
    //         //Vector3D targetVelTang = targetVel - targetVelOrth;
    //         Vector3D shotVelTang = targetVelTang; // уравниваем тангенциальную скорость снаряда со скоростью цели
    //         double shotVelSpeed = shotVelTang.Length(); // получаем ее длину

    //         if (shotVelSpeed > shotSpeed)  
    //         {
	// 			// требуемая тангенциальная скорость выше полной скорости снаряда
	// 			// попадание невозможно
    //             return Vector3D.Normalize(targetVel) * shotSpeed; 
    //         }
    //         else
    //         {
	// 			// иначе считаем тот "остаток" скорости снаряда, который мы можем направить в сторону цели.
    //             double shotSpeedOrth = Math.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
    //             Vector3D shotVelOrth = dirToTarget * shotSpeedOrth; //вектор ортогональной скорости снаряда
    //             return shotVelOrth + shotVelTang; // суммируем орт. и танг. скорости снаряда и получаем требуемый 
	// 			//для попадания вектор скорости снаряда. Он же вектор перехвата.
    //         }
    //     }
    // }


    // public class Radar //Наш собственный класс для захвата цели с помощью массива камер.  
    // {
    //     Program ParentProgram; //Ссылка на программу, "породившую объект этого класса" (на наш скрипт т.е.) 
    //     private static string Prefix = "RadarCam";  //префикс камер (с этого слова начинаются названия всех камер)  
    //     private List<IMyTerminalBlock> CamArray; //массив камер 
    //     private int CamIndex; //индекс текущей камеры в массиве 
    //     public MyDetectedEntityInfo CurrentTarget; // структура инфы о захваченном объекте 
    //     public Vector3D MyPos; // координаты 1й камеры (они и будут считаться нашим положением) 
    //     public Vector3D correctTargetLocation; //расчетные координаты захваченного объекта. (прежние координаты+вектор скорости * прошедшее время с последнего обновления захвата) 
    //     public double TargetDistance; //расстояние до ведомой цели	 
    //     public int LastLockTick; // программный тик последнего обновления захвата 
    //     public int TicksPassed; // сколько тиков прошло с последнего обновления захвата 


    //     // это конструктор. Он выполняется при создании объекта этого класса. Здесь я инициализирую массив камер, которые будут участвовать в захвате и сопровождении цели. 
    //     public Radar(Program MyProg)
    //     {
    //         ParentProgram = MyProg;
    //         CamIndex = 0;
    //         CamArray = new List<IMyTerminalBlock>();
    //         ParentProgram.GridTerminalSystem.SearchBlocksOfName(Prefix, CamArray);
    //         ParentProgram.TP1.WritePublicText("", false);
    //         for (int i = 0; i < CamArray.Count; i++)
    //         {
    //             IMyCameraBlock Cam = CamArray[i] as IMyCameraBlock;
    //             if (Cam != null)
    //             {
    //                 Cam.EnableRaycast = true;
    //                 //ParentProgram.TP1.WritePublicText(" " + Cam.CustomName + " - ", true); 
    //             }
    //         }
    //     }

    //     //Это основной метод, который осуществляет первоначальный захват и дальнейшее сопровождение цели с помощью массива камер.  
    //     public void Lock(bool TryLock = false, double InitialRange = 10000)
    //     {
    //         int initCamIndex = CamIndex;
    //         MyDetectedEntityInfo lastDetectedInfo = CurrentTarget;
    //         bool CanScan = true;
    //         // найдем первую после использованной в последний раз камеру, которая способна кастануть лучик на заданную дистанцию. 
    //         if (CurrentTarget.EntityId == 0)
    //             TargetDistance = InitialRange;

    //         while ((CamArray[CamIndex] as IMyCameraBlock)?.CanScan(TargetDistance) == false)
    //         {
    //             CamIndex++;
    //             if (CamIndex >= CamArray.Count)
    //                 CamIndex = 0;
    //             if (CamIndex == initCamIndex)
    //             {
    //                 CanScan = false;
    //                 break;
    //             }
    //         }
    //         //если такая камера в массиве найдена - кастуем ей луч. 
    //         if (CanScan)
    //         {
    //             //в случае, если мы осуществляем первоначальный захват цели, кастуем луч вперед 
    //             if ((TryLock) && (CurrentTarget.EntityId == 0))
    //                 lastDetectedInfo = (CamArray[CamIndex] as IMyCameraBlock).Raycast(InitialRange, 0, 0);
    //             else //иначе - до координат предполагаемого нахождения цели.	 
    //                 lastDetectedInfo = (CamArray[CamIndex] as IMyCameraBlock).Raycast(correctTargetLocation);
    //             //если что-то нашли лучем, то захват обновлен	 
    //             if (lastDetectedInfo.EntityId != 0)
    //             {
    //                 CurrentTarget = lastDetectedInfo;
    //                 LastLockTick = ParentProgram.Tick;
    //                 TicksPassed = 0;
    //             }
    //             else //иначе - захват потерян 
    //             {
    //                 ParentProgram.TP1.WritePublicText("Target Lost" + " \n", false);
    //                 CurrentTarget = lastDetectedInfo;
    //             }
    //             CamIndex++; //перебираем камеры в массиве по-очереди. 
    //             if (CamIndex >= CamArray.Count)
    //                 CamIndex = 0;
    //         }
    //     }

    //     //этот метод сбрасывает захват цели 
    //     public void StopLock()
    //     {
    //         CurrentTarget = (CamArray[0] as IMyCameraBlock).Raycast(0, 0, 0);
    //     }

    //     // этот метод выводит данные по захваченному объекту на панель 
    //     public void TargetInfoOutput()
    //     {
    //         if (CurrentTarget.EntityId != 0)
    //         {
    //             ParentProgram.TP2.WritePublicText("Target Info:" + " \n", false);
    //             ParentProgram.TP2.WritePublicText(CurrentTarget.EntityId + " \n", true);
    //             ParentProgram.TP2.WritePublicText(CurrentTarget.Type + " \n", true);
    //             ParentProgram.TP2.WritePublicText(CurrentTarget.Name + " \n", true);
    //             ParentProgram.TP2.WritePublicText("Position:" + " \n", true);
    //             ParentProgram.TP2.WritePublicText("Size: " + CurrentTarget.BoundingBox.Size.ToString("0.0") + " \n", true);
    //             ParentProgram.TP2.WritePublicText("X: " + Math.Round(CurrentTarget.Position.GetDim(0), 2).ToString() + " \n", true);
    //             ParentProgram.TP2.WritePublicText("Y: " + Math.Round(CurrentTarget.Position.GetDim(1), 2).ToString() + " \n", true);
    //             ParentProgram.TP2.WritePublicText("Z: " + Math.Round(CurrentTarget.Position.GetDim(2), 2).ToString() + " \n", true);
    //             ParentProgram.TP2.WritePublicText("Velocity: " + Math.Round(CurrentTarget.Velocity.Length(), 2).ToString() + " \n", true);
    //             ParentProgram.TP2.WritePublicText("Distance: " + Math.Round(TargetDistance, 2).ToString() + " \n", true);
    //         }
    //         else
    //             ParentProgram.TP2.WritePublicText("NO TARGET" + " \n", false);
    //     }


    //     //этот метод выполняет общее обновление объекта.  
    //     public void Update()
    //     {
    //         MyPos = CamArray[0].GetPosition();
    //         //если в захвате находится какой-то объект, выполняем следующие действия 
    //         if (CurrentTarget.EntityId != 0)
    //         {
    //             TicksPassed = ParentProgram.Tick - LastLockTick;
    //             //считаем предполагаемые координаты цели (прежние координаты + вектор скорости * прошедшее время с последнего обновления захвата) 
    //             correctTargetLocation = CurrentTarget.Position + (CurrentTarget.Velocity * TicksPassed / 60);
    //             // добавим к дистанции до объекта 10 м (так просто для надежности) 
    //             TargetDistance = (correctTargetLocation - MyPos).Length() + 10;

    //             //дальнейшее выполняется в случае, если пришло время обновить захват цели. Частота захвата в тиках считается как дистанция до объекта / 2000 * 60 / кол-во камер в массиве 
    //             // 2000 - это скорость восстановления дальности raycast по умолчанию) 
    //             // на 60 умножаем т.к. 2000 восстанавливается в сек, а в 1 сек 60 программных тиков 
    //             if (TicksPassed > TargetDistance * 0.03 / CamArray.Count)
    //             {
    //                 Lock();

    //                 ParentProgram.TP1.WritePublicText("Cam array info:" + " \n", false);
    //                 ParentProgram.TP1.WritePublicText("Cam quantity:" + CamArray.Count.ToString() + " \n", false);
    //                 ParentProgram.TP1.WritePublicText("Cam: " + CamArray[CamIndex].CustomName + " \n", true);
    //                 ParentProgram.TP1.WritePublicText("Distance: " + TargetDistance.ToString() + " \n", true);
    //                 ParentProgram.TP1.WritePublicText("Delay: " + Math.Round(TargetDistance * 0.03 / CamArray.Count, 0).ToString() + " \n", true);

    //                 TargetInfoOutput();
    //             }
    //         }
    //     }
    // }
