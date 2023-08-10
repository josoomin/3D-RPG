using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Boss : MonoBehaviour
    {
        public GameObject _fireBall;
        public List<GameObject> _fireBallList;
        public Transform _fierPool;

        Rigidbody _myRigidbody; // �� ������ �ٵ�
        Transform target; // �÷��̾�
        Animator _myAni; // �� �ִϸ�����

        public AudioSource _monsterSound; // ���� �Ҹ� 

        public AudioClip _attackClip; // ������ �� ���� �Ҹ�
        public AudioClip _deathClip; // ������ ���� �Ҹ�

        public BoxCollider _myAttackTrigger; // �� ���� �ݶ��̴�
        float _attackLange = 0.75f; // �� ���ݽ�������

        float _ATK = 20f; // �� ���ݷ�
        public float _hp; // �� ü��

        bool _takeDamage; // ���� ������ �޴� ������
        bool _die; // ���� �׾�����

        //�߰� �ӵ�
        [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;

        //���� �Ÿ�
        [SerializeField] [Range(0f, 10f)] float contactDistance = 10f;

        void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject _FB = Instantiate(_fireBall);
                _fireBallList.Add(_FB);
                _fireBallList[i].transform.position = _fierPool.position;
                _fireBallList[i].transform.parent = _fierPool;
                _fireBallList[i].SetActive(false);

            }

            _myRigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _myAni = GetComponent<Animator>();
            _myAttackTrigger.enabled = false;

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
                    _monsterSound.clip = _attackClip;
                    break;
                //case "HIT":
                //    _monsterSound.clip = _hitClip;
                //    break;
                case "DIE":
                    _monsterSound.clip = _deathClip;
                    break;
            }
            _monsterSound.Play();
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
                int _Patter = Random.Range(1, 2);
                Attack(_Patter);
            }

            else
            {
                _myRigidbody.velocity = Vector2.zero;
                _myAni.SetBool("Run Forward", false);
            }
        }

        void Attack(int patter)
        {
            if (patter == 1)
            {
                _myAni.SetTrigger("Attack 01");
                _myAni.SetBool("Run Forward", false);
            }
            else if (patter == 2)
            {
                _myAni.SetTrigger("Attack 02");


            }
        }

        void OnAttackCol()
        {
            BossSound("ATTACK");
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