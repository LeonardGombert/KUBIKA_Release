using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class RotatorLocker : _CubeScanner
    {
        GameObject Button;
        _BoutonFB BoutonScript;
        Vector3 newRotate;
        [SerializeField] private bool pressedDown;
        [SerializeField] private bool locked;

        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();
            _DataManager.instance.EndFalling.AddListener(CheckIfTouched);
            
//            if(UIManager.instance != null) LockRotation();

            SpawnButton();
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
            if(LevelsManager.instance._lockRotate == false)
            {
                pressedDown = AnyMoveableChecker(_DirectionCustom.LocalScanner(facingDirection));

                //locked == false ensures that the function doesn't loop
                if (pressedDown && locked == false)
                {
                    audioSourceCube.clip = _AudioManager.instance.Bouton;
                    PlaySound();
                    locked = true;
                    LockRotation();
                }

                // flip the bools when the delivery cube loses track of the victory cube
                if (pressedDown == false && locked == true)
                {
                    locked = false;
                    UnlockRotation();
                }
            }

            if (LevelsManager.instance._lockRotate == true)
            {
                pressedDown = AnyMoveableChecker(_DirectionCustom.LocalScanner(facingDirection));

                //locked == false ensures that the function doesn't loop
                if (pressedDown && locked == false)
                {
                    audioSourceCube.clip = _AudioManager.instance.Bouton;
                    PlaySound();
                    locked = true;
                    UnlockRotation();
                }

                // flip the bools when the delivery cube loses track of the victory cube
                if (pressedDown == false && locked == true)
                {
                    locked = false;
                    UnlockRotation();
                    LockRotation();
                }
            }
        }

        //allow the player to access the UI buttons
        private void UnlockRotation()
        {
            UIManager.instance.TurnOnRotate();
        }

        private void LockRotation()
        {
            UIManager.instance.TurnOffRotate();
        }

        void SpawnButton()
        {

            switch (facingDirection)
            {
                case FacingDirection.up:
                    Button = Instantiate(_BoutonManager.instance.RotatorUIButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 90;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.down:
                    Button = Instantiate(_BoutonManager.instance.RotatorUIButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 270;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.right:
                    Button = Instantiate(_BoutonManager.instance.RotatorUIButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.left:
                    Button = Instantiate(_BoutonManager.instance.RotatorUIButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.z = 180;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.forward:
                    Button = Instantiate(_BoutonManager.instance.RotatorUIButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.y = 270;
                    Button.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.backward:
                    Button = Instantiate(_BoutonManager.instance.RotatorUIButton, transform.position, Quaternion.identity, transform);
                    BoutonScript = Button.GetComponentInChildren<_BoutonFB>();
                    newRotate.y = 90;
                    Button.transform.eulerAngles = newRotate;
                    break;
            }
        }
    }


}