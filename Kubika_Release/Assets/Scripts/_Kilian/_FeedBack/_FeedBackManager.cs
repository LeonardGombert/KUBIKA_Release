using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _FeedBackManager : MonoBehaviour
    {
        private static _FeedBackManager _instance;
        public static _FeedBackManager instance { get { return _instance; } }

        public GameObject Fb_Delivry;
        [SerializeField] private AudioSource audioSourceFbMana;

        //EXPLOSION FX
        [Space]
        [Header("EXPLOSION FX")]

        public ParticleSystem MineExplosionBase;
        public ParticleSystem MineExplosionVictory;
        public ParticleSystem PopOutParticleSystem;

        [Space]
        [Header("VICTORY FX")]
        public ParticleSystem Victory_PS_BASE_CONFETTI;
        public ParticleSystem Victory_PS_GOLD_CONFETTI;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void PlayVictoryFX()
        {
            if(_DataManager.instance.isGolded == true)
            {
                audioSourceFbMana.clip = _AudioManager.instance.VictoryGold;
                Victory_PS_GOLD_CONFETTI.Play();
            }
            else
            {
                audioSourceFbMana.clip = _AudioManager.instance.VictoryBase;
                Victory_PS_BASE_CONFETTI.Play();
            }

            audioSourceFbMana.Play();
        }

        public void EveryCubeHappy()
        {
            foreach(_CubeMove cube in _DataManager.instance.moveCube)
            {
                cube.ChangeEmoteFace(cube._EmotePastilleTex);
            }
        }

        public void ResetVictoryFX()
        {
            Victory_PS_BASE_CONFETTI.Clear();
            Victory_PS_GOLD_CONFETTI.Clear();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
