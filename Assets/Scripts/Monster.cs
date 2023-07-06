using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Monster : MonoBehaviour
    {
        Rigidbody rb;
        Transform target;
        Animator _myAni;

        BoxCollider _myBoxCol;

        //추격 속도
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;

        //근접 거리
        [SerializeField] [Range(0f, 3f)] float contactDistance = 3f;

        bool follow = true;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
            _myBoxCol = GetComponent<BoxCollider>();
        }

        void Update()
        {
            FollowTarget();
        }

        void FollowTarget()
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < contactDistance && distance > 1f/*&& follow*/)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                transform.LookAt(target);
                _myAni.SetBool("Run Forward", true);
            }

            else if (distance > 1f)
            {

            }

            else
            {
                rb.velocity = Vector2.zero;
                _myAni.SetBool("Run Forward", false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                follow = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
                follow = true;
        }
    }
}