using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kubika.Game
{

    [CreateAssetMenu(fileName = "_StaticPack", menuName = "_MaterialPacks/_StaticPack", order = 1)]
    public class _StaticPack : ScriptableObject
    {
        [Header("Empty")]
        public Texture _EmptyTex;
        public Texture _EmptyTex2;
        public Mesh _EmptyMesh;

        [Header("Full")]
        public Texture _FullTex;
        public Texture _FullTex2;
        public Mesh _FullMesh;

        [Header("Top")]
        public Texture _TopTex;
        public Texture _TopTex2;
        public Mesh _TopMesh;

        [Header("Corner")]
        public Texture _CornerTex;
        public Texture _CornerTex2;
        public Mesh _CornerMesh;

        [Header("Triple")]
        public Texture _TripleTex;
        public Texture _TripleTex2;
        public Mesh _TripleMesh;

        [Header("Quad")]
        public Texture _QuadTex;
        public Texture _QuadTex2;
        public Mesh _QuadMesh;

        [Header("Color Parameters")]
        public Color _TextureColor;
        [Range(-360, 360)] public float _Hue;
        [Range(0, 2)] public float _Contrast;
        [Range(0, 2)] public float _Saturation;
        [Range(-1, 1)] public float _Brightness;

        [Header("Background")]
        public Texture _BGTex;
        public ParticleSystem _BGFX;

        [Header("BG Parameters")]
        [Range(-360, 360)] public float _HueBG;
        [Range(0, 2)] public float _ContrastBG;
        [Range(0, 2)] public float _SaturationBG;
        [Range(-1, 1)] public float _BrightnessBG;


    }
}
