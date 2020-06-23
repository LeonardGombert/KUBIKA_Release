using Kubika.CustomLevelEditor;
using Kubika.Saving;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Kubika.Game
{
    public class LevelsManager : MonoBehaviour
    {
        private static LevelsManager _instance;
        public static LevelsManager instance { get { return _instance; } }


        #region MAIN LEVELS
        [FoldoutGroup("Game Levels")] public List<LevelFile> gameMasterList = new List<LevelFile>();
        [FoldoutGroup("Game Levels")] [ShowInInspector] Queue<LevelFile> gameLevelQueue = new Queue<LevelFile>();

        [FoldoutGroup("User Levels")] public List<LevelFile> userMasterList = new List<LevelFile>();
        [FoldoutGroup("User Levels")] [ShowInInspector] Queue<LevelFile> userLevelQueue = new Queue<LevelFile>();

        List<List<TextAsset>> listOfLists = new List<List<TextAsset>>();

        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome1 = new List<TextAsset>();

        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome2 = new List<TextAsset>();
        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome3 = new List<TextAsset>();
        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome4 = new List<TextAsset>();
        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome5 = new List<TextAsset>();
        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome6 = new List<TextAsset>();
        [FoldoutGroup("Full Biomes")] [SerializeField] List<TextAsset> biome7 = new List<TextAsset>();
        #endregion

        #region LEVEL EDITOR
        [FoldoutGroup("Level Editor ")] public List<string> levelNames = new List<string>();
        [FoldoutGroup("Level Editor ")] public List<LevelFile> userLevelsList = new List<LevelFile>();
        public string userSceneToTest;
        #endregion

        //which level to load to
        public string loadToKubicode;

        public TextAsset testLevel;

        public TextAsset _levelFile;
        public string _levelName;
        public Biomes _levelBiome;
        public string _Kubicode;
        public int _minimumMoves;
        public bool _lockRotate;

        float timePassed;
        float startAlphaValue;
        float targetAlphaValue;
        [SerializeField] float transitionDuration;

        void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

            StartCoroutine(InitializeGameLevelsList());
        }

        // Start is called before the first frame update
        void Start()
        {
            if (ScenesManager.isLevelEditor) RefreshUserLevels();
        }

        IEnumerator InitializeGameLevelsList()
        {
            listOfLists.Add(biome1);
            listOfLists.Add(biome2);
            listOfLists.Add(biome3);
            listOfLists.Add(biome4);
            listOfLists.Add(biome5);
            listOfLists.Add(biome6);
            listOfLists.Add(biome7);

            InitializeLists();

            yield return null;
        }

        // Copy all of the individual lists to the master list
        private void InitializeLists()
        {
            gameMasterList.Clear();

            foreach (List<TextAsset> levelFileList in listOfLists)
            {
                foreach (TextAsset level in levelFileList)
                {
                    LevelFile levelInfo = UserLevelFiles.ConvertToLevelInfo(level);
                    gameMasterList.Add(levelInfo);
                }
            }
        }

        #region //LOAD TO KUBICODE
        // called when Game Scene is loaded, load the lloadTOKubicode level or set to baseState
        public void BakeLevels()
        {
            for (int i = 0; i < gameMasterList.Count; i++)
            {
                if (gameMasterList[i].kubicode != loadToKubicode) continue;

                else if (gameMasterList[i].kubicode == loadToKubicode)
                {
                    // load a specific level
                    LoadSpecific(i);
                    break;
                }
            }
        }

        //Clear and recreate the levels list starting at the level you want to load
        private void LoadSpecific(int startingIndex)
        {
            gameLevelQueue.Clear();

            for (int i = startingIndex; i < gameMasterList.Count; i++)
            {
                gameLevelQueue.Enqueue(gameMasterList[i]);
            }
            _LoadNextLevel();
        }
        #endregion

        // called when the user selects a level on the worlldmap
        public void SelectLevel(string kubicode)
        {
            LevelFile levelFile = GetMatching(loadToKubicode);

            UIManager.instance.UpdateWMInfo(levelFile);
            loadToKubicode = kubicode;
        }

        //used to find the matching LevelFile based on KubiCode
        LevelFile GetMatching(string kubiCode)
        {
            LevelFile returnfile = new LevelFile();
            for (int i = 0; i < gameMasterList.Count; i++)
            {
                if (gameMasterList[i].kubicode != kubiCode) continue;
                if (gameMasterList[i].kubicode == kubiCode) returnfile = gameMasterList[i];
            }

            return returnfile;
        }

        #region //USER LEVEL EDITOR
        public void RefreshUserLevels()
        {
            levelNames = UserLevelFiles.GetUserLevelNames();

            while (UIManager.instance == null) return;
            UIManager.instance.playerLevelsDropdown.ClearOptions();

            foreach (string levelName in levelNames)
            {
                //the info used to fill out the dropdown tabs are provided by the user level files
                UIManager.instance.playerLevelsDropdown.options.Add(new Dropdown.OptionData(levelName));
            }

            UIManager.instance.playerLevelsDropdown.RefreshShownValue();
        }

        public IEnumerator OpenTestLevel()
        {
            SaveAndLoad.instance.UserSavingCurrentLevel();

            ScenesManager.instance._LoadScene(ScenesIndex.CUSTOM_LEVELS);

            while (!ScenesManager.instance.finishedLoadingScene) yield return null;

            StartCoroutine(LoadUserLevel());

            yield return null;
        }

        public IEnumerator CloseTestLevel()
        {
            ScenesManager.instance._LoadScene(ScenesIndex.LEVEL_EDITOR);

            while (!ScenesManager.instance.finishedLoadingScene) yield return null;

            SaveAndLoad.instance.UserLoadLevel(SaveAndLoad.instance.currentOpenLevelName);

            yield return null;
        }
        #endregion

        #region //LOAD LEVEL PIPELINE
        //this kubicode is save into the player's progress file
        public string GetNextKubicode()
        {

            //get info
            GetNextLevelInfo();
            return _Kubicode;
        }

        public void _LoadNextLevel()
        {
            //get info
            GetNextLevelInfo();

            //load level
            StartCoroutine(LoadGameLevel());

            //remove from queue
            DequeueLevel();
        }

        // Get the next level's information and remove it from the queue when you load the next level
        void GetNextLevelInfo()
        {
            _levelName = gameLevelQueue.Peek().levelName;
            _levelBiome = gameLevelQueue.Peek().levelBiome;
            _Kubicode = gameLevelQueue.Peek().kubicode;
            _levelFile = gameLevelQueue.Peek().levelFile;
            _minimumMoves = gameLevelQueue.Peek().minimumMoves;
            _lockRotate = gameLevelQueue.Peek().lockRotate;
        }

        // Load the next level (extract the file)
        IEnumerator LoadGameLevel()
        {
            _KUBRotation.instance.ResetRotation();
            _FeedBackManager.instance.ResetVictoryFX();

            PlayerMoves.instance.ResetMoves();

            if (_lockRotate) UIManager.instance.UpdateRotateButtons(true);
            else UIManager.instance.TurnOnRotate();

            string json = _levelFile.ToString();

            LevelEditorData levelData = JsonUtility.FromJson<LevelEditorData>(json);

            SaveAndLoad.instance.finishedBuilding = false;

            SaveAndLoad.instance.ExtractAndRebuildLevel(levelData);

            while (!SaveAndLoad.instance.finishedBuilding) yield return null;

            // once the level is loaded 

            VictoryConditionManager.instance.CheckVictoryCubes();

            _DataManager.instance.GameSet();

            _MaterialCentral.instance.MaterialSet();
            _MaterialCentral.instance.ChangeUniverse(_levelBiome);

            UIManager.instance.GameCanvasPriority();

            yield return null;
        }

        IEnumerator LoadUserLevel()
        {
            _KUBRotation.instance.ResetRotation();
            _FeedBackManager.instance.ResetVictoryFX();

            if (_lockRotate) UIManager.instance.UpdateRotateButtons(true);
            else UIManager.instance.TurnOnRotate();

            SaveAndLoad.instance.UserLoadLevel(SaveAndLoad.instance.currentOpenLevelName);

            // once the level is loaded 

            VictoryConditionManager.instance.CheckVictoryCubes();
            _DataManager.instance.GameSet();
            _MaterialCentral.instance.MaterialSet();
            _MaterialCentral.instance.ChangeUniverse(_levelBiome);

            UIManager.instance.CustomLevelCanvasPriority();
            yield return null;
        }

        private void DequeueLevel()
        {
            gameLevelQueue.Dequeue();
        }
        #endregion

        public void RestartLevel()
        {
            StartCoroutine(LoadGameLevel());
        }

        public IEnumerator MoveToNextLevel()
        {
            ScenesManager.instance._LoadScene(ScenesIndex.TITLE_WORLD_MAP);

            while (!ScenesManager.instance.finishedLoadingScene) yield return null;

            StartCoroutine(_Planete.instance.StartOnFace((int)_levelBiome - 1));

            yield return null;
        }

        #region //FADE TRANSITION
        public void _FadeToBlack()
        {
            //base values for fade out
            timePassed = 0;
            startAlphaValue = 0;
            targetAlphaValue = 1;

            UIManager.instance.TransitionStart();

            StartCoroutine(UIManager.instance.FadeTransition(startAlphaValue,
                            targetAlphaValue, transitionDuration, timePassed));

            UIManager.instance.TransitionOver();
        }

        public void _FadeFromBlack()
        {
            //base values for fade out
            timePassed = 0;
            startAlphaValue = 1;
            targetAlphaValue = 0;

            UIManager.instance.TransitionStart();

            StartCoroutine(UIManager.instance.FadeTransition(startAlphaValue,
                            targetAlphaValue, transitionDuration, timePassed));

            UIManager.instance.TransitionOver();
        }
        #endregion
    }
}

namespace Kubika.Saving
{
    [Serializable]
    public struct LevelFile
    {
        public string levelName;
        public string kubicode;
        public Biomes levelBiome;
        public int minimumMoves;
        public bool lockRotate;
        [HideInInspector] public bool levelIsBeaten; //not saved in the file, but in player progress
        public TextAsset levelFile;
    }
}