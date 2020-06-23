using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Kubika.CustomLevelEditor;

namespace Kubika.Game
{
    public enum swipeDirection { Front, Right, Left, Back };

    public class _CubeMove : _CubeScanner
    {
        //FALLING 
        int nbrCubeMouvableBelow;
        public int nbrCubeEmptyBelow;
        int nbrCubeBelow;
        int indexTargetNode;
        public bool thereIsEmpty = false;
        Vector3 nextPosition;

        // BOOL ACTION
        [Space]
        [Header("BOOL ACTION")]
        public bool isCheckingMove;
        public bool isCheckingFall;
        public bool isFalling;
        public bool isMoving;
        public bool isOutside;
        public bool isOverNothing;
        public bool isReadyToMove;
        public bool isMovingAndSTFU;

        //FALL MOVE
        Vector3 currentPos;
        Vector3 basePos;
        float currentTime;
        public float moveTime = 0.5f;
        float time;

        //FALL OUTSIDE
        [Space]
        [Header("OUTSIDE")]
        public int nbrDeCubeFallOutside = 20;
        Vector3 moveOutsideTarget;
        [HideInInspector] public Vector3 moveOutsideTargetCustomVector;
        Vector3 fallOutsideTarget;

        // COORD SYSTEM
        [Space]
        [Header("COORD SYSTEM")]
        public int xCoordLocal;
        public int yCoordLocal;
        public int zCoordLocal;


        // MOVE
        [Space]
        [Header("MOVE")]
        public Node soloMoveTarget;
        public Node soloPileTarget;
        public Vector3 outsideMoveTarget;

        //PUSH
        public _CubeMove pushNextNodeCubeMove;

        //PILE
        public _CubeMove pileNodeCubeMove;

        //INPUT
        public bool isSelectable = true;
        public bool isSeletedNow = false;
        public swipeDirection enumSwipe;

        [Space]
        public static swipeDirection worldEnumSwipe;

        // WORLD TO SCREEN
        [HideInInspector] public Vector2 baseCube;
        [HideInInspector] public Vector2 nextCube;

        Vector2 baseSwipePos;
        Vector2 currentSwipePos;

        [HideInInspector] public float distanceBaseToNext;
        [HideInInspector] public float distanceTouch;

        public float angleDirection;
        float inverseAngleDirection;

        // DIRECTION
        public float KUBNord;
        public float KUBWest;
        public float KUBSud;
        public float KUBEst;

        bool movingToPos = false;


        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            _DataManager.instance.EndMoving.AddListener(ResetReadyToMove);
            _DataManager.instance.StartFalling.AddListener(FallMoveFunction);
            _DataManager.instance.EndSwipe.AddListener(ResetOutline);

            audioSourceCube.clip = _AudioManager.instance.Move;
        }

        // Update is called once per frame
        public override void Update()
        {
            //CheckIfFalling();//grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeMoveable;
            base.Update();

            if (Input.GetKeyDown(KeyCode.W))
            {
                // Put Actual Node as Moveable
                myCubeLayer = CubeLayers.cubeMoveable;
                grid.kuboGrid[myIndex - 1].cubeLayers = myCubeLayer;
            }
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.moveCube.Remove(this as _CubeMove);
            _DataManager.instance.EndMoving.RemoveListener(ResetReadyToMove);
            _DataManager.instance.StartFalling.RemoveListener(FallMoveFunction);
            _DataManager.instance.EndSwipe.RemoveListener(ResetOutline);
        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.moveCube.Add(this as _CubeMove);
            _DataManager.instance.EndMoving.AddListener(ResetReadyToMove);
            _DataManager.instance.StartFalling.AddListener(FallMoveFunction);
            _DataManager.instance.EndSwipe.AddListener(ResetOutline);
        }



        #region FALL


        public void CheckIfFalling()
        {
            if (!isStatic)
            {
                isCheckingFall = true;
                thereIsEmpty = false;
                nbrCubeMouvableBelow = 0;
                nbrCubeEmptyBelow = 0;
                indexTargetNode = 0;

                Fall(1);
            }
        }

