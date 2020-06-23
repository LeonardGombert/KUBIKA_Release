using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _MaterialCentral : MonoBehaviour
    {
        // INSTANCE
        private static _MaterialCentral _instance;
        public static _MaterialCentral instance { get { return _instance; } }

        [Space]
        [Header("ALL CUBES")]
        _CubeBase[] allCube;
        public _BackgroundMaterialPreset BG;

        [Space]
        public _StaticPack[] staticPack;
        public Biomes staticIndex;
        public _DynamicPack[] dynamicPack;
        public int dynamicIndex;
        public _EmotePack[] emotePack;
        public int emoteIndex;
        public _FXPack[] fxPack;
        public int fxIndex;
        [Space]
        public _ActualAllPack actualPack;

        //[Header("ACTUAL GRAPH SETTINGS")]
        /*
        #region DYNAMIC SETTINGS
        [Space]
        [Header("DYNAMIC-----")]
        [Header("Base")]
        public Texture _BaseTex;
        public Mesh _BaseMesh;
        public Color _BaseColor;
        [Range(-360, 360)] public float Base_Hue;
        [Range(0, 2)] public float Base_Contrast;
        [Range(0, 2)] public float Base_Saturation;
        [Range(-1, 1)] public float Base_Brightness;

        [Header("Beton")]
        public Texture _BetonTex;
        public Mesh _BetonMesh;
        public Color _BetonColor;
        [Range(-360, 360)] public float Beton_Hue;
        [Range(0, 2)] public float Beton_Contrast;
        [Range(0, 2)] public float Beton_Saturation;
        [Range(-1, 1)] public float Beton_Brightness;

        [Header("Elevator")]
        public Texture _ElevatorTex;
        public Mesh _ElevatorMesh;
        public Color _ElevatorColor;
        [Range(-360, 360)] public float Elevator_Hue;
        [Range(0, 2)] public float Elevator_Contrast;
        [Range(0, 2)] public float Elevator_Saturation;
        [Range(-1, 1)] public float Elevator_Brightness;

        [Header("Counter")]
        public Texture _CounterTex;
        public Mesh _CounterMesh;
        public Color _CounterColor;
        [Range(-360, 360)] public float Counter_Hue;
        [Range(0, 2)] public float Counter_Contrast;
        [Range(0, 2)] public float Counter_Saturation;
        [Range(-1, 1)] public float Counter_Brightness;

        [Header("Rotators")]
        public Texture _RotatorsTex;
        public Mesh _RotatorsMesh;
        public Color _RotatorsColor;
        [Range(-360, 360)] public float Rotators_Hue;
        [Range(0, 2)] public float Rotators_Contrast;
        [Range(0, 2)] public float Rotators_Saturation;
        [Range(-1, 1)] public float Rotators_Brightness;

        [Header("Bomb")]
        public Texture _BombTex;
        public Mesh _BombMesh;
        public Color _BombColor;
        [Range(-360, 360)] public float Bomb_Hue;
        [Range(0, 2)] public float Bomb_Contrast;
        [Range(0, 2)] public float Bomb_Saturation;
        [Range(-1, 1)] public float Bomb_Brightness;

        [Header("Switch")]
        public Texture _SwitchTex;
        public Mesh _SwitchMesh;
        public Color _SwitchColor;
        [Range(-360, 360)] public float Switch_Hue;
        [Range(0, 2)] public float Switch_Contrast;
        [Range(0, 2)] public float Switch_Saturation;
        [Range(-1, 1)] public float Switch_Brightness;

        [Header("Ball")]
        public Texture _BallTex;
        public Mesh _BallMesh;
        public Color _BallColor;
        [Range(-360, 360)] public float Ball_Hue;
        [Range(0, 2)] public float Ball_Contrast;
        [Range(0, 2)] public float Ball_Saturation;
        [Range(-1, 1)] public float Ball_Brightness;

        [Header("Pastille")]
        public Texture _PastilleTex;
        public Mesh _PastilleMesh;
        public Color _PastilleColor;
        [Range(-360, 360)] public float Pastille_Hue;
        [Range(0, 2)] public float Pastille_Contrast;
        [Range(0, 2)] public float Pastille_Saturation;
        [Range(-1, 1)] public float Pastille_Brightness;
        #endregion

        #region STATIC SETTINGS
        [Space]
        [Header("STATIC-----")]
        [Header("Empty")]
        public Texture _EmptyTex;
        public Mesh _EmptyMesh;

        [Header("Full")]
        public Texture _FullTex;
        public Mesh _FullMesh;

        [Header("Top")]
        public Texture _TopTex;
        public Mesh _TopMesh;

        [Header("Corner")]
        public Texture _CornerTex;
        public Mesh _CornerMesh;

        [Header("Triple")]
        public Texture _TripleTex;
        public Mesh _TripleMesh;

        [Header("Quad")]
        public Texture _QuadTex;
        public Mesh _QuadMesh;

        [Header("Color Parameters")]
        public Color _TextureColor;
        [Range(-360, 360)] public float _Hue;
        [Range(0, 2)] public float _Contrast;
        [Range(0, 2)] public float _Saturation;
        [Range(-1, 1)] public float _Brightness;
        #endregion

        #region EMOTE SETTINGS
        [Space]
        [Header("EMOTE-----")]
        [Header("Base")]
        public Texture _BaseEmoteTex;

        [Header("Beton")]
        public Texture _BetonEmoteTex;

        [Header("Elevator")]
        public Texture _ElevatorEmoteTex;

        [Header("Counter")]
        public Texture _CounterEmoteTex;

        [Header("Rotators")]
        public Texture _RotatorsEmoteTex;

        [Header("Bomb")]
        public Texture _BombEmoteTex;

        [Header("Switch")]
        public Texture _SwitchEmoteTex;

        [Header("Ball")]
        public Texture _BallEmoteTex;

        [Header("Pastille")]
        public Texture _PastilleEmoteTex;
        #endregion
        */
        #region FX SETTINGS
        #endregion

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

            MaterialSet();
        }

        private void Start()
        {

        }

        public void MaterialSet()
        {
            allCube = FindObjectsOfType<_CubeBase>();

            ResetDynamicPacks(dynamicIndex);
            ResetStaticPacks((int)staticIndex);
            ResetEmotePacks(emoteIndex);
            ResetFxPacks(fxIndex);

            foreach (_CubeBase cube in allCube)
            {
                cube.SetScriptablePreset();
            }

            if(BG != null)
                BG.SetScriptablePreset();
        }


        public void ResetDynamicPacks(int index)
        {

            #region DYNAMIC SETTINGS
            actualPack._BaseTex = dynamicPack[index]._BaseTex;
            actualPack._BaseMesh = dynamicPack[index]._BaseMesh;
            actualPack._BaseColor = dynamicPack[index]._BaseColor;
            actualPack._BaseTexInside = dynamicPack[index]._BaseTexInside;
            actualPack._BaseInsideStrength = dynamicPack[index]._BaseInsideStrength;
            actualPack._BaseColorInside = dynamicPack[index]._BaseColorInside;
            actualPack.Base_Hue = dynamicPack[index].Base_Hue;
            actualPack.Base_Contrast = dynamicPack[index].Base_Contrast;
            actualPack.Base_Saturation = dynamicPack[index].Base_Saturation;
            actualPack.Base_Brightness = dynamicPack[index].Base_Brightness;

            actualPack._BetonTex = dynamicPack[index]._BetonTex;
            actualPack._BetonMesh = dynamicPack[index]._BetonMesh;
            actualPack._BetonColor = dynamicPack[index]._BetonColor;
            actualPack._BetonTexInside = dynamicPack[index]._BetonTexInside;
            actualPack._BetonInsideStrength = dynamicPack[index]._BetonInsideStrength;
            actualPack._BetonColorInside = dynamicPack[index]._BetonColorInside;
            actualPack.Beton_Hue = dynamicPack[index].Beton_Hue;
            actualPack.Beton_Contrast = dynamicPack[index].Beton_Contrast;
            actualPack.Beton_Saturation = dynamicPack[index].Beton_Saturation;
            actualPack.Beton_Brightness = dynamicPack[index].Beton_Brightness;

            actualPack._ElevatorTex = dynamicPack[index]._ElevatorTex;
            actualPack._ElevatorBackTex = dynamicPack[index]._ElevatorBackTex;
            actualPack._ElevatorMesh = dynamicPack[index]._ElevatorMesh;
            actualPack._ElevatorColor = dynamicPack[index]._ElevatorColor;
            actualPack._ElevatorBackTexInside = dynamicPack[index]._ElevatorBackTexInside;
            actualPack._ElevatorTexInside = dynamicPack[index]._ElevatorTexInside;
            actualPack._ElevatorColorInside = dynamicPack[index]._ElevatorColorInside;
            actualPack._ElevatorInsideStrength = dynamicPack[index]._ElevatorInsideStrength;
            actualPack.Elevator_Hue = dynamicPack[index].Elevator_Hue;
            actualPack.Elevator_Contrast = dynamicPack[index].Elevator_Contrast;
            actualPack.Elevator_Saturation = dynamicPack[index].Elevator_Saturation;
            actualPack.Elevator_Brightness = dynamicPack[index].Elevator_Brightness;

            actualPack._CounterTex1 = dynamicPack[index]._CounterTex1;
            actualPack._CounterTex2 = dynamicPack[index]._CounterTex2;
            actualPack._CounterTex3 = dynamicPack[index]._CounterTex3;
            actualPack._CounterTex4 = dynamicPack[index]._CounterTex4;
            actualPack._CounterTex5 = dynamicPack[index]._CounterTex5;
            actualPack._CounterTex6 = dynamicPack[index]._CounterTex6;
            actualPack._CounterTex7 = dynamicPack[index]._CounterTex7;
            actualPack._CounterTex8 = dynamicPack[index]._CounterTex8;
            actualPack._CounterTex9 = dynamicPack[index]._CounterTex9;
            actualPack._CounterMesh = dynamicPack[index]._CounterMesh;
            actualPack._CounterColor = dynamicPack[index]._CounterColor;
            actualPack._CounterTexInside = dynamicPack[index]._CounterTexInside;
            actualPack._CounterInsideStrength = dynamicPack[index]._CounterInsideStrength;
            actualPack._CounterColorInside = dynamicPack[index]._CounterColorInside;
            actualPack.Counter_Hue = dynamicPack[index].Counter_Hue;
            actualPack.Counter_Contrast = dynamicPack[index].Counter_Contrast;
            actualPack.Counter_Saturation = dynamicPack[index].Counter_Saturation;
            actualPack.Counter_Brightness = dynamicPack[index].Counter_Brightness;

            //ROTATORS

            actualPack._RotatorsTexSocle = dynamicPack[index]._RotatorsTexSocle;
            actualPack._RotatorsMeshSocle = dynamicPack[index]._RotatorsMeshSocle;
            actualPack._RotatorsColorSocle = dynamicPack[index]._RotatorsColorSocle;
            actualPack.Rotators_Hue = staticPack[index]._Hue;
            actualPack.Rotators_Contrast = staticPack[index]._Contrast;
            actualPack.Rotators_Saturation = staticPack[index]._Saturation;
            actualPack.Rotators_Brightness = staticPack[index]._Brightness;

            actualPack._RotatorsTexLeft = dynamicPack[index]._RotatorsTexLeft;
            actualPack._RotatorsMeshLeft = dynamicPack[index]._RotatorsMeshLeft;
            actualPack._RotatorsColorLeft = dynamicPack[index]._RotatorsColorLeft;
            actualPack._RotatorsTexInsideLeft = dynamicPack[index]._RotatorsTexInsideLeft;
            actualPack._RotatorsInsideStrengthLeft = dynamicPack[index]._RotatorsInsideStrengthLeft;
            actualPack._RotatorsColorInsideLeft = dynamicPack[index]._RotatorsColorInsideLeft;

            actualPack._RotatorsTexRight = dynamicPack[index]._RotatorsTexRight;
            actualPack._RotatorsMeshRight = dynamicPack[index]._RotatorsMeshRight;
            actualPack._RotatorsColorRight = dynamicPack[index]._RotatorsColorRight;
            actualPack._RotatorsTexInsideRight = dynamicPack[index]._RotatorsTexInsideRight;
            actualPack._RotatorsInsideStrengthRight = dynamicPack[index]._RotatorsInsideStrengthRight;
            actualPack._RotatorsColorInsideRight = dynamicPack[index]._RotatorsColorInsideRight;

            actualPack._RotatorsTexUI = dynamicPack[index]._RotatorsTexUI;
            actualPack._RotatorsMeshUI = dynamicPack[index]._RotatorsMeshUI;
            actualPack._RotatorsColorUI = dynamicPack[index]._RotatorsColorUI;
            actualPack._RotatorsTexInsideUI = dynamicPack[index]._RotatorsTexInsideUI;
            actualPack._RotatorsInsideStrengthUI = dynamicPack[index]._RotatorsInsideStrengthUI;
            actualPack._RotatorsColorInsideUI = dynamicPack[index]._RotatorsColorInsideUI;

            //__

            actualPack._BombTex = dynamicPack[index]._BombTex;
            actualPack._BombMesh = dynamicPack[index]._BombMesh;
            actualPack._BombColor = dynamicPack[index]._BombColor;
            actualPack._BombTexInside = dynamicPack[index]._BombTexInside;
            actualPack._BombInsideStrength = dynamicPack[index]._BombInsideStrength;
            actualPack._BombColorInside = dynamicPack[index]._BombColorInside;
            actualPack.Bomb_Hue = dynamicPack[index].Bomb_Hue;
            actualPack.Bomb_Contrast = dynamicPack[index].Bomb_Contrast;
            actualPack.Bomb_Saturation = dynamicPack[index].Bomb_Saturation;
            actualPack.Bomb_Brightness = dynamicPack[index].Bomb_Brightness;

            actualPack._SwitchTexOn = dynamicPack[index]._SwitchTexOn;
            actualPack._SwitchTexOff = dynamicPack[index]._SwitchTexOff;
            actualPack._SwitchMesh = dynamicPack[index]._SwitchMesh;
            actualPack._SwitchColor = dynamicPack[index]._SwitchColor;
            actualPack._SwitchTexInside = dynamicPack[index]._SwitchTexInside;
            actualPack._SwitchInsideStrength = dynamicPack[index]._SwitchInsideStrength;
            actualPack._SwitchColorInside = dynamicPack[index]._SwitchColorInside;
            actualPack.Switch_Hue = dynamicPack[index].Switch_Hue;
            actualPack.Switch_Contrast = dynamicPack[index].Switch_Contrast;
            actualPack.Switch_Saturation = dynamicPack[index].Switch_Saturation;
            actualPack.Switch_Brightness = dynamicPack[index].Switch_Brightness;

            // Switch Bouton
            actualPack._SwitchTexOnButton = dynamicPack[index]._SwitchTexOnButton;
            actualPack._SwitchTexOffButton = dynamicPack[index]._SwitchTexOffButton;
            actualPack._SwitchTexInsideButton = dynamicPack[index]._SwitchTexInsideButton;
            actualPack._SwitchMeshButton = dynamicPack[index]._SwitchMeshButton;
            actualPack._SwitchColorButton = dynamicPack[index]._SwitchColorButton;
            actualPack._SwitchColorInsideButton = dynamicPack[index]._SwitchColorInsideButton;
            actualPack._SwitchInsideStrengthButton = dynamicPack[index]._SwitchInsideStrengthButton;

            //__

            actualPack._BallTex = dynamicPack[index]._BallTex;
            actualPack._BallMesh = dynamicPack[index]._BallMesh;
            actualPack._BallColor = dynamicPack[index]._BallColor;
            actualPack._BallTexInside = dynamicPack[index]._BallTexInside;
            actualPack._BallInsideStrength = dynamicPack[index]._BallInsideStrength;
            actualPack._BallColorInside = dynamicPack[index]._BallColorInside;
            actualPack.Ball_Hue = dynamicPack[index].Ball_Hue;
            actualPack.Ball_Contrast = dynamicPack[index].Ball_Contrast;
            actualPack.Ball_Saturation = dynamicPack[index].Ball_Saturation;
            actualPack.Ball_Brightness = dynamicPack[index].Ball_Brightness;

            actualPack._PastilleTex = dynamicPack[index]._PastilleTex;
            actualPack._PastilleMesh = dynamicPack[index]._PastilleMesh;
            actualPack._PastilleColor = dynamicPack[index]._PastilleColor;
            actualPack._PastilleTexInside = dynamicPack[index]._PastilleTexInside;
            actualPack._PastilleInsideStrength = dynamicPack[index]._PastilleInsideStrength;
            actualPack._PastilleColorInside = dynamicPack[index]._PastilleColorInside;
            actualPack.Pastille_Hue = dynamicPack[index].Pastille_Hue;
            actualPack.Pastille_Contrast = dynamicPack[index].Pastille_Contrast;
            actualPack.Pastille_Saturation = dynamicPack[index].Pastille_Saturation;
            actualPack.Pastille_Brightness = dynamicPack[index].Pastille_Brightness;


            // VICTORY

            actualPack._BaseVTex = dynamicPack[index]._BaseVTex;
            actualPack._BaseVMesh = dynamicPack[index]._BaseVMesh;
            actualPack._BaseVColor = dynamicPack[index]._BaseVColor;
            actualPack._BaseVTexInside = dynamicPack[index]._BaseVTexInside;
            actualPack._BaseVInsideStrength = dynamicPack[index]._BaseVInsideStrength;
            actualPack._BaseVColorInside = dynamicPack[index]._BaseVColorInside;
            actualPack.BaseV_Hue = dynamicPack[index].BaseV_Hue;
            actualPack.BaseV_Contrast = dynamicPack[index].BaseV_Contrast;
            actualPack.BaseV_Saturation = dynamicPack[index].BaseV_Saturation;
            actualPack.BaseV_Brightness = dynamicPack[index].BaseV_Brightness;

            actualPack._BetonVTex = dynamicPack[index]._BetonVTex;
            actualPack._BetonVMesh = dynamicPack[index]._BetonVMesh;
            actualPack._BetonVColor = dynamicPack[index]._BetonVColor;
            actualPack._BetonVTexInside = dynamicPack[index]._BetonVTexInside;
            actualPack._BetonVInsideStrength = dynamicPack[index]._BetonVInsideStrength;
            actualPack._BetonVColorInside = dynamicPack[index]._BetonVColorInside;
            actualPack.BetonV_Hue = dynamicPack[index].BetonV_Hue;
            actualPack.BetonV_Contrast = dynamicPack[index].BetonV_Contrast;
            actualPack.BetonV_Saturation = dynamicPack[index].BetonV_Saturation;
            actualPack.BetonV_Brightness = dynamicPack[index].BetonV_Brightness;

            actualPack._BombVTex = dynamicPack[index]._BombVTex;
            actualPack._BombVMesh = dynamicPack[index]._BombVMesh;
            actualPack._BombVColor = dynamicPack[index]._BombVColor;
            actualPack._BombVTexInside = dynamicPack[index]._BombVTexInside;
            actualPack._BombVInsideStrength = dynamicPack[index]._BombVInsideStrength;
            actualPack._BombVColorInside = dynamicPack[index]._BombVColorInside;
            actualPack.BombV_Hue = dynamicPack[index].BombV_Hue;
            actualPack.BombV_Contrast = dynamicPack[index].BombV_Contrast;
            actualPack.BombV_Saturation = dynamicPack[index].BombV_Saturation;
            actualPack.BombV_Brightness = dynamicPack[index].BombV_Brightness;

            actualPack._SwitchVTexOn = dynamicPack[index]._SwitchVTexOn;
            actualPack._SwitchVTexOff = dynamicPack[index]._SwitchVTexOff;
            actualPack._SwitchVMesh = dynamicPack[index]._SwitchVMesh;
            actualPack._SwitchVColor = dynamicPack[index]._SwitchVColor;
            actualPack._SwitchVTexInside = dynamicPack[index]._SwitchVTexInside;
            actualPack._SwitchVInsideStrength = dynamicPack[index]._SwitchVInsideStrength;
            actualPack._SwitchVColorInside = dynamicPack[index]._SwitchVColorInside;
            actualPack.SwitchV_Hue = dynamicPack[index].SwitchV_Hue;
            actualPack.SwitchV_Contrast = dynamicPack[index].SwitchV_Contrast;
            actualPack.SwitchV_Saturation = dynamicPack[index].SwitchV_Saturation;
            actualPack.SwitchV_Brightness = dynamicPack[index].SwitchV_Brightness;

            actualPack._BallVTex = dynamicPack[index]._BallVTex;
            actualPack._BallVMesh = dynamicPack[index]._BallVMesh;
            actualPack._BallVColor = dynamicPack[index]._BallVColor;
            actualPack._BallVTexInside = dynamicPack[index]._BallVTexInside;
            actualPack._BallVInsideStrength = dynamicPack[index]._BallVInsideStrength;
            actualPack._BallVColorInside = dynamicPack[index]._BallVColorInside;
            actualPack.BallV_Hue = dynamicPack[index].BallV_Hue;
            actualPack.BallV_Contrast = dynamicPack[index].BallV_Contrast;
            actualPack.BallV_Saturation = dynamicPack[index].BallV_Saturation;
            actualPack.BallV_Brightness = dynamicPack[index].BallV_Brightness;


            #endregion

        }

        public void ResetStaticPacks(int index)
        {

            #region STATIC SETTINGS

            actualPack._BGTex = staticPack[index]._BGTex;
            actualPack._BGFX = staticPack[index]._BGFX;

            actualPack._HueBG = staticPack[index]._HueBG;
            actualPack._ContrastBG = staticPack[index]._ContrastBG;
            actualPack._SaturationBG = staticPack[index]._SaturationBG;
            actualPack._BrightnessBG = staticPack[index]._BrightnessBG;

            actualPack._EmptyTex = staticPack[index]._EmptyTex;
            actualPack._EmptyTex2 = staticPack[index]._EmptyTex2;
            actualPack._EmptyMesh = staticPack[index]._EmptyMesh;

            actualPack._FullTex = staticPack[index]._FullTex;
            actualPack._FullTex2 = staticPack[index]._FullTex2;
            actualPack._FullMesh = staticPack[index]._FullMesh;

            actualPack._TopTex = staticPack[index]._TopTex;
            actualPack._TopTex2 = staticPack[index]._TopTex2;
            actualPack._TopMesh = staticPack[index]._TopMesh;

            actualPack._CornerTex = staticPack[index]._CornerTex;
            actualPack._CornerTex2 = staticPack[index]._CornerTex2;
            actualPack._CornerMesh = staticPack[index]._CornerMesh;

            actualPack._TripleTex = staticPack[index]._TripleTex;
            actualPack._TripleTex2 = staticPack[index]._TripleTex2;
            actualPack._TripleMesh = staticPack[index]._TripleMesh;

            actualPack._QuadTex = staticPack[index]._QuadTex;
            actualPack._QuadTex2 = staticPack[index]._QuadTex2;
            actualPack._QuadMesh = staticPack[index]._QuadMesh;

            actualPack._TextureColor = staticPack[index]._TextureColor;
            actualPack._Hue = staticPack[index]._Hue;
            actualPack._Contrast = staticPack[index]._Contrast;
            actualPack._Saturation = staticPack[index]._Saturation;
            actualPack._Brightness = staticPack[index]._Brightness;

            #endregion

        }

        public void ResetEmotePacks(int index)
        {

            #region EMOTE SETTINGS

            actualPack._BaseIdleEmoteTex = emotePack[index]._BaseIdleEmoteTex;
            actualPack._BaseFallEmoteTex = emotePack[index]._BaseFallEmoteTex;
            actualPack._BaseFatalFallEmoteTex = emotePack[index]._BaseFatalFallEmoteTex;
            actualPack._BaseSelectedEmoteTex = emotePack[index]._BaseSelectedEmoteTex;

            actualPack._BetonIdleEmoteTex = emotePack[index]._BetonIdleEmoteTex;
            actualPack._BetonFallEmoteTex = emotePack[index]._BetonFallEmoteTex;
            actualPack._BetonFatalFallEmoteTex = emotePack[index]._BetonFatalFallEmoteTex;
            actualPack._BetonSelectedEmoteTex = emotePack[index]._BetonSelectedEmoteTex;

            actualPack._BombIdleEmoteTex = emotePack[index]._BombIdleEmoteTex;
            actualPack._BombFallEmoteTex = emotePack[index]._BombFallEmoteTex;
            actualPack._BombFatalFallEmoteTex = emotePack[index]._BombFatalFallEmoteTex;
            actualPack._BombSelectedEmoteTex = emotePack[index]._BombSelectedEmoteTex;

            actualPack._SwitchIdleEmoteTex = emotePack[index]._SwitchIdleEmoteTex;
            actualPack._SwitchIdleOffEmoteTex = emotePack[index]._SwitchIdleOffEmoteTex;
            actualPack._SwitchFallEmoteTex = emotePack[index]._SwitchFallEmoteTex;
            actualPack._SwitchFatalFallEmoteTex = emotePack[index]._SwitchFatalFallEmoteTex;
            actualPack._SwitchSelectedEmoteTex = emotePack[index]._SwitchSelectedEmoteTex;

            actualPack._BallIdleEmoteTex = emotePack[index]._BallIdleEmoteTex;
            actualPack._BallFallEmoteTex = emotePack[index]._BallFallEmoteTex;
            actualPack._BallFatalFallEmoteTex = emotePack[index]._BallFatalFallEmoteTex;
            actualPack._BallSelectedEmoteTex = emotePack[index]._BallSelectedEmoteTex;

            actualPack._BaseVIdleEmoteTex = emotePack[index]._BaseVIdleEmoteTex;
            actualPack._BaseVFallEmoteTex = emotePack[index]._BaseVFallEmoteTex;
            actualPack._BaseVFatalFallEmoteTex = emotePack[index]._BaseVFatalFallEmoteTex;
            actualPack._BaseVSelectedEmoteTex = emotePack[index]._BaseVSelectedEmoteTex;
            actualPack._BaseVPastilleEmoteTex = emotePack[index]._BaseVPastilleEmoteTex;

            actualPack._BetonVIdleEmoteTex = emotePack[index]._BetonVIdleEmoteTex;
            actualPack._BetonVFallEmoteTex = emotePack[index]._BetonVFallEmoteTex;
            actualPack._BetonVFatalFallEmoteTex = emotePack[index]._BetonVFatalFallEmoteTex;
            actualPack._BetonVSelectedEmoteTex = emotePack[index]._BetonVSelectedEmoteTex;
            actualPack._BetonVPastilleEmoteTex = emotePack[index]._BetonVPastilleEmoteTex;

            actualPack._BombVIdleEmoteTex = emotePack[index]._BombVIdleEmoteTex;
            actualPack._BombVFallEmoteTex = emotePack[index]._BombVFallEmoteTex;
            actualPack._BombVFatalFallEmoteTex = emotePack[index]._BombVFatalFallEmoteTex;
            actualPack._BombVSelectedEmoteTex = emotePack[index]._BombVSelectedEmoteTex;
            actualPack._BombVPastilleEmoteTex = emotePack[index]._BombVPastilleEmoteTex;

            actualPack._SwitchVIdleEmoteTex = emotePack[index]._SwitchVIdleEmoteTex;
            actualPack._SwitchVIdleOffEmoteTex = emotePack[index]._SwitchVIdleOffEmoteTex;
            actualPack._SwitchVFallEmoteTex = emotePack[index]._SwitchVFallEmoteTex;
            actualPack._SwitchVFatalFallEmoteTex = emotePack[index]._SwitchVFatalFallEmoteTex;
            actualPack._SwitchVSelectedEmoteTex = emotePack[index]._SwitchVSelectedEmoteTex;
            actualPack._SwitchVPastilleEmoteTex = emotePack[index]._SwitchVPastilleEmoteTex;

            actualPack._BallVIdleEmoteTex = emotePack[index]._BallVIdleEmoteTex;
            actualPack._BallVFallEmoteTex = emotePack[index]._BallVFallEmoteTex;
            actualPack._BallVFatalFallEmoteTex = emotePack[index]._BallVFatalFallEmoteTex;
            actualPack._BallVSelectedEmoteTex = emotePack[index]._BallVSelectedEmoteTex;
            actualPack._BallVPastilleEmoteTex = emotePack[index]._BallVPastilleEmoteTex;

            #endregion

        }

        public void ResetFxPacks(int index)
        {

            #region FX SETTINGS
            #endregion

        }

        public void ChangeUniverse(Biomes newBiome)
        {
            allCube = FindObjectsOfType<_CubeBase>(); // TODO DEGEU
            if ((int)staticIndex < staticPack.Length)
            {
                staticIndex = newBiome;
                ResetStaticPacks((int)staticIndex);
                foreach (_CubeBase cube in allCube)
                {
                    cube.SetScriptablePreset();
                }
            }
        }

        public void ChangeUniverseRight()
        {
            allCube = FindObjectsOfType<_CubeBase>(); // TODO DEGEU
            if ((int)staticIndex < staticPack.Length)
            {
                staticIndex += 1;
                ResetStaticPacks((int)staticIndex);
                foreach (_CubeBase cube in allCube)
                {
                    cube.SetScriptablePreset();
                }
            }
        }

        public void ChangeUniverseLeft()
        {
            allCube = FindObjectsOfType<_CubeBase>();
            if ((int)staticIndex > 0)
            {
                staticIndex -= 1;
                ResetStaticPacks((int)staticIndex);
                foreach (_CubeBase cube in allCube)
                {
                    cube.SetScriptablePreset();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
