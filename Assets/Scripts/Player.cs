using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dreamingcat
{
    public class Player : MonoBehaviour
    {
        public BoxCollider _attackCol;
        public BoxCollider _defandCol;

        public GameObject _head;

        float _speed = 10.0f;
        float _rotateSpeed = 100.0f; // 회전 속도
        float _jumppower = 5.0f;

        bool _plane;
        bool _defand;

        Rigidbody _myRigidbody;
        CapsuleCollider _myCapCol;

        Animator _myAni;

        void Start()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            _myCapCol = GetComponent<CapsuleCollider>();
            _myAni = GetComponent<Animator>();
            _attackCol.enabled = false;
            _defandCol.enabled = false;
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            //_myCapCol.transform.position = _head.transform.position;

            if (h != 0 || v != 0)
            {
                Move(h, v);
                _myAni.SetBool("Walk", true);
            }
            else
            {
                _myAni.SetBool("Walk", false);
                _myRigidbody.angularVelocity = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.Space) && _plane)
            {
                Jump();
            }

            if (Input.GetMouseButtonDown(0) && !_defand)
            {
                Attack();
            }

            if (Input.GetMouseButton(1))
            {
                Defand();
            }

            if (Input.GetMouseButtonUp(1))
            {
                NotDefand();
            }

        }
        public void Move(float h, float v)
        {
            Vector3 dir = new Vector3(h, 0, v); // new Vector3(h, 0, v)가 자주 쓰이게 되었으므로 dir이라는 변수에 넣고 향후 편하게 사용할 수 있게 함

            transform.Translate(new Vector3(0, 0, v * _speed) * Time.deltaTime);
            transform.Rotate(new Vector3(0, h * _rotateSpeed, 0) * Time.deltaTime);
        }

        public void Jump()
        {
            _myAni.SetTrigger("Jump");
            _myRigidbody.AddForce(Vector3.up * _jumppower, ForceMode.Impulse);
            _plane = false;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Plane"))
            {
                _myCapCol.enabled = true;
                _plane = true;
            }
        }

        public void Attack()
        {
            _attackCol.enabled = true;
            _myAni.SetTrigger("Attack");
        }

        public void NotAttack()
        {
            _attackCol.enabled = false;
        }

        public void Defand()
        {
            _defand = true;
            _defandCol.enabled = true;
            _myAni.SetBool("Defand", true);
        }

        public void NotDefand()
        {
            _defand = false;
            _defandCol.enabled = false;
            _myAni.SetBool("Defand", false);
        }
    }
}