        public void Fall(int nbrCubeBelowParam)
        {

            if (grid.kuboGrid[myIndex - 1 + (_DirectionCustom.down * nbrCubeBelowParam)].cubeLayers == CubeLayers.cubeEmpty)
            {
                if (MatrixLimitCalcul((myIndex + (_DirectionCustom.down * nbrCubeBelowParam)), _DirectionCustom.down))
                {
                    thereIsEmpty = true;
                    nbrCubeEmptyBelow += 1;
                    Fall(nbrCubeBelowParam + 1);
                }
                else
                {
                    fallOutsideTarget = new Vector3(grid.kuboGrid[myIndex - 1].xCoord * grid.offset,
                                                     grid.kuboGrid[myIndex - 1].yCoord * grid.offset - ((nbrCubeEmptyBelow + nbrCubeMouvableBelow) * grid.offset),
                                                     grid.kuboGrid[myIndex - 1].zCoord * grid.offset);

                    fallOutsideTarget += (Vector3.down * nbrDeCubeFallOutside);
                    isOutside = true;
                    isCheckingFall = false;
                }
            }
            else if (grid.kuboGrid[myIndex - 1 + (_DirectionCustom.down * nbrCubeBelowParam)].cubeLayers == CubeLayers.cubeMoveable)
            {
                if (MatrixLimitCalcul((myIndex + (_DirectionCustom.down * nbrCubeBelowParam)), _DirectionCustom.down))
                {
                    nbrCubeMouvableBelow += 1;
                    Fall(nbrCubeBelowParam + 1);
                }
                else
                {
                    fallOutsideTarget = new Vector3(grid.kuboGrid[myIndex - 1].xCoord * grid.offset,
                                                     grid.kuboGrid[myIndex - 1].yCoord * grid.offset - ((nbrCubeEmptyBelow + nbrCubeMouvableBelow) * grid.offset),
                                                     grid.kuboGrid[myIndex - 1].zCoord * grid.offset);

                    fallOutsideTarget += (Vector3.down * nbrDeCubeFallOutside);
                    isOutside = true;
                    isCheckingFall = false;
                }
            }
            else if (grid.kuboGrid[myIndex - 1 + (_DirectionCustom.down * nbrCubeBelowParam)].cubeLayers == CubeLayers.cubeFull)
            {
                if (MatrixLimitCalcul(myIndex, _DirectionCustom.down))
                {

                    nbrCubeBelow = nbrCubeBelowParam;

                    indexTargetNode = myIndex + (_DirectionCustom.down * nbrCubeBelow) + (_DirectionCustom.up * (nbrCubeMouvableBelow + 1));
                    nextPosition = grid.kuboGrid[indexTargetNode - 1].worldPosition;


                    isCheckingFall = false;
                }
                else
                {

                    fallOutsideTarget = new Vector3(grid.kuboGrid[myIndex - 1].xCoord * grid.offset,
                                                     grid.kuboGrid[myIndex - 1].yCoord * grid.offset - ((nbrCubeEmptyBelow + nbrCubeMouvableBelow) * grid.offset),
                                                     grid.kuboGrid[myIndex - 1].zCoord * grid.offset);

                    fallOutsideTarget += (Vector3.down * nbrDeCubeFallOutside);
                    isOutside = true;
                    isCheckingFall = false;
                }

            }


        }

        public void FallMoveFunction()
        {
            if (thereIsEmpty == true && isOutside == false)
            {
                isFalling = true;
                StartCoroutine(FallMove(nextPosition, nbrCubeEmptyBelow, nbrCubeBelow));
            }
            else if (isOutside == true)
            {
                isFalling = true;
                StartCoroutine(FallFromMap(fallOutsideTarget, nbrDeCubeFallOutside));
            }
        }

