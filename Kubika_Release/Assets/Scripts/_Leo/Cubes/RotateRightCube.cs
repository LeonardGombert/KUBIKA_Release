﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class RotateRightCube : _CubeScanner
    {
        GameObject Button;
        _BoutonFB BoutonScript;
        Vector3 newRotate;
        public bool pressedDown;
        public bool locked;

        List<RotateRightCube> otherRotators = new List<RotateRightCube>();
        private bool isSoloToucher;

        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();
            _DataManager.instance.EndFalling.AddListener(CheckIfTouched);
            SpawnButton();

            StartCoroutine(CheckForOthers());
        }

        private IEnumerator CheckForOthers()
        {
            yield return new WaitForSeconds(.5f);

            otherRotators.AddRange(FindObjectsOfType<RotateRightCube>());
            otherRotators.Remove(this);

            yield return null;
        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.EndFalling.AddListener(CheckIfTouched);
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.EndFalling.RemoveListener(CheckIfTouched);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        void CheckIfTouched()
        {
            pressedDown = AnyMoveableChecker(_DirectionCustom.LocalScanner(facingDirection));

            //locked == false ensures that the function doesn't loop
            if (pressedDown && locked == false)
            {
                audioSourceCube.clip = _AudioManager.instance.Bouton;
                PlaySound();

                locked = true;
                if (CheckIfOthersTouch() == false) _KUBRotation.instance.RightTurn();
            }

            // flip the bools when the delivery cube loses track of the victory cube
            if (pressedDown == false && locked == true)
            {
                locked = false;
                if (CheckIfOthersTouch() == false) _KUBRotation.instance.LeftTurn();
            }
        }

        bool CheckIfOthersTouch()
        {
            foreach (RotateRightCube cube in otherRotators)
            {
                if (cube.IsTouching())
                    return true;
                else continue;
            }

            return false;
        }

        public bool IsTouching()
        {
            return AnyMoveableChecker(_DirectionCustom.LocalScanner(facingDirection));
        }

        void SpawnButton()
        {

            switch (facingDirection)
            {
                case FacingDirection.up:
                    Button = Instantiate(_BoutonManager.instance.RotatorRightButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 90;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.down:
                    Button = Instantiate(_BoutonManager.instance.RotatorRightButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 270;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.right:
                    Button = Instantiate(_BoutonManager.instance.RotatorRightButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.left:
                    Button = Instantiate(_BoutonManager.instance.RotatorRightButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 180;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.forward:
                    Button = Instantiate(_BoutonManager.instance.RotatorRightButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.y = 270;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.backward:
                    Button = Instantiate(_BoutonManager.instance.RotatorRightButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.y = 90;
                    Button.transform.eulerAngles = newRotate;
                    break;
            }
        }
    }
}