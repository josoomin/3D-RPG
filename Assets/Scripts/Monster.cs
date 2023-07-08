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
        public BoxCollider _myAttackTrigger;

        public float _hp;

        bool _takeDamage;
        bool _die;

        //추격 속도
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;

        //근접 거리
        [SerializeField] [Range(0f, 10f)] float contactDistance = 1f;

        bool follow = true;
        float _attackLange = 0.75f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
            _myBoxCol = GetComponent<BoxCollider>();
            _myAttackTrigger.enabled = false;

            _hp = 50f;
            _die = false;
        }

        void Update()
        {
            if (!_die && !_takeDamage)
            {
                FollowTarget();

                if (_hp <= 0)
                {
                    Die();
                }
            }
        }

        void FollowTarget()
        {
            float distance = Vector3.Distance(transform.position, target.position);
            _myAttackTrigger.enabled = false;

            if (distance < contactDistance && distance > _attackLange)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                _myAni.SetBool("Run Forward", true);
                transform.LookAt(target);
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
            _myAttackTrigger.enabled = true;
        }

        void OffAttackCol()
        {
            _myAttackTrigger.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                other.GetComponent<Player>()._hp -= 5;
        }

        void Die()
        {
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
            _takeDamage = true;
            _myAni.SetTrigger("Take Damage");
            _myAttackTrigger.enabled = false;
        }

        public void NoTakeDamage()
        {
            _takeDamage = false;
        }
    }
}