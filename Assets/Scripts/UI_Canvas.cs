using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace josoomin
{
    public class UI_Canvas : MonoBehaviour
    {
        public static UI_Canvas I;

        public GameObject _Setting; // 세팅 메누

        float _nowBGMLevel; // 현재 배경음 레밸
        float _nowSFXLevel; // 현재 효과음 레밸

        public Slider _BGMSlider; // 배경음 조절 슬라이더
        public Slider _SFXSlider; // 효과음 조절 슬라이더

        public Text _BGMText; // 현재 배경음 지수를 표시하는 텍스트
        public Text _SFXText; // 현재 효과음 지수를 표시하는 텍스트

        public GameObject _player; // 플레이어 게임 오브젝트
        Player _playerScript; // 플레이어 게임 오브젝트 스크립트

        public bool _breakRock; // 돌이 부숴졌는지 판단

        Text _alarmText; // 화면 상단 중앙 알림 텍스트
        Text _fKeyText; // 화면 중앙 하단 상호작용 가능 텍스트

        public Text _HPText; // 상태창 체력 표시 텍스트
        public Text _ATKText; // 상태창 공격력 표시 텍스트
        public Text _DEFText; // 상태창 방어력 표시 텍스트
        public Text _SPDText; // 상태창 스피드 표시 텍스트

        // 능력치 적용 버튼을 누르기 전 값을 저장하기 위한 값
        int _nowMoney; // 현재 보유 돈
        float _nowHP; // 현재 체력
        float _nowATK; // 현재 공격력
        float _nowDEF; // 현재 방어력
        float _nowSPD; // 현재 스피드

        GameObject _fKey; // 상호작용키 게임 오브젝트
        GameObject _talkWindow; // 대화창 게임 오브젝트

        GameObject _NPCQuestList; // NPC 퀘스트 리스트
        GameObject _NPCQuestInfoWindow; // 퀘스트 정보 표시창

        public GameObject _myQuest; // 플레이어의 퀘스트 화면
        public bool _questActive; // 퀘스트 창이 켜져있는지
        GameObject _myQuestInfo; // 플레이어 퀘스트 정보 창
        Text _playerQuestTitle; // 플레이어 퀘스트 정보 창의 퀘스트 이름
        Text _playerQuestInfo; // 플레이어 퀘스트 정보 창의 퀘스트 정보

        public GameObject _contents; // 퀘스트를 담고 있는 게임 오브젝트

        public GameObject _myInventory; // 플레이어 인벤토리
        public bool _inventoryActive; // 인벤토리 창이 켜져있는지

        public GameObject _itemIcons; // 아이템 아이콘들

        public Text _moneyText; // 내 돈
        public Slider _hpBar; // 내 채력바 UI
        public Text _hpText; // 내 체력 표시 택스트

        public Boss _Boss; // 보스 몬스터 스크립트
        public Slider _bossHpBar; //보스 체력바

        public GameObject _myState; // 플레이어 스테이터스
        public bool _StateActive; // 스테이터스 창이 켜져있는지

        public GameObject _menu; // 메뉴창

        public bool _playerUIActive; // 플레이어의 UI 창이 켜져 있는지

        public Material _alarmTextMaterial; // 알람 텍스트 마테리얼

        public bool _closeNPC; // 현재 NPC와 붙어있는지 확인하기 위한 값
        public bool _closeKey; // 현재 열쇠와 닿아있는지 판단하기 위한 값
        public bool _closeTreasureBox; // 현재 보물상자와 닿아있는지 판단하기 위한 값
        public bool _closeRock; // 현재 바위와 닿아있는지 판단하기 위한 값
        public bool _menuBool; // 현재 메뉴창이 켜져있는지 판단하기 위한 값

        public bool _talk; // 현재 NPC와 대화중인지

        Text _NPCQuestTitle; // 현재 열린 창의 퀘스트 이름
        Text _NPCQuestInfo; // 현재 열린 창의 퀘스트 정보
        string _openQuestTitle;// 지금 보고 있는 퀘스트 타이틀
        GameObject _openQuest; // 지금 보고 있는 퀘스트의 게임 오브젝트
        [SerializeField] List<GameObject> _allQuestList; // 모든 퀘스트 리스트

        public float fadeDuration = 2f; // 사라지는 데 걸리는 시간 (초)

        private Color originalColor; // 물체의 원래 색상을 저장할 변수

        // UI_Canvas 게임오브젝트 싱글톤
        private void Awake()
        {
            I = this;
        }

        void Start()
        {
            // 각각의 inspector 및 게임오브젝트를 가져온다.
            _playerScript = _player.GetComponent<Player>();
            _fKey = transform.Find("TalkKey").gameObject; 
            _talkWindow = transform.Find("TalkWindow").gameObject;
            _NPCQuestList = transform.Find("QuestListWindow").gameObject;
            _NPCQuestInfoWindow = transform.Find("QuestWindow").gameObject;
            _myQuest = transform.Find("MyQuest").gameObject;
            _myQuestInfo = _myQuest.transform.Find("QuestInfoWindow").gameObject;
            _playerQuestTitle = _myQuestInfo.transform.Find("Image/Title").GetComponent<Text>();
            _playerQuestInfo = _myQuestInfo.transform.Find("Image/Detail").GetComponent<Text>();
            _fKeyText = transform.Find("TalkKey/Text").GetComponent<Text>();
            _alarmText = transform.Find("AlarmText").GetComponent<Text>();
            _NPCQuestTitle = _NPCQuestInfoWindow.transform.Find("Image/Title").GetComponent<Text>();
            _NPCQuestInfo = _NPCQuestInfoWindow.transform.Find("Image/Detail").GetComponent<Text>();
            _menu = transform.Find("UI_Menu").gameObject;
            originalColor = _alarmText.material.color;

            // 실행시 불필요한 게임오브젝트를 false로 초기화
            _fKey.SetActive(false);
            _talkWindow.SetActive(false);
            _NPCQuestList.SetActive(false);
            _NPCQuestInfoWindow.SetActive(false);
            _myQuest.SetActive(false);
            _myQuestInfo.SetActive(false);
            _myInventory.SetActive(false);
            _myState.SetActive(false);
            _itemIcons.SetActive(false);
            _menu.SetActive(false);
            _bossHpBar.gameObject.SetActive(false);
            _Setting.SetActive(false);

            // 모든 Bool값을 false로 초기화
            _menuBool = false;
            _questActive = false;
            _inventoryActive = false;
            _StateActive = false;
            _playerUIActive = false;
            _alarmText.enabled = false;
            _breakRock = false;

            // 퀘스트 게임오브젝트를 가져와 게임오브젝트 리스트에 추가
            for (int i = 0; i < _contents.transform.childCount; i++)
            {
                GameObject _Quest = _contents.transform.GetChild(i).gameObject;
                _allQuestList.Add(_Quest);
            }
        }

        private void Update()
        {
            // 현재 체력바와 텍스트를 프레임마다 초기화
            _hpBar.value = ((_playerScript._hp / _playerScript._maxHp) * 100);
            _hpText.text = (_playerScript._hp + "/" + _playerScript._maxHp);

            // 보스 체력바를 프레임마다 초기화
            _bossHpBar.value = ((_Boss._hp / _Boss._maxHP) * 100);

            // 현재 돈, 체력, 공격력, 방여력, 스피드를 프레임마다 초기화
            _moneyText.text = _playerScript._money.ToString();
            _HPText.text = _playerScript._maxHp.ToString();
            _ATKText.text = _playerScript._ATK.ToString();
            _DEFText.text = _playerScript._DEF.ToString();
            _SPDText.text = _playerScript._speed.ToString();

            // 현재 배경음과 효과음 텍스트를 슬라이더 값에 따라 프레임마다 초기화
            _BGMText.text = Mathf.FloorToInt(_BGMSlider.value * 100).ToString();
            _SFXText.text = Mathf.FloorToInt(_SFXSlider.value * 100).ToString();

            // 현재 대화UI, 인벤토리UI, 퀘스트UI, 상태창UI, 메뉴UI가 켜져있으면 다른 UI는 킬 수 없게 불 값을 초기화
            if (_talk || _inventoryActive || _questActive || _StateActive || _menuBool)
            {
                _playerUIActive = true;
            }
            else
            {
                _playerUIActive = false;
            }
        }

        // 현재 닿고 있는 맵 오브젝트에 따라 상호작용 텍스트 및 닿은 오브젝트의 bool값 초기화 및 텍스트 표시
        public void CloseMapObject(string _object, bool Player)
        {
            if (Player == true)
            {
                if (_object == "Key")
                {
                    _fKeyText.text = "줍기";
                    _closeKey = true;
                }

                else if (_object == "TreasureBox")
                {
                    _fKeyText.text = "열기";
                    _closeTreasureBox = true;
                }

                else if (_object == "NPC")
                {
                    _fKeyText.text = "대화";
                    _closeNPC = true;
                }

                else if (_object == "Rock")
                {
                    _fKeyText.text = "파괴";
                    _closeRock = true;
                }

                _fKey.SetActive(true);
            }

            // 닿은 오브젝트 없으면 전부 false로 초기화
            else if (Player == false)
            {
                _fKey.SetActive(false);
                _closeKey = false;
                _closeTreasureBox = false;
                _closeNPC = false;
                _closeRock = false;
            }
        }

        // 상단 텍스트 표시
        public void ActiveText(string text)
        {
            _alarmText.text = text;
            _alarmText.material = _alarmTextMaterial;

            StartCoroutine(FadeAway());
        }

        // 상단 텍스트 표시 됬다 서서히 사라지게 함
        IEnumerator FadeAway()
        {
            var color = _alarmText.color;

            _alarmText.enabled = true;
            _alarmText.material.color = new Color(color.r, color.g, color.b, 255f);

            yield return new WaitForSeconds(1);

            // 시작 시간을 저장합니다.
            float startTime = Time.time;

            // 물체가 사라지는 동안의 처리
            while (Time.time < startTime + fadeDuration)
            {
                // 현재 시간에서 시작 시간을 뺀 시간의 비율을 계산합니다.
                float t = (Time.time - startTime) / fadeDuration;

                // 색상의 알파값을 서서히 줄여 물체를 사라지게 합니다.
                Color fadeColor = new Color(color.r, color.g, color.b, 1 - t);
                _alarmText.material.color = fadeColor;

                // 한 프레임씩 대기합니다.
                yield return null;
            }

            // Coroutine이 끝났을 때, 물체의 색상을 완전히 투명하게 만듭니다.
            _alarmText.enabled = false;
        }

        // 대화창 활성화
        public void ActiveTalkWindow()
        {
            _talk = true;
            _talkWindow.SetActive(true);
        }

        // 대화창 비활성화
        public void DeActiveAllWindow()
        {
            _talk = false;
            _talkWindow.SetActive(false);
            _NPCQuestList.SetActive(false);
            _NPCQuestInfoWindow.SetActive(false);
        }

        // NPC가 보유중인 퀘스트 리스트 활성화
        public void ActiveNPCQuestList()
        {
            _NPCQuestList.SetActive(true);
        }

        // 퀘스트 정보창 활성화
        public void ActiveQuestInfo()
        {
            GameObject _clickObject = EventSystem.current.currentSelectedGameObject;

            string _click4ParantName = _clickObject.transform.parent.parent.parent.parent.name;

            if (_click4ParantName == "QuestListWindow")
            {
                _NPCQuestInfoWindow.SetActive(true);
                SetInfo(_clickObject, _NPCQuestTitle, _NPCQuestInfo);

            }
            else if (_click4ParantName == "MyQuest")
            {
                _myQuestInfo.SetActive(true);
                SetInfo(_clickObject, _playerQuestTitle, _playerQuestInfo);
            }
        }

        // 누르는 퀘스트의 종류에 따라 정보창 내용 변경
        void SetInfo(GameObject Click, Text Title, Text Info)
        {
            if (Click.tag == "QuestButton")
            {
                Text _buttonTitle = Click.transform.Find("Title").GetComponent<Text>();

                Title.text = _buttonTitle.text;
                _openQuestTitle = Title.text;
                _openQuest = Click;

                if (Title.text == "몬스터 퇴치")
                {
                    Info.text = "중앙섬에 있는 몬스터를 모두 퇴치하세요 \n\n 보상: 500골드";
                }

                else if (Title.text == "바위 부수기")
                {
                    Info.text = "마지막 섬을 막는 바위를 파괴하세요 \n\n 보상: 1000골드";
                }
            }
        }

        // 퀘스트 수락시 NPC 퀘스트리스트에서 제외 후 플레이어 퀘스트 보유 리스트에 추가
        public void QuestAccept()
        {
            GameObject _MyQuestPannel = _myQuest.transform.Find("Scroll View/Viewport/Content").gameObject;

            for (int i = 0; i < _allQuestList.Count; i++)
            {
                string _QuestTitle = _allQuestList[i].transform.Find("Title").GetComponent<Text>().text;

                if (_QuestTitle == _openQuestTitle)
                {
                    _allQuestList[i].transform.parent = _MyQuestPannel.transform;
                    _playerScript._quest.Add(_QuestTitle);
                }
            }

            _NPCQuestInfoWindow.SetActive(false);
        }

        // 퀘스트 거절 시 퀘스트 정보창 비활성화
        public void QuestRefuse()
        {
            _NPCQuestInfoWindow.SetActive(false);
        }

        // 퀘스트 완료 시 중앙 상단에 보상 텍스트 표시 및 보상 지급
        public void QuestComplete()
        {
            if (_openQuestTitle == "몬스터 퇴치" && GameManager.I._monsterList.Count == 0)
            {
                ActiveText("500골드를 획득했습니다.");
                CloseQuestWindow();
                GameManager.I.ClearQuest(500);
            }
            else if (_openQuestTitle == "바위 부수기" && _breakRock)
            {
                ActiveText("1000골드를 획득했습니다.");
                CloseQuestWindow();
                GameManager.I.ClearQuest(1000);
            }
            else
            {
                ActiveText("아직 퀘스트를 완료하지 않았습니다.");
            }
        }

        // 플레이어 퀘스트 창 비활성화
        void CloseQuestWindow()
        {
            _openQuest.SetActive(false);
            _myQuestInfo.SetActive(false);
        }

        // 입려키에 따라 UI 활성화 및 비활성화
        public void PlayerUI(GameObject Chan, ref bool Set) 
        {
            // 열린 UI가 상태창인 경우 현재 능력치를 상태창에 적용
            if (Chan.name == "CharacterStats")
            {
                _nowMoney = _playerScript._money;
                _nowHP = _playerScript._maxHp;
                _nowATK = _playerScript._ATK;
                _nowDEF = _playerScript._DEF;
                _nowSPD = _playerScript._speed;
            }

            if (!_playerUIActive)
            {
                Set = true;
                Chan.SetActive(true);
            }

            else if (Set)
            {
                Set = false;
                Chan.SetActive(false);

                if (Chan.name == "MyQuest")
                {
                    _myQuestInfo.SetActive(false);
                }

                if (Chan.name == "CharacterStats")
                {
                    StateCancleButton();
                }
            }
        }

        // 상태창 +버튼 누를시 체력, 공격력, 방어력, 스피드, 돈 텍스트 변경
        public void StateUp()
        {
            GameObject _clickObject = EventSystem.current.currentSelectedGameObject;

            if (_clickObject.tag == "PlusButton" && _playerScript._money >= 100)
            {
                _playerScript._money -= 100;
                if (_clickObject.transform.parent.name == "MAXHP")
                {
                    _playerScript._maxHp += 10;
                }
                else if (_clickObject.transform.parent.name == "ATK")
                {
                    _playerScript._ATK += 1;
                }
                else if (_clickObject.transform.parent.name == "DEF")
                {
                    _playerScript._DEF += 1;
                }
                else if (_clickObject.transform.parent.name == "SPD")
                {
                    _playerScript._speed += 1;
                }
            }
        }

        // 상태창 +버튼 누를시 체력, 공격력, 방어력, 스피드, 돈 텍스트 변경
        public void StateDown()
        {
            GameObject _clickObject = EventSystem.current.currentSelectedGameObject;

            if (_clickObject.tag == "MinusButton")
            {
                if (_clickObject.transform.parent.name == "MAXHP" 
                    && _playerScript._maxHp > 10 
                    && _nowHP < _playerScript._maxHp)
                {
                    _playerScript._maxHp -= 10;
                    ReturnMoney();
                }
                else if (_clickObject.transform.parent.name == "ATK" 
                    && _playerScript._ATK > 1 
                    && _nowATK < _playerScript._ATK)
                {
                    _playerScript._ATK -= 1;
                    ReturnMoney();
                }
                else if (_clickObject.transform.parent.name == "DEF" 
                    && _playerScript._DEF > 0 
                    && _nowDEF < _playerScript._DEF)
                {
                    _playerScript._DEF -= 1;
                    ReturnMoney();
                }
                else if (_clickObject.transform.parent.name == "SPD" 
                    && _playerScript._speed > 1 
                    && _nowSPD < _playerScript._speed)
                {
                    _playerScript._speed -= 1;
                    ReturnMoney();
                }
            }
        }

        // -버튼 누를 시 돈 반환
        void ReturnMoney()
        {
            _playerScript._money += 100;
        }

        // 상태창 체력, 공격력, 방어력, 스피드, 돈 변환값 적용
        public void StateApplyButton()
        {
            if (_nowHP != _playerScript._maxHp)
            {
                _playerScript._hp = _playerScript._maxHp;
            }

            _nowMoney = _playerScript._money;
            _nowHP = _playerScript._maxHp;
            _nowATK = _playerScript._ATK;
            _nowDEF = _playerScript._DEF;
            _nowSPD = _playerScript._speed;
        }

        // 상태창 체력, 공격력, 방어력, 스피드, 돈 변환값 원상복귀
        public void StateCancleButton()
        {
            _playerScript._money = _nowMoney;
            _playerScript._maxHp = _nowHP;
            _playerScript._ATK = _nowATK;
            _playerScript._DEF = _nowDEF;
            _playerScript._speed = _nowSPD;
        }

        // 메뉴창 활성화
        public void OpenMenu()
        {
            _menuBool = true;
            _menu.SetActive(true);
            Time.timeScale = 0;
        }

        // 메뉴창 비활성화
        public void CloseMenu()
        {
            _menuBool = false;
            _menu.SetActive(false);
            Time.timeScale = 1;
            CloseSetting();
        }

        // 세팅창 활성화 및 현재 배경음과 효과음 값 슬라이더에 적용
        public void OpenSetting()
        {
            _nowBGMLevel = GameManager.I._BGM.volume;
            _nowSFXLevel = GameManager.I._objectSound.volume;

            _BGMSlider.value = _nowBGMLevel;
            _SFXSlider.value = _nowSFXLevel;

            _Setting.SetActive(true);
        }

        // 변경된 세팅값 적용
        public void SetSound()
        {
            _nowBGMLevel = _BGMSlider.value;
            _nowSFXLevel = _SFXSlider.value;

            GameManager.I.SetSoundLevel(_nowBGMLevel, _nowSFXLevel);
        }

        // 세팅창 비활성화
        public void CloseSetting()
        {
            _Setting.SetActive(false);
        }

        // 게임 재시작 버튼
        public void RestartGame()
        {
            GameManager.I.ReStart();
        }

        // 게임 종료 버튼
        public void ShutDown()
        {
            Application.Quit();
        }

        // 3스테이지 진입 시 보스 체력바 UI 활성화
        public void BossHpOnOff(bool _b)
        {
            _bossHpBar.gameObject.SetActive(_b);
        }
    }
}