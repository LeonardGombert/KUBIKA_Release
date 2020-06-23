using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _BackgroundMaterialPreset : MonoBehaviour
    {

        private static _BackgroundMaterialPreset _instance;
        public static _BackgroundMaterialPreset instance { get { return _instance; } }

        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public MeshFilter meshFilter;
        [HideInInspector] public MaterialPropertyBlock MatProp; // To change Mat Properties

        [Space]
        [Header("MATERIAL INFOS")]
        public Texture _MainTex;
        public Texture _RadialTexture;

        [Range(0,1)]public float _CutOff;

        [Range(-360, 360)] public float _Hue;
        [Range(0, 2)] public float _Contrast;
        [Range(0, 2)] public float _Saturation;
        [Range(-1, 1)] public float _Brightness;

        [Space]
        [Range(-360, 360)] public float _Hue2;
        [Range(0, 2)] public float _Contrast2;
        [Range(0, 2)] public float _Saturation2;
        [Range(-1, 1)] public float _Brightness2;

        public Transform BG_FX_TRANS;
        ParticleSystem BG_FX;

        [Space]
        public AnimationCurve BG_FX_Behavior;
        public float moveTime;
        float currentValue;
        float lerpValue;
        bool CoroutineAlreadyStarted = false;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;

        }

        // Start is called before the first frame update
        void Start()
        {
            MatProp = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            SetScriptablePreset();
        }

        // Update is called once per frame
        void Update()
        {
            //SetMaterial();
        }

        public void SetMaterial()
        {
            MatProp = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            meshRenderer.GetPropertyBlock(MatProp);

            MatProp.SetTexture("_MainTex", _MainTex);
            MatProp.SetTexture("_RadialTexture", _RadialTexture);

            MatProp.SetFloat("_CutOff", _CutOff);
            
            MatProp.SetFloat("_Hue", _Hue);
            MatProp.SetFloat("_Contrast", _Contrast);
            MatProp.SetFloat("_Saturation", _Saturation);
            MatProp.SetFloat("_Brightness", _Brightness);

            MatProp.SetFloat("_Hue2", _Hue2);
            MatProp.SetFloat("_Contrast2", _Contrast2);
            MatProp.SetFloat("_Saturation2", _Saturation2);
            MatProp.SetFloat("_Brightness2", _Brightness2);

            if(BG_FX != null)
                Destroy(BG_FX.gameObject);

            BG_FX = Instantiate(_MaterialCentral.instance.actualPack._BGFX, BG_FX_TRANS.transform.position, BG_FX_TRANS.rotation, BG_FX_TRANS.transform);

            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void SetScriptablePreset()
        {
            _MainTex = _MaterialCentral.instance.actualPack._BGTex;
            _Hue2 = _MaterialCentral.instance.actualPack._HueBG;
            _Contrast2 = _MaterialCentral.instance.actualPack._ContrastBG;
            _Saturation2 = _MaterialCentral.instance.actualPack._SaturationBG;
            _Brightness2 = _MaterialCentral.instance.actualPack._BrightnessBG;

            SetMaterial();
        }

        public IEnumerator FallBGFeedback()
        {
            if (CoroutineAlreadyStarted == false)
            {
                CoroutineAlreadyStarted = true;
                currentValue = lerpValue = 0;
                meshRenderer.GetPropertyBlock(MatProp);

                while (currentValue < 1)
                {
                    lerpValue += Time.deltaTime;
                    currentValue = lerpValue / moveTime;

                    _CutOff = (Mathf.Lerp(0, 1, BG_FX_Behavior.Evaluate(currentValue)));

                    MatProp.SetFloat("_CutOff", _CutOff);
                    meshRenderer.SetPropertyBlock(MatProp);

                    yield return _CutOff;
                }

                CoroutineAlreadyStarted = false;
            }

        }
    }
}
