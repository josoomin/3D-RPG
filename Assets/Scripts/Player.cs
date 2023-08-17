using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace josoomin
{
    public class Player : MonoBehaviour
    {
        public GameObject _camera;

        public GameObject _closeObject; // 닿고 있는 오브젝트
        public GameObject _reSponePoint; // 떨어지면 다시 스폰되는 위치

        public BoxCollider _attackCol; // 내 검 콜라이더 

        public AudioSource _playerSound; //플레이어가 내는 소리
        public AudioSource _stepSound; // 걸을 때 소리

        public AudioClip _attackClip; // 검 휘두를때 소리
        public AudioClip _jumpClip; // 점프 할 때 나는 소리
        public AudioClip _hitClip; // 맞을때 나는 소리
        public AudioClip _deathClip; // 죽을때 나는 소리

        //public BoxCollider _defandCol; // 내 방패 콜라이더

        public float _hp; // 내 체력
        public float _maxHp; // 내 최대 체력

        public float _ATK; // 공격력

        public float _DEF; //방어력

        public int _money;

        public float _speed; // 내 이동 속도
        float _rotateSpeed = 100.0f; // 내 회전 속도
        float _jumppower = 5.0f; // 내 점프 파워

        bool _plane; // 현재 바닥에 닿았는지 유무 확인
        public bool _defand; // 방어 중인지 확인
        bool _attack; // 내가 공격 중인지
        public bool _die; // 내가 죽었는지

        public GameObject _inven; // 인벤토리
        public List<string> _invenList; // 인벤토리 텍스트 리스트
        public List<string> _quest; // 퀘스트 리스트

        Rigidbody _myRigidbody; // 내 리지드바디

        Animator _myAni; // 내 애니메이터

        private Vector3 dir = Vector3.zero;

        void Start()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            _myAni = GetComponent<Animator>();
            _attackCol.enabled = false;
            //_defandCol.enabled = false;
            _die = false;
            _maxHp = 100;
            _hp = _maxHp;
            _money = 500;
            _ATK = 5f;
            _DEF = 1f;
        }

        void Update()
        {
            if (!GameManager.I._gameClear)
            {
                dir.x = Input.GetAxis("Horizontal");
                dir.z = Input.GetAxis("Vertical");
                dir.Normalize();

                if (UI_Canvas.I._talk == false && !_die)
                {
                    if (!UI_Canvas.I._playerUIActive)
                    {
                        if (!_attack && !_defand)
                        {
                            if (dir != Vector3.zero)
                            {
                                if (!_stepSound.isPlaying && _plane)
                                    _stepSound.Play();

                                Move();
                                _myAni.SetBool("Walk", true);
                            }
                            else
                            {
                                _stepSound.Stop();
                                _myAni.SetBool("Walk", false);
                                _myRigidbody.angularVelocity = new Vector3(0, 0, 0);
                            }

                            if (_plane)
                            {
                                if (Input.GetMouseButtonDown(0))
                                {
                                    Attack();
                                }

                                if (Input.GetMouseButton(1))
                                {
                                    Defand();
                                }

                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    Jump();
                                }
                            }
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
                                _closeObject.GetComponent<Key>().ActiveObject(gameObject);
                            }

                            if (UI_Canvas.I._closeTreasureBox || UI_Canvas.I._closeRock)
                            {
                                InteractionObject();
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        UI_Canvas.I.PlayerUI(UI_Canvas.I._myQuest, ref UI_Canvas.I._questActive);
                    }

                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        UI_Canvas.I.PlayerUI(UI_Canvas.I._myInventory, ref UI_Canvas.I._inventoryActive);
                    }

                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        UI_Canvas.I.PlayerUI(UI_Canvas.I._myState, ref UI_Canvas.I._StateActive);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_Canvas.I.OpenMenu();
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (GameManager.I._gameClear || _die)
                        GameManager.I.ReStart();
                }
            }
        }

        void PlaySound(string action)
        {
            switch (action)
            {
                case "ATTACK":
                    _playerSound.clip = _attackClip;
                    break;
                case "JUMP":
                    _playerSound.clip = _jumpClip;
                    break;
                case "HIT":
                    _playerSound.clip = _hitClip;
                    break;
                case "DIE":
                    _playerSound.clip = _deathClip;
                    break;
            }
            _playerSound.Play();
        }

        public void Move()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;

            Vector3 moveDirection = (cameraForward * dir.z + Camera.main.transform.right * dir.x).normalized;

            //transform.Translate(dir * _speed * Time.deltaTime);

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);

            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }

            //transform.forward = Vector3.Lerp(transform.forward, dir, _rotateSpeed * Time.deltaTime);

            //_myRigidbody.MovePosition(gameObject.transform.position + dir * _speed * Time.deltaTime);

            if (dir.z >= 0)
                transform.Translate(Vector3.forward * Time.deltaTime * _speed * dir.z);
            else if (dir.z <= 0)
                transform.Translate(Vector3.back * Time.deltaTime * _speed * dir.z);

            if (dir.x >= 0)
                transform.Translate(Vector3.forward * Time.deltaTime * _speed * dir.x);
            else if (dir.x <= 0)
                transform.Translate(Vector3.back * Time.deltaTime * _speed * dir.x);
        }

        public void Jump()
        {
            PlaySound("JUMP");
            _myAni.SetTrigger("Jump");
            _myRigidbody.AddForce(Vector3.up * _jumppower, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("GetOffPoint"))
            {
                PlayerReSpone();
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Plane"))
            {
                _plane = true;

                if (collision.gameObject.transform.parent.name == "Plane3")
                {
                    UI_Canvas.I.BossHpOnOff(true);
                }

                else
                {
                    UI_Canvas.I.BossHpOnOff(false);
                }
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
                if (_invenList.Contains("Key"))
                {
                    _invenList.Remove("Key");
                    UI_Canvas.I.ActiveText("망치를 얻었습니다.");
                    _closeObject.GetComponent<TreasureBox>().ActiveObject(gameObject);
                }

                else if (!_invenList.Contains("Key"))
                {
                    UI_Canvas.I.ActiveText("열쇠가 없습니다.");
                    return;
                }
            }

            else if (_closeObject.name == "Rock")
            {
                if (_invenList.Contains("Hammer"))
                {
                    _invenList.Remove("Hammer");
                    UI_Canvas.I.ActiveText("바위를 부쉈습니다.");
                    GameManager.I._breakRockCount += 1;
                    _closeObject.GetComponent<Rock>().ActiveObject(gameObject);

                    for (int i = 0; i < _inven.transform.childCount; i++)
                    {
                        if (_inven.transform.GetChild(i).name == "Hammer")
                        {
                            _inven.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }

                else if (!_invenList.Contains("Hammer"))
                {
                    UI_Canvas.I.ActiveText("망치가 없습니다.");
                    return;
                }
            }
        }

        public void Attack()
        {
            PlaySound("ATTACK");
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
            //_defandCol.enabled = true;
            _myAni.SetBool("Defand", true);
        }

        public void NotDefand()
        {
            _defand = false;
            //_defandCol.enabled = false;
            _myAni.SetBool("Defand", false);
        }

        public void TakeDamage(float Damage)
        {
            PlaySound("HIT");
            NotAttack();
            _myAni.SetTrigger("TakeDamage");
            _hp -= Damage;

            if (_hp < 0)
            {
                _hp = 0;
            }
        }

        void PlayerReSpone()
        {
            TakeDamage(10);
            transform.position = _reSponePoint.transform.position;
        }

        public void Die()
        {
            GameManager.I.GameOver();
            PlaySound("DIE");
            _myAni.SetTrigger("Die");
            _die = true;
        }
    }
}