    // // !!! поддерживает наклоны у дрона в горизонтальном направлении, буры всегда направлены вниз
    // // проверить с дроном и 1 гироскопом
    
    // //Объявляем нужные блоки как глоб. переменные
    // IMyRemoteControl RemCon;
    // List<IMyGyro> gyroList;

    // //Находим блоки, устанавливаем частоту обновления
    // public Program()
    // {
    //     gyroList = new List<IMyGyro>();
    //     GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);
    //     RemCon = GridTerminalSystem.GetBlockWithName("RemCon") as IMyRemoteControl;
    //     Runtime.UpdateFrequency = UpdateFrequency.Update1;
    // }
    
    // public void Main()
    // {
    //     //Получаем и нормализуем вектор гравитации. Это наше направление "вниз" на планете.
    //     Vector3D GravityVector = RemCon.GetNaturalGravity();
    //     Vector3D GravNorm = Vector3D.Normalize(GravityVector);

    //     //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
    //     float PitchInput = -(float)GravNorm.Dot(RemCon.WorldMatrix.Forward);
    //     float RollInput = (float)GravNorm.Dot(RemCon.WorldMatrix.Left);
        
    //     //На рыскание просто отправляем сигнал рыскания с контроллера. Им мы будем управлять вручную.
    //     float YawInput = RemCon.RotationIndicator.Y;

    //     //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
    //     foreach (IMyGyro gyro in gyroList)
    //     {
    //         gyro.GyroOverride = true;
    //         // тут можно добавить коэфициент умножения для быстрого востановления в гор. положение
    //         // проверить если все значения установить в 1, посмотреть значения у гироскопа в панели управления
    //         gyro.Yaw = YawInput;
    //         gyro.Roll = RollInput;
    //         gyro.Pitch = PitchInput;
    //     }
    // }
