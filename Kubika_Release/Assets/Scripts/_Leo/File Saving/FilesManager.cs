using Kubika.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Files
{
    public class FilesManager : MonoBehaviour
    {
        private static FilesManager _instance;
        public static FilesManager instance { get { return _instance; } }

        public GameObject cubePrefab;
        private bool finishedBuilding;
        private KuboNode currentNode;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region //INGAME 
        public void InGameLoad()
        {

        }

        public PlayerProgress UpdatePlayerProg()
        {
            return null;
        }

        public string GetFileFolder(Biomes biome)
        {
            string levelFolder = "";

            switch (biome)
            {
                case Biomes.Plains:
                    levelFolder = "01_Plains";
                    break;
                case Biomes.Mountains:
                    levelFolder = "02_Mountains";
                    break;
                case Biomes.Underwater:
                    levelFolder = "03_Underwater";
                    break;
                case Biomes.Ruins:
                    levelFolder = "04_Ruins";
                    break;
                case Biomes.Statues:
                    levelFolder = "05_Statues";
                    break;
                case Biomes.Temple:
                    levelFolder = "06_Temple";
                    break;
                case Biomes.Chaos:
                    levelFolder = "07_Chaos";
                    break;
                default:
                    break;
            }

            return levelFolder;
        }
        #endregion

        #region //UNITY EDITOR
        public void DevSave()
        {

        }

        public void DevLoad()
        {

        }

        //quickly save the current open level
        public void DevSaveCurr()
        {

        }
        #endregion

        #region //INGAME EDITOR
        public void UserSave()
        {

        }

        public void UserSaveCurr()
        {

        }

        public void UserLoad()
        {

        }

        public void UserDelete()
        {

        }
        #endregion

        public void ExtractLevel(LevelEditorData recoveredData)
        {
            finishedBuilding = false;

            // start by resetting the grid's nodes to their base states
            Kubo.instance.ResetIndexGrid();

            foreach (KuboNode recoveredNode in recoveredData.nodesToSave)
            {
                // EXTREMELY IMPORTANT -> CONVERTS THE CUBE'S TYPE FROM STRING TO ENUM
                recoveredNode.cubeType = KuboNode.ConvertStringToCubeType(recoveredNode.savedCubeType);

                //Set the universe textures
                //_MaterialCentral.instance.ChangeUniverse(recoveredData.biome);

                currentNode = recoveredNode;

                GameObject newCube = Instantiate(cubePrefab);

                Kubo.instance.placedCubes.Add(newCube);

                // get the kuboGrid and set the information on each of the nodes
                SetNodeInfo(newCube, recoveredNode.nodeIndex, recoveredNode.worldPosition, recoveredNode.worldRotation,
                    recoveredNode.facingDirection, recoveredNode.cubeLayers, recoveredNode.cubeType);

                // check the node's cube type and setup the relevant cube and its transform + individual information
                switch (recoveredNode.cubeType)
                {
                    case CubeTypes.FullStaticCube:
                        newCube.AddComponent(typeof(StaticCube));
                        StaticCube fullStaticCube = newCube.GetComponent<StaticCube>();
                        SetCubeInfo(fullStaticCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.FullStaticCube, true);
                        break;

                    case CubeTypes.EmptyStaticCube:
                        newCube.AddComponent(typeof(StaticCube));
                        StaticCube emptyCube = newCube.GetComponent<StaticCube>();
                        SetCubeInfo(emptyCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.EmptyStaticCube, true);
                        break;

                    case CubeTypes.TopStaticCube:
                        newCube.AddComponent(typeof(StaticCube));
                        StaticCube topStaticCube = newCube.GetComponent<StaticCube>();
                        SetCubeInfo(topStaticCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.TopStaticCube, true);
                        break;

                    case CubeTypes.CornerStaticCube:
                        newCube.AddComponent(typeof(StaticCube));
                        StaticCube cornerStaticCube = newCube.GetComponent<StaticCube>();
                        SetCubeInfo(cornerStaticCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.CornerStaticCube, true);
                        break;

                    case CubeTypes.TripleStaticCube:
                        newCube.AddComponent(typeof(StaticCube));
                        StaticCube tripleStaticCube = newCube.GetComponent<StaticCube>();
                        SetCubeInfo(tripleStaticCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.TripleStaticCube, true);
                        break;

                    case CubeTypes.QuadStaticCube:
                        newCube.AddComponent(typeof(StaticCube));
                        StaticCube quadStaticCube = newCube.GetComponent<StaticCube>();
                        SetCubeInfo(quadStaticCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.QuadStaticCube, true);
                        break;

                    case CubeTypes.MoveableCube:
                        newCube.AddComponent(typeof(MoveableCube));
                        MoveableCube moveableCube = newCube.GetComponent<MoveableCube>();
                        SetCubeInfo(moveableCube as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.MoveableCube, false);
                        break;

                    case CubeTypes.BaseVictoryCube:
                        newCube.AddComponent(typeof(BaseVictoryCube));
                        BaseVictoryCube baseVictoryCube = newCube.GetComponent<BaseVictoryCube>();
                        SetCubeInfo(baseVictoryCube as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.BaseVictoryCube, false);
                        break;

                    case CubeTypes.ConcreteVictoryCube:
                        newCube.AddComponent(typeof(ConcreteVictoryCube));
                        ConcreteVictoryCube concreteVictoryCube = newCube.GetComponent<ConcreteVictoryCube>();
                        SetCubeInfo(concreteVictoryCube as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.ConcreteVictoryCube, false);
                        break;

                    case CubeTypes.BombVictoryCube:
                        newCube.AddComponent(typeof(BombVictoryCube));
                        BombVictoryCube bombVictoryCube = newCube.GetComponent<BombVictoryCube>();
                        SetCubeInfo(bombVictoryCube as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.BombVictoryCube, false);
                        break;

                    case CubeTypes.SwitchVictoryCube:
                        newCube.AddComponent(typeof(SwitchVictoryCube));
                        SwitchVictoryCube switchVictoryCube = newCube.GetComponent<SwitchVictoryCube>();
                        SetCubeInfo(switchVictoryCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.SwitchVictoryCube, true);
                        break;

                    case CubeTypes.DeliveryCube:
                        newCube.AddComponent(typeof(DeliveryCube));
                        DeliveryCube deliveryCube = newCube.GetComponent<DeliveryCube>();
                        SetCubeInfo(deliveryCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.DeliveryCube, true);
                        break;

                    case CubeTypes.BlueElevatorCube:
                        newCube.AddComponent(typeof(ElevatorCube));
                        ElevatorCube blueElevatorCube = newCube.GetComponent<ElevatorCube>();
                        blueElevatorCube.isGreen = false;
                        SetCubeInfo(blueElevatorCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.BlueElevatorCube, false);
                        break;

                    case CubeTypes.GreenElevatorCube:
                        newCube.AddComponent(typeof(ElevatorCube));
                        ElevatorCube greenElevatorCube = newCube.GetComponent<ElevatorCube>();
                        greenElevatorCube.isGreen = true;
                        SetCubeInfo(greenElevatorCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.GreenElevatorCube, false);
                        break;

                    case CubeTypes.ConcreteCube:
                        newCube.AddComponent(typeof(ConcreteCube));
                        ConcreteCube concreteCube = newCube.GetComponent<ConcreteCube>();
                        SetCubeInfo(concreteCube as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.ConcreteCube, false);
                        break;

                    case CubeTypes.BombCube:
                        newCube.AddComponent(typeof(BombCube));
                        BombCube bombCube = newCube.GetComponent<BombCube>();
                        SetCubeInfo(bombCube as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.BombCube, false);
                        break;

                    #region // TIMER CUBE
                    case CubeTypes.TimerCube1:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube1, true);
                        timerCube.timerValue = 1;
                        break;

                    case CubeTypes.TimerCube2:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube2 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube2 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube2, true);
                        timerCube2.timerValue = 2;
                        break;

                    case CubeTypes.TimerCube3:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube3 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube3 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube3, true);
                        timerCube3.timerValue = 3;
                        break;

                    case CubeTypes.TimerCube4:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube4 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube4 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube4, true);
                        timerCube4.timerValue = 4;
                        break;

                    case CubeTypes.TimerCube5:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube5 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube5 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube5, true);
                        timerCube5.timerValue = 5;
                        break;

                    case CubeTypes.TimerCube6:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube6 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube6 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube6, true);
                        timerCube6.timerValue = 6;
                        break;

                    case CubeTypes.TimerCube7:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube7 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube7 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube7, true);
                        timerCube7.timerValue = 7;
                        break;

                    case CubeTypes.TimerCube8:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube8 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube8 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube8, true);
                        timerCube8.timerValue = 8;
                        break;

                    case CubeTypes.TimerCube9:
                        newCube.AddComponent(typeof(TimerCube));
                        TimerCube timerCube9 = newCube.GetComponent<TimerCube>();
                        SetCubeInfo(timerCube9 as _CubeBase, CubeLayers.cubeFull, CubeTypes.TimerCube9, true);
                        timerCube9.timerValue = 9;
                        break;
                    #endregion CUBES

                    case CubeTypes.SwitchButton:
                        newCube.AddComponent(typeof(SwitchButton));
                        SwitchButton switchButton = newCube.GetComponent<SwitchButton>();
                        SetCubeInfo(switchButton as _CubeBase, CubeLayers.cubeFull, CubeTypes.SwitchButton, true);
                        break;

                    case CubeTypes.SwitchCube:
                        newCube.AddComponent(typeof(SwitchCube));
                        SwitchCube switchCube = newCube.GetComponent<SwitchCube>();
                        SetCubeInfo(switchCube as _CubeBase, CubeLayers.cubeFull, CubeTypes.SwitchCube, true);
                        break;

                    case CubeTypes.RotatorRightTurner:
                        newCube.AddComponent(typeof(RotateRightCube));
                        RotateRightCube rotatorRightTurn = newCube.GetComponent<RotateRightCube>();
                        SetCubeInfo(rotatorRightTurn as _CubeBase, CubeLayers.cubeFull, CubeTypes.RotatorRightTurner, true);
                        break;

                    case CubeTypes.RotatorLeftTurner:
                        newCube.AddComponent(typeof(RotateLeftCube));
                        RotateLeftCube rotatorLeftTurn = newCube.GetComponent<RotateLeftCube>();
                        SetCubeInfo(rotatorLeftTurn as _CubeBase, CubeLayers.cubeFull, CubeTypes.RotatorLeftTurner, true);
                        break;

                    case CubeTypes.RotatorLocker:
                        newCube.AddComponent(typeof(RotatorLocker));
                        RotatorLocker rotatorLocker = newCube.GetComponent<RotatorLocker>();
                        SetCubeInfo(rotatorLocker as _CubeBase, CubeLayers.cubeFull, CubeTypes.RotatorLocker, true);
                        break;

                    case CubeTypes.ChaosBall:
                        newCube.AddComponent(typeof(ChaosBall));
                        ChaosBall chaosBall = newCube.GetComponent<ChaosBall>();
                        SetCubeInfo(chaosBall as _CubeBase, CubeLayers.cubeMoveable, CubeTypes.ChaosBall, false);
                        break;

                    default:
                        //set epmty cubes as cubeEmpty
                        Kubo.instance.kuboGrid[recoveredNode.nodeIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
                        Kubo.instance.kuboGrid[recoveredNode.nodeIndex - 1].cubeType = CubeTypes.None;
                        break;
                }
            }

            finishedBuilding = true;
        }

        void SetNodeInfo(GameObject newCube, int nodeIndex, Vector3 worldPosition, Vector3 worldRotation, FacingDirection facingDirection, CubeLayers cubeLayers, CubeTypes cubeTypes)
        {
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].cubeOnPosition = newCube;
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].nodeIndex = nodeIndex;
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].worldPosition = worldPosition;
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].worldRotation = worldRotation;
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].facingDirection = facingDirection;
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].cubeLayers = cubeLayers;
            Kubo.instance.kuboGrid[currentNode.nodeIndex - 1].cubeType = cubeTypes;
        }

        //set the node's information relevant to the cube type, then send the Transform information to the cube
        void SetCubeInfo(_CubeBase cube, CubeLayers cubeLayers, CubeTypes cubeTypes, bool _static)
        {
            cube.myIndex = currentNode.nodeIndex;
            cube.myCubeType = cubeTypes;
            cube.myCubeLayer = cubeLayers;
            cube.isStatic = _static;
            cube.facingDirection = currentNode.facingDirection;

            //sets rotation and position according to the values of the relevant node in kuboGrid
            cube.OnLoadSetTransform();
        }


    }
}