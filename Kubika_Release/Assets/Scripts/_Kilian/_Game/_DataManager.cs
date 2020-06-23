using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Kubika.CustomLevelEditor;
using System;

namespace Kubika.Game
{
    public enum Platform { PC, Mobile };
    public class _DataManager : MonoBehaviour
    {
        // INSTANCE
        private static _DataManager _instance;
        public static _DataManager instance { get { return _instance; } }

        // MOVEABLE CUBE
        //public _CubeBase[] baseCubeArray;
        //public _CubeMove[] moveCubeArray;
        //public ElevatorCube[] elevatorsArray;

        public List<_CubeBase> baseCubeArray = new List<_CubeBase>();
        public List<_CubeMove> moveCubeArray = new List<_CubeMove>();
        public List<ElevatorCube> elevatorsArray = new List<ElevatorCube>();
        public List<TimerCube> timersArray = new List<TimerCube>();

        public List<_CubeMove> moveCube = new List<_CubeMove>();
        public List<ElevatorCube> elevators = new List<ElevatorCube>();
        public List<TimerCube> timers = new List<TimerCube>();

        //UNITY EVENT
        public UnityEvent StartChecking;
        public UnityEvent StartMoving;
        public UnityEvent EndMoving;
        public UnityEvent StartFalling;
        public UnityEvent EndFalling;

        public UnityEvent StartSwipe;
        public UnityEvent EndSwipe;

        // _DIRECTION_CUSTOM
        public int actualRotation;

        //INDEX BANK
        [Space]
        [Header("INDEX BANK")]
        public _DataMatrixScriptable indexBankScriptable;
        public List<_CubeBase> baseCube = new List<_CubeBase>();

        ///////////////INPUT
        [HideInInspector] public Platform platform;
        [HideInInspector] public RaycastHit aimingHit;
        _CubeMove cubeMove;

        //TOUCH INPUT
        [HideInInspector] public Touch touch;
        [HideInInspector] public Vector3 inputPosition;
        Ray rayTouch;
        public int swipeMinimalDistance = 100;

        //PC INPUT
        Ray rayPC;

        // GOLD
        public bool isGolded;

        //AUDIO
        [SerializeField] AudioSource audioSource;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

            // CAP LE FPS A 60 FPS
            if (_CheckCurrentPlatform.platform == RuntimePlatform.Android || _CheckCurrentPlatform.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.targetFrameRate = 60;
                QualitySettings.vSyncCount = 0;
                platform = Platform.Mobile;
            }
            else
            { 
                platform = Platform.PC;
                swipeMinimalDistance /= 3;
            }
            audioSource.clip = _AudioManager.instance.Selection;

        }

        // Start is called before the first frame update

        public void GameSet()
        {
#region LIST VERSION            
            baseCubeArray = new List<_CubeBase>();
            moveCubeArray = new List<_CubeMove>();
            elevatorsArray = new List<ElevatorCube>();
            timersArray = new List<TimerCube>();

            baseCube = new List<_CubeBase>();
            moveCube = new List<_CubeMove>();
            elevators = new List<ElevatorCube>();
            timers = new List<TimerCube>();

            foreach (var item in _Grid.instance.kuboGrid)
            {
                if (item.cubeOnPosition != null)
                {
                    _CubeBase baseCube = item.cubeOnPosition.gameObject.GetComponent<_CubeBase>();
                    _CubeMove cubeMove = item.cubeOnPosition.gameObject.GetComponent<_CubeMove>();
                    ElevatorCube elevatorCube = item.cubeOnPosition.gameObject.GetComponent<ElevatorCube>();
                    TimerCube timerCube = item.cubeOnPosition.gameObject.GetComponent<TimerCube>();

                    if (cubeMove != null)
                    {
                        moveCubeArray.Add(cubeMove);
                    }

                    if (baseCube != null)
                    {
                        baseCubeArray.Add(baseCube);
                    }

                    if (elevatorCube != null)
                    {
                        elevatorsArray.Add(elevatorCube);
                    }

                    if(timerCube != null)
                    {
                        timersArray.Add(timerCube);
                    }
                }
            }
#endregion

#region ARRAY VERSION
            /*elevators.Clear();
            moveCube.Clear();
            baseCube.Clear();

            Array.Clear(moveCubeArray, 0, moveCubeArray.Length);
            Array.Clear(baseCubeArray, 0, baseCubeArray.Length);
            Array.Clear(elevatorsArray, 0, elevatorsArray.Length);

            moveCubeArray = FindObjectsOfType<_CubeMove>(); // TODO : DEGEULASSE
            baseCubeArray = FindObjectsOfType<_CubeBase>();
            elevatorsArray = FindObjectsOfType<ElevatorCube>(); */
#endregion


            foreach (ElevatorCube elevator in elevatorsArray)
            {
                elevators.Add(elevator);
            }

            foreach (_CubeMove cube in moveCubeArray)
            {
                moveCube.Add(cube);
            }

            foreach (_CubeBase cube in baseCubeArray)
            {
                baseCube.Add(cube);
            }
            foreach (TimerCube cube in timersArray)
            {
                timers.Add(cube);
            }


        }

