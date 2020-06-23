using System;
using System.Collections;
using UnityEngine;

namespace Kubika.Game
{
    public class BombCube : _CubeMove
    {
        int cubeTopIndex;
        protected bool victoryBomb;
        ParticleSystem bombExplosion;

        // Start is called before the first frame update
        public override void Start()
        {
            //call base.start AFTER assigning the cube's layers
            base.Start();

            SetScanDirections();
            _DataManager.instance.EndFalling.AddListener(CheckBlowUp);
        }

        public override void UndoProcedure()
        {
            base.UndoProcedure();
            _DataManager.instance.EndFalling.AddListener(CheckBlowUp);
        }

        public override void HideCubeProcedure()
        {
            base.HideCubeProcedure();
            _DataManager.instance.EndFalling.RemoveListener(CheckBlowUp);
        }


        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            // if a chaos ball hits the mine cube
            /*if(Input.GetKeyDown(KeyCode.F2)) BlowAcross(myIndex - 1 + _DirectionCustom.right);
            if(Input.GetKeyDown(KeyCode.F3)) BlowAcross(myIndex - 1 + _DirectionCustom.forward);*/
            //BlowAcross();
        }

        void CheckBlowUp()
        {
            if (nbrCubeEmptyBelow > 0) BlowUp();
        }

        void BlowUp()
        {
            _InGameCamera.instance.ScreenShake();

            if(victoryBomb == false)
            {
                bombExplosion = Instantiate(_FeedBackManager.instance.MineExplosionBase, transform.position, Quaternion.Euler(-90,0,0));
            }

            else
            {
                bombExplosion = Instantiate(_FeedBackManager.instance.MineExplosionVictory, transform.position, Quaternion.Euler(-90, 0, 0));
            }

            //shoot up
            for (int position = myIndex; position < grid.gridSize * grid.gridSize * grid.gridSize; position++)
            {
                RemoveCube(position);
                if (!MatrixLimitCalcul(position, _DirectionCustom.up)) break;
            }

            //shoot down
            for (int position = myIndex - 1; position > 0; position--)
            {
                RemoveCube(position);
                if (!MatrixLimitCalcul(position, _DirectionCustom.down)) break;
            }

            SetupBombAudio();
            PlaySound();

            StartCoroutine(BombDeleteLatence());

            _DataManager.instance.EndFalling.RemoveListener(CheckBlowUp);
        }

        IEnumerator BombDeleteLatence()
        {
            yield return new WaitForSeconds(_FeedBackManager.instance.MineExplosionVictory.main.duration);
            Destroy(bombExplosion.gameObject);
        }

        //for when the ball hits a mine
        private void BlowAcross(int ballIndex)
        {
            //explode on Z axis
            if(ballIndex == myIndex -1 + _DirectionCustom.forward || ballIndex == myIndex - 1 + _DirectionCustom.backward)
            {
                //shoot forwards
                for (int position = myIndex; position < grid.gridSize * grid.gridSize * grid.gridSize; position += _DirectionCustom.forward)
                {
                    RemoveCube(position);
                    if (!MatrixLimitCalcul(position, _DirectionCustom.forward)) break;
                } 
                
                //shoot back
                for (int position = myIndex; position < grid.gridSize * grid.gridSize * grid.gridSize; position += _DirectionCustom.backward)
                {
                    RemoveCube(position);
                    if (!MatrixLimitCalcul(position, _DirectionCustom.backward)) break;
                }
            }

            //explode on X axis
            else if (ballIndex == myIndex - 1 + _DirectionCustom.right || ballIndex == myIndex - 1 + _DirectionCustom.left)
            {
                //shoot right
                for (int position = myIndex; position < grid.gridSize * grid.gridSize * grid.gridSize; position += _DirectionCustom.right)
                {
                    RemoveCube(position);
                    if (!MatrixLimitCalcul(position, _DirectionCustom.right)) break;
                }

                //shoot left
                for (int position = myIndex; position < grid.gridSize * grid.gridSize * grid.gridSize; position += _DirectionCustom.left)
                {
                    RemoveCube(position);
                    if (!MatrixLimitCalcul(position, _DirectionCustom.left)) break;
                }
            }

        }

        #region AUDIO
        void SetupBombAudio()
        {
            audioSourceCube.clip = _AudioManager.instance.Explosion;
        }

        #endregion
    }
}