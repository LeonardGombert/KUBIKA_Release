using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.CustomLevelEditor
{
    public class _FeedBackManagerLevelEditor : MonoBehaviour
    {
        private static _FeedBackManagerLevelEditor _instance;
        public static _FeedBackManagerLevelEditor instance { get { return _instance; } }

        public ParticleSystem Placing_FB;
        public ParticleSystem Deleting_FB;

        void Start()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;
        }
    }
}
