using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Monster : MonoBehaviour
    {
        Rigidbody _myRigidbody; // 내 리지드 바디
        Transform target; // 플레이어
        Animator _myAni; // 내 애니메이터

        public BoxCollider _myAttackTrigger; // 내 공격 콜라이더
        float _attackLange = 0.75f; // 내 공격시전범위

        float _ATK = 5f; // 내 공격력
        public float _hp; // 내 체력

        bool _takeDamage; // 내가 공격을 받는 중인지
        bool _die; // 내가 죽었는지

        //추격 속도
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;

        //근접 거리
        [SerializeField] [Range(0f, 10f)] float contactDistance = 1f;

        void Start()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
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

            else if (distance < _attackLange && target.GetComponent<Player>()._die == false)
            {
                _myAni.SetTrigger("Attack 02");
                _myAni.SetBool("Run Forward", false);
            }

            else
            {
                _myRigidbody.velocity = Vector2.zero;
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
            Player _player = other.GetComponent<Player>();

            if (other.tag == "Player" && _player._hp > 0)
            {
                if (_player._defand)
                {
                    float _dmg = _ATK - _player._DEF;

                    if (_dmg < 0)
                        _player.TakeDamage(0);

                    else
                        _player.TakeDamage(_dmg);
                }

                else
                    _player.TakeDamage(_ATK);
            }

            if (other.CompareTag("GetOffPoint"))
                TakeDamage(100);
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

        void Die()
        {
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;
            GameManager.I._deathMonsterCount += 1;

            List<string> _MonLi = GameManager.I._monsterList;

            for (int i = 0; i < _MonLi.Count; i++)
            {
                if (_MonLi[i] == gameObject.name)
                {
                    _MonLi.RemoveAt(i);
                    break;
                }
            }

            if (_MonLi.Count == 0)
            {
                GameManager.I.DropKey();
            }
        }

        void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}