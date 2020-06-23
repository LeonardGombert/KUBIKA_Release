using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class CameraMovement : MonoBehaviour
    {
        Vector3 worldRotation;
        float xAxis, yAxis, zAxis;
        float xAxisRotation, yAxisRotation;
        public float cameraPanSpeed = 4f;
        public float cameraMoveSpeed;

        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
            MoveCamera();
        }

        void MoveCamera()
        {
            if (ScenesManager.isLevelEditor)
            {
                xAxis = Input.GetAxis("Horizontal") * Time.deltaTime * cameraMoveSpeed;
                zAxis = Input.GetAxis("Vertical") * Time.deltaTime * cameraMoveSpeed;
                yAxis = Input.GetAxis("Jump") * Time.deltaTime * cameraMoveSpeed;
                if (Input.GetKey(KeyCode.LeftControl)) yAxis = -1 * Time.deltaTime * cameraMoveSpeed;
                transform.Translate(new Vector3(xAxis, yAxis, zAxis));

                if (Input.GetMouseButton(2))
                {
                    xAxisRotation = Input.GetAxis("Mouse X") * cameraPanSpeed;
                    yAxisRotation = -Input.GetAxis("Mouse Y") * cameraPanSpeed;
                    transform.Rotate(new Vector3(Mathf.Round(yAxisRotation), Mathf.Round(xAxisRotation), 0));

                    worldRotation = transform.localEulerAngles;
                    transform.rotation = Quaternion.Euler(worldRotation.x, worldRotation.y, 0f);
                }
            }
        }
    }
}
