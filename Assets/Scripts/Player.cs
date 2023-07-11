using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace josoomin
{
    public class Player : MonoBehaviour
    {
        public GameObject _closeObject; // 닿고 있는 오브젝트

        public BoxCollider _attackCol; // 내 검 콜라이더 
        public BoxCollider _defandCol; // 내 방패 콜라이더

        public Slider _hpBar; // 내 채력바 UI
        public Text _hpText; // 내 체력 표시 택스트
        public float _hp; // 내 체력
        public float _maxHp = 100; // 내 최대 체력

        float _speed = 10.0f; // 내 이동 속도
        float _rotateSpeed = 300.0f; // 내 회전 속도
        float _jumppower = 5.0f; // 내 점프 파워

        [SerializeField] bool _plane; // 현재 바닥에 닿았는지 유무 확인
        bool _defand; // 방어 중인지 확인
        bool _attack; // 내가 공격 중 인지
        bool _die; // 내가 죽었는지

        public List<string> _inven; // 인벤토리

        Rigidbody _myRigidbody; // 내 리지드바디

        Animator _myAni; // 내 애니메이터

        void Start()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            _myAni = GetComponent<Animator>();
            _attackCol.enabled = false;
            _defandCol.enabled = false;
            _die = false;
            _hp = _maxHp;
            _hpText.text = (_hp + "/" + _maxHp);
        }

        void Update()
        {
            _hpBar.value = _hp;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (UI_Canvas.I._talk == false && !_die)
            {
                if (h != 0 || v != 0)
                {
                    Move(h, v);
                    _myAni.SetBool("Walk", true);
                }
                else
                {
                    _myAni.SetBool("Walk", false);
                    _myRigidbody.angularVelocity = new Vector3(0, 0, 0);
                }

                if (Input.GetKeyDown(KeyCode.Space) && _plane)
                {
                    Jump();
                }

                if (Input.GetMouseButtonDown(0) && !_defand)
                {
                    Attack();
                }

                if (Input.GetMouseButton(1) && !_attack)
                {
                    Defand();
                }

                if (Input.GetMouseButtonUp(1))
                {
                    NotDefand();
                }

                if (_hp <= 0)
                {
                    Die();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (UI_Canvas.I._closeNPC)
                    {
                        UI_Canvas.I.ActiveTalkWindow();
                    }

                    if (UI_Canvas.I._closeKey)
                    {
                        _closeObject.GetComponent<Key>().DestroyMe(gameObject);
                    }

                    if (UI_Canvas.I._closeTreasureBox)
                    {
                        GetTreasureBox();
                    }
                }
            }
        }
        public void Move(float h, float v)
        {
            transform.Translate(new Vector3(0, 0, v * _speed) * Time.deltaTime);
            transform.Rotate(new Vector3(0, h * _rotateSpeed, 0) * Time.deltaTime);
        }

        public void Jump()
        {
            _myAni.SetTrigger("Jump");
            _myRigidbody.AddForce(Vector3.up * _jumppower, ForceMode.Impulse);
        }

        void OnCollisionStay (Collision collision)
        {
            if (collision.gameObject.CompareTag("Plane"))
            {
                _plane = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Plane"))
            {
                _plane = false;
            }
        }

        void GetTreasureBox()
        {
            for (int i = 0; i < _inven.Count; i++)
            {
                if (_inven[i] == "Key")
                {
                    _inven.RemoveAt(i);
                    _closeObject.GetComponent<TreasureBox>().OpenMe(gameObject);
                    break;
                }

                else
                {
                    UI_Canvas.I.ActiveNoKey();
                    break;
                }
            }
        }

        public void Attack()
        {
            _attack = true;
            _attackCol.enabled = true;
            _myAni.SetTrigger("Attack");
        }

        public void NotAttack()
        {
            _attack = false;
            _attackCol.enabled = false;
        }

        public void Defand()
        {
            _defand = true;
            _defandCol.enabled = true;
            _myAni.SetBool("Defand", true);
        }

        public void NotDefand()
        {
            _defand = false;
            _defandCol.enabled = false;
            _myAni.SetBool("Defand", false);
        }

        public void TakeDamage(float Damage)
        {
            _myAni.SetTrigger("TakeDamage");
            _hp -= Damage;
            _hpText.text = (_hp + "/" + _maxHp);
        }

        public void Die()
        {
            _myAni.SetTrigger("Die");
            _die = true;
        }
    }
}