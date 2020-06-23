using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Kubika.CustomLevelEditor;
using Kubika.Saving;

namespace Kubika.Game
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager instance { get { return _instance; } }

        #region VARIABLE DECLARATIONS
        #region SCENE CANVASES
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas worldMapCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas transitionCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas hamburgerMenuCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas hamburgerMenuCanvas2;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas gameCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas levelPassedCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas levelEditorCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas customTestCanvas;
        [FoldoutGroup("Scene Canvases")] [SerializeField] Canvas winCanvas;
        #endregion

        #region WORLDMAP
        [FoldoutGroup("World Map")] [SerializeField] Text levelNameWM;
        [FoldoutGroup("World Map")] [SerializeField] GameObject mainMenuButton;
        [FoldoutGroup("World Map")] [SerializeField] GameObject playButton;
        [FoldoutGroup("World Map")] public Image topArrow; //used by WorldMap Rotation script
        [FoldoutGroup("World Map")] public Image bottomArrow; //used by WorldMap Rotation script
        #endregion         

        #region BURGER MENU
        [FoldoutGroup("Burger Menu")] [SerializeField] Button music;
        [FoldoutGroup("Burger Menu")] [SerializeField] Sprite musicOn;
        [FoldoutGroup("Burger Menu")] [SerializeField] Sprite musicOff;

        [FoldoutGroup("Burger Menu")] [SerializeField] Button sound;
        [FoldoutGroup("Burger Menu")] [SerializeField] Sprite soundOn;
        [FoldoutGroup("Burger Menu")] [SerializeField] Sprite soundOff;

        [FoldoutGroup("Burger Menu")] [SerializeField] GameObject hiddenMenuButtons;
        [FoldoutGroup("Burger Menu")] [SerializeField] GameObject openBurgerMenuButton;
        private bool soundIsOn = true, musicIsOn = true;
        #endregion

        #region IN GAME
        [FoldoutGroup("In-Game")] [SerializeField] Text inGameLevelName;
        [FoldoutGroup("In-Game")] [SerializeField] Image rightRotate, leftRotate;
        [FoldoutGroup("In-Game")] [SerializeField] Image rightRotateBackground, leftRotateBackground;
        [FoldoutGroup("In-Game")] [SerializeField] Button rightRotateButton, leftRotateButton;
        [FoldoutGroup("In-Game")] [SerializeField] Sprite rightRotateOn, leftRotateOn;
        [FoldoutGroup("In-Game")] [SerializeField] Sprite rotateLock, rotateOnBackground, rotateLockBackground;
        [FoldoutGroup("In-Game")] [SerializeField] Text currentMoves, minimumMoves;
        #endregion

        #region LEVEL PASSED
        [FoldoutGroup("Level Passed")] [SerializeField] Text levelName;
        [FoldoutGroup("Level Passed")] [SerializeField] Text playerScore;
        [FoldoutGroup("Level Passed")] [SerializeField] Text levelScore;
        #endregion

        #region LEVEL EDITOR
        [FoldoutGroup("Level Editor")] [SerializeField] GameObject levelEditorOptionsWindow;
        [FoldoutGroup("Level Editor")] [SerializeField] GameObject levelEditorSaveWindow;

        [FoldoutGroup("Level Editor")] [SerializeField] GameObject UniverseMode;
        [FoldoutGroup("Level Editor")] [SerializeField] GameObject FunctionMode;
        [FoldoutGroup("Level Editor")] [SerializeField] GameObject DecoratorMode;
        [FoldoutGroup("Level Editor")] [SerializeField] GameObject EmoteMode;
        [FoldoutGroup("Level Editor")] public Dropdown playerLevelsDropdown;
        [FoldoutGroup("Level Editor")] public InputField saveLevelName;

        [FoldoutGroup("Level Editor")] public Image deleteLogo, rotateLogo;

        [FoldoutGroup("Level Editor")] [SerializeField] Sprite BackgroundSelected, BackgroundUnselected;
        [FoldoutGroup("Level Editor")] [SerializeField] Text playerMovesInEditor;

        [FoldoutGroup("Level Editor/Universe Panel")] [SerializeField] Image UniverseBackgroundImage, UniverseIconImage;
        [FoldoutGroup("Level Editor/Universe Panel")] [SerializeField] Sprite UniverseIconSelected, UniverseIconUnselected;

        [FoldoutGroup("Level Editor/Function Panel")] [SerializeField] Image FunctionBackgroundImage, FunctionIconImage;
        [FoldoutGroup("Level Editor/Function Panel")] [SerializeField] Sprite FunctionIconSelected, FunctionIconUnselected;

        [FoldoutGroup("Level Editor/Decorator Panel")] [SerializeField] Image DecoratorBackgroundImage, DecoratorIconImage;
        [FoldoutGroup("Level Editor/Decorator Panel")] [SerializeField] Sprite DecoratorIconSelected, DecoratorIconUnselected;

        [FoldoutGroup("Level Editor/Emote Panel")] [SerializeField] Image EmoteBackgroundImage, EmoteIconImage;
        [FoldoutGroup("Level Editor/Emote Panel")] [SerializeField] Sprite EmoteIconSelected, EmoteIconUnselected;
        #endregion

        #region TRANSITION
        [FoldoutGroup("Fade Transition")] [SerializeField] public Image fadeImage;
        [FoldoutGroup("Fade Transition")] [SerializeField] public TransitionType transitionType;
        [FoldoutGroup("Fade Transition")] [SerializeField] float transitionDuration;
        [FoldoutGroup("Fade Transition")] [SerializeField] float timePassed;
        float startAlphaValue;
        float targetAlphaValue;
        bool gameDimmed = false;
        public bool transitionFinished = false;
        #endregion
        #endregion

        void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

            RefreshActiveScene();
        }

        private void Update()
        {
            //MOVE THIS SOMWHERE ELSE
            //
        }

        #region REFRESH ACTIVE CANVASES
        public void RefreshActiveScene()
        {
            TurnOffAllCanvases();

            switch (ScenesManager.instance.currentActiveScene)
            {
                /*case ScenesIndex.MANAGER:
                    break;

                case ScenesIndex.USER_INTERFACE:
                    break;*/

                case ScenesIndex.TITLE_WORLD_MAP:
                    WorldMapPriority();
                    break;

                case ScenesIndex.GAME_SCENE:
                    GameCanvasPriority();
                    LevelsManager.instance.BakeLevels();
                    _DataManager.instance.EndFalling.RemoveListener(UpdateText);
                    _DataManager.instance.EndFalling.AddListener(UpdateText);
                    break;

                case ScenesIndex.WIN:
                    WinScreenSettings();
                    break;

                case ScenesIndex.LEVEL_EDITOR:
                    LevelEditorPriority();
                    UniverseModePriority();
                    ResetCurrentValues();
                    break;

                case ScenesIndex.CUSTOM_LEVELS:
                    LevelsManager.instance.RefreshUserLevels();
                    CustomLevelCanvasPriority();
                    break;

                case ScenesIndex.CREDITS:
                    break;

                default:
                    break;
            }
        }

        private void ResetCurrentValues()
        {
            SaveAndLoad.instance.currentOpenLevelName = "";
            SaveAndLoad.instance.currentKubicode = "";
            SaveAndLoad.instance.currentLevelLockRotate = false;
        }

        void ResetCanvasSortOrder()
        {
            worldMapCanvas.sortingOrder = 0;
            //transitionCanvas.sortingOrder = 0;
            gameCanvas.sortingOrder = 0;
            winCanvas.sortingOrder = 0;
            levelEditorCanvas.sortingOrder = 0;
            levelPassedCanvas.sortingOrder = 0;
            customTestCanvas.sortingOrder = 0;
        }

        void TurnOffAllCanvases()
        {
            worldMapCanvas.enabled = false;
            //transitionCanvas.enabled = false;
            gameCanvas.enabled = false;
            winCanvas.enabled = false;
            levelEditorCanvas.enabled = false;
            hamburgerMenuCanvas.enabled = false;
            hamburgerMenuCanvas2.enabled = false;
            levelPassedCanvas.enabled = false;
            customTestCanvas.enabled = false;

            hiddenMenuButtons.SetActive(false);
            openBurgerMenuButton.SetActive(false);
            levelEditorOptionsWindow.SetActive(false);
            levelEditorSaveWindow.SetActive(false);
        }

        public void WorldMapPriority()
        {
            ResetCanvasSortOrder();
            if (worldMapCanvas != null) worldMapCanvas.enabled = true;
            worldMapCanvas.sortingOrder = 1000;

            playButton.SetActive(false);
            levelNameWM.enabled = false;

            mainMenuButton.gameObject.SetActive(false);
            topArrow.gameObject.SetActive(false);
            bottomArrow.gameObject.SetActive(false);
        }

        // called when camera is zoomed in
        public IEnumerator ZoomedWorldMapPriority()
        {
            ResetCanvasSortOrder();

            if (worldMapCanvas != null) worldMapCanvas.enabled = true;
            worldMapCanvas.sortingOrder = 1000;

            yield return new WaitForSeconds(1.5f);

            playButton.SetActive(true);
            levelNameWM.enabled = true;

            mainMenuButton.gameObject.SetActive(true);
            topArrow.gameObject.SetActive(true);
            bottomArrow.gameObject.SetActive(true);

            yield return null;
        }

        private void LevelEditorPriority()
        {
            ResetCanvasSortOrder();
            levelEditorCanvas.enabled = true;

            levelEditorOptionsWindow.SetActive(false);
            levelEditorSaveWindow.SetActive(false);

            levelEditorCanvas.sortingOrder = 1000;
        }

        //also caled from levels manager
        public void GameCanvasPriority()
        {
            ResetCanvasSortOrder();
            gameCanvas.enabled = true;

            levelPassedCanvas.sortingOrder = 1010;

            //reset burger enu params
            openBurgerMenuButton.SetActive(true);
            gameDimmed = false;

            if (hamburgerMenuCanvas != null) hamburgerMenuCanvas.enabled = true;
            if (hamburgerMenuCanvas2 != null) hamburgerMenuCanvas2.enabled = true;
            if (levelPassedCanvas != null) levelPassedCanvas.enabled = false;

            inGameLevelName.text = LevelsManager.instance._levelName;

            hiddenMenuButtons.SetActive(false);
            gameCanvas.sortingOrder = 1000;


            //Checking if the current level has ROtation enabled
            /*if (!_LoaderQueuer.instance._hasRotate) foreach (Button item in RotateButtons) item.gameObject.SetActive(false);
            else if (_LoaderQueuer.instance._hasRotate) foreach (Button item in RotateButtons) item.gameObject.SetActive(true);*/
        }

        public void CustomLevelCanvasPriority()
        {
            ResetCanvasSortOrder();

            gameCanvas.enabled = false;

            if (levelPassedCanvas != null) levelPassedCanvas.enabled = false;
            levelPassedCanvas.sortingOrder = 1000;

            customTestCanvas.enabled = true;
            customTestCanvas.sortingOrder = 1000;
        }

        private void WinScreenSettings()
        {
            ResetCanvasSortOrder();
            winCanvas.enabled = true;
            winCanvas.sortingOrder = 1000;
        }
        #endregion

        #region CHECK PLAYER INPUTS
        public void ButtonCallback(string button)
        {
            switch (button)
            {
                #region //GAME INPUTS
                case "GAME_RotateRight":
                    PlayerMoves.instance.IncrementMoves();
                    _KUBRotation.instance.RightTurn();
                    break;

                case "GAME_RotateLeft":
                    PlayerMoves.instance.IncrementMoves();
                    _KUBRotation.instance.LeftTurn();
                    break;

                case "GAME_Restart":
                    RefreshActiveScene(); 
                    LevelsManager.instance.RestartLevel();
                    break;

                case "GAME_Undo":
                    break;

                case "GAME_Skip":
                    StartCoroutine(DimGame());
                    LevelsManager.instance._LoadNextLevel();
                    LevelsManager.instance.loadToKubicode = LevelsManager.instance._Kubicode;
                    break;

                case "GAME_BurgerMenu":
                    StartCoroutine(DimGame());
                    break;
                #endregion

                #region //WORLDMAP INPUTS
                case "WORLDMAP_Play":
                    ScenesManager.instance._LoadScene(ScenesIndex.GAME_SCENE);
                    break;

                case "WORLDMAP_ZoomOut":
                    StartCoroutine(_Planete.instance.MainPlaneteView());
                    break;

                case "WORLDMAP_LevelEditor":
                    ScenesManager.instance._LoadScene(ScenesIndex.LEVEL_EDITOR);
                    break;

                case "WORLDMAP_LevelEditorLevelsList":
                    //IMPLEMENT CODE
                    break;

                case "WORLDMAP_UpArrow":
                    WorldmapManager.instance.currentBiome++;
                    WorldmapManager.instance.RefreshWorldArrowTargets();
                    _Planete.instance.AfterFace();
                    break;

                case "WORLDMAP_DownArrow":
                    WorldmapManager.instance.currentBiome--;
                    WorldmapManager.instance.RefreshWorldArrowTargets();
                    _Planete.instance.BeforeFace();
                    break;
                #endregion

                #region //BURGER MENU
                case "BURGER_Sound":
                    soundIsOn = !soundIsOn;
                    SwitchButtonSprite();
                    break;

                case "BURGER_Music":
                    musicIsOn = !musicIsOn;
                    SwitchButtonSprite();
                    break;

                case "BURGER_Close":
                    StartCoroutine(DimGame());
                    break;
                #endregion

                #region //LEVEL EDITOR
                case "LEVELEDITOR_isPlacing":
                    LevelEditor.instance.SwitchAction("isPlacing");
                    break;

                case "LEVELEDITOR_isDeleting":
                    LevelEditor.instance.SwitchAction("isDeleting");
                    break;

                case "LEVELEDITOR_isRotating":
                    LevelEditor.instance.SwitchAction("isRotating");
                    break;

                case "LEVELEDITOR_TestLevel":
                    LevelEditor.instance.SwitchAction(""); //turns off isRotating/Deleting/Placing bools
                    TestUserLevel();
                    break;

                case "LEVELEDITOR_SaveLevel":
                    UserSavedLevel();
                    break;

                case "LEVELEDITOR_SaveCurrentLevel":
                    UserSavedCurrentLevel();
                    break;

                case "LEVELEDITOR_NewLevel":
                    UserCreateNewLevel();
                    break;

                case "LEVELEDITOR_LoadLevel":
                    UserLoadLevel();
                    break;

                case "LEVELEDITOR_DeleteLevel":
                    UserDeleteLevel();
                    break;

                case "LEVELEDITOR_LeaveTestLevel":
                    ReturnToLevelEditor();
                    break;

                case "LEVELEDITOR_OptionsWindow":
                    OpenOptionsWindow();
                    break;

                case "LEVELEDITOR_SaveWindow":
                    OpenSaveWindow();
                    break;

                case "LEVELEDITOR_EmoteMode":
                    EmoteModePriority();
                    break;

                case "LEVELEDITOR_UniverseMode":
                    UniverseModePriority();
                    break;

                case "LEVELEDITOR_FunctionMode":
                    FunctionModePriority();
                    break;

                case "LEVELEDITOR_DecoratorMode":
                    DecoratorModePriority();
                    break;
                #endregion

                #region //GENERAL
                case "MAIN_MENU":
                    ScenesManager.instance._LoadScene(ScenesIndex.TITLE_WORLD_MAP);
                    break;

                case "TITLE_WORLDMAP":
                    ScenesManager.instance._LoadScene(ScenesIndex.TITLE_WORLD_MAP);
                    break;
                #endregion

                default:
                    break;
            }
        }
        #endregion

        #region GAME
        public void UpdateText()
        {
            currentMoves.text = PlayerMoves.instance.numberOfMoves.ToString();
            minimumMoves.text = LevelsManager.instance._minimumMoves.ToString();

            playerMovesInEditor.text = PlayerMoves.instance.numberOfMoves.ToString();
        }

        //called to turn sound on/off
        void SwitchButtonSprite()
        {
            if (musicIsOn == true) music.image.sprite = musicOn;
            if (musicIsOn == false) music.image.sprite = musicOff;

            if (soundIsOn == true) sound.image.sprite = soundOn;
            if (soundIsOn == false) sound.image.sprite = soundOff;
        }

        //called when loading a new scene
        public void UpdateRotateButtons(bool isAbsent)
        {
            if (isAbsent)
            {
                rightRotate.enabled = false;
                leftRotate.enabled = false;

                rightRotateButton.enabled = false;
                leftRotateButton.enabled = false;

                rightRotateBackground.enabled = false;
                leftRotateBackground.enabled = false;
            }

            else TurnOffRotate();
        }

        // called by rotator unlock
        public void TurnOnRotate()
        {
            rightRotate.enabled = true;
            leftRotate.enabled = true;

            rightRotateButton.enabled = true;
            leftRotateButton.enabled = true;

            rightRotateBackground.enabled = true;
            leftRotateBackground.enabled = true;

            rightRotate.sprite = rightRotateOn;
            leftRotate.sprite = leftRotateOn;

            rightRotateBackground.sprite = rotateOnBackground;
            leftRotateBackground.sprite = rotateOnBackground;
        }

        // called by rotator lock
        public void TurnOffRotate()
        {
            rightRotate.enabled = true;
            leftRotate.enabled = true;

            rightRotateButton.enabled = false;
            leftRotateButton.enabled = false;

            rightRotateBackground.enabled = true;
            leftRotateBackground.enabled = true;

            rightRotate.sprite = rotateLock;
            leftRotate.sprite = rotateLock;

            rightRotateBackground.sprite = rotateLockBackground;
            leftRotateBackground.sprite = rotateLockBackground;
        }

        //opens next level window
        public void OpenWinLevelWindow()
        {
            TurnOffAllCanvases();
            ResetCanvasSortOrder();
            levelPassedCanvas.enabled = true;
            levelName.text = LevelsManager.instance._levelName;
            playerScore.text = "Your Score : " + PlayerMoves.instance.numberOfMoves.ToString();
            levelScore.text = "Gold Score : " + LevelsManager.instance._minimumMoves.ToString();
        }

        //called on WinLeveWindow button press
        public void NextLevel()
        {
            levelPassedCanvas.enabled = false;

            StartCoroutine(LevelsManager.instance.MoveToNextLevel());

            LevelsManager.instance.loadToKubicode = LevelsManager.instance._Kubicode;
        }
        #endregion

        #region WORLDMAP
        //called when the user selects a level from the worldmap
        public void UpdateWMInfo(LevelFile levelFile)
        {
            levelNameWM.enabled = true;
            levelNameWM.text = levelFile.levelName;
            playButton.SetActive(true);
        }
        #endregion

        #region LEVEL EDITOR
        private void ResetSelections()
        {
            UniverseBackgroundImage.sprite = BackgroundUnselected;
            UniverseIconImage.sprite = UniverseIconUnselected;

            FunctionBackgroundImage.sprite = BackgroundUnselected;
            FunctionIconImage.sprite = FunctionIconUnselected;

            DecoratorBackgroundImage.sprite = BackgroundUnselected;
            DecoratorIconImage.sprite = DecoratorIconUnselected;

            EmoteBackgroundImage.sprite = BackgroundUnselected;
            EmoteIconImage.sprite = EmoteIconUnselected;
        }

        void UniverseModePriority()
        {
            ResetSelections();

            UniverseBackgroundImage.sprite = BackgroundSelected;
            UniverseIconImage.sprite = UniverseIconSelected;

            UniverseMode.SetActive(true);
            FunctionMode.SetActive(false);
            DecoratorMode.SetActive(false);
            EmoteMode.SetActive(false);
        }

        //set function cubes as the active panel
        void FunctionModePriority()
        {
            ResetSelections();

            FunctionBackgroundImage.sprite = BackgroundSelected;
            FunctionIconImage.sprite = FunctionIconSelected;

            LevelEditor.instance.currentCube = CubeTypes.MoveableCube; //optional, remove to let player pick Cube
            UniverseMode.SetActive(false);
            FunctionMode.SetActive(true);
            DecoratorMode.SetActive(false);
            EmoteMode.SetActive(false);
        }

        //set decorator cubes as the active panel
        void DecoratorModePriority()
        {
            ResetSelections();

            DecoratorBackgroundImage.sprite = BackgroundSelected;
            DecoratorIconImage.sprite = DecoratorIconSelected;

            LevelEditor.instance.currentCube = CubeTypes.FullStaticCube; //optional, remove to let player pick Cube
            UniverseMode.SetActive(false);
            FunctionMode.SetActive(false);
            DecoratorMode.SetActive(true);
            EmoteMode.SetActive(false);
        }

        //set decorator cubes as the active panel
        void EmoteModePriority()
        {
            ResetSelections();

            EmoteBackgroundImage.sprite = BackgroundSelected;
            EmoteIconImage.sprite = EmoteIconSelected;

            UniverseMode.SetActive(false);
            FunctionMode.SetActive(false);
            DecoratorMode.SetActive(false);
            EmoteMode.SetActive(true);
        }

        //called by user when saving a level
        void UserSavedLevel()
        {
            SaveAndLoad.instance.UserSavingLevel(saveLevelName.text);
        }

        //called by user when saving a level
        void UserSavedCurrentLevel()
        {
            if (SaveAndLoad.instance.currentOpenLevelName != "") SaveAndLoad.instance.UserSavingCurrentLevel();
            else OpenSaveWindow();
        }

        //called by user when loading a level
        void UserLoadLevel()
        {
            //these dropdown caption texts are filled with the info provided by User Level Files
            SaveAndLoad.instance.UserLoadLevel(playerLevelsDropdown.captionText.text);
        }

        //called by user when loading a level
        void UserDeleteLevel()
        {
            //these dropdown caption texts are filled with the info provided by User Level Files
            SaveAndLoad.instance.UserDeleteLevel(playerLevelsDropdown.captionText.text);
        }

        private void UserCreateNewLevel()
        {
            _Grid.instance.RefreshGrid();
            SaveAndLoad.instance.currentOpenLevelName = "";
        }

        //called to open Level Editor options
        void OpenOptionsWindow()
        {
            levelEditorOptionsWindow.SetActive(!levelEditorOptionsWindow.activeInHierarchy);
        }

        //called to open Level Editor options
        void OpenSaveWindow()
        {
            levelEditorSaveWindow.SetActive(!levelEditorSaveWindow.activeInHierarchy);
        }


        // called when user tests level in Level Editor
        void TestUserLevel()
        {
            StartCoroutine(LevelsManager.instance.OpenTestLevel());

        }

        // called when user presses back button
        public void ReturnToLevelEditor()
        {
            StartCoroutine(LevelsManager.instance.CloseTestLevel());
        }
        #endregion

        #region FADING
        public void TransitionStart()
        {
            transitionCanvas.enabled = true;
            transitionFinished = false;
            transitionCanvas.sortingOrder = 2000;
        }

        public void TransitionOver()
        {
            transitionCanvas.sortingOrder = 0;
            transitionFinished = true;
            transitionCanvas.enabled = false;
        }

        IEnumerator DimGame()
        {
            if (gameDimmed == false)
            {
                TransitionStart();

                openBurgerMenuButton.SetActive(false);
                hiddenMenuButtons.SetActive(true);

                timePassed = 0f;
                startAlphaValue = 0f;
                targetAlphaValue = .5f;

                StartCoroutine(FadeTransition(startAlphaValue, targetAlphaValue, transitionDuration, timePassed));

                hamburgerMenuCanvas.sortingOrder = 3000;
                hamburgerMenuCanvas2.sortingOrder = 3000;
                gameDimmed = true;
            }

            else if (gameDimmed == true)
            {
                timePassed = 0f;
                startAlphaValue = .5f;
                targetAlphaValue = 0f;
                StartCoroutine(FadeTransition(startAlphaValue, targetAlphaValue, transitionDuration, timePassed));
                gameDimmed = false;

                yield return new WaitForSeconds(transitionDuration);

                hiddenMenuButtons.SetActive(false);
                openBurgerMenuButton.SetActive(true);

                TransitionOver();
            }
        }

        public IEnumerator FadeTransition(float startValue, float targetValue, float transitionDuration, float timePassed)
        {
            float valueChange = targetValue - startValue;
            Color alphaColor = new Color();

            alphaColor = fadeImage.color;

            while (timePassed <= transitionDuration)
            {
                timePassed += Time.deltaTime;

                switch (transitionType)
                {
                    case TransitionType.LinearTween:
                        alphaColor.a = TweenManager.LinearTween(timePassed, startValue, valueChange, transitionDuration);
                        yield return null;
                        break;
                    case TransitionType.EaseInQuad:
                        alphaColor.a = TweenManager.EaseInQuad(timePassed, startValue, valueChange, transitionDuration);
                        yield return null;
                        break;
                    case TransitionType.EaseOutQuad:
                        alphaColor.a = TweenManager.EaseOutQuad(timePassed, startValue, valueChange, transitionDuration);
                        yield return null;
                        break;
                    case TransitionType.EaseInOutQuad:
                        alphaColor.a = TweenManager.EaseInOutQuad(timePassed, startValue, valueChange, transitionDuration);
                        yield return null;
                        break;
                    case TransitionType.EaseInOutQuint:
                        alphaColor.a = TweenManager.EaseInOutQuint(timePassed, startValue, valueChange, transitionDuration);
                        yield return null;
                        break;
                    case TransitionType.EaseInOutSine:
                        alphaColor.a = TweenManager.EaseInOutSine(timePassed, startValue, valueChange, transitionDuration);
                        yield return null;
                        break;
                }

                fadeImage.color = alphaColor;
            }
        }
        #endregion
    }
}