        // Update is called once per frame
        void Update()
        {

            if (platform == Platform.Mobile)
                PhoneInput();
            else
                PCInput();

            if (Input.GetKeyDown(KeyCode.W))
            {
                GameSet();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                MakeFall();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _DirectionCustom.rotationState = actualRotation;
            }
        }

#region INPUT

        void PhoneInput()
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                inputPosition = touch.position;

                rayTouch = _InGameCamera.instance.NormalCam.ScreenPointToRay(touch.position);
                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        if (_KUBRotation.instance.isTurning == false)
                        {
                            if (Physics.Raycast(rayTouch, out aimingHit))
                            {
                                if (aimingHit.collider.gameObject.GetComponent<_CubeMove>() == true)
                                {
                                    cubeMove = aimingHit.collider.gameObject.GetComponent<_CubeMove>();

                                    if (cubeMove.isSelectable == true)
                                    {
                                        cubeMove.isSeletedNow = true;
                                        cubeMove.GetBasePoint();
                                        cubeMove.AddOutline(true);
                                        audioSource.Play();
                                    }
                                    else
                                    {
                                        cubeMove.SetupCantMoveSound();
                                        cubeMove.AddOutline(false);
                                    }
                                }
                            }
                            else
                            {
                                _InGameCamera.instance.isCameraMove = true;

                            }
                        }



                        break;

                    case TouchPhase.Moved:
                        //EMouv.Invoke();
                        inputPosition = touch.position;
                        if (cubeMove != null && cubeMove.isSelectable == true)
                        {
                            cubeMove.NextDirection();
                        }

                        break;

                    case TouchPhase.Ended:
                        {
                            _InGameCamera.instance.isCameraMove = false;
                            if (cubeMove != null)
                            {
                                cubeMove.StopPlayingSound();
                                EndSwipe.Invoke();
                                cubeMove.isSeletedNow = false;
                                cubeMove = null;
                            }
                        }
                        break;
                }


            }

        }
        //__
        void PCInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_KUBRotation.instance.isTurning == false)
                {
                    rayPC = _InGameCamera.instance.NormalCam.ScreenPointToRay(Input.mousePosition);

                    inputPosition = Input.mousePosition;

                    if (Physics.Raycast(rayPC, out aimingHit))
                    {
                        if (aimingHit.collider.gameObject.GetComponent<_CubeMove>() == true)
                        {
                            cubeMove = aimingHit.collider.gameObject.GetComponent<_CubeMove>();

                            if (cubeMove.isSelectable == true)
                            {
                                cubeMove.isSeletedNow = true;
                                cubeMove.GetBasePoint();
                                cubeMove.AddOutline(true);
                                audioSource.Play();
                            }
                            else
                            {
                                cubeMove.SetupCantMoveSound();
                                cubeMove.AddOutline(false);
                            }
                        }
                    }
                    else
                    {
                        _InGameCamera.instance.isCameraMove = true;

                    }
                }

            }
            else if (Input.GetMouseButton(0))
            {
                inputPosition = Input.mousePosition;
                if (cubeMove != null && cubeMove.isSelectable == true)
                {
                    cubeMove.NextDirection();
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                _InGameCamera.instance.isCameraMove = false;
                _InGameCamera.instance.SetupCameraPcInputBool = false;
                if (cubeMove != null)
                {
                    EndSwipe.Invoke();
                    cubeMove.isSeletedNow = false;
                    cubeMove = null;
                }
            }

        }

#endregion


#region MAKE FALL
        public void MakeFall()
        {
            foreach (_CubeMove cubes in moveCube)
            {
                cubes.CheckIfFalling();
            }
            StartCoroutine(CubesAreCheckingFall());
        }
#endregion

#region INDEX RESET

        public void ResetIndex(int rotationState)
        {

            foreach (_CubeBase cBase in baseCube)
            {
                _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeOnPosition = null;
                _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
                _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeType = CubeTypes.None;
            }

            /////// DEMON SCRIPT TODO DEGEULASS

            switch (rotationState)
            {
                case 0:
                    {
                        foreach (_CubeBase cBase in baseCube)
                        {
                            cBase.myIndex = indexBankScriptable.indexBank[cBase.myIndex - 1].nodeIndex0;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeOnPosition = cBase.gameObject;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeLayers = cBase.myCubeLayer;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeType = cBase.myCubeType;

                        }
                    }
                    break;
                case 1:
                    {


                        foreach (_CubeBase cBase in baseCube)
                        {                          
                            cBase.myIndex = indexBankScriptable.indexBank[cBase.myIndex - 1].nodeIndex1;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeOnPosition = cBase.gameObject;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeLayers = cBase.myCubeLayer;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeType = cBase.myCubeType;                           

                        }
                    }
                    break;
                case 2:
                    {


                        foreach (_CubeBase cBase in baseCube)
                        {
                            
                            cBase.myIndex = indexBankScriptable.indexBank[cBase.myIndex - 1].nodeIndex2;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeOnPosition = cBase.gameObject;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeLayers = cBase.myCubeLayer;
                            _Grid.instance.kuboGrid[cBase.myIndex - 1].cubeType = cBase.myCubeType;
                        }

                    }
                    break;
            }


        }
