namespace Kubika.Game
{
    public class BaseVictoryCube : _CubeMove
    {
        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }
    }
}