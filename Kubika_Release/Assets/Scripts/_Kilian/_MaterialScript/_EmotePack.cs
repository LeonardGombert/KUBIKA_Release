using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kubika.Game
{

    [CreateAssetMenu(fileName = "_EmotePack", menuName = "_MaterialPacks/_EmotePack", order = 2)]
    public class _EmotePack : ScriptableObject
    {
        [Header("Base")]
        public Texture _BaseIdleEmoteTex;
        public Texture _BaseFallEmoteTex;
        public Texture _BaseFatalFallEmoteTex;
        public Texture _BaseSelectedEmoteTex;

        [Header("Beton")]
        public Texture _BetonIdleEmoteTex;
        public Texture _BetonFallEmoteTex;
        public Texture _BetonFatalFallEmoteTex;
        public Texture _BetonSelectedEmoteTex;

        [Header("Bomb")]
        public Texture _BombIdleEmoteTex;
        public Texture _BombFallEmoteTex;
        public Texture _BombFatalFallEmoteTex;
        public Texture _BombSelectedEmoteTex;

        [Header("Switch")]
        public Texture _SwitchIdleEmoteTex;
        public Texture _SwitchIdleOffEmoteTex;
        public Texture _SwitchFallEmoteTex;
        public Texture _SwitchFatalFallEmoteTex;
        public Texture _SwitchSelectedEmoteTex;

        [Header("Ball")]
        public Texture _BallIdleEmoteTex;
        public Texture _BallFallEmoteTex;
        public Texture _BallFatalFallEmoteTex;
        public Texture _BallSelectedEmoteTex;

        [Header("BaseV")]
        public Texture _BaseVIdleEmoteTex;
        public Texture _BaseVFallEmoteTex;
        public Texture _BaseVFatalFallEmoteTex;
        public Texture _BaseVSelectedEmoteTex;
        public Texture _BaseVPastilleEmoteTex;


        [Header("BetonV")]
        public Texture _BetonVIdleEmoteTex;
        public Texture _BetonVFallEmoteTex;
        public Texture _BetonVFatalFallEmoteTex;
        public Texture _BetonVSelectedEmoteTex;
        public Texture _BetonVPastilleEmoteTex;

        [Header("BombV")]
        public Texture _BombVIdleEmoteTex;
        public Texture _BombVFallEmoteTex;
        public Texture _BombVFatalFallEmoteTex;
        public Texture _BombVSelectedEmoteTex;
        public Texture _BombVPastilleEmoteTex;

        [Header("SwitchV")]
        public Texture _SwitchVIdleEmoteTex;
        public Texture _SwitchVIdleOffEmoteTex;
        public Texture _SwitchVFallEmoteTex;
        public Texture _SwitchVFatalFallEmoteTex;
        public Texture _SwitchVSelectedEmoteTex;
        public Texture _SwitchVPastilleEmoteTex;

        [Header("BallV")]
        public Texture _BallVIdleEmoteTex;
        public Texture _BallVFallEmoteTex;
        public Texture _BallVFatalFallEmoteTex;
        public Texture _BallVSelectedEmoteTex;
        public Texture _BallVPastilleEmoteTex;

    }
}
