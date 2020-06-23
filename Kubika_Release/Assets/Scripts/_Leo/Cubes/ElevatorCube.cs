using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kubika.CustomLevelEditor;

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
        Node lastNodeBeforeGoingOutside;

        // MOVE
        [Space]
        [Header("MOVE")]
        public Node soloMoveTarget;
        public Node soloPileTarget;
        public Vector3 outsideMoveTarget;
        int indexTargetNode;

        //PUSH
        public _CubeMove pushNextNodeCubeMove;

        //PILE
        public _CubeMove pileNodeCubeMove;

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

        public IEnumerator Move(Node nextNode)
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

                currentPos = Vector3.Lerp(basePos, nextNode.worldPosition, currentTime);

                transform.position = currentPos;
                yield return transform.position;
            }




            myIndex = nextNode.nodeIndex;
            nextNode.cubeOnPosition = gameObject;
            //set updated index to cubeMoveable
            nextNode.cubeLayers = CubeLayers.cubeFull;
            nextNode.cubeType = myCubeType;



            xCoordLocal = grid.kuboGrid[nextNode.nodeIndex - 1].xCoord;
            yCoordLocal = grid.kuboGrid[nextNode.nodeIndex - 1].yCoord;
            zCoordLocal = grid.kuboGrid[nextNode.nodeIndex - 1].zCoord;

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

            myIndex = indexTargetNode;

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
            pileNodeCubeMove = null;
            pushNextNodeCubeMove = null;
            IsMovingUpDown = false;
        }

        void CheckingMove(int index, int nodeDirection)
        {
            isMoving = true;

            _DataManager.instance.StartMoving.AddListener(MoveToTarget);
            CheckSoloMove(index, nodeDirection);
        }

        public void CheckSoloMove(int index, int nodeDirection)
        {
            if (MatrixLimitCalcul(index, nodeDirection))
            {
                indexTargetNode = index + nodeDirection;


                switch (grid.kuboGrid[indexTargetNode - 1].cubeLayers)
                {
                    case CubeLayers.cubeFull:
                        {

                            soloMoveTarget = grid.kuboGrid[myIndex - 1];
                            isCheckingMove = false;
                        }
                        break;
                    case CubeLayers.cubeEmpty:
                        {


                            if (nodeDirection == _DirectionCustom.up)
                            {

                                isReadyToMove = true;
                                IsMovingUpDown = true;
                                soloMoveTarget = grid.kuboGrid[myIndex + nodeDirection - 1];

                                isCheckingMove = false;
                            }
                            else if (nodeDirection == _DirectionCustom.down)
                            {

                                isReadyToMove = true;
                                IsMovingUpDown = true;
                                soloMoveTarget = grid.kuboGrid[myIndex + nodeDirection - 1];
                                GoingDownCheck(myIndex, -nodeDirection);
                            }
                            else
                            {

                                isReadyToMove = true;
                                soloMoveTarget = grid.kuboGrid[myIndex + nodeDirection - 1];
                                if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeLayers == CubeLayers.cubeMoveable && MatrixLimitCalcul(myIndex, _DirectionCustom.up))
                                {
                                    pileNodeCubeMove = grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>();
                                    pileNodeCubeMove.CheckingPile(pileNodeCubeMove.myIndex - 1, nodeDirection);
                                }


                                isCheckingMove = false;
                            }
                        }
                        break;
                    case CubeLayers.cubeMoveable:
                        {
                            pushNextNodeCubeMove = grid.kuboGrid[indexTargetNode - 1].cubeOnPosition.GetComponent<_CubeMove>();

                            if (nodeDirection == _DirectionCustom.up || nodeDirection == _DirectionCustom.down)
                            {

                                pushNextNodeCubeMove.soloMoveTarget = grid.kuboGrid[indexTargetNode - 1 + nodeDirection];
                                _DataManager.instance.StartMoving.AddListener(pushNextNodeCubeMove.MoveToTarget);
                                pushNextNodeCubeMove.isReadyToMove = true;
                                cubeMoveUpDown.Add(pushNextNodeCubeMove);
                            }
                            else
                            {
                                if (pushNextNodeCubeMove.isReadyToMove == false)
                                {


                                    pushNextNodeCubeMove.isReadyToMove = true;
                                    pushNextNodeCubeMove.isMovingAndSTFU = true;
                                    pushNextNodeCubeMove.CheckingMove(indexTargetNode, nodeDirection);
                                }
                            }
                            CheckSoloMove(indexTargetNode, nodeDirection);

                        }
                        break;
                }

            }
            else
            {
                isOutside = true;
                outsideMoveTarget = outsideCoord(myIndex, -nodeDirection);

                isCheckingMove = false;
            }

        }

        void GoingDownCheck(int index, int nodeDirection)
        {
            if (MatrixLimitCalcul(index, nodeDirection))
            {
                indexTargetNode = index + nodeDirection;


                switch (grid.kuboGrid[indexTargetNode - 1].cubeLayers)
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
                            pushNextNodeCubeMove = grid.kuboGrid[indexTargetNode - 1].cubeOnPosition.GetComponent<_CubeMove>();


                            pushNextNodeCubeMove.soloMoveTarget = grid.kuboGrid[indexTargetNode - 1 - nodeDirection];
                            _DataManager.instance.StartMoving.AddListener(pushNextNodeCubeMove.MoveToTarget);
                            pushNextNodeCubeMove.isReadyToMove = true;
                            cubeMoveUpDown.Add(pushNextNodeCubeMove);

                            GoingDownCheck(indexTargetNode, nodeDirection);
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