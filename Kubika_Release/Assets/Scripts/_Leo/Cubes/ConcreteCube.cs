using UnityEngine;

namespace Kubika.Game
{
    public class ConcreteCube : _CubeMove
    {
        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();

            //starts as a static cube
            isSelectable = false;
            SetOutlineColor();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        void SetOutlineColor()
        {
            meshRenderer.GetPropertyBlock(MatProp);
            MatProp.SetColor("_ColorOutline", Color.black);
            meshRenderer.SetPropertyBlock(MatProp);
        }
    }
}