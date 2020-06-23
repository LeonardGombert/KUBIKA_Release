using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    [ExecuteInEditMode]
    public class _DecorMaterialPreset : MonoBehaviour
    {

        [Space]
        [Header("MATERIAL INFOS")]
        public bool randomizeRotate;
        public Texture _MainTex;
        public Texture _MainTexNormal;
        public Texture _MainTexVariation;

        [Space]
        public Color _ColorMin;
        public Color _ColorMax; 
        Color _Color; 
        public float numberOfVariations;
        float actualColorChoice;

        [Space]
        public float _WindSpeed;
        public float _WindMouv;
        public float _WindOffset;
        public float _WindStrength;

        Vector3 startRotation;


        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public MeshFilter meshFilter;
        [HideInInspector] public MaterialPropertyBlock MatProp; // To change Mat Properties

        // Start is called before the first frame update
        void Start()
        {
            if (randomizeRotate)
            {
                startRotation = transform.localEulerAngles;
                startRotation.y = Random.Range(1, 360);
                transform.localEulerAngles = startRotation;
            }


            SetMaterialRandomValues();

            SetMaterial();
        }

        void Update()
        {
            SetMaterial();
        }

        void SetMaterialRandomValues()
        {
            actualColorChoice = Random.Range(1, numberOfVariations);
            _Color = Color.Lerp(_ColorMin, _ColorMax, actualColorChoice / numberOfVariations);

            if(Random.Range(1,5) == 3)
            {
                _MainTex = _MainTexVariation;
            }
            else
            {
                _MainTex = _MainTexNormal;
            }
        }

        public void SetMaterial()
        {
            MatProp = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            meshRenderer.GetPropertyBlock(MatProp);

            MatProp.SetTexture("_MainTex", _MainTex);
            MatProp.SetColor("_Color", _Color);

            MatProp.SetFloat("_WindSpeed", _WindSpeed);
            MatProp.SetFloat("_WindMouv", _WindMouv);
            MatProp.SetFloat("_WindOffset", _WindOffset);
            MatProp.SetFloat("_WindStrength", _WindStrength);


            meshRenderer.SetPropertyBlock(MatProp);
        }
    }
}