        public IEnumerator FallMove(Vector3 fallPosition, int nbrCub, int nbrCubeBelowParam)
        {

            Debug.Log("yes " + grid.kuboGrid[myIndex - 1].cubeLayers);
            ChangeEmoteFace(_EmoteFallTex);

            grid.kuboGrid[myIndex - 1].cubeOnPosition = null;
            grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            grid.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;


            Debug.Log("yes2 " + grid.kuboGrid[myIndex - 1].cubeLayers);

            basePos = transform.position;
            currentTime = 0;

            while (currentTime <= 1)
            {
                currentTime += Time.deltaTime / nbrCub;
                currentTime = (currentTime / moveTime);

                currentPos = Vector3.Lerp(basePos, fallPosition, currentTime);

                transform.position = currentPos;
                yield return transform.position;
            }

            grid.kuboGrid[myIndex - 1].cubeOnPosition = null;
            grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            grid.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;

            Debug.Log("yes2.25 " + grid.kuboGrid[myIndex - 1].cubeLayers + " " + grid.kuboGrid[indexTargetNode - 1].cubeLayers);
            myIndex = indexTargetNode;
            grid.kuboGrid[indexTargetNode - 1].cubeOnPosition = gameObject;
            //set updated index to cubeMoveable
            grid.kuboGrid[indexTargetNode - 1].cubeLayers = CubeLayers.cubeMoveable;
            grid.kuboGrid[indexTargetNode - 1].cubeType = myCubeType;

            xCoordLocal = grid.kuboGrid[indexTargetNode - 1].xCoord;
            yCoordLocal = grid.kuboGrid[indexTargetNode - 1].yCoord;
            zCoordLocal = grid.kuboGrid[indexTargetNode - 1].zCoord;

            ChangeEmoteFace(_EmoteIdleTex);
            isFalling = false;
            Debug.Log("yes2.5 " + grid.kuboGrid[myIndex - 1].cubeLayers + " " + grid.kuboGrid[indexTargetNode - 1].cubeLayers);
        }


        public IEnumerator FallFromMap(Vector3 fallFromMapPosition, int nbrCaseBelow)
        {
            willPOP = true;
            isFalling = true;

            audioSourceCube.clip = _AudioManager.instance.FatalChute;
            PlaySound();

            ChangeEmoteFace(_EmoteFatalFallTex);
            basePos = transform.position;
            currentTime = 0;

            grid.kuboGrid[myIndex - 1].cubeOnPosition = null;
            grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            grid.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;

            while (currentTime <= 1)
            {
                currentTime += Time.deltaTime / (nbrCaseBelow + 200);
                currentTime = (currentTime / moveTime);

                currentPos = Vector3.Lerp(basePos, fallFromMapPosition, currentTime);

                transform.position = currentPos;
                yield return transform.position;
            }


            xCoordLocal = Mathf.RoundToInt(fallFromMapPosition.x / grid.offset);
            yCoordLocal = Mathf.RoundToInt(fallFromMapPosition.y / grid.offset);
            zCoordLocal = Mathf.RoundToInt(fallFromMapPosition.z / grid.offset);

            _DataManager.instance.moveCube.Remove(this);
            _DataManager.instance.baseCube.Remove(this);
            StartCoroutine(PopOut(false));
            StartCoroutine(_BackgroundMaterialPreset.instance.FallBGFeedback());

            isFalling = false;
        }

        #endregion

        #region MOVE

        public IEnumerator Move(Node nextNode)
        {
            isMoving = true;


            if (isSeletedNow && movingToPos)
            {
                PlayerMoves.instance.IncrementMoves();
                PlaySound();
            }
            movingToPos = false;

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
            nextNode.cubeLayers = CubeLayers.cubeMoveable;
            nextNode.cubeType = myCubeType;


            xCoordLocal = grid.kuboGrid[nextNode.nodeIndex - 1].xCoord;
            yCoordLocal = grid.kuboGrid[nextNode.nodeIndex - 1].yCoord;
            zCoordLocal = grid.kuboGrid[nextNode.nodeIndex - 1].zCoord;

            isPastilleAndIsOn = false;

            isMoving = false;

            if (isSeletedNow)
            {
                GetBasePoint(); // RESET SWIPE POS
            }


        }

