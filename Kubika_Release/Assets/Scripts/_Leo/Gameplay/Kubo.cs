using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class Kubo : MonoBehaviour
    {
        private static Kubo _instance;
        public static Kubo instance { get { return _instance; } }

        public const int gridSize = 12;
        public const int gridLength = gridSize * gridSize * gridSize;

        public KuboNode[] kuboGrid = new KuboNode[gridLength];
        
        private Vector3Int gridSizeVector = new Vector3Int(gridSize, gridSize, gridSize);
        public float offset;
        private List<GameObject> placedCubes = new List<GameObject>();

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            CreateGrid();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CreateGrid()
        {
            //centerPosition = gridSize * gridSize * gridMargin + gridSize * gridMargin + gridMargin;

            //kuboGrid = new KuboNode[gridSize * gridSize * gridSize];

            for (int index = 1, z = 0; z < gridSizeVector.z; z++)
            {
                for (int x = 0; x < gridSizeVector.x; x++)
                {
                    for (int y = 0; y < gridSizeVector.y; y++, index++)
                    {
                        Vector3 nodePosition = new Vector3(x * offset, y * offset, z * offset);

                        KuboNode currentNode = new KuboNode();

                        currentNode.xCoord = x;
                        currentNode.yCoord = y;
                        currentNode.zCoord = z;

                        currentNode.nodeIndex = index;
                        currentNode.worldPosition = nodePosition;
                        currentNode.cubeLayers = CubeLayers.cubeEmpty;
                        currentNode.cubeType = CubeTypes.None;
                        currentNode.facingDirection = FacingDirection.forward;

                        kuboGrid[index - 1] = currentNode;
                    }
                }
            }

           /* if (ScenesManager.isDevScene || ScenesManager.isLevelEditor && LevelEditor.instance != null)
                LevelEditor.instance.GenerateBaseGrid();*/
        }

        public void RefreshGrid()
        {
            ResetIndexGrid();
            CreateGrid();
        }

        //set all index to their default state
        public void ResetIndexGrid()
        {
            for (int i = 0; i < kuboGrid.Length; i++)
            {
                kuboGrid[i].cubeLayers = CubeLayers.cubeEmpty;
                kuboGrid[i].cubeType = CubeTypes.None;

                if (kuboGrid[i].cubeOnPosition != null)
                {
                    Destroy(kuboGrid[i].cubeOnPosition.gameObject);
                    kuboGrid[i].cubeOnPosition = null;
                }

                placedCubes.Clear();
            }

            foreach (Transform child in transform) Destroy(child.gameObject);
        }
    }
}