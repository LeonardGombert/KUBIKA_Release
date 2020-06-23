using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _AudioPasserelle : MonoBehaviour
    {
        public void AudioManagerTurnMusic()
        {
            _AudioManager.instance.TurnMusicOnOff();
        }

        public void AudioManagerTurnSFX()
        {
            _AudioManager.instance.TurnSFXOnOff();
        }
    }
}
