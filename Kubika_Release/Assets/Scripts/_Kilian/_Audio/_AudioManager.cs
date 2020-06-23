using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Kubika.Game
{
    public enum DifferentSound
    {
        SelectUivers,
        SelectPlanete,
        SelectMoon,
        UnlockedLevelSound,
        PlayedLevelSound,
        GoldLevelSound,
        Explosion,
        CantMove,
        Pop,
        Pastille,
        VictoryBase,
        VictoryGold,
        Rotate,
        Bouton,
        SwitchON,
        SwitchOFF,
        PlaceCube,
        Delete,
        RotateLE,
        BoutonLE
    }
    public enum DifferentSceneSound
    {
        WorldMap,
        InGame,
        LevelEditor
    }


    public class _AudioManager : MonoBehaviour
    {
        private static _AudioManager _instance;
        public static _AudioManager instance { get { return _instance; } }

        bool MusicON = true;
        bool SFXON = true;

        public DifferentSceneSound SoundScene;

        [Space]
        [Header("AUDIO MIXER")]
        public AudioMixerGroup audioMixer;
        public AudioMixerGroup audioMixerBGMusic;

        [Space]
        [Header("BG MUSIC")]
        public AudioClip BGMusic;

        [Space]
        [Header("WORLD MAP SOUNDS")]
        public AudioClip SelectUivers;
        public AudioClip SelectPlanete;
        public AudioClip SelectMoon;
        public AudioClip UnlockedLevelSound;
        public AudioClip PlayedLevelSound;
        public AudioClip GoldLevelSound;

        [Space]
        [Header("IN GAME SOUNDS")]
        public AudioClip Explosion;
        public AudioClip EndPush;
        public AudioClip Pop;
        public AudioClip Pastille;
        public AudioClip VictoryBase;
        public AudioClip VictoryGold;
        public AudioClip Rotate;
        public AudioClip Bouton;
        public AudioClip SwitchON;
        public AudioClip SwitchOFF;
        public AudioClip CANTMOVE;
        public AudioClip Selection;
        public AudioClip Move;
        public AudioClip ElevatorGoUp;
        public AudioClip ElevatorGoDoxn;
        public AudioClip FatalChute;

        [Space]
        [Header("LEVEL EDITOR SOUNDS")]
        public AudioClip PlaceCube;
        public AudioClip Delete;
        public AudioClip RotateLE;
        public AudioClip BoutonLE;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else _instance = this;

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TurnMusicOnOff()
        {
            if(MusicON == true)
            {
                MusicON = false;
                audioMixerBGMusic.audioMixer.SetFloat("VolumeMusic", -80);
            }
            else
            {
                MusicON = true;
                audioMixerBGMusic.audioMixer.SetFloat("VolumeMusic", 0);
            }

        }


        public void TurnSFXOnOff()
        {
            if (SFXON == true)
            {
                SFXON = false;
                audioMixer.audioMixer.SetFloat("Volume", -80);
            }
            else
            {
                SFXON = true;
                audioMixer.audioMixer.SetFloat("Volume", 0);
            }
        }
    }
}
