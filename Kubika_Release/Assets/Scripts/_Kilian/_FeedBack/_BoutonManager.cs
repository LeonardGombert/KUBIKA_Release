using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _BoutonManager : MonoBehaviour
    {
        public GameObject SwitchButton;
        public GameObject RotatorLeftButton;
        public GameObject RotatorRightButton;
        public GameObject RotatorUIButton;

        private static _BoutonManager _instance;
        public static _BoutonManager instance { get { return _instance; } }

        private void Start()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;
        }

    }
}
