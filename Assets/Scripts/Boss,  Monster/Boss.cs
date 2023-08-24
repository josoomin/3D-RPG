using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Boss : MonoBehaviour
    {
        public Transform _firePoint;

        public GameObject _fireBall;
        public List<GameObject> _fireBallList;
        public Transform _fierPool;

        Rigidbody _myRigidbody; // �� ������ �ٵ�
        Transform target; // �÷��̾�
        Animator _myAni; // �� �ִϸ�����

        public AudioSource _bossSound; // ���� �Ҹ� 

        public AudioClip _attackClip1; // ������ �� ���� �Ҹ�
        public AudioClip _attackClip2; // ������ �� ���� �Ҹ�
        public AudioClip _deathClip; // ������ ���� �Ҹ�

        public BoxCollider _myAttackTrigger; // �� ���� �ݶ��̴�
        float _attackLange; // �� ���ݽ�������
        float _shortAttack = 2f;
        float _longAttack = 10f;

        public float _maxHP = 200;
        public float _hp; // �� ü��

        bool _nowAttack; //현재 공격 중인지
        bool _takeDamage; // ���� ������ �޴� ������
        bool _die; // ���� �׾�����

        int _pattern;
        bool _setPattern;

        private Vector3 targetPosition;

        //�߰� �ӵ�
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;

        //���� �Ÿ�
        [SerializeField] [Range(0f, 100f)] float contactDistance = 50f;

        void Start()
        {
            SetPattern();

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
            _myAttackTrigger.enabled = false;
            _nowAttack = false;

            _hp = _maxHP;
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

        void OnAttackCol()
        {
            BossSound("ATTACK1");
            _myAttackTrigger.enabled = true;
        }

        void OffAttackCol()
        {
            _myAttackTrigger.enabled = false;
            _nowAttack = false;
            _setPattern = true;

            SetPattern();
        }

        void FireBall()
        {
            _fireBallList[0].SetActive(true);
            _fireBallList[0].transform.position = _firePoint.position;
            _fireBallList.RemoveAt(0);
            _nowAttack = false;
            _setPattern = true;

            SetPattern();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("GetOffPoint"))
                TakeDamage(200);
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
            _myAttackTrigger.enabled = false;
        }

        void Die()
        {
            BossSound("DIE");
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;
        }

        void DestroyMe()
        {
            Destroy(gameObject);
            GameManager.I.GameClear();
        }
    }
}