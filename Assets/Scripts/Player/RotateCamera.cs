using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class RotateCamera : MonoBehaviour
    {
        public Transform targetTransform; // 플레이어 위치값
        public Vector3 CameraOffset; // 카메라의 Vector3

        private float xRotate, yRotate, xRotateMove, yRotateMove; // 
        public float rotateSpeed = 500.0f; // 카메라 회전 속도
        int _xMaxValue = -10; // 카메라 올려다 보는 값 최대값

        void Update()
        {
            if (!UI_Canvas.I._playerUIActive)
            {
                xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
                yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

                yRotate = transform.eulerAngles.y + yRotateMove;
                xRotate = xRotate + xRotateMove;

                xRotate = Mathf.Clamp(xRotate, -90, 90);

                transform.position = targetTransform.position + CameraOffset;

                if (UI_Canvas.I._talk == false)
                {
                    if (xRotate >= _xMaxValue)
                    {
                        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(_xMaxValue, yRotate, 0);
                    }
                }
            }
        }
    }
}