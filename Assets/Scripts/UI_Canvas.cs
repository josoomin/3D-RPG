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

        public GameObject _player;
        Player _playerScript;

        public bool _breakRock;

        Text _alarmText;
        Text _fKeyText;

        public Text _HPText;
        public Text _ATKText;
        public Text _DEFText;
        public Text _SPDText;

        int _nowMoney;
        float _nowHP;
        float _nowATK;
        float _nowDEF;
        float _nowSPD;

        GameObject _fKey;
        GameObject _talkWindow;

        GameObject _NPCQuestList;
        GameObject _NPCQuestInfoWindow;

        public GameObject _myQuest; // 플레이어의 퀘스트 화면
        public bool _questActive; // 퀘스트 창이 켜져있는지
        GameObject _myQuestInfo; // 플레이어 퀘스트 정보 창
        Text _playerQuestTitle; // 플레이어 퀘스트 정보 창의 퀘스트 이름
        Text _playerQuestInfo; // 플레이어 퀘스트 정보 창의 퀘스트 정보

        public GameObject _contents; // 퀘스트를 담고 있는 게임 오브젝트

        public GameObject _myInventory; // 플레이어 인벤토리
        public bool _inventoryActive; // 인벤토리 창이 켜져있는지

        public GameObject _itemIcons; // 아이템 아이콘들

        public Text _moneyText;
        public Slider _hpBar; // 내 채력바 UI
        public Text _hpText; // 내 체력 표시 택스트

        public GameObject _myState; // 플레이어 스테이터스
        public bool _StateActive; // 스테이터스 창이 켜져있는지

        public GameObject _menu; // 메뉴창

        public bool _playerUIActive; // 플레이어의 UI 창이 켜져 있는지

        public Material _alarmTextMaterial;

        public bool _closeNPC;
        public bool _closeKey;
        public bool _closeTreasureBox;
        public bool _closeRock;

        public bool _talk; // 현재 NPC와 대화중인지

        Text _NPCQuestTitle; // 현재 열린 창의 퀘스트 이름
        Text _NPCQuestInfo; // 현재 열린 창의 퀘스트 정보
        string _openQuestTitle;// 지금 보고 있는 퀘스트 타이틀
        GameObject _openQuest; // 지금 보고 있는 퀘스트의 게임 오브젝트
        [SerializeField] List<GameObject> _allQuestList; // 모든 퀘스트 리스트

        public float fadeDuration = 2f; // 사라지는 데 걸리는 시간 (초)

        private Color originalColor; // 물체의 원래 색상을 저장할 변수

        private void Awake()
        {
            I = this;
        }

        void Start()
        {
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
            _questActive = false;
            _inventoryActive = false;
            _StateActive = false;
            _playerUIActive = false;
            _alarmText.enabled = false;
            _breakRock = false;

            for (int i = 0; i < _contents.transform.childCount; i++)
            {
                GameObject _Quest = _contents.transform.GetChild(i).gameObject;
                _allQuestList.Add(_Quest);
            }
        }

        private void Update()
        {
            _hpBar.value = ((_playerScript._hp / _playerScript._maxHp) * 100);
            _hpText.text = (_playerScript._hp + "/" + _playerScript._maxHp);

            _moneyText.text = _playerScript._money.ToString();
            _HPText.text = _playerScript._maxHp.ToString();
            _ATKText.text = _playerScript._ATK.ToString();
            _DEFText.text = _playerScript._DEF.ToString();
            _SPDText.text = _playerScript._speed.ToString();

            if (_inventoryActive || _questActive || _StateActive)
            {
                _playerUIActive = true;
            }
            else
            {
                _playerUIActive = false;
            }
        }

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

            else if (Player == false)
            {
                _fKey.SetActive(false);
                _closeKey = false;
                _closeTreasureBox = false;
                _closeNPC = false;
                _closeRock = false;
            }
        }

        public void ActiveText(string text)
        {
            _alarmText.text = text;
            _alarmText.material = _alarmTextMaterial;

            StartCoroutine(FadeAway());
        }

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

        public void ActiveTalkWindow()
        {
            _talk = true;
            _talkWindow.SetActive(true);
        }

        public void DeActiveAllWindow()
        {
            _talk = false;
            _talkWindow.SetActive(false);
            _NPCQuestList.SetActive(false);
            _NPCQuestInfoWindow.SetActive(false);
        }

        public void ActiveNPCQuestList()
        {
            _NPCQuestList.SetActive(true);
        }

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
                    Info.text = "중앙섬에 있는 몬스터를 모두 퇴치하자 \n\n 보상: 500골드";
                }

                else if (Title.text == "바위 부수기")
                {
                    Info.text = "마지막 섬을 막는 바위를 부숴라 \n\n 보상: 1000골드";
                }
            }
        }

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

        public void QuestRefuse()
        {
            _NPCQuestInfoWindow.SetActive(false);
        }

        public void QuestComplete()
        {
            if (_openQuestTitle == "몬스터 퇴치" && GameManager.I._monsterList.Count == 0)
            {
                ActiveText("500골드를 획득했습니다.");
                _openQuest.SetActive(false);
                _myQuestInfo.SetActive(false);
                GameManager.I.ClearMonsterQuest();
            }
            else if (_openQuestTitle == "바위 부수기" && _breakRock)
            {
                ActiveText("1000골드를 획득했습니다.");
                _openQuest.SetActive(false);
                _myQuestInfo.SetActive(false);
                GameManager.I.ClearRockQuest();
            }
            else
            {
                ActiveText("아직 퀘스트를 완료하지 않았습니다.");
            }
        }

        public void PlayerUI(GameObject Chan, ref bool Set) // 플레이어의 UI 창을 껐다 켰다 한다.
        {
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
                    _playerScript._money += 100;
                }
                else if (_clickObject.transform.parent.name == "ATK" 
                    && _playerScript._ATK > 1 
                    && _nowATK < _playerScript._ATK)
                {
                    _playerScript._ATK -= 1;
                    _playerScript._money += 100;
                }
                else if (_clickObject.transform.parent.name == "DEF" 
                    && _playerScript._DEF > 0 
                    && _nowDEF < _playerScript._DEF)
                {
                    _playerScript._DEF -= 1;
                    _playerScript._money += 100;
                }
                else if (_clickObject.transform.parent.name == "SPD" 
                    && _playerScript._speed > 1 
                    && _nowSPD < _playerScript._speed)
                {
                    _playerScript._speed -= 1;
                    _playerScript._money += 100;
                }
            }
        }

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

        public void StateCancleButton()
        {
            _playerScript._money = _nowMoney;
            _playerScript._maxHp = _nowHP;
            _playerScript._ATK = _nowATK;
            _playerScript._DEF = _nowDEF;
            _playerScript._speed = _nowSPD;
        }

        public void OpenMenu()
        {
            _menu.SetActive(true);
            Time.timeScale = 0;
        }

        public void CloseMenu()
        {
            _menu.SetActive(false);
            Time.timeScale = 1;
        }

        public void RestartGame()
        {
            GameManager.I.ReStart();
        }
    }
}