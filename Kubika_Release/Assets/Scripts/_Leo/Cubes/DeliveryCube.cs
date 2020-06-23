using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class DeliveryCube : _CubeScanner
    {
        bool touchingVictory;
        private bool locked;

        [HideInInspector] public GameObject LightShaft;
        [HideInInspector] public GameObject LightShaftTrue;
        Vector3 newRotate = new Vector3();

        // VICTORY FX
        public _CubeBase victoryCubeTracker;
        Vector3 pushDirection;

        [HideInInspector] public ParticleSystem ExplosionEND_PS;
        float StartExplosion;

        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();

            _DataManager.instance.EndFalling.AddListener(CheckForVictory);
            _DataManager.instance.EndFalling.AddListener(ScannerSet);

            SetFb();
            SetupSoundPastille();

        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.EndFalling.AddListener(ScannerSet);
            _DataManager.instance.EndFalling.AddListener(CheckForVictory);
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.EndFalling.RemoveListener(CheckForVictory);
            _DataManager.instance.EndFalling.RemoveListener(ScannerSet);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        private void CheckForVictory()
        {
            //touchingVictory = ProximityChecker(_DirectionCustom.fixedUp, CubeTypes.BaseVictory, CubeLayers.None);
            touchingVictory = VictoryChecker(_DirectionCustom.LocalScanner(facingDirection));

            // flip the bools when the delivery cube loses track of the victory cube
            if (touchingVictory == false && locked == true)
            {
                victoryCubeOnPistion.isPastilleAndIsOn = false;
                StartCoroutine(victoryCubeTracker.VictoryFX(false));
                victoryCubeTracker.ChangeEmoteFace(_EmoteIdleTex);
                VictoryConditionManager.instance.DecrementVictory();
                locked = false;

            }

            if (touchingVictory && locked == false)
            {
                locked = true;
                LightShaft.GetComponent<FB_Delivry>().ActivatePSFB();
                StartCoroutine(victoryCubeOnPistion.VictoryFX(true));
                victoryCubeOnPistion.isPastilleAndIsOn = true;
                victoryCubeOnPistion.ChangeEmoteFace(victoryCubeOnPistion._EmotePastilleTex);
                victoryCubeTracker = victoryCubeOnPistion;
                VictoryConditionManager.instance.IncrementVictory();

                PlaySound();
            }
        }

        void SetFb()
        {

            switch (facingDirection)
            {
                case FacingDirection.up:
                    LightShaft = Instantiate(_FeedBackManager.instance.Fb_Delivry, transform.position, Quaternion.identity, transform);
                    newRotate.z = 90;
                    LightShaft.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.down:
                    LightShaft = Instantiate(_FeedBackManager.instance.Fb_Delivry, transform.position, Quaternion.identity, transform);
                    newRotate.z = 270;
                    LightShaft.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.right:
                    LightShaft = Instantiate(_FeedBackManager.instance.Fb_Delivry, transform.position, Quaternion.identity, transform);
                    LightShaft.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.left:
                    LightShaft = Instantiate(_FeedBackManager.instance.Fb_Delivry, transform.position, Quaternion.identity, transform);
                    newRotate.z = 180;
                    LightShaft.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.forward:
                    LightShaft = Instantiate(_FeedBackManager.instance.Fb_Delivry, transform.position, Quaternion.identity, transform);
                    newRotate.y = 270;
                    LightShaft.transform.eulerAngles = newRotate;
                    break;
                case FacingDirection.backward:
                    LightShaft = Instantiate(_FeedBackManager.instance.Fb_Delivry, transform.position, Quaternion.identity, transform);
                    newRotate.y = 90;
                    LightShaft.transform.eulerAngles = newRotate;
                    break;
            }
        }

        public IEnumerator VictoryPSLatence()
        {
            StartExplosion = UnityEngine.Random.Range(0.2f, 1);
            yield return new WaitForSeconds(StartExplosion);
            SetupSoundExplosion();
            PlaySound();
            AddForceToVictoryCube();
            ExplosionEND_PS.Play();
            BlowUpDirection();
            _InGameCamera.instance.ScreenShake();
            GetComponent<MeshRenderer>().enabled = false;
            LightShaftTrue.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(15);
            Destroy(gameObject);
        }

        void BlowUpDirection()
        {
            //shoot forwards
            for (int position = myIndex; position < grid.gridSize * grid.gridSize * grid.gridSize; position -= _DirectionCustom.LocalScanner(facingDirection))
            {
                if (position > 0)
                {
                    if (!MatrixLimitCalcul(position, _DirectionCustom.LocalScanner(facingDirection))) break;
                    RemoveCube(position);
                }
                else break;
            }

        }

        void AddForceToVictoryCube()
        {
            pushDirection = transform.position - victoryCubeOnPistion.transform.position;
            victoryCubeOnPistion.ApplyRigidbody(pushDirection);
        }

        void SetupSoundPastille()
        {
            audioSourceCube.clip = _AudioManager.instance.Pastille;
        }

        void SetupSoundExplosion()
        {
            audioSourceCube.clip = _AudioManager.instance.EndPush;
        }
    }
}