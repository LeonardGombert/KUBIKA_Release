using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class ElevatorCube : _CubeScanner
    {
        // Start is called before the first frame update

        public bool isGreen = true;

        // BOOL ACTION
        [Space]
        [Header("BOOL ACTION")]
        public bool isCheckingMove;
        public bool isMoving;
        public bool isOutside;
        public bool isReadyToMove;
        public bool cubeIsStillInPlace;
        public bool IsMovingUpDown;

        // LERP
        Vector3 currentPos;
        Vector3 basePos;
        float currentTime;
        public float moveTime = 0.5f;

        // COORD SYSTEM
        [Space]
        [Header("COORD SYSTEM")]
        public int xCoordLocal;
        public int yCoordLocal;
        public int zCoordLocal;

        //FALL OUTSIDE
        [Space]
        [Header("OUTSIDE")]
        public int nbrDeCubeFallOutside = 10;
        Vector3 moveOutsideTarget;
        KuboNode lastKuboNodeBeforeGoingOutside;

        // MOVE
        [Space]
        [Header("MOVE")]
        public KuboNode soloMoveTarget;
        public KuboNode soloPileTarget;
        public Vector3 outsideMoveTarget;
        int indexTargetKuboNode;

        //PUSH
        public _CubeMove pushNextKuboNodeCubeMove;

        //PILE
        public _CubeMove pileKuboNodeCubeMove;

        // UP / DOWN MOVE
        List<_CubeMove> cubeMoveUpDown = new List<_CubeMove>();


        public override  void Start()
        {
            //ScannerSet();
            _DataManager.instance.EndFalling.AddListener(CheckingIfCanPush);
            _DataManager.instance.EndMoving.AddListener(ResetReadyToMove);

            //call base.start AFTER assigning the cube's layers
            base.Start();

            //starts as a static cube
            CheckCubeType();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.B))
            {
                //ScannerSet();
                CheckingIfCanPush();
            }
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.elevators.Remove(this as ElevatorCube);
            _DataManager.instance.EndFalling.RemoveListener(CheckingIfCanPush);
            _DataManager.instance.EndMoving.RemoveListener(ResetReadyToMove);
        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.elevators.Add(this as ElevatorCube);
            _DataManager.instance.EndFalling.AddListener(CheckingIfCanPush);
            _DataManager.instance.EndMoving.AddListener(ResetReadyToMove);
        }

        void CheckCubeType()
        {
            if(myCubeType == CubeTypes.GreenElevatorCube)
            {
                isGreen = true;
            }
            else
            {
                isGreen = false;
            }
        }

        void CheckingIfCanPush()
        {
            isCheckingMove = true;
            if (isGreen)
            {

                if (MatrixLimitCalcul(myIndex, _DirectionCustom.LocalScanner(facingDirection)))
                {

                    if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.LocalScanner(facingDirection)].cubeLayers == CubeLayers.cubeEmpty)
                    {
                        isCheckingMove = false;
                        cubeIsStillInPlace = false;
                    }
                    else if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.LocalScanner(facingDirection)].cubeLayers == CubeLayers.cubeMoveable && cubeIsStillInPlace == false)
                    {

                        isCheckingMove = true;
                        cubeIsStillInPlace = true;
                        CheckingMove(myIndex, _DirectionCustom.LocalScanner(facingDirection));
                        StartCoroutine(_DataManager.instance.CubesAndElevatorAreCheckingMove());
                    }
                    else
                    {

                        isCheckingMove = false;
                    }
                }
                else
                {

                    isCheckingMove = false;
                }
            }
            else
            {

                if (MatrixLimitCalcul(myIndex, _DirectionCustom.LocalScanner(facingDirection)))
                {

                    if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.LocalScanner(facingDirection)].cubeLayers == CubeLayers.cubeEmpty)
                    {
                        cubeIsStillInPlace = false;
                        isCheckingMove = false;
                    }
                    else if (grid.kuboGrid[myIndex - 1 + (_DirectionCustom.LocalScanner(facingDirection))].cubeLayers == CubeLayers.cubeMoveable)
                    {
                          
                        if (cubeIsStillInPlace == false)
                        {

                            isCheckingMove = true;
                            CheckingMove(myIndex, -_DirectionCustom.LocalScanner(facingDirection));
                            StartCoroutine(_DataManager.instance.CubesAndElevatorAreCheckingMove());
                        }
                        else
                        {
                            isCheckingMove = false;
                        }

                    }
                    else
                    {

                        isCheckingMove = false;
                    }
                }
                else
                {

                    isCheckingMove = false;
                }
            }
        }

        #region MOVE

        public IEnumerator Move(KuboNode nextKuboNode)
        {

            isMoving = true;
            isCheckingMove = false;


            grid.kuboGrid[myIndex - 1].cubeOnPosition = null;
            grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            grid.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;

            basePos = transform.position;
            currentTime = 0;

            while (currentTime <= 1)
            {
                currentTime += Time.deltaTime;
                currentTime = (currentTime / moveTime);

                currentPos = Vector3.Lerp(basePos, nextKuboNode.worldPosition, currentTime);

                transform.position = currentPos;
                yield return transform.position;
            }




            myIndex = nextKuboNode.nodeIndex;
            nextKuboNode.cubeOnPosition = gameObject;
            //set updated index to cubeMoveable
            nextKuboNode.cubeLayers = CubeLayers.cubeFull;
            nextKuboNode.cubeType = myCubeType;



            xCoordLocal = grid.kuboGrid[nextKuboNode.nodeIndex - 1].xCoord;
            yCoordLocal = grid.kuboGrid[nextKuboNode.nodeIndex - 1].yCoord;
            zCoordLocal = grid.kuboGrid[nextKuboNode.nodeIndex - 1].zCoord;

            isMoving = false;




        }

        public IEnumerator MoveFromMap(Vector3 nextPosition)
        {
            isMoving = true;
            isOutside = true;
            isCheckingMove = false;

            moveOutsideTarget = new Vector3(nextPosition.x * grid.offset, nextPosition.y * grid.offset, nextPosition.z * grid.offset);

            grid.kuboGrid[myIndex - 1].cubeOnPosition = null;
            grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            grid.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;

            basePos = transform.position;
            currentTime = 0;

            while (currentTime <= 1)
            {
                currentTime += Time.deltaTime;
                currentTime = (currentTime / moveTime);

                currentPos = Vector3.Lerp(basePos, moveOutsideTarget, currentTime);

                transform.position = currentPos;
                yield return transform.position;
            }

            myIndex = indexTargetKuboNode;

            xCoordLocal = Mathf.RoundToInt(moveOutsideTarget.x / grid.offset);
            yCoordLocal = Mathf.RoundToInt(moveOutsideTarget.y / grid.offset);
            zCoordLocal = Mathf.RoundToInt(moveOutsideTarget.z / grid.offset);

            isMoving = false;

        }

        public void MoveToTarget()
        {
            if(isGreen == true)
            {
                isGreen = false;
                audioSourceCube.clip = _AudioManager.instance.ElevatorGoDoxn;
                PlaySound();
                ChangeElevatorTexture(_MaterialCentral.instance.actualPack._ElevatorBackTex);

            }
            else 
            {
                isGreen = true;
                audioSourceCube.clip = _AudioManager.instance.ElevatorGoUp;
                PlaySound();
                ChangeElevatorTexture(_MaterialCentral.instance.actualPack._ElevatorTex);

            }

            if (isOutside == false)
            {

                StartCoroutine(Move(soloMoveTarget));
            }
            else
            {

                StartCoroutine(MoveFromMap(outsideMoveTarget));
            }
        }

        public void ResetReadyToMove()
        {

            isReadyToMove = false;
            pileKuboNodeCubeMove = null;
            pushNextKuboNodeCubeMove = null;
            IsMovingUpDown = false;
        }

        void CheckingMove(int index, int KuboNodeDirection)
        {
            isMoving = true;

            _DataManager.instance.StartMoving.AddListener(MoveToTarget);
            CheckSoloMove(index, KuboNodeDirection);
        }

        public void CheckSoloMove(int index, int KuboNodeDirection)
        {
            if (MatrixLimitCalcul(index, KuboNodeDirection))
            {
                indexTargetKuboNode = index + KuboNodeDirection;


                switch (grid.kuboGrid[indexTargetKuboNode - 1].cubeLayers)
                {
                    case CubeLayers.cubeFull:
                        {

                            soloMoveTarget = grid.kuboGrid[myIndex - 1];
                            isCheckingMove = false;
                        }
                        break;
                    case CubeLayers.cubeEmpty:
                        {


                            if (KuboNodeDirection == _DirectionCustom.up)
                            {

                                isReadyToMove = true;
                                IsMovingUpDown = true;
                                soloMoveTarget = grid.kuboGrid[myIndex + KuboNodeDirection - 1];

                                isCheckingMove = false;
                            }
                            else if (KuboNodeDirection == _DirectionCustom.down)
                            {

                                isReadyToMove = true;
                                IsMovingUpDown = true;
                                soloMoveTarget = grid.kuboGrid[myIndex + KuboNodeDirection - 1];
                                GoingDownCheck(myIndex, -KuboNodeDirection);
                            }
                            else
                            {

                                isReadyToMove = true;
                                soloMoveTarget = grid.kuboGrid[myIndex + KuboNodeDirection - 1];
                                if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeLayers == CubeLayers.cubeMoveable && MatrixLimitCalcul(myIndex, _DirectionCustom.up))
                                {
                                    pileKuboNodeCubeMove = grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>();
                                    pileKuboNodeCubeMove.CheckingPile(pileKuboNodeCubeMove.myIndex - 1, KuboNodeDirection);
                                }


                                isCheckingMove = false;
                            }
                        }
                        break;
                    case CubeLayers.cubeMoveable:
                        {
                            pushNextKuboNodeCubeMove = grid.kuboGrid[indexTargetKuboNode - 1].cubeOnPosition.GetComponent<_CubeMove>();

                            if (KuboNodeDirection == _DirectionCustom.up || KuboNodeDirection == _DirectionCustom.down)
                            {

                                pushNextKuboNodeCubeMove.soloMoveTarget = grid.kuboGrid[indexTargetKuboNode - 1 + KuboNodeDirection];
                                _DataManager.instance.StartMoving.AddListener(pushNextKuboNodeCubeMove.MoveToTarget);
                                pushNextKuboNodeCubeMove.isReadyToMove = true;
                                cubeMoveUpDown.Add(pushNextKuboNodeCubeMove);
                            }
                            else
                            {
                                if (pushNextKuboNodeCubeMove.isReadyToMove == false)
                                {


                                    pushNextKuboNodeCubeMove.isReadyToMove = true;
                                    pushNextKuboNodeCubeMove.isMovingAndSTFU = true;
                                    pushNextKuboNodeCubeMove.CheckingMove(indexTargetKuboNode, KuboNodeDirection);
                                }
                            }
                            CheckSoloMove(indexTargetKuboNode, KuboNodeDirection);

                        }
                        break;
                }

            }
            else
            {
                isOutside = true;
                outsideMoveTarget = outsideCoord(myIndex, -KuboNodeDirection);

                isCheckingMove = false;
            }

        }

        void GoingDownCheck(int index, int KuboNodeDirection)
        {
            if (MatrixLimitCalcul(index, KuboNodeDirection))
            {
                indexTargetKuboNode = index + KuboNodeDirection;


                switch (grid.kuboGrid[indexTargetKuboNode - 1].cubeLayers)
                {
                    case CubeLayers.cubeFull:
                        {

                            isCheckingMove = false;
                        }
                        break;
                    case CubeLayers.cubeEmpty:
                        {


                            isCheckingMove = false;
                        }
                        break;
                    case CubeLayers.cubeMoveable:
                        {
                            pushNextKuboNodeCubeMove = grid.kuboGrid[indexTargetKuboNode - 1].cubeOnPosition.GetComponent<_CubeMove>();


                            pushNextKuboNodeCubeMove.soloMoveTarget = grid.kuboGrid[indexTargetKuboNode - 1 - KuboNodeDirection];
                            _DataManager.instance.StartMoving.AddListener(pushNextKuboNodeCubeMove.MoveToTarget);
                            pushNextKuboNodeCubeMove.isReadyToMove = true;
                            cubeMoveUpDown.Add(pushNextKuboNodeCubeMove);

                            GoingDownCheck(indexTargetKuboNode, KuboNodeDirection);
                            cubeIsStillInPlace = true;
                        }
                        break;
                }
            }
            else
            {
                isCheckingMove = false;
            }
        }


        #endregion

        #region FEEDBACK

        protected void ChangeElevatorTexture(Texture newTex)
        {
            meshRenderer.GetPropertyBlock(MatProp);

            MatProp.SetTexture("_MainTex", newTex);

            meshRenderer.SetPropertyBlock(MatProp);
        }

        #endregion



    }
}