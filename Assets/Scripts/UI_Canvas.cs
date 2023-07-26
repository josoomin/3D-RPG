using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace josoomin
{
    public class UI_Canvas : MonoBehaviour
    {
        public static UI_Canvas I;

        public Text _alarmText;
        Text _fKeyText;

        public GameObject _player;

        GameObject _fKey;
        GameObject _talkWindow;
        GameObject _questListWindow;
        GameObject _questWindow;
        public GameObject _myQuestWindow; // 플레이어의 퀘스트 화면
        public GameObject _contents; // 퀘스트를 담고 있는 게임 오브젝트
        public Material _alarmTextMaterial;

        public bool _closeNPC;
        public bool _closeKey;
        public bool _closeTreasureBox;
        public bool _closeRock;

        public bool _talk; // 현재 NPC와 대화중인지
        public bool _questWindowActive; // 퀘스트 창이 켜져있는지

        Text _questTitle; // 현재 열린 창의 퀘스트 이름
        Text _questInfo; // 현재 열린 창의 퀘스트 정보
        string _openQuest; // 지금 보고 있는 퀘스트
        [SerializeField] List<GameObject> _allQuestList; // 모든 퀘스트 리스트

        public float fadeDuration = 2f; // 사라지는 데 걸리는 시간 (초)

        private Color originalColor; // 물체의 원래 색상을 저장할 변수

        private void Awake()
        {
            I = this;
        }

        void Start()
        {
            _fKey = transform.Find("TalkKey").gameObject;
            _talkWindow = transform.Find("TalkWindow").gameObject;
            _questListWindow = transform.Find("QuestListWindow").gameObject;
            _questWindow = transform.Find("QuestWindow").gameObject;
            _myQuestWindow = transform.Find("MyQuest").gameObject;
            _fKeyText = transform.Find("TalkKey/Text").GetComponent<Text>();
            _alarmText = transform.Find("AlarmText").GetComponent<Text>();
            _questTitle = _questWindow.transform.Find("Image/Title").GetComponent<Text>();
            _questInfo = _questWindow.transform.Find("Image/Detail").GetComponent<Text>();
            originalColor = _alarmText.material.color;

            _fKey.SetActive(false);
            _talkWindow.SetActive(false);
            _questListWindow.SetActive(false);
            _questWindow.SetActive(false);
            _myQuestWindow.SetActive(false);
            _questWindowActive = false;
            _alarmText.enabled = false;

            for (int i = 0; i < _contents.transform.childCount; i++)
            {
                GameObject _Quest = _contents.transform.GetChild(i).gameObject;
                _allQuestList.Add(_Quest);
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
            _questListWindow.SetActive(false);
            _questWindow.SetActive(false);
        }

        public void ActiveQuestListWindow()
        {
            _questListWindow.SetActive(true);
        }
        
        public void ActiveQuestWindow()
        {
            _questWindow.SetActive(true);

            GameObject _clickObject = EventSystem.current.currentSelectedGameObject;

            if (_clickObject.tag == "QuestButton")
            {
                Text _buttonTitle = _clickObject.transform.Find("Title").GetComponent<Text>();

                _questTitle.text = _buttonTitle.text;
                _openQuest = _questTitle.text;

                if (_questTitle.text == "몬스터 퇴치")
                {
                    _questInfo.text = "중앙섬에 있는 몬스터를 모두 퇴치하자 \n\n 보상: 100골드";
                }

                else if (_questTitle.text == "바위 부수기")
                {
                    _questInfo.text = "마지막 섬을 막는 바위를 부숴라 \n\n 보상: 1000골드";
                }
            }
        }

        public void QuestAccept()
        {
            Player _Player = _player.GetComponent<Player>();
            GameObject _MyQuestPannel = _myQuestWindow.transform.Find("List/Scroll View/Viewport/Content").gameObject;


            for (int i = 0; i < _allQuestList.Count; i++)
            {
                string _QuestTitle = _allQuestList[i].transform.Find("Title").GetComponent<Text>().text;

                if (_QuestTitle == _openQuest)
                {
                    _allQuestList[i].transform.parent = _MyQuestPannel.transform;
                    _Player._quest.Add(_QuestTitle);
                }
            }

            _questWindow.SetActive(false);
        }

        public void QuestRefuse()
        {
            _questWindow.SetActive(false);
        }
    }
}