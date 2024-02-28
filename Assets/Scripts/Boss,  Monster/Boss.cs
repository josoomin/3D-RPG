using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Boss : MonoBehaviour
    {
        public Transform _firePoint; // ȭ�� �߻� ����

        public GameObject _fireBall; // ȭ����
        public List<GameObject> _fireBallList; // ȭ���� ����Ʈ
        public Transform _fierPool; // ȭ���� ���� ������Ʈ Ǯ�� ��ġ

        Rigidbody _myRigidbody; // ���� ������ �ٵ�
        Transform target; // ���� Ÿ��(�÷��̾�)
        Animator _myAni; // ���� �ִϸ�����

        public AudioSource _bossSound; // ���� ����ҽ�

        public AudioClip _attackClip1; // ���� ���� ����
        public AudioClip _attackClip2; // ���Ÿ� ���� ����
        public AudioClip _deathClip; // ���� ��� ����

        public BoxCollider _myAttackTrigger; // ���� ���� ���� �ݶ��̴�
        float _attackLange; // ���� ���� ��Ÿ�
        float _shortAttack = 2f; // ���� ���� ��Ÿ�
        float _longAttack = 10f; // ���Ÿ� ���� ��Ÿ�

        public float _maxHP = 200; // ���� ����, �ִ� ü��
        public float _hp; // ���� ���� ü��

        bool _nowAttack; // ������ ���� ������ �� �ִ� �������� �Ǻ�
        bool _takeDamage; // ���� ������ �޴� �ִϸ��̼� ���� ��
        bool _die; // ���

        int _pattern; // ���� �������� ���Ÿ� �������� ���� ����
        bool _setPattern; // ���� ������ ������ �� �ִ� �������� �Ǻ�

        private Vector3 targetPosition; // Ÿ��(�÷��̾�) ������

        [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f; //  �̵� �ӵ�

        [SerializeField] [Range(0f, 100f)] float contactDistance = 50f; // �÷��̾� ���� �Ÿ�

        void Start()
        {
            SetPattern(); // ���� ���� ����

            // ȭ���� 100�� ���� �� ������Ʈ Ǯ��
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

            // �⺻�� �ʱ�ȭ
            _hp = _maxHP;

            _myAttackTrigger.enabled = false;
            _nowAttack = false;
            _die = false;
        }

        void Update()
        {
            // ���� �ʾҰų� ������ �޴� �� �ƴϸ� Ÿ�� ����
            if (!_die && !_takeDamage)
            {
                FollowTarget();

                if (_hp <= 0)
                {
                    Die();
                }
            }
        }

        // ���� ���� ���
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

        // Ÿ�� ����
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

        // ���� ���� ����
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

        // ����
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

        // ���� �ݶ��̴� Ȱ��ȭ
        void OnAttackCol()
        {
            BossSound("ATTACK1");
            _myAttackTrigger.enabled = true;
        }

        // ���� �ݶ��̴� ��Ȱ��ȭ
        void OffAttackCol()
        {
            _myAttackTrigger.enabled = false;
            _nowAttack = false;
            _setPattern = true;

            SetPattern();
        }

        // ȭ���� �߻�
        void FireBall()
        {
            _fireBallList[0].SetActive(true);
            _fireBallList[0].transform.position = _firePoint.position;
            _fireBallList.RemoveAt(0);
            _nowAttack = false;
            _setPattern = true;

            SetPattern();
        }

        // �߶���
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("GetOffPoint"))
                TakeDamage(200);
        }

        // ������ ����
        public void TakeDamage(float damage)
        {
            _hp -= damage;
            _myAttackTrigger.enabled = false;
        }

        // ���
        void Die()
        {
            BossSound("DIE");
            _die = true;
            _myAni.SetTrigger("Die");
            _myAttackTrigger.enabled = false;
        }

        // ��� �ִϸ��̼� �� �� �ڽ��� �ı�
        void DestroyMe()
        {
            Destroy(gameObject);
            GameManager.I.GameClear();
        }
    }
}