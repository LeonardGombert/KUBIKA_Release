using Kubika.CustomLevelEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kubika.Game
{
    [RequireComponent(typeof(Button))]
    public class CubeSelector : MonoBehaviour
    {
        //set by CubePopulator
        public CubeTypes selectedCubeType;
        public Biomes biomes;

        // called on button press
        public void ChangeCubeSelection()
        {
            UIManager.instance.ButtonCallback("LEVELEDITOR_isPlacing");
            LevelEditor.instance.currentCube = selectedCubeType;
        }

        public void ChangeUniverseSelection()
        {
            //UIManager.instance.ButtonCallback("LEVELEDITOR_isPlacing");

            _MaterialCentral.instance.ChangeUniverse(biomes);

            CubePopulatorManager.instance.RefreshDecoratorCubes();
        }
    }
}