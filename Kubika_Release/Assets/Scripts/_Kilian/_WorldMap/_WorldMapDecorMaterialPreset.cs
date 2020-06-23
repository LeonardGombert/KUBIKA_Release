using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    [ExecuteInEditMode]
    public class _WorldMapDecorMaterialPreset : MonoBehaviour
    {
        [Space]
        [Header("MATERIAL INFOS")]
        public Texture _MainTex;
        public Color _MainColor;

        [Range(-360, 360)] public float _Hue;
        [Range(0, 2)] public float _Contrast;
        [Range(0, 2)] public float _Saturation;
        [Range(-1, 1)] public float _Brightness;

        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public MeshFilter meshFilter;
        [HideInInspector] public MaterialPropertyBlock MatProp; // To change Mat Properties


        // Start is called before the first frame update
        void Start()
        {
            SetMaterial();
        }

        // Update is called once per frame
        void Update()
        {
            SetMaterial();
        }

        public void SetMaterial()
        {
            MatProp = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            meshRenderer.GetPropertyBlock(MatProp);

            MatProp.SetTexture("_MainTex", _MainTex);
            MatProp.SetColor("_MainColor", _MainColor);

            MatProp.SetFloat("_Hue", _Hue);
            MatProp.SetFloat("_Contrast", _Contrast);
            MatProp.SetFloat("_Saturation", _Saturation);
            MatProp.SetFloat("_Brightness", _Brightness);

            meshRenderer.SetPropertyBlock(MatProp);
        }
    }
}
