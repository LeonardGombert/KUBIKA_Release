using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace Kubika.Game
{
    public class _Planete : MonoBehaviour
    {
        private static _Planete _instance;
        public static _Planete instance { get { return _instance; } }


        // ROTATION
        public Transform planeteTrans;
        public float rotationSpeed;
        public float facesSpeed;
        public float planeteSpeed;

        public _PlaneteCamera[] raycastFaces;
        public int actualIndex;
        public _PlaneteCamera currentFace;
        public _PlaneteCamera nextFace;
        public _PlaneteCamera oldFace;

        //BASE CAMERA
        public CinemachineVirtualCamera baseVCam;
        public CinemachineBrain brainVCam;
        public CinemachineBlenderSettings blenderSettingsVCam; // the resulting settings
        public CinemachineBlenderSettings blenderCUTSettingsVCam; // for a cut transition
        public Camera MainCam;

        //INPUT
        Touch touch;
        Vector3 mouse;
        RaycastHit hit;
        Ray ray;

        //PLANETE TO FACE VIEW
        public bool planeteView;

        //CAMERA MOVE--
        // ZOOM & CAM MOVING
        Touch touch0;
        Vector3 mouse0;
        Vector3 mouse0LastPos;
        Touch touch1;
        Vector2 touch0PrevPos;
        Vector2 touch1PrevPos;
        float prevMagn;
        float currentMagn;
        float difference;
        [Space(10)] public float zoomPower;
        //
        Vector3 currentCamPos;
        float currentZoommCam;
        float currentCamZPos;
        //MOUV
        public Transform pivotMainCamera;
        Vector3 baseRotation;
        Vector3 currentRotation;
        float mediumXMouv;
        float mediumYMouv;
        float baseYRotation;
        float baseXRotation;
        public float mouvPower = 0.1f;

        public static float KUBNordScreenAngle;
        public static float KUBWestScreenAngle;
        public static float KUBSudScreenAngle;
        public static float KUBEstScreenAngle;

        // TRANSITION TO NEXT LEVEL
        public LevelCube[] levelCubes;
        public GameObject targetLevel;

        float time;
        float changeX, changeY, changeZ;
        float startValueX, startValueY, startValueZ;
        float targetValueX, targetValueY, targetValueZ;
        float tweenDuration = 1f;

        // PLATFORM
        public bool isMobilePlatform = false;

        // MOON
        public bool isMoonDisabled = false;

        //AUDIO
        [SerializeField] AudioSource audioSource;


        // Start is called before the first frame update
        void Start()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;

            planeteView = true;
            planeteSpeed = rotationSpeed;

            levelCubes = FindObjectsOfType<LevelCube>();

            audioSource.clip = _AudioManager.instance.SelectPlanete;

            if (_CheckCurrentPlatform.platform == RuntimePlatform.Android || _CheckCurrentPlatform.platform == RuntimePlatform.IPhonePlayer)
                isMobilePlatform = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (planeteView == true)
            {
                //RotationPlanete();

                if (Input.GetMouseButtonUp(0))
                    CheckTouch();
                else if(Input.touchCount == 1)
                    CheckTouch();

                if (isMobilePlatform == true)
                    CameraPhoneInput();
                else
                    CameraPCInput();
            }
            else
            {

            }
            //else
            //CameraPCInput();
        }


        void RotationPlanete()
        {
            planeteTrans.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
        }

        void CheckTouch()
        {
            if (isMobilePlatform == true)
            {
                    touch = Input.GetTouch(0);
                    ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit))
                    {
                        nextFace = hit.collider.gameObject.GetComponent<_PlaneteCamera>();

                        if (hit.collider.gameObject.GetComponent<_PlaneteCamera>() == true)
                        {
                            nextFace = hit.collider.gameObject.GetComponent<_PlaneteCamera>();
                        }
                        else if (hit.collider.gameObject.tag == "Moon")
                        {
                            planeteView = false;
                            audioSource.Play();
                            _MOON.instance.MoveToMoon();
                            return;
                        }

                        if (nextFace != null && touch.phase == TouchPhase.Ended)
                        {
                            if (planeteView == true)
                            {
                                nextFace.isActive = true;
                                nextFace.ActivatePSFB();
                                audioSource.Play();
                                foreach (_PlaneteCamera faces in raycastFaces)
                                {
                                    faces.gameObject.GetComponent<Collider>().enabled = false;
                                }

                                facesSpeed = rotationSpeed * 0.33f;
                                rotationSpeed = facesSpeed;
                            }

                            OnZoomToPlanetCalls();

                            CameraTransition(nextFace);
                            if (isMoonDisabled == false)
                            {
                                StartCoroutine(DisableMoon());
                                isMoonDisabled = true;
                            }
                            planeteView = false;

                        }
                    }
                
            }
            else
            {
                mouse = Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.gameObject.GetComponent<_PlaneteCamera>() == true)
                    {
                        nextFace = hit.collider.gameObject.GetComponent<_PlaneteCamera>();
                    }
                    else if (hit.collider.gameObject.tag == "Moon")
                    {
                        planeteView = false;
                        audioSource.Play();
                        _MOON.instance.MoveToMoon();
                        return;
                    }

                    if (nextFace != null)
                    {
                        if (planeteView == true)
                        {
                            nextFace.isActive = true;
                            nextFace.ActivatePSFB();
                            audioSource.Play();
                            foreach (_PlaneteCamera faces in raycastFaces)
                            {
                                faces.gameObject.GetComponent<Collider>().enabled = false;
                            }

                            facesSpeed = rotationSpeed * 0.33f;
                            rotationSpeed = facesSpeed;
                        }

                        OnZoomToPlanetCalls();

                        CameraTransition(nextFace);
                        if (isMoonDisabled == false)
                        {
                            StartCoroutine(DisableMoon());
                            isMoonDisabled = true;
                        }
                        planeteView = false;
                    }
                }
            }



        }

        private void OnZoomToPlanetCalls()
        {
            WorldmapManager.instance.UpdateWorldMap();

            WorldmapManager.instance.currentBiome = (Biomes)int.Parse(hit.collider.gameObject.name.Replace("Face", "")) - 1; // cursed memes for depressed programming teens
                                                                                                                             // it returns which biome is being selected base on the name of the face
            StartCoroutine(UIManager.instance.ZoomedWorldMapPriority());                                                     
        }

        public void ZoomOutPlanetCalls()
        {
            UIManager.instance.WorldMapPriority();
        }

        void CameraTransition(_PlaneteCamera next)
        {
            actualIndex = next.index;
            next.isActive = true;

            if (oldFace != null) oldFace.vCam.Priority = 0;
            currentFace = next;
            currentFace.vCam.Priority = 20;
            baseVCam.Priority = 0;
            oldFace = currentFace;
        }

        public void ResetAllCameras()
        {
            foreach (_PlaneteCamera faces in raycastFaces)
            {
                faces.ResetCamPos();
            }
        }

        public IEnumerator MainPlaneteView()
        {
            ResetAllCameras();
            
            ZoomOutPlanetCalls();

            yield return new WaitForSeconds(0.1f);

            if (planeteView == false)
            {
                foreach (_PlaneteCamera faces in raycastFaces)
                {
                    faces.gameObject.GetComponent<Collider>().enabled = true;
                    faces.isActive = false;
                }

                rotationSpeed = planeteSpeed;
            }

            isMoonDisabled = false;
            _MOON.instance.gameObject.SetActive(true);
            _MOON.instance.isMoonView = false;
            _MOON.instance.BaseView.Priority = 0;
            planeteView = true;
            currentFace.vCam.Priority = 0;
            baseVCam.Priority = 20;

            yield return null;
        }

        //used to start the player's cam on the face after beating a level
        public IEnumerator StartOnFace(int faceEnQuestion)
        {
            brainVCam.m_CustomBlends = blenderCUTSettingsVCam;

            _MOON.instance.gameObject.SetActive(false);
            isMoonDisabled = true;

            planeteView = false;
            
            StartCoroutine(UIManager.instance.ZoomedWorldMapPriority());

            raycastFaces[faceEnQuestion + 1].isActive = true;
            foreach (_PlaneteCamera faces in raycastFaces)
            {
                faces.gameObject.GetComponent<Collider>().enabled = false;
            }
            facesSpeed = rotationSpeed * 0.33f;
            rotationSpeed = facesSpeed;

            CameraTransition(raycastFaces[faceEnQuestion + 1]);

            WorldmapManager.instance.UpdateWorldMap();

            //StartCoroutine(FocusOnNextLevel(LevelsManager.instance._Kubicode, faceEnQuestion + 1));

            yield return null;
        }

        public IEnumerator FocusOnNextLevel(string _Kubicode, int faceEnQuestion)
        {
            Transform faceRotationPoint = gameObject.transform.GetChild(1).transform.GetChild(0).
                                          transform.GetChild(faceEnQuestion).transform.GetChild(1).transform;

            foreach (var item in levelCubes)
            {
                if (item.kubicode == _Kubicode)
                {
                    targetLevel = item.gameObject;
                    break;
                }
                else continue;
            }

            currentFace.PutCameraInfrontOfCube(targetLevel.transform.position);

            yield return null;
        }

        IEnumerator DisableMoon()
        {
            yield return new WaitForSeconds(2);
            _MOON.instance.gameObject.SetActive(false);
        }

        //for rotation
        public void AfterFace()
        {

            if (actualIndex + 1 < raycastFaces.Length)
            {
                raycastFaces[actualIndex].isActive = false;
                CameraTransition(raycastFaces[actualIndex + 1]);
            }
        }

        //for rotation
        public void BeforeFace()
        {

            if (actualIndex - 1 >= 0)
            {
                ResetAllCameras();
                raycastFaces[actualIndex].isActive = false;
                CameraTransition(raycastFaces[actualIndex - 1]);
            }
        }


        //CAMERA-----------------------------------------------------

        void CameraPhoneInput()
        {
            if (Input.touchCount == 1)
            {
                if(touch.phase == TouchPhase.Began)
                    baseRotation = transform.eulerAngles;

                touch0 = Input.GetTouch(0);
                Scrolling(touch0.deltaPosition, touch0.deltaPosition);
                //GetScreenSwipeAngle();
                //ScrollingSimple(touch0.deltaPosition);


            }
            else if (Input.touchCount == 2)
            {
                touch0 = Input.GetTouch(0);
                touch1 = Input.GetTouch(1);

                touch0PrevPos = touch0.position - touch0.deltaPosition;
                touch1PrevPos = touch1.position - touch1.deltaPosition;

                prevMagn = (touch0PrevPos - touch1PrevPos).magnitude;
                currentMagn = (touch0.position - touch1.position).magnitude;

                difference = currentMagn - prevMagn;
                Zooming(difference);
                Scrolling(touch0.deltaPosition, touch1.deltaPosition);


            }
        }

        void CameraPCInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouse0LastPos = Input.mousePosition;
                baseRotation = currentRotation = transform.eulerAngles;
            }
            else if (Input.GetMouseButton(0))
            {
                mouse0 = Input.mousePosition;
                ScrollingSimple(mouse0, mouse0LastPos);
            }

            ZoomingPC(Input.mouseScrollDelta.y);
        }

        void ZoomingPC(float scrollValue)
        {
            currentZoommCam = baseVCam.m_Lens.OrthographicSize;
            currentZoommCam = Mathf.Clamp(currentZoommCam - (scrollValue * zoomPower * 10), 20, 56);
            baseVCam.m_Lens.OrthographicSize = currentZoommCam;
        }

        void ScrollingSimple(Vector2 touch0Pos, Vector2 base0Pos)
        {

            mediumYMouv = ((touch0Pos.x - base0Pos.x)) * mouvPower;
            mediumXMouv = ((touch0Pos.y - base0Pos.y)) * mouvPower;

            baseRotation = transform.eulerAngles;

            baseYRotation = baseRotation.y;
            baseXRotation = baseRotation.x;
            baseRotation.y = baseYRotation - mediumYMouv;
            baseRotation.x = baseXRotation + mediumXMouv;
            transform.eulerAngles = baseRotation;
            //baseYRotation = baseRotation.y + mediumYMouv;
            //baseXRotation = baseRotation.x - mediumXMouv;
            //currentRotation.y = baseYRotation;
            //currentRotation.x = baseXRotation;
            //pivotMainCamera.eulerAngles = currentRotation;
        }

        #region SCROLL & ZOOM
        void Zooming(float difference)
        {
            currentZoommCam = baseVCam.m_Lens.OrthographicSize;
            currentZoommCam = Mathf.Clamp(currentZoommCam - (difference * zoomPower), currentZoommCam - 20, currentZoommCam + 20);
            baseVCam.m_Lens.OrthographicSize = currentZoommCam;
        }

        void Scrolling(Vector2 touch0Pos, Vector2 touch1Pos)
        {

            mediumYMouv = ((touch0Pos.x + touch1Pos.x) * 0.5f) * mouvPower;
            mediumXMouv = ((touch0Pos.y + touch1Pos.y) * 0.5f) * mouvPower;

            baseYRotation = baseRotation.y;
            baseXRotation = baseRotation.x;
            baseRotation.y = baseYRotation - mediumYMouv;
            baseRotation.x = baseXRotation + mediumXMouv;
            transform.eulerAngles = baseRotation;
        }
        #endregion
    }
}
