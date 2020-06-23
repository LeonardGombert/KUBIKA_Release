using Kubika.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class SwitchButton : _CubeScanner
    {
        SwitchCube[] switchCubes;
        GameObject Button;
        _BoutonFB BoutonScript;
        Vector3 newRotate;
        bool isPressed;
        private bool locked;

        // Start is called before the first frame update
        public override void Start()
        { 
            //call base.start AFTER assigning the cube's layers
            base.Start();
            _DataManager.instance.EndFalling.AddListener(CheckIfPressed);
            switchCubes = FindObjectsOfType<SwitchCube>();
            SpawnButton();
            CheckIfPressed();
        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.EndFalling.AddListener(CheckIfPressed);
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.EndFalling.RemoveListener(CheckIfPressed);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        private void CheckIfPressed()
        {
            isPressed = AnyMoveableChecker(_DirectionCustom.LocalScanner(facingDirection)); // doont use up, because it needds to be LOCAL up
            
            if (isPressed == true && locked == false)
            {
                SetupSoundSwitch(true);
                locked = true;
                ActivateSwitches();
                PlaySound();
            }

            if(isPressed == false && locked == true)
            {
                SetupSoundSwitch(false);
                locked = false;
                DeactivateSwitches();
                PlaySound();
            }
        }

        private void ActivateSwitches()
        {
            foreach (SwitchCube cube in switchCubes)
            {
                cube.isSelectable = true;
                cube.ChangeEmoteFace(cube._EmoteIdleTex);
                cube.SetOutlineColor(true);
                StartCoroutine(cube.IncreaseInside(true));

                if (cube.isSwitchVictory == false)
                    cube.ChangeTex(_MaterialCentral.instance.actualPack._SwitchTexOn);
                else
                    cube.ChangeTex(_MaterialCentral.instance.actualPack._SwitchVTexOn);

                cube.isActive = true;
                cube.StatusUpdate();
            }
        }

        private void DeactivateSwitches()
        {
            foreach (SwitchCube cube in switchCubes)
            {
                cube.SetOutlineColor(false);
                cube.isSelectable = false;
                cube.ChangeEmoteFace(cube._EmoteIdleOffTex);
                StartCoroutine(cube.IncreaseInside(false));

                if (cube.isSwitchVictory == false)
                    cube.ChangeTex(_MaterialCentral.instance.actualPack._SwitchTexOff);
                else
                    cube.ChangeTex(_MaterialCentral.instance.actualPack._SwitchVTexOff);

                cube.isActive = false;
                cube.StatusUpdate();
            }
        }

        void SpawnButton()
        {
            switch (facingDirection)
            {
                case FacingDirection.up:
                    Button = Instantiate(_BoutonManager.instance.SwitchButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 90;
                    Button.transform.eulerAngles = newRotate;
                    break;

                case FacingDirection.down:
                    Button = Instantiate(_BoutonManager.instance.SwitchButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 270;
                    Button.transform.eulerAngles = newRotate;
                    break;

                case FacingDirection.right:
                    Button = Instantiate(_BoutonManager.instance.SwitchButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    Button.transform.eulerAngles = newRotate;
                    break;

                case FacingDirection.left:
                    Button = Instantiate(_BoutonManager.instance.SwitchButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 180;
                    Button.transform.eulerAngles = newRotate;
                    break;

                case FacingDirection.forward:
                    Button = Instantiate(_BoutonManager.instance.SwitchButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.y = 270;
                    Button.transform.eulerAngles = newRotate;
                    break;

                case FacingDirection.backward:
                    Button = Instantiate(_BoutonManager.instance.SwitchButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.y = 90;
                    Button.transform.eulerAngles = newRotate;
                    break;
            }
        }


        void SetupSoundSwitch(bool isON)
        {
            if (isON == true)
                audioSourceCube.clip = _AudioManager.instance.SwitchON;
            else
                audioSourceCube.clip = _AudioManager.instance.SwitchOFF;
        }
    }
}