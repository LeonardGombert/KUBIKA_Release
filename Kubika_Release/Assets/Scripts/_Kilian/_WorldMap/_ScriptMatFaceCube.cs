using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _ScriptMatFaceCube : MonoBehaviour
    {
        public _StaticPack staticPack;
        MaterialPropertyBlock MatProp;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        [Space]
        public Color outlineGold = Color.yellow;
        public float saturation;
        float DESATURATION_BASE_VALUE;
        float DESATURATION_TARGET_VALUE;
        float DESATURATION_CURRENT_VALUE;

        [Space]
        public float distanceInGround;
        float moveSpeed = 0.5f;
        public float currentTime;
        Vector3 basePosition;
        Vector3 nextPosition;
        Vector3 currentPosition;

        [Space]
        public bool isMoving = false;
        public bool isLocked = true;
        public bool isUnlocked = false;
        public bool isSelected = false;
        public bool isPlayed = false;
        public bool isGold = false;

        [Space]
        public Texture _MainTex;
        public Mesh _MainMesh;
        public Color _MainColor;
        public float _Hue;
        public float _Contrast;
        public float _Saturation;
        public float _Brightness;

        [Space]
        public bool boolDoneOnce1 = false;
        public bool boolDoneOnce2 = false;
        public bool boolDoneOnce3 = false;
        public bool boolDoneOnce4 = false;

        [Space]
        public Texture _EdgeTex;
        public float _EdgeStrength;
        public Color _EdgeColor;

        // UNLOCKED RING
        public SpriteRenderer ring;

        // Start is called before the first frame update
        void Start()
        {
            MatProp = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            basePosition = nextPosition = transform.localPosition;
            basePosition.y += distanceInGround;

            transform.localPosition = basePosition;

            SetScriptablePreset();
            SetBaseContraste();

        }

        // Update is called once per frame
        void Update()
        {

            if (boolDoneOnce1 == false && isUnlocked == true)
            {
                StartCoroutine(Unlocking());
                boolDoneOnce1 = true;
            }
            if (boolDoneOnce2 == false && isPlayed == true)
            {
                HasBeenPlayed();
                boolDoneOnce2 = true;
            }
            if (boolDoneOnce3 == false && isGold == true)
            {
                HasBeenGold();
                boolDoneOnce3 = true;
            }
            if (boolDoneOnce4 == false && isSelected == true)
            {
                meshRenderer.GetPropertyBlock(MatProp);
                MatProp.SetFloat("_Outline", 0.1f);
                meshRenderer.SetPropertyBlock(MatProp);
                boolDoneOnce4 = true;
            }
            if (isLocked == true)
            {
                DebugConstrast();
            }
        }      
        public IEnumerator Unlocking()
        {
            meshRenderer.GetPropertyBlock(MatProp);
            currentTime = 0;



            while (currentTime <= 1)
            {

                currentTime += Time.deltaTime;
                currentTime = (currentTime / moveSpeed);

                DESATURATION_CURRENT_VALUE = Mathf.Lerp(DESATURATION_BASE_VALUE, DESATURATION_TARGET_VALUE, currentTime);
                currentPosition = Vector3.Lerp(basePosition, nextPosition, currentTime);

                transform.localPosition = currentPosition;

                MatProp.SetFloat("_Saturation", DESATURATION_CURRENT_VALUE);
                meshRenderer.SetPropertyBlock(MatProp);

                yield return transform.localPosition;
                yield return DESATURATION_CURRENT_VALUE;
            }

            //ring.enabled = true;
            meshRenderer.SetPropertyBlock(MatProp);
        }

        void DebugConstrast()
        {
            meshRenderer.GetPropertyBlock(MatProp);

            MatProp.SetFloat("_Saturation", _Saturation);
            meshRenderer.SetPropertyBlock(MatProp);
        }

        void SetBaseContraste()
        {
            meshRenderer.GetPropertyBlock(MatProp);

            DESATURATION_TARGET_VALUE = _Saturation;
            DESATURATION_BASE_VALUE = saturation;

            MatProp.SetFloat("_Saturation", DESATURATION_BASE_VALUE);
            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void HasBeenPlayed()
        {
            meshRenderer.GetPropertyBlock(MatProp);
            ring.enabled = false;
            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void HasBeenGold()
        {
            meshRenderer.GetPropertyBlock(MatProp);
            ring.enabled = false;
            MatProp.SetColor("_ColorOutline", outlineGold);
            MatProp.SetFloat("_Outline", 0.1f);
            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void IsNotSelectedAnymore()
        {
            meshRenderer.GetPropertyBlock(MatProp);
            MatProp.SetFloat("_Outline", 0);
            meshRenderer.SetPropertyBlock(MatProp);
            isSelected = false;
            boolDoneOnce4 = false;
        }


        #region MATERIAL

        public void SetMaterial()
        {

            meshRenderer.GetPropertyBlock(MatProp);

            meshFilter.mesh = _MainMesh;

            MatProp.SetTexture("_MainTex", _MainTex);

            MatProp.SetColor("_MainColor", _MainColor);

            MatProp.SetFloat("_Hue", _Hue);
            MatProp.SetFloat("_Contrast", _Contrast);
            MatProp.SetFloat("_Saturation", _Saturation);
            MatProp.SetFloat("_Brightness", _Brightness);

            MatProp.SetTexture("_EdgeTex", _EdgeTex);
            MatProp.SetColor("_EdgeColor", _EdgeColor);
            MatProp.SetFloat("_EdgeTexStrength", _EdgeStrength);

            meshRenderer.SetPropertyBlock(MatProp);
        }

        public void SetScriptablePreset()
        {

            _MainTex = staticPack._TopTex;
            _MainMesh = staticPack._TopMesh;
            _MainColor = staticPack._TextureColor;

            _Hue = staticPack._Hue;
            _Contrast = staticPack._Contrast;
            _Saturation = staticPack._Saturation;
            _Brightness = staticPack._Brightness;

            SetMaterial();

        }

        #endregion

    }

}
