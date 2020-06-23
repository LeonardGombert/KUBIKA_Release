using Kubika.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class SwitchVictoryCube : SwitchCube
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            isSwitchVictory = true;
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }
    }
}