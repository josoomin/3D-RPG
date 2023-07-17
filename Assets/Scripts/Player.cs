using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace josoomin
{
    public class Player : MonoBehaviour
    {
        public GameObject _closeObject; // 닿고 있는 오브젝트
        public GameObject _reSponePoint; // 떨어지면 다시 스폰되는 위치

        public BoxCollider _attackCol; // 내 검 콜라이더 
        public BoxCollider _defandCol; // 내 방패 콜라이더

        public Slider _hpBar; // 내 채력바 UI
        public Text _hpText; // 내 체력 표시 택스트

        float _hp; // 내 체력
        float _maxHp = 100; // 내 최대 체력

        float _speed = 10f; // 내 이동 속도
        float _rotateSpeed = 100.0f; // 내 회전 속도
        float _jumppower = 5.0f; // 내 점프 파워

        bool _plane; // 현재 바닥에 닿았는지 유무 확인
        bool _defand; // 방어 중인지 확인
        bool _attack; // 내가 공격 중 인지
        bool _die; // 내가 죽었는지

        public List<string> _inven; // 인벤토리

        Rigidbody _myRigidbody; // 내 리지드바디

        Animator _myAni; // 내 애니메이터

        private Vector3 dir = Vector3.zero;

        void Start()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            _myAni = GetComponent<Animator>();
            _attackCol.enabled = false;
            _defandCol.enabled = false;
            _die = false;
            _hp = _maxHp;
        }

        void Update()
        {
            _hpBar.value = _hp;
            _hpText.text = (_hp + "/" + _maxHp);
            //float h = Input.GetAxisRaw("Horizontal");
            //float v = Input.GetAxisRaw("Vertical");

            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            dir.Normalize();

            if (UI_Canvas.I._talk == false && !_die)
            {
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

                if (transform.position.y <= -10)
                {
                    PlayerReSpone();
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
                        _closeObject.GetComponent<Key>().ActiveObject(gameObject);
                    }

                    if (UI_Canvas.I._closeTreasureBox || UI_Canvas.I._closeRock)
                    {
                        InteractionObject();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (UI_Canvas.I._talk == false && !_die)
            {
                if (dir != Vector3.zero)
                {
                    Move();
                    _myAni.SetBool("Walk", true);
                }
                else
                {
                    _myAni.SetBool("Walk", false);
                    _myRigidbody.angularVelocity = new Vector3(0, 0, 0);
                }
            }
        }

        public void Move()
        {
            //지금 바라보는 방향의 부호 != 나아갈 방향 부호
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }

            transform.forward = Vector3.Lerp(transform.forward, dir, _rotateSpeed* Time.deltaTime);

            _myRigidbody.MovePosition(gameObject.transform.position + dir * _speed * Time.deltaTime);
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

        void InteractionObject()
        {
            if (_closeObject.name == "TreasureBox")
            {
                if (_inven.Contains("Key"))
                {
                    _inven.Remove("Key");
                    UI_Canvas.I.ActiveText("망치를 얻었습니다.");
                    _closeObject.GetComponent<TreasureBox>().ActiveObject(gameObject);
                }

                else if (!_inven.Contains("Key"))
                {
                    UI_Canvas.I.ActiveText("열쇠가 없습니다.");
                    return;
                }
            }

            else if (_closeObject.name == "Rock")
            {
                if (_inven.Contains("망치"))
                {
                    _inven.Remove("망치");
                    UI_Canvas.I.ActiveText("바위를 부쉈습니다.");
                    _closeObject.GetComponent<Rock>().ActiveObject(gameObject);
                }

                else if (!_inven.Contains("망치"))
                {
                    UI_Canvas.I.ActiveText("망치가 없습니다.");
                    return;
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
        }

        void PlayerReSpone()
        {
            TakeDamage(10);
            transform.position = _reSponePoint.transform.position;
        }

        public void Die()
        {
            _myAni.SetTrigger("Die");
            _die = true;
        }
    }
}