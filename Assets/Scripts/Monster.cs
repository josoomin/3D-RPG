using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Monster : MonoBehaviour
    {
        Rigidbody rb;
        Transform target;

        //추격 속도
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;

        //근접 거리
        [SerializeField] [Range(0f, 3f)] float contactDistance = 3f;

        bool follow = false;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        void Update()
        {
            FollowTarget();
        }

        void FollowTarget()
        {
            if (Vector3.Distance(transform.position, target.position) > contactDistance && follow)
                transform.position = Vector3.MoveTowards(transform.position, transform.position, moveSpeed * Time.deltaTime);

            else
                rb.velocity = Vector2.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            follow = true;
        }

        private void OnTriggerExit(Collider other)
        {
            follow = false;
        }
    }
}