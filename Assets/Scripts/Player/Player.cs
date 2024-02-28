using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace josoomin
{
    public class Player : MonoBehaviour
    {
        public GameObject _camera; // 카메라 게임오브젝트

        public GameObject _closeObject; // 닿고 있는 오브젝트
        public GameObject _reSponePoint; // 떨어지면 다시 스폰되는 위치

        public BoxCollider _attackCol; // 내 검 콜라이더 

        public AudioSource _playerSound; //플레이어가 내는 소리
        public AudioSource _stepSound; // 걸을 때 소리

        public AudioClip _attackClip; // 검 휘두를때 소리
        public AudioClip _jumpClip; // 점프 할 때 나는 소리
        public AudioClip _hitClip; // 맞을때 나는 소리
        public AudioClip _deathClip; // 죽을때 나는 소리

        public float _hp; // 내 체력
        public float _maxHp; // 내 최대 체력

        public float _ATK; // 공격력

        public float _DEF; //방어력

        public int _money; // 보유 돈

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

        private Vector3 dir = Vector3.zero; // 캐릭터 이동 값

        void Start()
        {
            // 각 기본값 초기화
            _myRigidbody = GetComponent<Rigidbody>();
            _myAni = GetComponent<Animator>();
            _attackCol.enabled = false;
            _die = false;
            _maxHp = 100;
            _hp = _maxHp;
            _money = 500;
            _ATK = 5f;
            _DEF = 1f;
            _speed = 5f;
            transform.position = _reSponePoint.transform.position;
        }

        void Update()
        {
            dir.x = Input.GetAxis("Horizontal"); // 좌 우 이동
            dir.z = Input.GetAxis("Vertical"); // 앞 뒤 이동
            dir.Normalize(); // 대각선 이동 속도를 위한 백터의 정규화

            if (!_die && !GameManager.I._gameClear)
            {
                if (!UI_Canvas.I._playerUIActive)
                {
                    if (!_attack && !_defand)
                    {
                        // 방향키 입력이 있으면 캐릭터 이동
                        if (dir != Vector3.zero)
                        {
                            if (!_stepSound.isPlaying && _plane)
                                _stepSound.Play();

                            Move();
                            _myAni.SetBool("Walk", true);
                        }
                        // 방향키 입력이 없으면 캐릭터 정지
                        else
                        {
                            StopWalk();
                        }

                        // 캐릭터가 바닥에 닿아 있을 때만 공격, 방어, 이동 가능
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

                    // 왼쪽 마우스 놓으면 방어 취소
                    if (Input.GetMouseButtonUp(1))
                    {
                        NotDefand();
                    }

                    // NPC, 열쇠, 보물상자 상호작용
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (UI_Canvas.I._closeNPC)
                        {
                            StopWalk();
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

                    // 죽음
                    if (_hp <= 0)
                    {
                        Die();
                    }
                }

                // 퀘스트창
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StopWalk();
                    UI_Canvas.I.PlayerUI(UI_Canvas.I._myQuest, ref UI_Canvas.I._questActive);
                }

                // 인벤토리
                if (Input.GetKeyDown(KeyCode.I))
                {
                    StopWalk();
                    UI_Canvas.I.PlayerUI(UI_Canvas.I._myInventory, ref UI_Canvas.I._inventoryActive);
                }

                // 스텟창
                if (Input.GetKeyDown(KeyCode.C))
                {
                    StopWalk();
                    UI_Canvas.I.PlayerUI(UI_Canvas.I._myState, ref UI_Canvas.I._StateActive);
                }
            }

            // 메뉴창
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!UI_Canvas.I._menuBool)
                {
                    StopWalk();
                    UI_Canvas.I.OpenMenu();
                }

                else if (UI_Canvas.I._menuBool)
                {
                    UI_Canvas.I.CloseMenu();
                }
            }

            // 재시작
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (GameManager.I._gameClear || _die)
                    GameManager.I.ReStart();
            }
        }

        // 걷는 도중 애니메이션, 효과음, 미끄러짐을 동작을 방지하기 위한 함수
        void StopWalk()
        {
            _stepSound.Stop();
            _myAni.SetBool("Walk", false);
            _myRigidbody.angularVelocity = new Vector3(0, 0, 0);
        }

        // 플레이어 효과음 재생
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

        // 카메라 방향과 조작키에 따른 캐릭터 이동
        public void Move()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;

            Vector3 moveDirection = (cameraForward * dir.z + Camera.main.transform.right * dir.x).normalized;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);

            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }

            if (dir.z >= 0)
                transform.Translate(Vector3.forward * Time.deltaTime * _speed * dir.z);
            else if (dir.z <= 0)
                transform.Translate(Vector3.back * Time.deltaTime * _speed * dir.z);

            if (dir.x >= 0)
                transform.Translate(Vector3.forward * Time.deltaTime * _speed * dir.x);
            else if (dir.x <= 0)
                transform.Translate(Vector3.back * Time.deltaTime * _speed * dir.x);
        }

        // 점프
        public void Jump()
        {
            PlaySound("JUMP");
            _myAni.SetTrigger("Jump");
            _myRigidbody.AddForce(Vector3.up * _jumppower, ForceMode.Impulse);
        }

        // 추락 시 캐릭터 리스폰
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("GetOffPoint"))
            {
                PlayerReSpone();
            }
        }

        // 캐릭터가 땅에 닿아 있는지 판별
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

        // 캐릭터가 땅에 안 닿아 있는지 판별
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Plane"))
            {
                _plane = false;
            }
        }

        // 상호 작용 결과에 따라 중앙 상단에 매세지 표시
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

        // 공격 시 검 콜라이더 활성화
        public void Attack()
        {
            PlaySound("ATTACK");
            _attack = true;
            _attackCol.enabled = true;
            _myAni.SetTrigger("Attack");
        }

        // 공격 종료 시 검 콜라이더 비활성화
        public void NotAttack()
        {
            _attack = false;
            _attackCol.enabled = false;
        }
        
        // 방어
        public void Defand()
        {
            _defand = true;
            _myAni.SetBool("Defand", true);
        }

        // 방어 종료
        public void NotDefand()
        {
            _defand = false;
            _myAni.SetBool("Defand", false);
        }

        // 데미지 받음
        public void TakeDamage(float Damage)
        {
            PlaySound("HIT");
            NotAttack();
            _myAni.SetTrigger("TakeDamage");

            if (_defand)
            {
                float _dmg = Damage - _DEF;

                if (_dmg < 0)
                    _hp -= 0;

                else
                    _hp -= _dmg;
            }

            else
            _hp -= Damage;

            if (_hp < 0)
            {
                _hp = 0;
            }
        }

        // 리스폰
        void PlayerReSpone()
        {
            TakeDamage(10);
            transform.position = _reSponePoint.transform.position;
        }

        // 사망
        public void Die()
        {
            GameManager.I.GameOver();
            PlaySound("DIE");
            _myAni.SetTrigger("Die");
            _die = true;
        }
    }
}