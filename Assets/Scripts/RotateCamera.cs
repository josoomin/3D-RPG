using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class RotateCamera : MonoBehaviour
    {
        private float xRotate, yRotate, xRotateMove, yRotateMove;
        public float rotateSpeed = 500.0f;
        int _xMaxValue = -10;

        void Update()
        {
            xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
            yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

            yRotate = transform.eulerAngles.y + yRotateMove;
            //xRotate = transform.eulerAngles.x + xRotateMove; 
            xRotate = xRotate + xRotateMove;

            xRotate = Mathf.Clamp(xRotate, -90, 90); // ��, �Ʒ� ����


            if (UI_Canvas._talk == false)
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