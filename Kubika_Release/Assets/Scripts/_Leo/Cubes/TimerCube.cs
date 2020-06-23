using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class TimerCube : _CubeScanner
    {
        public int timerValue;
        [SerializeField] bool touchingCube;

        [SerializeField] GameObject touchingGO;
        [SerializeField] GameObject newTouchingGO;
        [SerializeField] List<int> touchingCubeIndex = new List<int>();

        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();
            SetScanDirections();
            _DataManager.instance.EndMoving.AddListener(CubeListener);
            _DataManager.instance.EndFalling.AddListener(CubeListener);

            GuessTimerValue();
        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.EndMoving.AddListener(CubeListener);
            _DataManager.instance.EndFalling.AddListener(CubeListener);
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.EndMoving.RemoveListener(CubeListener);
            _DataManager.instance.EndFalling.RemoveListener(CubeListener);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        private void CubeListener()
        {
            if (!touchingCube)
            {
                //returns the gameobject you are touching
                touchingGO = TimerChecker(_DirectionCustom.up);

                if (touchingGO != null) touchingCube = true;
            }

            else if (touchingCube)
            {
                newTouchingGO = TimerChecker(_DirectionCustom.up);

                if (newTouchingGO != touchingGO)
                {
                    touchingCube = false;
                    newTouchingGO = null;
                    touchingGO = null;
                    timerValue--;
                    ChangeTexture(timerValue);
                }
            }

            if (timerValue <= 0)
            {
                willPOP = true;
                StartCoroutine(PopOut(true));
            }
        }

        //called by a bomb explosion
        public void BombDecrementMe()
        {
            timerValue--;
            ChangeTexture(timerValue);
            CubeListener();
        }

        void GuessTimerValue()
        {
            switch(myCubeType)
            {
                case CubeTypes.TimerCube9:
                    ChangeTexture(9);
                    break;
                case CubeTypes.TimerCube8:
                    ChangeTexture(8);
                    break;
                case CubeTypes.TimerCube7:
                    ChangeTexture(7);
                    break;
                case CubeTypes.TimerCube6:
                    ChangeTexture(6);
                    break;
                case CubeTypes.TimerCube5:
                    ChangeTexture(5);
                    break;
                case CubeTypes.TimerCube4:
                    ChangeTexture(4);
                    break;
                case CubeTypes.TimerCube3:
                    ChangeTexture(3);
                    break;
                case CubeTypes.TimerCube2:
                    ChangeTexture(2);
                    break;
                case CubeTypes.TimerCube1:
                    ChangeTexture(1);
                    break;
            }
        }

        void ChangeTexture(int actualTimerValue)
        {
            switch (actualTimerValue)
            {
                case 9:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex9;
                    break;
                case 8:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex8;
                    break;
                case 7:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex7;
                    break;
                case 6:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex6;
                    break;
                case 5:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex5;
                    break;
                case 4:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex4;
                    break;
                case 3:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex3;
                    break;
                case 2:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex2;
                    break;
                case 1:
                    _MainTex = _MaterialCentral.instance.actualPack._CounterTex1;
                    break;
            }

            meshRenderer.GetPropertyBlock(MatProp);

            MatProp.SetTexture("_MainTex", _MainTex);

            meshRenderer.SetPropertyBlock(MatProp);
        }

    }
}