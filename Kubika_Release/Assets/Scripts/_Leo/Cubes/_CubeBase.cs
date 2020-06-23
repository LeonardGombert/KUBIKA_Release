using Kubika.CustomLevelEditor;
using System;
using System.Collections;
using UnityEngine;

namespace Kubika.Game
{
    //base cube class
    public class _CubeBase : MonoBehaviour
    {
        //starts at 1
        public int myIndex;
        //use this to set node data
        public CubeTypes myCubeType;
        public CubeLayers myCubeLayer;
        public FacingDirection facingDirection;

        public bool isStatic;
        public _Grid grid;

        [Space][Header("MATERIAL INFOS")]
        public Texture _MainTex;
        public Mesh _MainMesh;
        public Color _MainColor;

        [Range(-360, 360)] public float _Hue;
        [Range(0, 2)] public float _Contrast;
        [Range(0, 2)] public float _Saturation;
        [Range(-1, 1)] public float _Brightness;

        [Space]
        public Texture _EmoteIdleTex;
        public Texture _EmoteIdleOffTex;
        public Texture _EmoteFallTex;
        public Texture _EmoteFatalFallTex;
        public Texture _EmoteSelectedTex;
        public Texture _EmotePastilleTex;
        public float _EmoteStrength;
        [HideInInspector] public bool isPastilleAndIsOn;

        [Space]
        public Texture _InsideTex;
        public Color _InsideColor;
        public float _InsideStrength;

        public Texture _EdgeTex;
        public Color _EdgeColor;
        public float _EdgeStrength;

        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public MeshFilter meshFilter;
        [HideInInspector] public MaterialPropertyBlock MatProp; // To change Mat Properties


        // FEEDBACKS
        float actualContrast;
        float actualSaturation;
        float currentOfValueChange;
        float timeOfValueChange = 0.5f;
        float currentValue;
        float currentValueSaturation;
        float maxValueColor = 2;
        float maxValueColorSaturation = 1.1f;

        //POP_OUT 
        float scaleMul = 1.2f;
        float currentOfValueChangePOP;
        float timeOfValueChangePOP = 0.2f;
        Vector3 actualScale;
        Vector3 baseScale;
        Vector3 targetScale;
        protected ParticleSystem PopOutPS;
        public bool willPOP;

        // RIGIDBODY
        Rigidbody rigidbody;

        // AUDIO 
        public AudioSource audioSourceCube;

        // Start is called before the first frame update
        public virtual void Start()
        {
            grid = _Grid.instance;
            SetScriptablePreset();

            audioSourceCube = gameObject.AddComponent<AudioSource>();
            audioSourceCube.playOnAwake = false;
            audioSourceCube.outputAudioMixerGroup = _AudioManager.instance.audioMixer;
            audioSourceCube.loop = false;
        }

        public virtual void Update()
        {
            //SetScriptablePreset(); /////////// DE LA MERDE
        }

        //Use to update Cube Info in Matrix, called on place and rotate cube
        public void SetRelevantNodeInfo()
        {
            _Grid.instance.kuboGrid[myIndex - 1].cubeLayers = myCubeLayer;
            _Grid.instance.kuboGrid[myIndex - 1].cubeType = myCubeType;
            _Grid.instance.kuboGrid[myIndex - 1].cubeOnPosition = gameObject;
            _Grid.instance.kuboGrid[myIndex - 1].facingDirection = facingDirection;

            //_Grid.instance.kuboGrid[myIndex - 1].worldPosition = transform.position; -> DON'T RESET WORLDPOS because cubes must reset to nodePos
            _Grid.instance.kuboGrid[myIndex - 1].worldRotation = transform.eulerAngles;
        }

        //called on Cube Destroy, which actives on editor deleteCube()
        public void ResetCubeInfo()
        {
            _Grid.instance.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            _Grid.instance.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;

            //_Grid.instance.kuboGrid[myIndex - 1].worldPosition = Vector3.zero; -> DON'T RESET WORLDPOS because cubes must reset to nodePos
            _Grid.instance.kuboGrid[myIndex - 1].worldRotation = Vector3.zero;
        }