        public IEnumerator MoveFromMap(Vector3 nextPosition)
        {
            isMoving = true;

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


            fallOutsideTarget = moveOutsideTarget;
            fallOutsideTarget += (Vector3.down * nbrDeCubeFallOutside);

            isMoving = false;

            //_DataManager.instance.EndChecking.Invoke();

        }

        public void MoveToTarget()
        {

            if (isOutside == false)
            {
                StartCoroutine(Move(soloMoveTarget));
            }
            else
            {
                StartCoroutine(MoveFromMap(moveOutsideTargetCustomVector));
            }
        }

        public void MoveToTargetPile()
        {
            StartCoroutine(Move(soloPileTarget));
        }

        public void ResetReadyToMove()
        {
            isReadyToMove = false;
            pileNodeCubeMove = null;
            pushNextNodeCubeMove = null;
            isOverNothing = false;
            isMovingAndSTFU = false;
        }

        public void CheckingMove(int index, int nodeDirection)
        {
            isMoving = true;
            isCheckingMove = true;

            _DataManager.instance.StartMoving.AddListener(MoveToTarget);
            CheckSoloMove(index, nodeDirection);
        }
        public void CheckingPile(int index, int nodeDirection)
        {
            isMoving = true;
            isCheckingMove = true;
            _DataManager.instance.StartMoving.AddListener(MoveToTargetPile);
            CheckPileMove(index, nodeDirection);
        }

