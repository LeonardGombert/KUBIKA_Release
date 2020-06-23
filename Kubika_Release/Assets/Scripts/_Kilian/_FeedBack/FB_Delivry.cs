using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class FB_Delivry : MonoBehaviour
    {
        public ParticleSystem PS;
        public ParticleSystem END_BASEExplosion;
        public Transform pivotPS;
        public GameObject LightShaft;

        public void ActivatePSFB()
        {
            PS.Play();
            GetComponentInParent<DeliveryCube>().ExplosionEND_PS = END_BASEExplosion;
            GetComponentInParent<DeliveryCube>().LightShaftTrue = LightShaft;
        }

    }
}