        //call when level is loaded, places cube in world
        public void OnLoadSetTransform()
        {
            transform.position = _Grid.instance.kuboGrid[myIndex - 1].worldPosition;
            transform.rotation = Quaternion.Euler(_Grid.instance.kuboGrid[myIndex - 1].worldRotation);
            transform.parent = _Grid.instance.gameObject.transform;
        }

        public void DisableCube()
        {
            meshRenderer.enabled = false;
            HideCubeProcedure();
        }

        public void EnableCube()
        {
            gameObject.SetActive(true);
            meshRenderer.enabled = true;
            UndoProcedure();
        }

        //gets called when you "hide"/"destroy a cube
        public virtual void HideCubeProcedure()
        {
            _Grid.instance.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            _Grid.instance.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;
            _Grid.instance.kuboGrid[myIndex - 1].cubeOnPosition = null;

            _DataManager.instance.baseCube.Remove(this);
        }

        // call when "reactivating" cubes
        public virtual void UndoProcedure()
        {
            _Grid.instance.kuboGrid[myIndex - 1].cubeLayers = myCubeLayer;
            _Grid.instance.kuboGrid[myIndex - 1].cubeType = myCubeType;
            _Grid.instance.kuboGrid[myIndex - 1].cubeOnPosition = gameObject;

            _DataManager.instance.baseCube.Add(this);
        }


        public bool MatrixLimitCalcul(int index, int nodeDirection)
        {
            // X
            if (nodeDirection == _DirectionCustom.left)
            {
                if (((index - grid.gridSize) + (grid.gridSize * grid.gridSize) - 1) / ((grid.gridSize * grid.gridSize) * (index / (grid.gridSize * grid.gridSize)) + (grid.gridSize * grid.gridSize)) != 0)
                    return true;
                else return false;
            }
            // -X
            else if (nodeDirection == _DirectionCustom.right)
            {
                if ((index + grid.gridSize) / ((grid.gridSize * grid.gridSize) * (index / (grid.gridSize * grid.gridSize) + 1)) != 1)
                    return true;
                else return false;
            }
            // Z
            else if (nodeDirection == _DirectionCustom.forward)
            {
                if ((index + (grid.gridSize * grid.gridSize)) / ((grid.gridSize * grid.gridSize * grid.gridSize)) != 1)
                    return true;
                else return false;
            }
            // -Z
            else if (nodeDirection == _DirectionCustom.backward)
            {
                if (index - (grid.gridSize * grid.gridSize) >= 0)
                    return true;
                else return false;
            }
            // Y
            else if (nodeDirection == _DirectionCustom.up)
            {
                if (index % grid.gridSize != 0)
                    return true;
                else return false;
            }
            // -Y
            else if (nodeDirection == _DirectionCustom.down)
            {
                if ((index - 1) % grid.gridSize != 0)
                    return true;
                else return false;
            }
            else return false;
        }


        #region MATERIAL