        public void CheckSoloMove(int index, int nodeDirection)
        {
            if (isFalling == false)
            {
                if (isMovingAndSTFU == false)
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


                                    if (grid.kuboGrid[indexTargetNode - 1 + _DirectionCustom.down].cubeLayers == CubeLayers.cubeMoveable || grid.kuboGrid[indexTargetNode - 1 + _DirectionCustom.down].cubeLayers == CubeLayers.cubeFull)
                                    {
                                        isReadyToMove = true;
                                        soloMoveTarget = grid.kuboGrid[myIndex + nodeDirection - 1];


                                        movingToPos = true;

                                        if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeLayers == CubeLayers.cubeMoveable && MatrixLimitCalcul(myIndex, _DirectionCustom.up))
                                        {
                                            pileNodeCubeMove = grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>();
                                            pileNodeCubeMove.CheckingPile(pileNodeCubeMove.myIndex - 1, nodeDirection);
                                        }

                                    }
                                    else
                                    {
                                        isReadyToMove = true;

                                        soloMoveTarget = grid.kuboGrid[myIndex - 1];
                                    }
                                    isCheckingMove = false;

                                }
                                break;
                            case CubeLayers.cubeMoveable:
                                {

                                    pushNextNodeCubeMove = grid.kuboGrid[indexTargetNode - 1].cubeOnPosition.GetComponent<_CubeMove>();
                                    if (pushNextNodeCubeMove.isReadyToMove == false)
                                    {
                                        pushNextNodeCubeMove.CheckingMove(indexTargetNode, nodeDirection);
                                    }
                                    CheckSoloMove(indexTargetNode, nodeDirection);
                                }
                                break;
                        }

                    }
                    else
                    {

                        soloMoveTarget = grid.kuboGrid[myIndex - 1];
                        isCheckingMove = false;
                    }
                }
                else
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

                                    isReadyToMove = true;
                                    soloMoveTarget = grid.kuboGrid[index + nodeDirection - 1];



                                    if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeLayers == CubeLayers.cubeMoveable && MatrixLimitCalcul(myIndex, _DirectionCustom.up))
                                    {
                                        pileNodeCubeMove = grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>();
                                        pileNodeCubeMove.CheckingPile(pileNodeCubeMove.myIndex - 1, nodeDirection);
                                    }


                                    isCheckingMove = false;
                                }
                                break;
                            case CubeLayers.cubeMoveable:
                                {

                                    pushNextNodeCubeMove = grid.kuboGrid[indexTargetNode - 1].cubeOnPosition.GetComponent<_CubeMove>();
                                    if (pushNextNodeCubeMove.isReadyToMove == false)
                                    {
                                        pushNextNodeCubeMove.CheckingMove(indexTargetNode, nodeDirection);
                                    }
                                    CheckSoloMove(indexTargetNode, nodeDirection);
                                }
                                break;
                        }

                    }
                    else
                    {

                        moveOutsideTargetCustomVector = outsideCoord(myIndex, -nodeDirection);
                        isOutside = true;
                        isCheckingMove = false;
                    }
                }
            }

        }

        public void CheckPileMove(int index, int nodeDirection)
        {
            if (MatrixLimitCalcul(index, nodeDirection))
            {
                indexTargetNode = index + nodeDirection;


                switch (grid.kuboGrid[indexTargetNode].cubeLayers)
                {
                    case CubeLayers.cubeFull:
                        {

                            soloPileTarget = grid.kuboGrid[myIndex - 1];
                            isCheckingMove = false;
                        }
                        break;
                    case CubeLayers.cubeEmpty:
                        {


                            isReadyToMove = true;
                            soloPileTarget = grid.kuboGrid[myIndex - 1 + nodeDirection];


                            if (grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeLayers == CubeLayers.cubeMoveable && MatrixLimitCalcul(myIndex, _DirectionCustom.up))
                            {
                                Debug.Log("TEST1");
                                pileNodeCubeMove = grid.kuboGrid[myIndex - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>();
                                Debug.Log("TEST 2 = " + pileNodeCubeMove);
                                pileNodeCubeMove.CheckingPile(pileNodeCubeMove.myIndex - 1, nodeDirection);
                            }

                            isCheckingMove = false;
                        }
                        break;
                    case CubeLayers.cubeMoveable:
                        {

                            CheckPileMove(indexTargetNode, nodeDirection);
                        }
                        break;
                }

            }
            else
            {

                soloPileTarget = grid.kuboGrid[myIndex - 1];
                isCheckingMove = false;
            }
        }


        #endregion

        #region INPUT

        public void NextDirection()
        {

            if (!isStatic)
            {
                // Calcul the swip angle
                currentSwipePos = _DataManager.instance.inputPosition;

                distanceTouch = Vector3.Distance(baseSwipePos, currentSwipePos);

                angleDirection = Mathf.Abs(Mathf.Atan2(currentSwipePos.y - baseSwipePos.y, baseSwipePos.x - currentSwipePos.x) * 180 / Mathf.PI - 180);


                KUBNord = _InGameCamera.KUBNordScreenAngle;
                KUBWest = _InGameCamera.KUBWestScreenAngle;
                KUBSud = _InGameCamera.KUBSudScreenAngle;
                KUBEst = _InGameCamera.KUBEstScreenAngle;

                // Check in which direction the player swiped 

                if (angleDirection < KUBNord && angleDirection > KUBEst)
                {
                    enumSwipe = swipeDirection.Front;
                }
                else if (angleDirection < KUBWest && angleDirection > KUBNord)
                {
                    enumSwipe = swipeDirection.Left;
                }
                else if (angleDirection < KUBSud && angleDirection > KUBWest)
                {
                    enumSwipe = swipeDirection.Back;
                }
                else if (angleDirection < KUBEst && angleDirection > KUBSud)
                {
                    enumSwipe = swipeDirection.Right;
                }

                else
                {

                    if (angleDirection > 180)
                        inverseAngleDirection = angleDirection - 180;
                    else
                        inverseAngleDirection = angleDirection + 180;


                    if (inverseAngleDirection < KUBNord && inverseAngleDirection > KUBEst)
                    {
                        enumSwipe = swipeDirection.Back;
                    }
                    else if (inverseAngleDirection < KUBWest && inverseAngleDirection > KUBNord)
                    {
                        enumSwipe = swipeDirection.Right;
                    }
                    else if (inverseAngleDirection < KUBSud && inverseAngleDirection > KUBWest)
                    {
                        enumSwipe = swipeDirection.Front;
                    }
                    else if (inverseAngleDirection < KUBEst && inverseAngleDirection > KUBSud)
                    {
                        enumSwipe = swipeDirection.Left;
                    }
                }

                if (distanceTouch > _DataManager.instance.swipeMinimalDistance)
                    CheckDirection(enumSwipe);
            }
        }

        public void GetBasePoint()
        {
            //Reset Base Touch position
            baseSwipePos = _DataManager.instance.inputPosition;
        }

        public void CheckDirection(swipeDirection swipeDir)
        {
            // Check dans quel direction le joueur swipe
            switch (swipeDir)
            {
                case swipeDirection.Front:
                    CheckWhenToMove(_DirectionCustom.left);
                    break;
                case swipeDirection.Right:
                    CheckWhenToMove(_DirectionCustom.forward);
                    break;
                case swipeDirection.Left:
                    CheckWhenToMove(_DirectionCustom.backward);
                    break;
                case swipeDirection.Back:
                    CheckWhenToMove(_DirectionCustom.right);
                    break;
            }
        }

        void CheckWhenToMove(int direction)
        {


            if (MatrixLimitCalcul(myIndex, direction) == true)
            {
                baseCube = _InGameCamera.instance.NormalCam.WorldToScreenPoint(grid.kuboGrid[myIndex - 1].worldPosition);
                nextCube = _InGameCamera.instance.NormalCam.WorldToScreenPoint(grid.kuboGrid[myIndex - 1 + direction].worldPosition);

                distanceBaseToNext = Vector3.Distance(baseCube, nextCube);


                if (distanceTouch > (distanceBaseToNext * 0.5f) && isMoving == false)
                {
                    CheckingMove(myIndex, direction);
                    StartCoroutine(_DataManager.instance.CubesAreCheckingMove());
                }


            }
        }

        #endregion

        #region FEEDBACK

        public virtual void OutlineActive(int isActive)
        {
            meshRenderer.GetPropertyBlock(MatProp);

            if (isActive == 1)
                MatProp.SetFloat("_Outline", 0.1f);
            else if (isActive == 2)
                MatProp.SetFloat("_Outline", 0);

            meshRenderer.SetPropertyBlock(MatProp);
        }

        public virtual void AddOutline(bool Selected)
        {
            OutlineActive(1);
            if(Selected == true)
            {
                ChangeEmoteFace(_EmoteSelectedTex);
                GetChildRecursive(myIndex);
            }

        }

        public void GetChildRecursive(int index)
        {

            if (grid.kuboGrid[index - 1 + _DirectionCustom.up].cubeLayers == CubeLayers.cubeMoveable && MatrixLimitCalcul(index, _DirectionCustom.up))
            {
                grid.kuboGrid[index - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>().OutlineActive(1);
                grid.kuboGrid[index - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>().ChangeEmoteFace(grid.kuboGrid[index - 1 + _DirectionCustom.up].cubeOnPosition.GetComponent<_CubeMove>()._EmoteSelectedTex);
                GetChildRecursive(grid.kuboGrid[index + _DirectionCustom.up].nodeIndex - 1);
            }
        }


        public virtual void ResetOutline()
        {
            OutlineActive(2);

            if (isPastilleAndIsOn == false)
                if (gameObject.GetComponent<SwitchCube>() == true && gameObject.GetComponent<SwitchCube>().isActive == false)
                    ChangeEmoteFace(_EmoteIdleOffTex);
                else
                    ChangeEmoteFace(_EmoteIdleTex);
            else
                ChangeEmoteFace(_EmotePastilleTex);
        }

        #endregion

        #region AUDIO

        public void SetupCantMoveSound()
        {
            audioSourceCube.clip = _AudioManager.instance.CANTMOVE;
            audioSourceCube.Play();
        }

        #endregion

    }
}