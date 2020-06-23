using Kubika.CustomLevelEditor;
using Kubika.Saving;
using System.Collections;
using UnityEngine;

namespace Kubika.Game
{
    public class VictoryConditionManager : MonoBehaviour
    {
        private static VictoryConditionManager _instance;
        public static VictoryConditionManager instance { get { return _instance; } }

        public int currentVictoryPoints;
        public int levelVictoryPoints;

        [Header("WAIT TIME")]
        public float waitTime = 1.5f;
        public float waitTimeBeforePS = 0.0f;

        BaseVictoryCube[] victoryCubes;
        DeliveryCube[] deliveryCube;
        

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;
        }

        private void Start()
        {
            //_DataManager.instance.EndFalling.AddListener(VictoryConditionStatus);
        }

        // Call when a new level is loaded
        public void CheckVictoryCubes()
        {
            currentVictoryPoints = 0;
            levelVictoryPoints = 0;

            for (int i = 0; i < _Grid.instance.kuboGrid.Length; i++)
            {
                if (_Grid.instance.kuboGrid[i].cubeType >= CubeTypes.BaseVictoryCube &&
                    _Grid.instance.kuboGrid[i].cubeType <= CubeTypes.SwitchVictoryCube)

                {

                    levelVictoryPoints++;
                }
            }
        }

        public void DecrementVictory()
        {

            currentVictoryPoints--;

            StartCoroutine(VictoryConditionStatus());
        }

        public void IncrementVictory()
        {

            currentVictoryPoints++;

            StartCoroutine(VictoryConditionStatus());
        }

        IEnumerator VictoryConditionStatus()
        {
            yield return new WaitForSeconds(.5f);

            if (currentVictoryPoints == levelVictoryPoints)
            {

                StartCoroutine(LevelPassedRoutine());
            }

            yield return null;
        }

        public IEnumerator LevelPassedRoutine()
        {
            PlayerMoves.instance.CheckIfGolden();

            deliveryCube = FindObjectsOfType<DeliveryCube>();
            
            _FeedBackManager.instance.EveryCubeHappy();

            foreach (DeliveryCube cube in deliveryCube)
            {
                StartCoroutine(cube.VictoryPSLatence());
            }

            yield return new WaitForSeconds(waitTimeBeforePS);

            _FeedBackManager.instance.PlayVictoryFX();

            yield return new WaitForSeconds(waitTime);

            UIManager.instance.OpenWinLevelWindow();

            //save the level's progress
            SaveAndLoad.instance.SaveAndLoadPlayerProgress(LevelsManager.instance._Kubicode, 
                                              LevelsManager.instance.GetNextKubicode(), 
                                              PlayerMoves.instance.CheckIfGolden());
        }
    }
}