        public void SetMaterial()
        {
            MatProp = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            meshRenderer.GetPropertyBlock(MatProp);

            meshFilter.mesh = _MainMesh;

            MatProp.SetTexture("_MainTex", _MainTex);
            MatProp.SetTexture("_InsideTex", _InsideTex);
            MatProp.SetTexture("_EdgeTex", _EdgeTex);
            MatProp.SetTexture("_Emote", _EmoteIdleTex);

            MatProp.SetColor("_MainColor", _MainColor);
            MatProp.SetColor("_InsideColor", _InsideColor);
            MatProp.SetColor("_EdgeColor", _EdgeColor);

            MatProp.SetFloat("_InsideTexStrength", _InsideStrength);
            MatProp.SetFloat("_EdgeTexStrength", _EdgeStrength);
            MatProp.SetFloat("_EmoteStrength", _EmoteStrength);

            MatProp.SetFloat("_Hue", _Hue);
            MatProp.SetFloat("_Contrast", _Contrast);
            MatProp.SetFloat("_Saturation", _Saturation);
            MatProp.SetFloat("_Brightness", _Brightness);

            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void SetScriptablePreset()
        {
            switch (myCubeType)
            {
                case CubeTypes.MoveableCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BaseTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BaseMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BaseColor;

                        _Hue = _MaterialCentral.instance.actualPack.Base_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Base_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Base_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Base_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BaseTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BaseColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BaseInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;


                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BaseIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BaseFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BaseFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BaseSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BaseVPastilleEmoteTex;

                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.ConcreteCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BetonTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BetonMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BetonColor;

                        _Hue = _MaterialCentral.instance.actualPack.Beton_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Beton_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Beton_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Beton_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BetonTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BetonColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BetonInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BetonIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BetonFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BetonFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BetonSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BetonVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube1:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex1; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube2:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex2; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube3:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex3; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube4:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex4; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube5:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex5; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube6:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex6; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break; ///
                case CubeTypes.TimerCube7:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex7;//
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube8:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex8; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.TimerCube9:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CounterTex9; /////
                        _MainMesh = _MaterialCentral.instance.actualPack._CounterMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._CounterColor;

                        _Hue = _MaterialCentral.instance.actualPack.Counter_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Counter_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Counter_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Counter_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._CounterTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._CounterColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._CounterInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.BombCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BombTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BombMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BombColor;

                        _Hue = _MaterialCentral.instance.actualPack.Bomb_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Bomb_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Bomb_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Bomb_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BombTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BombColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BombInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BombIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BombFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BombFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BombSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BombVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.BlueElevatorCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._ElevatorBackTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._ElevatorMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._ElevatorColor;

                        _Hue = _MaterialCentral.instance.actualPack.Elevator_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Elevator_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Elevator_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Elevator_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._ElevatorTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._ElevatorColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._ElevatorInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.GreenElevatorCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._ElevatorTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._ElevatorMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._ElevatorColor;

                        _Hue = _MaterialCentral.instance.actualPack.Elevator_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Elevator_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Elevator_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Elevator_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._ElevatorTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._ElevatorColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._ElevatorInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.ChaosBall:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BallTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BallMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BallColor;

                        _Hue = _MaterialCentral.instance.actualPack.Ball_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Ball_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Ball_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Ball_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BallTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BallColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BallInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BallIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BallFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BallFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BallSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BallVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.SwitchCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._SwitchTexOff; ///////
                        _MainMesh = _MaterialCentral.instance.actualPack._SwitchMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._SwitchColor;

                        _Hue = _MaterialCentral.instance.actualPack.Switch_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Switch_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Switch_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Switch_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._SwitchTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._SwitchColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._SwitchInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._SwitchIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._SwitchIdleOffEmoteTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._SwitchFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._SwitchFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._SwitchSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._SwitchVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.SwitchButton:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._EmptyTex; ////////
                        _MainMesh = _MaterialCentral.instance.actualPack._EmptyMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.RotatorLocker:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._EmptyTex; ////////
                        _MainMesh = _MaterialCentral.instance.actualPack._EmptyMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.RotatorLeftTurner:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._EmptyTex; ////////
                        _MainMesh = _MaterialCentral.instance.actualPack._EmptyMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.RotatorRightTurner:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._EmptyTex; ////////
                        _MainMesh = _MaterialCentral.instance.actualPack._EmptyMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.DeliveryCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._PastilleTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._PastilleMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._PastilleColor;

                        _Hue = _MaterialCentral.instance.actualPack.Pastille_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.Pastille_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.Pastille_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.Pastille_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._PastilleTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._PastilleColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._PastilleInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.BaseVictoryCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BaseVTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BaseVMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BaseVColor;

                        _Hue = _MaterialCentral.instance.actualPack.BaseV_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.BaseV_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.BaseV_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.BaseV_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BaseVTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BaseVColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BaseVInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BaseVIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BaseVFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BaseVFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BaseVSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BaseVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.ConcreteVictoryCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BetonVTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BetonVMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BetonVColor;

                        _Hue = _MaterialCentral.instance.actualPack.BetonV_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.BetonV_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.BetonV_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.BetonV_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BetonVTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BetonVColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BetonVInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BetonVIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BetonVFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BetonVFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BetonVSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BetonVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.BombVictoryCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._BombVTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._BombVMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._BombVColor;

                        _Hue = _MaterialCentral.instance.actualPack.BombV_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.BombV_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.BombV_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.BombV_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._BombVTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._BombVColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._BombVInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._BombVIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._BombVFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._BombVFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._BombVSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._BombVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;
                case CubeTypes.SwitchVictoryCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._SwitchVTexOff; ///////
                        _MainMesh = _MaterialCentral.instance.actualPack._SwitchVMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._SwitchVColor;

                        _Hue = _MaterialCentral.instance.actualPack.SwitchV_Hue;
                        _Contrast = _MaterialCentral.instance.actualPack.SwitchV_Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack.SwitchV_Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack.SwitchV_Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._SwitchVTexInside;
                        _InsideColor = _MaterialCentral.instance.actualPack._SwitchVColorInside;
                        _InsideStrength = _MaterialCentral.instance.actualPack._SwitchVInsideStrength;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._SwitchVIdleEmoteTex;
                        _EmoteIdleOffTex = _MaterialCentral.instance.actualPack._SwitchVIdleOffEmoteTex;
                        _EmoteFallTex = _MaterialCentral.instance.actualPack._SwitchVFallEmoteTex;
                        _EmoteFatalFallTex = _MaterialCentral.instance.actualPack._SwitchVFatalFallEmoteTex;
                        _EmoteSelectedTex = _MaterialCentral.instance.actualPack._SwitchVSelectedEmoteTex;
                        _EmotePastilleTex = _MaterialCentral.instance.actualPack._SwitchVPastilleEmoteTex;
                        _EmoteStrength = 1;
                    }
                    break;

                case CubeTypes.CornerStaticCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._CornerTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._CornerMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;

                case CubeTypes.EmptyStaticCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._EmptyTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._EmptyMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.FullStaticCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._FullTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._FullMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.QuadStaticCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._QuadTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._QuadMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.TopStaticCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._TopTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._TopMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;
                case CubeTypes.TripleStaticCube:
                    {
                        _MainTex = _MaterialCentral.instance.actualPack._TripleTex;
                        _MainMesh = _MaterialCentral.instance.actualPack._TripleMesh;
                        _MainColor = _MaterialCentral.instance.actualPack._TextureColor;

                        _Hue = _MaterialCentral.instance.actualPack._Hue;
                        _Contrast = _MaterialCentral.instance.actualPack._Contrast;
                        _Saturation = _MaterialCentral.instance.actualPack._Saturation;
                        _Brightness = _MaterialCentral.instance.actualPack._Brightness;

                        _InsideTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _InsideColor = Color.white;
                        _InsideStrength = 0;

                        _EdgeTex = _MaterialCentral.instance.actualPack._EdgeTex;
                        _EdgeColor = _MaterialCentral.instance.actualPack._EdgeColor;
                        _EdgeStrength = _MaterialCentral.instance.actualPack._EdgeTexStrength;

                        _EmoteIdleTex = _MaterialCentral.instance.actualPack._VoidTex;
                        _EmoteStrength = 0;
                    }
                    break;

            }

            SetMaterial();

        }

