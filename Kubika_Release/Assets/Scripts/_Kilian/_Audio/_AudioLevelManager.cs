using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kubika.Game;

namespace Kubika.CustomLevelEditor
{

    public class _AudioLevelManager : MonoBehaviour
    {
        private static _AudioLevelManager _instance;
        public static _AudioLevelManager instance { get { return _instance; } }
        // Start is called before the first frame update

        public AudioSource objectAudioSourceADD;
        public AudioSource objectAudioSourceDelete;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

        }

        void Start()
        {
            objectAudioSourceADD.clip = _AudioManager.instance.PlaceCube;
            objectAudioSourceDelete.clip = _AudioManager.instance.Delete;
        }

        public void PlaySoundAdd()
        {
            objectAudioSourceADD.Play();
        }

        public void PlaySoundDelete()
        {
            objectAudioSourceDelete.Play();
        }

    }
}
