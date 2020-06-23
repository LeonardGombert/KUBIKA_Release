using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kubika.Game
{
    public class CubePopulatorManager : MonoBehaviour
    {
        private static CubePopulatorManager _instance;
        public static CubePopulatorManager instance { get { return _instance; } }

        public GameObject cubePrefab, universePrefab;

        public GridLayoutGroup staticGridGroup, functionGridGroup, universeGridGroup;
        [Header("DO NOT MOVE ORDER")] //order is based on the order the cubes are declared in
        public Sprite[] selectedStaticCubesArray;
        [FoldoutGroup("Static Cubes")] public Sprite[] staticCubes1, staticCubes2, staticCubes3, staticCubes4, staticCubes5, staticCubes6;
        public Sprite[] functionCubes, universePack;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            StaticCubePopulator();
            FunctionCubePopulator();
            UniversePackPopulator();
        }

        private void FunctionCubePopulator()
        {
            functionGridGroup.constraintCount = (int)CubeTypes.Count - (int)CubeTypes.MoveableCube;

            for (int i = (int)CubeTypes.MoveableCube, j = 0; i < (int)CubeTypes.Count; i++, j++)
            {
                GameObject newObj = Instantiate(cubePrefab, functionGridGroup.gameObject.transform);
                newObj.GetComponent<Image>().sprite = functionCubes[j];
                newObj.GetComponent<Image>().SetNativeSize();
                newObj.GetComponent<CubeSelector>().selectedCubeType = (CubeTypes)i;
            }
        }

        //called by cube selector
        public void RefreshDecoratorCubes()
        {
            StaticCubePopulator();
        }

        private void StaticCubePopulator()
        {
            if (_MaterialCentral.instance.staticIndex == Biomes.Plains) selectedStaticCubesArray = staticCubes1;
            if (_MaterialCentral.instance.staticIndex == Biomes.Mountains) selectedStaticCubesArray = staticCubes2;
            if (_MaterialCentral.instance.staticIndex == Biomes.Underwater) selectedStaticCubesArray = staticCubes3;
            if (_MaterialCentral.instance.staticIndex == Biomes.Ruins) selectedStaticCubesArray = staticCubes4;
            if (_MaterialCentral.instance.staticIndex == Biomes.Statues) selectedStaticCubesArray = staticCubes5;
            if (_MaterialCentral.instance.staticIndex == Biomes.Temple) selectedStaticCubesArray = staticCubes6;

            staticGridGroup.constraintCount = (int)CubeTypes.QuadStaticCube; // 6 

            foreach (Transform child in staticGridGroup.gameObject.transform) Destroy(child.gameObject);

            for (int i = (int)CubeTypes.FullStaticCube; i <= selectedStaticCubesArray.Length; i++)
            {
                GameObject newObj = Instantiate(cubePrefab, staticGridGroup.gameObject.transform);
                newObj.GetComponent<Image>().sprite = selectedStaticCubesArray[i - 1]; //-1 because i starts at 1
                newObj.GetComponent<Image>().SetNativeSize();
                newObj.GetComponent<CubeSelector>().selectedCubeType = (CubeTypes)i;
            }
        }


        public void UniversePackPopulator()
        {
            universeGridGroup.constraintCount = (int)Biomes.Count;

            for (int i = 0; i < universePack.Length; i++)
            {
                GameObject newObj = Instantiate(universePrefab, universeGridGroup.gameObject.transform);
                newObj.GetComponent<Image>().sprite = universePack[i];
                newObj.GetComponent<Image>().SetNativeSize();
                newObj.GetComponent<CubeSelector>().biomes = (Biomes)i;
            }
        }
    }
}
