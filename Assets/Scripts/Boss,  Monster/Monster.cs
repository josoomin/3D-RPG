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

        public AudioSource _monsterSound; // 몬스터 소리 

        public AudioClip _attackClip; // 공격할 때 나는 소리
        public AudioClip _deathClip; // 죽을때 나는 소리

        public BoxCollider _myAttackTrigger; // 내 공격 콜라이더
        float _attackLange = 0.75f; // 내 공격시전범위

        float _ATK = 5f; // 내 공격력
        public float _hp; // 내 체력

        private Vector3 targetPosition;

        [SerializeField] bool _takeDamage; // 내가 공격을 받는 중인지
        [SerializeField] bool _die; // 내가 죽었는지

        //추격 속도
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;

        //근접 거리
        [SerializeField] [Range(0f, 10f)] float contactDistance = 1f;

        void Start()
        {
            // 게임오브젝트 가져오기
            _myRigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
            
            // 체력 초기화
            _hp = 50f;

            // 공격 콜라이더 및 사망 bool값 초기화
            _myAttackTrigger.enabled = false;
            _die = false;
        }

        void Update()
        {
            // 사망했거나 데미지 받는 중 아니면 타겟 추적 및 사망
            if (!_die && !_takeDamage)
            {
                FollowTarget();

                if (_hp <= 0)
                {
                    Die();
                }
            }
        }

        // 몬스터 사운드 재생
        void MonsterSound(string action)
        {
            switch (action)
            {
                case "ATTACK":
                    _monsterSound.clip = _attackClip;
                    break;
                case "DIE":
                    _monsterSound.clip = _deathClip;
                    break;
            }
            _monsterSound.Play();
        }

        // 타겟(플레이어) 추적
        void FollowTarget()
        {
            float distance = Vector3.Distance(transform.position, target.position);
            _myAttackTrigger.enabled = false;

            if (distance < contactDistance && distance > _attackLange)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                _myAni.SetBool("Run Forward", true);

                targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                transform.LookAt(targetPosition);
            }

            else if (distance < _attackLange && target.GetComponent<Player>()._die == false)
            {
                Attack();
            }

            else
            {
                _myRigidbody.velocity = Vector2.zero;
                _myAni.SetBool("Run Forward", false);
            }
        }

        // 공격
        void Attack()
        {
            _myAni.SetTrigger("Attack 02");
            _myAni.SetBool("Run Forward", false);
        }

        // 공격 콜라이더 활성화
        void OnAttackCol()
        {
            MonsterSound("ATTACK");
            _myAttackTrigger.enabled = true;
        }

        // 공격 콜라이더 비활성화
        void OffAttackCol()
        {
            _myAttackTrigger.enabled = false;
        }

        // 공격 콜라이더에 플레이어가 닿으면 플레이어에게 데미지
        private void OnTriggerEnter(Collider other)
        {
            Player _player = other.GetComponent<Player>();

            if (other.tag == "Player" && _player._hp > 0)
            {
                _player.TakeDamage(_ATK);
            }

            if (other.CompareTag("GetOffPoint"))
                TakeDamage(100);
        }

        // 데미지 받음
        public void TakeDamage(float damage)
        {
            _hp -= damage;
            _takeDamage = true;
            _myAni.SetTrigger("Take Damage");
            _myAttackTrigger.enabled = false;
        }

        // 검이 통과하는 동안 연속 데미지 방지
        public void NoTakeDamage()
        {
            _takeDamage = false;
        }

        // 사망
        void Die()
        {
            MonsterSound("DIE");
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;

            List<GameObject> _MonLi = GameManager.I._monsterList;

            for (int i = 0; i < _MonLi.Count; i++)
            {
                if (_MonLi[i].name == gameObject.name)
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

        // 나 자시을 파괴
        void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}