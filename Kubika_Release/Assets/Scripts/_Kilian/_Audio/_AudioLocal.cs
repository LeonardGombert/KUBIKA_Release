using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _AudioLocal : MonoBehaviour
    {
        public AudioSource objectAudioSource;
        public DifferentSound sounds;

        // Start is called before the first frame update
        void Start()
        {
            objectAudioSource = GetComponent<AudioSource>();
            CheckSoundDegeu();
        }

        void CheckWhatSound()
        {
            switch(_AudioManager.instance.SoundScene)
            {
                case DifferentSceneSound.InGame:
                    {
                        switch (sounds)
                        {
                            case DifferentSound.Bouton:
                                objectAudioSource.clip = _AudioManager.instance.Bouton;
                                break;
                            case DifferentSound.Explosion:
                                objectAudioSource.clip = _AudioManager.instance.Explosion;
                                break;
                            case DifferentSound.Pastille:
                                objectAudioSource.clip = _AudioManager.instance.Pastille;
                                break;
                            case DifferentSound.Pop:
                                objectAudioSource.clip = _AudioManager.instance.Pop;
                                break;
                            case DifferentSound.Rotate:
                                objectAudioSource.clip = _AudioManager.instance.Rotate;
                                break;
                            case DifferentSound.SwitchOFF:
                                objectAudioSource.clip = _AudioManager.instance.SwitchOFF;
                                break;
                            case DifferentSound.SwitchON:
                                objectAudioSource.clip = _AudioManager.instance.SwitchON;
                                break;
                            case DifferentSound.VictoryBase:
                                objectAudioSource.clip = _AudioManager.instance.VictoryBase;
                                break;
                            case DifferentSound.VictoryGold:
                                objectAudioSource.clip = _AudioManager.instance.VictoryGold;
                                break;
                            case DifferentSound.CantMove:
                                objectAudioSource.clip = _AudioManager.instance.CANTMOVE;
                                break;
                        }
                    }
                    break;
                case DifferentSceneSound.WorldMap:
                    {
                        switch (sounds)
                        {

                            case DifferentSound.GoldLevelSound:
                                objectAudioSource.clip = _AudioManager.instance.GoldLevelSound;
                                break;
                            case DifferentSound.PlayedLevelSound:
                                objectAudioSource.clip = _AudioManager.instance.PlayedLevelSound;
                                break;
                            case DifferentSound.SelectMoon:
                                objectAudioSource.clip = _AudioManager.instance.SelectMoon;
                                break;
                            case DifferentSound.SelectPlanete:
                                objectAudioSource.clip = _AudioManager.instance.SelectPlanete;
                                break;
                            case DifferentSound.SelectUivers:
                                objectAudioSource.clip = _AudioManager.instance.SelectUivers;
                                break;
                            case DifferentSound.UnlockedLevelSound:
                                objectAudioSource.clip = _AudioManager.instance.UnlockedLevelSound;
                                break;
                        }
                    }
                    break;
                case DifferentSceneSound.LevelEditor:
                    {
                        switch (sounds)
                        {
                            case DifferentSound.BoutonLE:
                                objectAudioSource.clip = _AudioManager.instance.BoutonLE;
                                break;
                            case DifferentSound.Delete:
                                objectAudioSource.clip = _AudioManager.instance.Delete;
                                break;
                            case DifferentSound.PlaceCube:
                                objectAudioSource.clip = _AudioManager.instance.PlaceCube;
                                break;
                            case DifferentSound.RotateLE:
                                objectAudioSource.clip = _AudioManager.instance.RotateLE;
                                break;
                        }
                    }
                    break;
            }

        }

        void CheckSoundDegeu()
        {
            switch (sounds)
            {
                case DifferentSound.Bouton:
                    objectAudioSource.clip = _AudioManager.instance.Bouton;
                    break;
                case DifferentSound.Explosion:
                    objectAudioSource.clip = _AudioManager.instance.Explosion;
                    break;
                case DifferentSound.Pastille:
                    objectAudioSource.clip = _AudioManager.instance.Pastille;
                    break;
                case DifferentSound.Pop:
                    objectAudioSource.clip = _AudioManager.instance.Pop;
                    break;
                case DifferentSound.Rotate:
                    objectAudioSource.clip = _AudioManager.instance.Rotate;
                    break;
                case DifferentSound.SwitchOFF:
                    objectAudioSource.clip = _AudioManager.instance.SwitchOFF;
                    break;
                case DifferentSound.SwitchON:
                    objectAudioSource.clip = _AudioManager.instance.SwitchON;
                    break;
                case DifferentSound.VictoryBase:
                    objectAudioSource.clip = _AudioManager.instance.VictoryBase;
                    break;
                case DifferentSound.VictoryGold:
                    objectAudioSource.clip = _AudioManager.instance.VictoryGold;
                    break;
                case DifferentSound.CantMove:
                    objectAudioSource.clip = _AudioManager.instance.CANTMOVE;
                    break;
                case DifferentSound.BoutonLE:
                    objectAudioSource.clip = _AudioManager.instance.BoutonLE;
                    break;
                case DifferentSound.Delete:
                    objectAudioSource.clip = _AudioManager.instance.Delete;
                    break;
                case DifferentSound.PlaceCube:
                    objectAudioSource.clip = _AudioManager.instance.PlaceCube;
                    break;
                case DifferentSound.RotateLE:
                    objectAudioSource.clip = _AudioManager.instance.RotateLE;
                    break;
                case DifferentSound.GoldLevelSound:
                    objectAudioSource.clip = _AudioManager.instance.GoldLevelSound;
                    break;
                case DifferentSound.PlayedLevelSound:
                    objectAudioSource.clip = _AudioManager.instance.PlayedLevelSound;
                    break;
                case DifferentSound.SelectMoon:
                    objectAudioSource.clip = _AudioManager.instance.SelectMoon;
                    break;
                case DifferentSound.SelectPlanete:
                    objectAudioSource.clip = _AudioManager.instance.SelectPlanete;
                    break;
                case DifferentSound.SelectUivers:
                    objectAudioSource.clip = _AudioManager.instance.SelectUivers;
                    break;
                case DifferentSound.UnlockedLevelSound:
                    objectAudioSource.clip = _AudioManager.instance.UnlockedLevelSound;
                    break;
            }
        }

        public void PlaySound()
        {
            objectAudioSource.Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
