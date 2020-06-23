using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Kubika.Game
{
    public class _MOON : MonoBehaviour
    {
        private static _MOON _instance;
        public static _MOON instance { get { return _instance; } }


        public CinemachineVirtualCamera BaseView;
        public _MOONfaces LEView;

        public bool isMoonView;
        public bool isMobilePlatform;

        //INPUT
        Touch touch;
        Vector3 mouse;
        RaycastHit hit;
        Ray ray;

        // MOONF FACES
        _MOONfaces moonFaceSelected;

        // Start is called before the first frame update
        void Start()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;

            if (_CheckCurrentPlatform.platform == RuntimePlatform.Android || _CheckCurrentPlatform.platform == RuntimePlatform.IPhonePlayer)
                isMobilePlatform = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(isMoonView == true)
            {
                if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
                    CheckTouch();
            }
        }

        void CheckTouch()
        {
            if (isMobilePlatform == true)
            {
                touch = Input.GetTouch(0);
                ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<_MOONfaces>() == true)
                    {
                        moonFaceSelected = hit.collider.gameObject.GetComponent<_MOONfaces>();
                    }
                    else return;


                    if (moonFaceSelected != null && touch.phase == TouchPhase.Ended)
                    {
                        moonFaceSelected.ActivatePSFB();
                    }
                }
            }
            else
            {
                mouse = Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<_MOONfaces>() == true)
                    {
                        moonFaceSelected = hit.collider.gameObject.GetComponent<_MOONfaces>();
                    }
                    else return;

                    if (moonFaceSelected != null)
                    {
                        moonFaceSelected.ActivatePSFB();
                    }
                }

            }



        }

        public void MoveToMoon()
        {

            BaseView.Priority = 50;
            isMoonView = true;
        }
    }
}
