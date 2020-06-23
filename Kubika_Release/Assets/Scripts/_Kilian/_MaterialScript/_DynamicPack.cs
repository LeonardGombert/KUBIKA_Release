using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kubika.Game
{

    [CreateAssetMenu(fileName = "_DynamicPack", menuName = "_MaterialPacks/_DynamicPack", order = 2)]
    public class _DynamicPack : ScriptableObject
    {
        [Header("Base")]
        public Texture _BaseTex;
        public Mesh _BaseMesh;
        public Color _BaseColor;
        public Texture _BaseTexInside;
        public float _BaseInsideStrength;
        public Color _BaseColorInside;
        [Range(-360, 360)] public float Base_Hue;
        [Range(0, 2)] public float Base_Contrast;
        [Range(0, 2)] public float Base_Saturation;
        [Range(-1, 1)] public float Base_Brightness;

        [Header("Beton")]
        public Texture _BetonTex;
        public Mesh _BetonMesh;
        public Color _BetonColor;
        public Texture _BetonTexInside;
        public float _BetonInsideStrength;
        public Color _BetonColorInside;
        [Range(-360, 360)] public float Beton_Hue;
        [Range(0, 2)] public float Beton_Contrast;
        [Range(0, 2)] public float Beton_Saturation;
        [Range(-1, 1)] public float Beton_Brightness;

        [Header("Elevator")]
        public Texture _ElevatorTex;
        public Texture _ElevatorBackTex;
        public Mesh _ElevatorMesh;
        public Color _ElevatorColor;
        public Texture _ElevatorTexInside;
        public Texture _ElevatorBackTexInside;
        public float _ElevatorInsideStrength;
        public Color _ElevatorColorInside;
        [Range(-360, 360)] public float Elevator_Hue;
        [Range(0, 2)] public float Elevator_Contrast;
        [Range(0, 2)] public float Elevator_Saturation;
        [Range(-1, 1)] public float Elevator_Brightness;

        [Header("Counter")]
        public Texture _CounterTex1;
        public Texture _CounterTex2;
        public Texture _CounterTex3;
        public Texture _CounterTex4;
        public Texture _CounterTex5;
        public Texture _CounterTex6;
        public Texture _CounterTex7;
        public Texture _CounterTex8;
        public Texture _CounterTex9;
        public Mesh _CounterMesh;
        public Color _CounterColor;
        public Texture _CounterTexInside;
        public float _CounterInsideStrength;
        public Color _CounterColorInside;
        [Range(-360, 360)] public float Counter_Hue;
        [Range(0, 2)] public float Counter_Contrast;
        [Range(0, 2)] public float Counter_Saturation;
        [Range(-1, 1)] public float Counter_Brightness;

        [Header("RotatorAll")]
        public Texture _RotatorsTexSocle;
        public Mesh _RotatorsMeshSocle;
        public Color _RotatorsColorSocle;
        [Range(-360, 360)] public float Rotators_Hue;
        [Range(0, 2)] public float Rotators_Contrast;
        [Range(0, 2)] public float Rotators_Saturation;
        [Range(-1, 1)] public float Rotators_Brightness;

        [Header("RotatorLeft")]
        public Texture _RotatorsTexLeft;
        public Mesh _RotatorsMeshLeft;
        public Color _RotatorsColorLeft;
        public Texture _RotatorsTexInsideLeft;
        public float _RotatorsInsideStrengthLeft;
        public Color _RotatorsColorInsideLeft;

        [Header("RotatorRight")]
        public Texture _RotatorsTexRight;
        public Mesh _RotatorsMeshRight;
        public Color _RotatorsColorRight;
        public Texture _RotatorsTexInsideRight;
        public float _RotatorsInsideStrengthRight;
        public Color _RotatorsColorInsideRight;

        [Header("RotatorUI")]
        public Texture _RotatorsTexUI;
        public Mesh _RotatorsMeshUI;
        public Color _RotatorsColorUI;
        public Texture _RotatorsTexInsideUI;
        public float _RotatorsInsideStrengthUI;
        public Color _RotatorsColorInsideUI;

        [Header("Bomb")]
        public Texture _BombTex;
        public Mesh _BombMesh;
        public Color _BombColor;
        public Texture _BombTexInside;
        public float _BombInsideStrength;
        public Color _BombColorInside;
        [Range(-360, 360)] public float Bomb_Hue;
        [Range(0, 2)] public float Bomb_Contrast;
        [Range(0, 2)] public float Bomb_Saturation;
        [Range(-1, 1)] public float Bomb_Brightness;

        [Header("Switch")]
        public Texture _SwitchTexOn;
        public Texture _SwitchTexOff;
        public Texture _SwitchTexInside;
        public Mesh _SwitchMesh;
        public Color _SwitchColor;
        public Color _SwitchColorInside;
        public float _SwitchInsideStrength;
        [Range(-360, 360)] public float Switch_Hue;
        [Range(0, 2)] public float Switch_Contrast;
        [Range(0, 2)] public float Switch_Saturation;
        [Range(-1, 1)] public float Switch_Brightness;

        [Header("Switch Bouton")]
        public Texture _SwitchTexOnButton;
        public Texture _SwitchTexOffButton;
        public Texture _SwitchTexInsideButton;
        public Mesh _SwitchMeshButton;
        public Color _SwitchColorButton;
        public Color _SwitchColorInsideButton;
        public float _SwitchInsideStrengthButton;

        [Header("Ball")]
        public Texture _BallTex;
        public Mesh _BallMesh;
        public Color _BallColor;
        public Texture _BallTexInside;
        public float _BallInsideStrength;
        public Color _BallColorInside;
        [Range(-360, 360)] public float Ball_Hue;
        [Range(0, 2)] public float Ball_Contrast;
        [Range(0, 2)] public float Ball_Saturation;
        [Range(-1, 1)] public float Ball_Brightness;

        [Header("Pastille")]
        public Texture _PastilleTex;
        public Mesh _PastilleMesh;
        public Color _PastilleColor;
        public Texture _PastilleTexInside;
        public float _PastilleInsideStrength;
        public Color _PastilleColorInside;
        [Range(-360, 360)] public float Pastille_Hue;
        [Range(0, 2)] public float Pastille_Contrast;
        [Range(0, 2)] public float Pastille_Saturation;
        [Range(-1, 1)] public float Pastille_Brightness;

        //////// Victory
        [Header("BombV")]
        public Texture _BombVTex;
        public Mesh _BombVMesh;
        public Color _BombVColor;
        public Texture _BombVTexInside;
        public float _BombVInsideStrength;
        public Color _BombVColorInside;
        [Range(-360, 360)] public float BombV_Hue;
        [Range(0, 2)] public float BombV_Contrast;
        [Range(0, 2)] public float BombV_Saturation;
        [Range(-1, 1)] public float BombV_Brightness;

        [Header("BallV")]
        public Texture _BallVTex;
        public Mesh _BallVMesh;
        public Color _BallVColor;
        public Texture _BallVTexInside;
        public float _BallVInsideStrength;
        public Color _BallVColorInside;
        [Range(-360, 360)] public float BallV_Hue;
        [Range(0, 2)] public float BallV_Contrast;
        [Range(0, 2)] public float BallV_Saturation;
        [Range(-1, 1)] public float BallV_Brightness;

        [Header("SwitchV")]
        public Texture _SwitchVTexOn;
        public Texture _SwitchVTexOff;
        public Texture _SwitchVTexInside;
        public Mesh _SwitchVMesh;
        public Color _SwitchVColor;
        public Color _SwitchVColorInside;
        public float _SwitchVInsideStrength;
        [Range(-360, 360)] public float SwitchV_Hue;
        [Range(0, 2)] public float SwitchV_Contrast;
        [Range(0, 2)] public float SwitchV_Saturation;
        [Range(-1, 1)] public float SwitchV_Brightness;

        [Header("BaseV")]
        public Texture _BaseVTex;
        public Mesh _BaseVMesh;
        public Color _BaseVColor;
        public Texture _BaseVTexInside;
        public float _BaseVInsideStrength;
        public Color _BaseVColorInside;
        [Range(-360, 360)] public float BaseV_Hue;
        [Range(0, 2)] public float BaseV_Contrast;
        [Range(0, 2)] public float BaseV_Saturation;
        [Range(-1, 1)] public float BaseV_Brightness;

        [Header("BetonV")]
        public Texture _BetonVTex;
        public Mesh _BetonVMesh;
        public Color _BetonVColor;
        public Texture _BetonVTexInside;
        public float _BetonVInsideStrength;
        public Color _BetonVColorInside;
        [Range(-360, 360)] public float BetonV_Hue;
        [Range(0, 2)] public float BetonV_Contrast;
        [Range(0, 2)] public float BetonV_Saturation;
        [Range(-1, 1)] public float BetonV_Brightness;
    }
}