        #endregion

        #region FEEDBACK

        public IEnumerator VictoryFX(bool isON)
        {

            meshRenderer.GetPropertyBlock(MatProp);
            currentOfValueChangePOP = 0;

            if (isON)
            {
                actualContrast = _Contrast;
                actualSaturation = _Saturation;

                while (currentOfValueChangePOP <= timeOfValueChange)
                {
                    currentOfValueChangePOP += Time.deltaTime;

                    currentValue = Mathf.SmoothStep(actualContrast, maxValueColor, currentOfValueChangePOP / timeOfValueChange);
                    currentValueSaturation = Mathf.SmoothStep(actualSaturation, maxValueColorSaturation, currentOfValueChangePOP / timeOfValueChange);

                    MatProp.SetFloat("_Contrast", currentValue);
                    MatProp.SetFloat("_Saturation", currentValueSaturation);

                    meshRenderer.SetPropertyBlock(MatProp);
                    yield return currentValue;
                }
            }
            else
            {
                while (currentOfValueChange <= timeOfValueChange)
                {
                    currentOfValueChange += Time.deltaTime;

                    currentValue = Mathf.SmoothStep(maxValueColor, actualContrast, currentOfValueChange / timeOfValueChange);
                    currentValueSaturation = Mathf.SmoothStep(maxValueColorSaturation, actualSaturation, currentOfValueChange / timeOfValueChange);

                    MatProp.SetFloat("_Contrast", currentValue);
                    MatProp.SetFloat("_Saturation", currentValueSaturation);

                    meshRenderer.SetPropertyBlock(MatProp);
                    yield return currentValue;
                }
            }
        }

