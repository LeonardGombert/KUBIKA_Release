using UnityEngine;

namespace Kubika.Game
{
    public class _CubeScanner : _CubeBase
    {
        public int[] indexesToCheck = new int[6];
        public _CubeBase victoryCubeOnPistion;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        protected void ScannerSet()
        {
            //facingDirection = CubeFacingDirection.CubeFacingFromRotation(transform.localEulerAngles);
        }

        public void SetScanDirections()
        {
            indexesToCheck[0] = _DirectionCustom.up; //+ 1
            indexesToCheck[1] = _DirectionCustom.down; //- 1
            indexesToCheck[2] = _DirectionCustom.right; //+ the grid size
            indexesToCheck[3] = _DirectionCustom.left; //- the grid size
            indexesToCheck[4] = _DirectionCustom.forward; //+ the grid size squared
            indexesToCheck[5] = _DirectionCustom.backward;//- the grid size squared
        }

        public bool VictoryChecker(int targetIndex)
        {
            if (myIndex - 1 + targetIndex < grid.kuboGrid.Length && myIndex - 1 + targetIndex >= 0)
            {
                if (grid.kuboGrid[myIndex - 1 + targetIndex] != null)
                {
                    if (grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition != null)
                    {
                        if (grid.kuboGrid[myIndex - 1 + targetIndex].cubeType >= CubeTypes.BaseVictoryCube
                            && grid.kuboGrid[myIndex - 1 + targetIndex].cubeType <= CubeTypes.SwitchVictoryCube)
                        {
                            victoryCubeOnPistion = grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition.GetComponent<_CubeBase>();
                            return true;
                        }
                        else return false;
                    }
                    else return false;

                }
                else return false;
            }
            else return false;
        }

        public bool AnyMoveableChecker(int targetIndex)
        {
            if (myIndex - 1 + targetIndex < grid.kuboGrid.Length && myIndex - 1 + targetIndex >= 0)
            {
                if (grid.kuboGrid[myIndex - 1 + targetIndex] != null)
                {
                    if (grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition != null)
                    {
                        if (grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition.gameObject.GetComponent<_CubeMove>() != null) 
                            return true;
                        else return false;
                    }
                    else return false;

                }
                else return false;
            }
            else return false;
        }

        public GameObject TimerChecker(int targetIndex)
        {
            if (myIndex - 1 + targetIndex < grid.kuboGrid.Length && myIndex - 1 + targetIndex >= 0)
            {
                if (grid.kuboGrid[myIndex - 1 + targetIndex] != null)
                {
                    if (grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition != null)
                    {
                        if (grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition.gameObject.GetComponent<_CubeMove>() != null)
                            return grid.kuboGrid[myIndex - 1 + targetIndex].cubeOnPosition.gameObject;
                        else return null;
                    }
                    else return null;

                }
                else return null;
            }
            else return null;
        }

        protected Vector3 outsideCoord(int myIndexParam, int knowedDirection)
        {
            int newXCoord = grid.kuboGrid[myIndexParam - 1].xCoord - grid.kuboGrid[myIndexParam - 1 + knowedDirection].xCoord;
            int newYCoord = grid.kuboGrid[myIndexParam - 1].yCoord - grid.kuboGrid[myIndexParam - 1 + knowedDirection].yCoord;
            int newZCoord = grid.kuboGrid[myIndexParam - 1].zCoord - grid.kuboGrid[myIndexParam - 1 + knowedDirection].zCoord;

            newXCoord += grid.kuboGrid[myIndexParam - 1].xCoord;
            newYCoord += grid.kuboGrid[myIndexParam - 1].yCoord;
            newZCoord += grid.kuboGrid[myIndexParam - 1].zCoord;

            return new Vector3(newXCoord, newYCoord, newZCoord);
        }

    }
}