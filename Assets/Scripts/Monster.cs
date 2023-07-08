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
        public BoxCollider _myTrigger;

        //추격 속도
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;

        //근접 거리
        [SerializeField] [Range(0f, 3f)] float contactDistance = 3f;

        bool follow = true;
        float _attackLange = 0.75f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
            _myBoxCol = GetComponent<BoxCollider>();
            _myTrigger.enabled = false;
        }

        void Update()
        {
            FollowTarget();
        }

        void FollowTarget()
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < contactDistance && distance > _attackLange)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                _myAni.SetBool("Run Forward", true);

                if (target.GetComponent<Player>()._plane == false)
                {
                    transform.LookAt(target);
                }
            }

            else if (distance < _attackLange)
            {
                _myAni.SetTrigger("Attack 02");
                _myAni.SetBool("Run Forward", false);
            }

            else
            {
                rb.velocity = Vector2.zero;
                _myAni.SetBool("Run Forward", false);
            }
        }

        void OnAttackCol()
        {
            _myTrigger.enabled = true;
        }

        void OffAttackCol()
        {
            _myTrigger.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                other.GetComponent<Player>()._hp -= 5;
        }
    }
}