        virtual public IEnumerator PopOut(bool timer)
        {
            willPOP = true;

            gameObject.GetComponent<Collider>().enabled = false;

            audioSourceCube.clip = _AudioManager.instance.Pop;
            currentOfValueChange = 0;
            baseScale = transform.localScale;
            targetScale = new Vector3(baseScale.x * scaleMul, baseScale.y * scaleMul, baseScale.z * scaleMul);

            grid.kuboGrid[myIndex - 1].cubeOnPosition = null;
            grid.kuboGrid[myIndex - 1].cubeLayers = CubeLayers.cubeEmpty;
            grid.kuboGrid[myIndex - 1].cubeType = CubeTypes.None;

            if (timer == true)
            {
                _DataManager.instance.timers.Remove(this as TimerCube);
            }


            while (currentOfValueChange <= timeOfValueChangePOP)
            {
                currentOfValueChange += Time.deltaTime;

                //actualScale = Vector3.Lerp(baseScale, targetScale, EaseInBack(currentOfValueChange / timeOfValueChange))

                //transform.localScale = actualScale;

                transform.localScale = new Vector3(Mathf.Clamp(baseScale.x + ((baseScale.x * EaseInBack(currentOfValueChange / timeOfValueChangePOP))), 0, targetScale.x),
                    Mathf.Clamp(baseScale.y + ((baseScale.y * EaseInBack(currentOfValueChange / timeOfValueChangePOP))), 0, targetScale.y),
                    Mathf.Clamp(baseScale.z + ((baseScale.z * EaseInBack(currentOfValueChange / timeOfValueChangePOP))), 0, targetScale.z));

                yield return transform.localScale;
            }

            DisableCube();

            PopOutPS = Instantiate(_FeedBackManager.instance.PopOutParticleSystem, transform.position, Quaternion.identity);
            PlaySound();

            willPOP = false;

            yield return new WaitForSeconds(_FeedBackManager.instance.PopOutParticleSystem.main.duration);

            Destroy(PopOutPS.gameObject);
            gameObject.SetActive(false);

        }

        float c1;
        float c3;

        float EaseInBack(float x)
        {
            c1 = 2.8f;
            c3 = c1 + 1;

            return c3 * x * x * x - c1 * x * x;
        }

        public void ChangeEmoteFace(Texture emoteTex)
        {
            meshRenderer.GetPropertyBlock(MatProp);
            MatProp.SetTexture("_Emote", emoteTex);
            meshRenderer.SetPropertyBlock(MatProp);
        }

        #endregion

        public void RemoveCube(int indexToDestroy)
        {

            if (grid.kuboGrid[indexToDestroy - 1].cubeOnPosition != null 
                && grid.kuboGrid[indexToDestroy - 1].cubeType != CubeTypes.DeliveryCube
                /*&& grid.kuboGrid[indexToDestroy - 1].cubeType > CubeTypes.TimerCube1 
                && grid.kuboGrid[indexToDestroy - 1].cubeType < CubeTypes.TimerCube9*/)
            {
                willPOP = true; 
                StartCoroutine(grid.kuboGrid[indexToDestroy - 1].cubeOnPosition.GetComponent<_CubeBase>().PopOut(false));
            }

            /*
            if(grid.kuboGrid[indexToDestroy - 1].cubeOnPosition != null 
                && grid.kuboGrid[indexToDestroy - 1].cubeType <= CubeTypes.TimerCube1
                && grid.kuboGrid[indexToDestroy - 1].cubeType >= CubeTypes.TimerCube9)
            {
                grid.kuboGrid[indexToDestroy - 1].cubeOnPosition.GetComponent<TimerCube>().BombDecrementMe();
            }*/

            else return;
        }

        public void ApplyRigidbody(Vector3 force)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;

            rigidbody.maxAngularVelocity = UnityEngine.Random.Range(10, 80);
            rigidbody.AddTorque(new Vector3(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1)), ForceMode.Impulse);

            rigidbody.velocity = force * 6.0f;
            rigidbody.AddForce(force * 6.0f, ForceMode.Impulse);

        }


        #region AUDIO
        public void StopPlayingSound()
        {
            audioSourceCube.Stop();
        }

        public void PlaySound()
        {
            audioSourceCube.Play();
        }

        #endregion
    }
}