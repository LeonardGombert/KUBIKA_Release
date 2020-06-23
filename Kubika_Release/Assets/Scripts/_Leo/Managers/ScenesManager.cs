using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Kubika.CustomLevelEditor;
using Sirenix.OdinInspector;

namespace Kubika.Game
{
    public class ScenesManager : MonoBehaviour
    {
        private static ScenesManager _instance;
        public static ScenesManager instance { get { return _instance; } }

        public ScenesIndex loadToScene;
        public ScenesIndex currentActiveScene;
        AsyncOperation loadingSceneOp;

        [ShowInInspector] public static bool isLevelEditor = true;
        [ShowInInspector] public static bool isDevScene = false;

        public bool finishedLoadingScene = false;

        void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

            if (SceneManager.GetActiveScene().buildIndex == (int)ScenesIndex.LEVEL_EDITOR) isLevelEditor = true;
            if (SceneManager.GetActiveScene().name.Contains("DevScene")) isDevScene = true;

            SceneManager.LoadSceneAsync((int)ScenesIndex.USER_INTERFACE, LoadSceneMode.Additive);
            loadingSceneOp = SceneManager.LoadSceneAsync((int)loadToScene, LoadSceneMode.Additive);
            currentActiveScene = loadToScene;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void _LoadScene(ScenesIndex targetScene)
        {
            finishedLoadingScene = false;
            StartCoroutine(LoadScene(targetScene));
        }

        IEnumerator LoadScene(ScenesIndex targetScene)
        {
            LevelsManager.instance._FadeToBlack();

            while (!UIManager.instance.transitionFinished) yield return null;

            SceneManager.UnloadSceneAsync((int)currentActiveScene);
            
            loadingSceneOp = SceneManager.LoadSceneAsync((int)targetScene, LoadSceneMode.Additive);

            while (!loadingSceneOp.isDone) yield return null;

            currentActiveScene = targetScene;

            finishedLoadingScene = true;

            while (!UIManager.instance.transitionFinished) yield return null;

            LevelsManager.instance._FadeFromBlack();

            UIManager.instance.RefreshActiveScene();

            yield return null;
        }
    }
}