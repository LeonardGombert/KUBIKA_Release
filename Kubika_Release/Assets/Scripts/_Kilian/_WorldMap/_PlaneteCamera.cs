using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Kubika.Game
{
    public class _PlaneteCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera vCam;
        public int index;
        public ParticleSystem PS;

        [Space]
        [Header("CamLimit")]
        public Transform pivotVCam;
        Vector3 currentPivotPosition;
        public Transform LimitStart;
        public Transform LimitEnd;
        public Transform LimitStartScroll;
        public Transform LimitEndScroll;

        [Space]
        public bool isActive = false;
        public float ScrollPower = 0.1f;

        // TOUCH
        Touch touch;
        Vector3 baseMouse;
        Vector3 mouse;
        Ray ray;
        Ray ray2;
        RaycastHit hit;
        bool hasTouchedLevel = false;

        //SCROLL
        public float distanceLimitCam;
        public float distanceLimitScroll;
        protected Plane Plane;
        protected Vector3 BaseScroll;
        protected Vector3 CurrentScroll;

        protected Vector2 BaseScrollScreen;
        protected Vector2 CurrentScrollScreen;
        protected float CurrentPositionSymbol;
        float actualPosScroll;
        float actualPosCam;

        private void Start()
        {
            ResetCamPos();
        }

        public void ActivatePSFB()
        {
            PS.Play();
        }

        void Update()
        {
            if(isActive == true)
            {
                if(_Planete.instance.isMobilePlatform == true)
                {
                    MobileMove();
                }
                else
                {
                    PcMove();
                }
            }
        }

        void MobileMove()
        {
            touch = Input.GetTouch(0);
            ray = Camera.main.ScreenPointToRay(touch.position);


            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit))
                    {
        

                        if (hit.collider.gameObject.GetComponent<_ScriptMatFaceCube>())
                        {
                            hasTouchedLevel = true;
                        }
                        else
                        {
                            BaseScroll = hit.point;
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    if (hasTouchedLevel == false)
                    {
                        ScrollingSimple(touch.deltaPosition.y);
                    }
                    break;
                case TouchPhase.Ended:
                    hasTouchedLevel = false;
                    break;
            }
        }
        void PcMove()
        {
            mouse = Input.mousePosition;
            ray = Camera.main.ScreenPointToRay(mouse);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.gameObject.GetComponent<_ScriptMatFaceCube>())
                    {
                        hasTouchedLevel = true;
                    }
                    else
                    {
                        BaseScroll = hit.point;
                        baseMouse = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (hasTouchedLevel == false)
                {
                    ScrollingSimple(mouse.y - baseMouse.y);
                }
            }
            else
            {
                hasTouchedLevel = false;
            }

        }

        public void ResetCamPos()
        {
            pivotVCam.transform.position = LimitStart.position;
        }


        void ScrollingSimple(float FingerPosition)
        {
            if (Physics.Raycast(ray, out hit))
            {
                CurrentScroll = hit.point;

                currentPivotPosition = pivotVCam.transform.position;

                distanceLimitCam = Vector3.Distance(LimitStart.position, LimitEnd.position);
                distanceLimitScroll = Vector3.Distance(LimitStartScroll.position, LimitEndScroll.position);

                BaseScrollScreen = _Planete.instance.MainCam.WorldToScreenPoint(BaseScroll);
                CurrentScrollScreen = _Planete.instance.MainCam.WorldToScreenPoint(CurrentScroll);
                CurrentScrollScreen.x = BaseScrollScreen.x;

                ray2 = _Planete.instance.MainCam.ScreenPointToRay(CurrentScrollScreen);

                if (Physics.Raycast(ray2, out hit))
                {
                    CurrentScroll = hit.point;
                }

                
                CurrentPositionSymbol = Mathf.Sign(CurrentScrollScreen.y - BaseScrollScreen.y);

                actualPosCam = InverseLerp(LimitStart.position, LimitEnd.position, pivotVCam.transform.position);
                actualPosScroll = (((actualPosCam - ((Vector3.Distance(BaseScroll, CurrentScroll) / distanceLimitScroll) * CurrentPositionSymbol))));


                currentPivotPosition = Vector3.Lerp(LimitStart.position, LimitEnd.position, Mathf.Clamp(Mathf.Abs(actualPosScroll),0.05f , 0.95f));
                pivotVCam.transform.position = currentPivotPosition;
            }
        }

        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        protected Vector3 PlanePositionDelta(Touch touch)
        {
            //not moved
            if (touch.phase != TouchPhase.Moved)
                return Vector3.zero;

            //delta
            var rayBefore = _Planete.instance.MainCam.ScreenPointToRay(touch.position - touch.deltaPosition);
            var rayNow = _Planete.instance.MainCam.ScreenPointToRay(touch.position);
            if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
                return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

            //not on plane
            return Vector3.zero;
        }

        public void PutCameraInfrontOfCube(Vector3 CubePosition)
        {

            currentPivotPosition = pivotVCam.transform.position;

            distanceLimitCam = Vector3.Distance(LimitStart.position, LimitEnd.position);
            distanceLimitScroll = Vector3.Distance(LimitStartScroll.position, LimitEndScroll.position);

            float actualPosScroll = InverseLerp(LimitStartScroll.position, LimitEndScroll.position, CubePosition);

            currentPivotPosition = Vector3.Lerp(LimitStart.position, LimitEnd.position, Mathf.Clamp(Mathf.Abs(actualPosScroll), 0.1f, 0.95f));
            pivotVCam.transform.position = currentPivotPosition;
        }
    }
}
