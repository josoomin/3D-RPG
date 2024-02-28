using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Boss : MonoBehaviour
    {
        public Transform _firePoint; // 화염 발사 지점

        public GameObject _fireBall; // 화염구
        public List<GameObject> _fireBallList; // 화염구 리스트
        public Transform _fierPool; // 화염구 게임 오브젝트 풀링 위치

        Rigidbody _myRigidbody; // 보스 리지드 바디
        Transform target; // 공격 타겟(플레이어)
        Animator _myAni; // 보스 애니메이터

        public AudioSource _bossSound; // 보스 사운드소스

        public AudioClip _attackClip1; // 근접 공격 사운드
        public AudioClip _attackClip2; // 원거리 공격 사운드
        public AudioClip _deathClip; // 보스 사망 사운드

        public BoxCollider _myAttackTrigger; // 보스 근접 공격 콜라이더
        float _attackLange; // 공격 가능 사거리
        float _shortAttack = 2f; // 근접 공격 사거리
        float _longAttack = 10f; // 원거리 공격 사거리

        public float _maxHP = 200; // 보스 시작, 최대 체력
        public float _hp; // 보스 현재 체력

        bool _nowAttack; // 보스가 현재 공격할 수 있는 상태인지 판별
        bool _takeDamage; // 보스 데미지 받는 애니메이션 실행 중
        bool _die; // 사망

        int _pattern; // 근접 공격인지 원거리 공격인지 값을 저장
        bool _setPattern; // 현재 패턴을 지정할 수 있는 상태인지 판별

        private Vector3 targetPosition; // 타겟(플레이어) 포지션

        [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f; //  이동 속도

        [SerializeField] [Range(0f, 100f)] float contactDistance = 50f; // 플레이어 추적 거리

        void Start()
        {
            SetPattern(); // 공격 패턴 설정

            // 화염구 100개 생성 후 오브젝트 풀링
            for (int i = 0; i < 100; i++)
            {
                GameObject _FB = Instantiate(_fireBall);
                _fireBallList.Add(_FB);
                _fireBallList[i].transform.position = _fierPool.position;
                _fireBallList[i].transform.parent = _fierPool;

                FireBall _fbc = _fireBallList[i].GetComponent<FireBall>();
                _fbc._firePool = _fierPool;
                _fbc._fireBallList = _fireBallList;
                _fbc._boss = gameObject.transform;
                _fireBallList[i].SetActive(false);
                _setPattern = true;
            }


            _myRigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();

            // 기본값 초기화
            _hp = _maxHP;

            _myAttackTrigger.enabled = false;
            _nowAttack = false;
            _die = false;
        }

        void Update()
        {
            // 죽지 않았거나 데미지 받는 중 아니면 타겟 추적
            if (!_die && !_takeDamage)
            {
                FollowTarget();

                if (_hp <= 0)
                {
                    Die();
                }
            }
        }

        // 보스 사운드 재생
        void BossSound(string action)
        {
            switch (action)
            {
                case "ATTACK1":
                    _bossSound.clip = _attackClip1;
                    break;
                case "ATTACK2":
                    _bossSound.clip = _attackClip2;
                    break;
                case "DIE":
                    _bossSound.clip = _deathClip;
                    break;
            }
            _bossSound.Play();
        }

        // 타겟 추적
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
                Attack(_pattern);
            }

            else
            {
                _myRigidbody.velocity = Vector2.zero;
                _myAni.SetBool("Run Forward", false);
            }
        }

        // 패턴 랜덤 결정
        void SetPattern()
        {
            if (_setPattern)
            {
                _pattern = Random.Range(1, 11);
            }

            if (_pattern % 2 == 0)
            {
                _attackLange = _shortAttack;
                _setPattern = false;
            }
            else if (_pattern % 2 != 0)
            {
                _attackLange = _longAttack;
                _setPattern = false;
            }
        }

        // 공격
        void Attack(int patter)
        {
            _nowAttack = true;

            if (patter % 2 == 0)
            {
                transform.LookAt(target);
                _myAni.SetTrigger("Attack 01");
            }
            else if (patter % 2 != 0)
            {
                transform.LookAt(target);
                BossSound("ATTACK2");
                _myAni.SetTrigger("Attack 02");
            }
            _myAni.SetBool("Run Forward", false);
        }

        // 보스 콜라이더 활성화
        void OnAttackCol()
        {
            BossSound("ATTACK1");
            _myAttackTrigger.enabled = true;
        }

        // 보스 콜라이더 비활성화
        void OffAttackCol()
        {
            _myAttackTrigger.enabled = false;
            _nowAttack = false;
            _setPattern = true;

            SetPattern();
        }

        // 화염구 발사
        void FireBall()
        {
            _fireBallList[0].SetActive(true);
            _fireBallList[0].transform.position = _firePoint.position;
            _fireBallList.RemoveAt(0);
            _nowAttack = false;
            _setPattern = true;

            SetPattern();
        }

        // 추락사
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("GetOffPoint"))
                TakeDamage(200);
        }

        // 데미지 받음
        public void TakeDamage(float damage)
        {
            _hp -= damage;
            _myAttackTrigger.enabled = false;
        }

        // 사망
        void Die()
        {
            BossSound("DIE");
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;
        }

        // 사망 애니메이션 후 나 자신을 파괴
        void DestroyMe()
        {
            Destroy(gameObject);
            GameManager.I.GameClear();
        }
    }
}