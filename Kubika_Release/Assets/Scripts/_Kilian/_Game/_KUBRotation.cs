using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class _KUBRotation : MonoBehaviour
    {
        private static _KUBRotation _instance;
        public static _KUBRotation instance { get { return _instance; } }


        // ROTATION
        public List<Vector3> eulerAnglesList;
        public float rotationSpeed = 0.01f;
        public float turningSpeed = 0.5f;
        public int actualRotation;

        // BOOL CHECK
        public bool isTurning;

        // ROTATION LERP
        Vector3 currentRot;
        Vector3 baseRot;
        Vector3 moveRot;
        float lerpValue;
        float currentValue;
        public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        // AUDIO
        [SerializeField] AudioSource audioSource;



        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;

            SetupSound();
        }


        #region CONNECTION
        void Start()
        {
 
        }

        #endregion

        public void RightTurn()
        {
            if (isTurning == false)
                StartCoroutine(Rotate(true));
        }

        public void LeftTurn()
        {
            if (isTurning == false)
                StartCoroutine(Rotate(false));
        }

        public void ResetRotation()
        {
            moveRot = transform.eulerAngles;
            moveRot.z = 0;
            transform.eulerAngles = moveRot;

            _DirectionCustom.rotationState = 0;
            actualRotation = 0;
        }


        public IEnumerator Rotate(bool rightSide)
        {
            if (_DataManager.instance.AreCubesEndingToFall(_DataManager.instance.moveCube.ToArray()) == true && _DataManager.instance.AreCubesEndingToMove(_DataManager.instance.moveCube.ToArray()) == true)
            {
                yield return new WaitForSeconds(0.1f);

                isTurning = true;

                currentRot = baseRot = moveRot = transform.eulerAngles;

                lerpValue = 0;
                currentValue = 0;

                audioSource.Play();


                // There is two axis of rotation (left and right) , TODO = OPTI
                if (rightSide)
                {
                    moveRot.z = transform.eulerAngles.z + 120;

                    _DataManager.instance.ResetIndex(1);
                    // Rotation using a Lerp
                    do
                    {
                        lerpValue += Time.deltaTime;
                        currentValue = lerpValue / turningSpeed;

                        currentRot.z = (Mathf.SmoothStep(baseRot.z, moveRot.z, curve.Evaluate(currentValue)));
                        transform.eulerAngles = currentRot;

                        yield return null;
                    }
                    while (currentValue < 1);

                }

                else
                {
                    moveRot.z = transform.eulerAngles.z - 120;
                    _DataManager.instance.ResetIndex(2);
                    do
                    {
                        lerpValue += Time.deltaTime;
                        currentValue = lerpValue / turningSpeed;

                        currentRot.z = (Mathf.SmoothStep(baseRot.z, moveRot.z, curve.Evaluate(currentValue)));
                        transform.eulerAngles = currentRot;

                        yield return null;
                    }
                    while (currentValue < 1);
                }
                currentRot.z = Mathf.RoundToInt((int)moveRot.z);

                currentRot.z = currentRot.z >= 118 && currentRot.z <= 122 ? 120 :
                                        (currentRot.z >= -122 && currentRot.z <= -118 ? -120 :
                                        currentRot.z >= 238 && currentRot.z <= 242 ? 240 : 0);

                transform.eulerAngles = currentRot;

                //=============// //


                //Debug.Log("Tout les Cubes sont posé");


                _DirectionCustom.rotationState = (int)transform.eulerAngles.z % 360 == 0 ? 0 :
                                            ((int)transform.eulerAngles.z % 360 >= 110 && (int)transform.eulerAngles.z % 360 <= 130 ? 1 :
                                            ((int)transform.eulerAngles.z % 360 >= 230 && (int)transform.eulerAngles.z % 360 <= 250 ? 2 : 0)); 

                _DataManager.instance.MakeFall();
                isTurning = false;
            }
        }

        #region AUDIO

        void SetupSound()
        {
            audioSource.clip = _AudioManager.instance.Rotate;
        }
        #endregion
    }
}

