using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _LightManager : MonoBehaviour
    {
        public Material[] cubeMasterMaterial;
        public float _LightIntensity;
        public Color _LightColor;
        public Color _AmbientColor;
        public Vector4 _LightDirection;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SetMaterialPReset();
        }

        void SetMaterialPReset()
        {
            foreach(Material mat in cubeMasterMaterial)
            {
                mat.SetColor("_LightColor", _LightColor);
                mat.SetColor("_AmbientColor", _AmbientColor);
                mat.SetFloat("_LightIntensity", _LightIntensity);
                mat.SetVector("_LightDirection", _LightDirection);
            }
        }
    }
}
