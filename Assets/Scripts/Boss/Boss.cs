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

        public AudioClip _attackClip; // ������ �� ���� �Ҹ�
        public AudioClip _deathClip; // ������ ���� �Ҹ�

        public BoxCollider _myAttackTrigger; // �� ���� �ݶ��̴�
        float _attackLange; // �� ���ݽ�������
        float _shortAttack = 2f;
        float _longAttack = 10f;

        float _ATK = 20f; // �� ���ݷ�
        public float _hp; // �� ü��

        bool _nowAttack; //현재 공격 중인지
        bool _takeDamage; // ���� ������ �޴� ������
        bool _die; // ���� �׾�����

        int _pattern;

        //�߰� �ӵ�
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;

        //���� �Ÿ�
        [SerializeField] [Range(0f, 100f)] float contactDistance = 50f;

        void Start()
        {
            _pattern = Random.Range(1, 2);

            for (int i = 0; i < 100; i++)
            {
                GameObject _FB = Instantiate(_fireBall);
                _fireBallList.Add(_FB);
                _fireBallList[i].transform.position = _fierPool.position;
                _fireBallList[i].transform.parent = _fierPool;

                FireBall _fbc = _fireBallList[i].GetComponent<FireBall>();
                _fbc._firePool = _fierPool;
                _fbc._fireBallList = _fireBallList;
                _fireBallList[i].SetActive(false);
            }

            _myRigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
            _myAttackTrigger.enabled = false;
            _nowAttack = false;

            _hp = 500f;
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
                case "ATTACK":
                    _bossSound.clip = _attackClip;
                    break;
                //case "HIT":
                //    _monsterSound.clip = _hitClip;
                //    break;
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
                transform.LookAt(target);
            }

            else
            {
                _myRigidbody.velocity = Vector2.zero;
                _myAni.SetBool("Run Forward", false);
            }

            if (!_nowAttack)
            {
                if (_pattern == 1)
                {
                    _attackLange = _shortAttack;
                }
                else if (_pattern == 2)
                {
                    _attackLange = _longAttack;
                }

                else if (distance < _attackLange && target.GetComponent<Player>()._die == false)
                {
                    Attack(_pattern);
                }
            }


        }

        void Attack(int patter)
        {
            _nowAttack = true;

            if (patter == 1)
            {
                _myAni.SetTrigger("Attack 01");
            }
            else if (patter == 2)
            {
                _myAni.SetTrigger("Attack 02");
            }
            _myAni.SetBool("Run Forward", false);
        }

        void OnAttackCol()
        {
            BossSound("ATTACK");
            _myAttackTrigger.enabled = true;
        }

        void OffAttackCol()
        {
            _nowAttack = false;
            _myAttackTrigger.enabled = false;
            _pattern = Random.Range(1, 2);
        }

        void FireBall()
        {
            _fireBallList[0].SetActive(true);
            _fireBallList[0].transform.position = _firePoint.position;
            _fireBallList.RemoveAt(0);
            _pattern = Random.Range(1, 2);
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
            _myAttackTrigger.enabled = false;
        }

        void Die()
        {
            BossSound("DIE");
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;
            GameManager.I._deathMonsterCount += 1;

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

        void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}