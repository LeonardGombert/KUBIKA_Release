using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kubika.Game;

namespace Kubika.Test
{
    public class _Matrix : MonoBehaviour
    {
        public Transform[] nodeMatrix;
        public int matrixLength;

        // CUBE MOVABLE
        public _CubeTestSynchronized Cube1;
        public int indexC1;



        // MOVE LERP
        Vector3 currentPos;
        Vector3 basePos;
        float currentTime;
        public float moveTime;
        float time;

        // Start is called before the first frame update
        void Start()
        {
            Cube1.transform.position = nodeMatrix[indexC1 - 1].position;

            _DirectionCustom.matrixLengthDirection = 4;

        }

        // Update is called once per frame
        void Update()
        { 
            // X Axis
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (((indexC1 - matrixLength) + (matrixLength * matrixLength) - 1) / ((matrixLength * matrixLength) * (indexC1 / (matrixLength * matrixLength)) + (matrixLength * matrixLength)) != 0)
                {
                    StartCoroutine(Cube1.Move(nodeMatrix[indexC1 + _DirectionCustom.left - 1].position));
                    indexC1 = indexC1 - matrixLength;
                }

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // -X Axis
                if ((indexC1 + matrixLength) / ((matrixLength * matrixLength) * (indexC1 / (matrixLength * matrixLength) + 1)) != 1)
                {
                    StartCoroutine(Cube1.Move(nodeMatrix[indexC1 + _DirectionCustom.right - 1].position));
                    indexC1 = indexC1 + matrixLength;
                }

            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                // -Z Axis
                if (indexC1 -(matrixLength * matrixLength) >= 0)
                {
                    StartCoroutine(Cube1.Move( nodeMatrix[indexC1 + _DirectionCustom.backward - 1].position));
                    indexC1 = indexC1 - (matrixLength * matrixLength);
                }

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Z Axis
                if ((indexC1 + (matrixLength * matrixLength)) / ((matrixLength * matrixLength * matrixLength)) != 1)
                {
                    StartCoroutine(Cube1.Move(nodeMatrix[indexC1 + _DirectionCustom.forward - 1].position));
                    indexC1 = indexC1 + (matrixLength * matrixLength);
                }

            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // Y Axis
                if (indexC1 % matrixLength != 0)
                {
                    StartCoroutine(Cube1.Move(nodeMatrix[indexC1 + _DirectionCustom.up - 1].position));
                    indexC1 = indexC1 + 1;
                }

            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                // -Y Axis
                if ((indexC1 - 1) % matrixLength != 0)
                {
                    StartCoroutine(Cube1.Move( nodeMatrix[indexC1 + _DirectionCustom.down - 1].position));
                    indexC1 = indexC1 - 1;
                }

            }
        }

    }

}
