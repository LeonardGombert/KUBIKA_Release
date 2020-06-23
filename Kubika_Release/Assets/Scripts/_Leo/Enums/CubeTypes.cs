public enum CubeTypes
{
    None = 0, //empty layer
    FullStaticCube, //full layer
    EmptyStaticCube, //full layer
    TopStaticCube, //full layer
    CornerStaticCube, //full layer
    TripleStaticCube, //full layer
    QuadStaticCube, //full layer

    //less than 6 means full
    //bigger than 6 means it's moveable

    MoveableCube, //moveable layer
    
    BaseVictoryCube, //moveable layer
    ConcreteVictoryCube, //moveable layer
    BombVictoryCube, //moveable layer
    SwitchVictoryCube, //moveable layer

    DeliveryCube, //full layer

    GreenElevatorCube, //moveable layer
    BlueElevatorCube, //moveable layer

    ConcreteCube, //moveable layer
    BombCube, //moveable layer

    TimerCube1, //full layer
    TimerCube2,
    TimerCube3,
    TimerCube4,
    TimerCube5,
    TimerCube6,
    TimerCube7,
    TimerCube8,
    TimerCube9,

    SwitchButton, //full layer
    SwitchCube, //moveable & full layer

    RotatorRightTurner, //full layer
    RotatorLeftTurner, //full layer
    RotatorLocker, //full layer

    Count, //use to get the total count of enums

    ChaosBall, //moveable layer
}
