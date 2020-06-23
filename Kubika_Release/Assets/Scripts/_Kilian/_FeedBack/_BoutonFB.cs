using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public enum BoutonTypes
    {
        Left,
        Right,
        UI,
        Switch
    }

    public class _BoutonFB : MonoBehaviour
    {
        public BoutonTypes boutonType;

        [Space]
        [Header("MATERIAL INFOS SOCLE")]
        public Texture _MainTex;
        public Mesh _MainMesh;
        public Color _MainColor;

        [Range(-360, 360)] public float _Hue;
        [Range(0, 2)] public float _Contrast;
        [Range(0, 2)] public float _Saturation;
        [Range(-1, 1)] public float _Brightness;

        [Space]
        [Header("MATERIAL INFOS BOUTON")]
        public Texture _MainTexBouton;
        public Mesh _MainMeshBouton;
        public Color _MainColorBouton;

        public Texture _EmoteTexBouton;
        public float _EmoteStrengthBouton;

        public Texture _InsideTexBouton;
        public Color _InsideColorBouton;
        public float _InsideStrengthBouton;

        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public MeshFilter meshFilter;
        [HideInInspector] public MaterialPropertyBlock MatProp; // To change Mat Properties

        public MeshRenderer meshRendererCHILD;
        public MeshFilter meshFilterCHILD;
        [HideInInspector] public MaterialPropertyBlock MatPropCHILD; // To change Mat Properties

        // Start is called before the first frame update
        void Start()
        {
            MatProp = new MaterialPropertyBlock();
            MatPropCHILD = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            SetScriptablePreset();
            SetMaterialBOUTON();
            SetMaterialSOCLE();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetMaterialBOUTON()
        {
            meshRendererCHILD.GetPropertyBlock(MatPropCHILD);

            meshFilterCHILD.mesh = _MainMeshBouton;

            MatPropCHILD.SetTexture("_MainTex", _MainTexBouton);
            MatPropCHILD.SetTexture("_InsideTex", _InsideTexBouton);
            MatPropCHILD.SetTexture("_Emote", _EmoteTexBouton);

            MatPropCHILD.SetColor("_MainColor", _MainColorBouton);
            MatPropCHILD.SetColor("_InsideColor", _InsideColorBouton);

            MatPropCHILD.SetFloat("_InsideTexStrength", _InsideStrengthBouton);
            MatPropCHILD.SetFloat("_EmoteStrength", _EmoteStrengthBouton);

            MatPropCHILD.SetFloat("_Hue", _Hue);
            MatPropCHILD.SetFloat("_Contrast", _Contrast);
            MatPropCHILD.SetFloat("_Saturation", _Saturation);
            MatPropCHILD.SetFloat("_Brightness", _Brightness);

            meshRendererCHILD.SetPropertyBlock(MatPropCHILD);
        }

        public void SetMaterialSOCLE()
        {
            meshRenderer.GetPropertyBlock(MatProp);

            meshFilter.mesh = _MainMesh;

            MatProp.SetTexture("_MainTex", _MainTex);
            MatProp.SetColor("_MainColor", _MainColor);

            MatProp.SetFloat("_Hue", _Hue);
            MatProp.SetFloat("_Contrast", _Contrast);
            MatProp.SetFloat("_Saturation", _Saturation);
            MatProp.SetFloat("_Brightness", _Brightness);

            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void SetScriptablePreset()
        {
            switch(boutonType)
            {
                case BoutonTypes.Left:
                    _MainTex = _MaterialCentral.instance.actualPack._RotatorsTexSocle;
                    _MainMesh = _MaterialCentral.instance.actualPack._RotatorsMeshSocle;
                    _MainColor = _MaterialCentral.instance.actualPack._RotatorsColorSocle;
                    _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                    _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                    _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                    _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;

                    _MainTexBouton = _MaterialCentral.instance.actualPack._RotatorsTexLeft;
                    _MainMeshBouton = _MaterialCentral.instance.actualPack._RotatorsMeshLeft;
                    _MainColorBouton = _MaterialCentral.instance.actualPack._RotatorsColorLeft;

                    _EmoteTexBouton = _MaterialCentral.instance.actualPack._VoidTex;
                    _EmoteStrengthBouton = 0;

                    _InsideTexBouton = _MaterialCentral.instance.actualPack._RotatorsTexInsideLeft;
                    _InsideColorBouton = _MaterialCentral.instance.actualPack._RotatorsColorInsideLeft;
                    _InsideStrengthBouton = _MaterialCentral.instance.actualPack._RotatorsInsideStrengthLeft;

                    break;
                case BoutonTypes.Right:
                    _MainTex = _MaterialCentral.instance.actualPack._RotatorsTexSocle;
                    _MainMesh = _MaterialCentral.instance.actualPack._RotatorsMeshSocle;
                    _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                    _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                    _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                    _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;
                    _MainColor = _MaterialCentral.instance.actualPack._RotatorsColorSocle;

                    _MainTexBouton = _MaterialCentral.instance.actualPack._RotatorsTexRight;
                    _MainMeshBouton = _MaterialCentral.instance.actualPack._RotatorsMeshRight;
                    _MainColorBouton = _MaterialCentral.instance.actualPack._RotatorsColorRight;

                    _EmoteTexBouton = _MaterialCentral.instance.actualPack._VoidTex;
                    _EmoteStrengthBouton = 0;

                    _InsideTexBouton = _MaterialCentral.instance.actualPack._RotatorsTexInsideRight;
                    _InsideColorBouton = _MaterialCentral.instance.actualPack._RotatorsColorInsideRight;
                    _InsideStrengthBouton = _MaterialCentral.instance.actualPack._RotatorsInsideStrengthRight;
                    break;
                case BoutonTypes.UI:
                    _MainTex = _MaterialCentral.instance.actualPack._RotatorsTexSocle;
                    _MainMesh = _MaterialCentral.instance.actualPack._RotatorsMeshSocle;
                    _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                    _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                    _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                    _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;
                    _MainColor = _MaterialCentral.instance.actualPack._RotatorsColorSocle;

                    _MainTexBouton = _MaterialCentral.instance.actualPack._RotatorsTexUI;
                    _MainMeshBouton = _MaterialCentral.instance.actualPack._RotatorsMeshUI;
                    _MainColorBouton = _MaterialCentral.instance.actualPack._RotatorsColorUI;

                    _EmoteTexBouton = _MaterialCentral.instance.actualPack._VoidTex;
                    _EmoteStrengthBouton = 0;

                    _InsideTexBouton = _MaterialCentral.instance.actualPack._RotatorsTexInsideUI;
                    _InsideColorBouton = _MaterialCentral.instance.actualPack._RotatorsColorInsideUI;
                    _InsideStrengthBouton = _MaterialCentral.instance.actualPack._RotatorsInsideStrengthUI;
                    break;
                case BoutonTypes.Switch:
                    _MainTex = _MaterialCentral.instance.actualPack._RotatorsTexSocle;
                    _MainMesh = _MaterialCentral.instance.actualPack._RotatorsMeshSocle;
                    _Hue = _MaterialCentral.instance.actualPack.Rotators_Hue;
                    _Contrast = _MaterialCentral.instance.actualPack.Rotators_Contrast;
                    _Saturation = _MaterialCentral.instance.actualPack.Rotators_Saturation;
                    _Brightness = _MaterialCentral.instance.actualPack.Rotators_Brightness;
                    _MainColor = _MaterialCentral.instance.actualPack._RotatorsColorSocle;

                    _MainTexBouton = _MaterialCentral.instance.actualPack._SwitchTexOffButton;
                    _MainMeshBouton = _MaterialCentral.instance.actualPack._SwitchMeshButton;
                    _MainColorBouton = _MaterialCentral.instance.actualPack._SwitchColorButton;

                    _EmoteTexBouton = _MaterialCentral.instance.actualPack._VoidTex;
                    _EmoteStrengthBouton = 0;

                    _InsideTexBouton = _MaterialCentral.instance.actualPack._SwitchTexInsideButton;
                    _InsideColorBouton = _MaterialCentral.instance.actualPack._SwitchColorInsideButton;
                    _InsideStrengthBouton = _MaterialCentral.instance.actualPack._SwitchInsideStrengthButton;
                    break;
            }
        }
    }
}