#endregion

#region TIMED EVENT
        public IEnumerator CubesAreCheckingMove()
        {
            while (AreCubesCheckingMove(moveCube.ToArray()) == false)
            {
                yield return null;
            }
            //EndFalling.RemoveAllListeners();
            StartMoving.Invoke();
            StartCoroutine(CubesAreEndingToMove());
        }

        public IEnumerator CubesAndElevatorAreCheckingMove()
        {
            while (AreCubesAndElevatorsAndTimerCheckingMove(elevators.ToArray(), moveCube.ToArray(), timers.ToArray()) == false)
            {
                yield return null;
            }
            //EndFalling.RemoveAllListeners();
            StartMoving.Invoke();
            StartCoroutine(CubesAndElevatorsAreEndingToMove());
        }

        public IEnumerator CubesAreEndingToMove()
        {
            while (AreCubesEndingToMove(moveCube.ToArray()) == false)
            {
                yield return null;
            }
            StartMoving.RemoveAllListeners();
            EndMoving.Invoke();
            MakeFall();
        }

        public IEnumerator CubesAndElevatorsAreEndingToMove()
        {
            while (AreCubesAndElevatorsMoving(elevators.ToArray(), moveCube.ToArray()) == false)
            {
                yield return null;
            }
            StartMoving.RemoveAllListeners();
            EndMoving.Invoke();
            MakeFall();
        }


        

        public IEnumerator CubesAreCheckingFall()
        {
            while (AreCubesCheckingFall(moveCube.ToArray()) == false)
            {
                yield return null;
            }
            //EndMoving.RemoveAllListeners();
            StartFalling.Invoke();
            StartCoroutine(CubesAreEndingToFall());
        }

        public IEnumerator CubesAreEndingToFall()
        {
            while (AreCubesEndingToFall(moveCube.ToArray()) == false)
            {
                yield return null;
            }
            //StartFalling.RemoveAllListeners();
            EndFalling.Invoke();
        }


        //////////////////////

        public bool AreCubesCheckingMove(_CubeMove[] cubeMove)
        {
            for (int i = 0; i < cubeMove.Length; i++)
            {
                if (cubeMove[i].isCheckingMove == true)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreCubesCheckingFall(_CubeMove[] cubeMove)
        {
            for (int i = 0; i < cubeMove.Length; i++)
            {
                if (cubeMove[i].isCheckingFall == true)
                {
                    return false;
                }
            }

            return true;
        }


        public bool AreCubesEndingToMove(_CubeMove[] cubeMove)
        {

            for (int i = 0; i < cubeMove.Length; i++)
            {

                if (cubeMove[i].isMoving == true)
                {
                    return false;
                }
            }

            return true;
        }


        public bool AreCubesEndingToFall(_CubeMove[] cubeMove)
        {
            for (int i = 0; i < cubeMove.Length; i++)
            {
                if (cubeMove[i].isFalling == true)
                {
                    return false;
                }
            }

            return true;
        }


        ///// ELEVATOR

        public bool AreElevatorsCheckingMove(ElevatorCube[] elevators)
        {
            for (int i = 0; i < elevators.Length; i++)
            {
                if (elevators[i].isCheckingMove == true)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreElevatorsEndingToMove(ElevatorCube[] elevators)
        {
            for (int i = 0; i < elevators.Length; i++)
            {
                if (elevators[i].isMoving == true)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AreCubesAndElevatorsAndTimerCheckingMove(ElevatorCube[] elevators, _CubeMove[] cubeMove, TimerCube[] timer)
        {
            if(AreElevatorsCheckingMove(elevators) == true && AreCubesCheckingMove(cubeMove) == true && AreTimersPoping(timer))
            {

                return true;
            }
            else
            {

                return false;
            }

        }

        public bool AreTimersPoping(TimerCube[] timer)
        {
            for (int i = 0; i < timer.Length; i++)
            {
                if (timer[i].willPOP == true)
                {

                    return false;
                }
            }


            return true;

        }

        public bool AreCubesAndElevatorsMoving(ElevatorCube[] elevators, _CubeMove[] cubeMove)
        {
            if (AreElevatorsEndingToMove(elevators) == true && AreCubesEndingToMove(cubeMove) == true)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

#endregion



    }

}
