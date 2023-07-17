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

        Text _alarmText;
        Text _fKeyText;

        [SerializeField] GameObject _fKey;
        [SerializeField] GameObject _talkWindow;
        [SerializeField] GameObject _questListWindow;
        [SerializeField] GameObject _questWindow;

        [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;

        public bool _closeNPC;
        public bool _closeKey;
        public bool _closeTreasureBox;
        public bool _closeRock;

        public bool _talk;

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
            _fKeyText = transform.Find("TalkKey/Text").GetComponent<Text>();
            _alarmText = transform.Find("AlarmText").GetComponent<Text>();

            _fKey.SetActive(false);
            _talkWindow.SetActive(false);
            _questListWindow.SetActive(false);
            _questWindow.SetActive(false);
            _alarmText.enabled = false;
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
            StartCoroutine(FadeAway());
        }

        IEnumerator FadeAway()
        {
            var color = _alarmText.color;

            _alarmText.enabled = true;
            yield return new WaitForSeconds(2);
            while (_alarmText.color.a > 0)
            {
                //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
                color.a -= (.5f * Time.deltaTime);

                _alarmText.color = color;
                //wait for a frame
                yield return null;
            }

            if (_alarmText.color.a == 0)
            {
                _alarmText.enabled = false;
                color.a = 255;
                _alarmText.color = color;
            }
        }

        public void ActiveTalkWindow()
        {
            _talk = true;
            _talkWindow.SetActive(true);
        }

        public void DeActiveTalkWindow()
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
            Text _questTitle = _questWindow.transform.Find("Image/Title").GetComponent<Text>();
            Text _questInfo = _questWindow.transform.Find("Image/Detail").GetComponent<Text>();
            GameObject _clickObject = EventSystem.current.currentSelectedGameObject;

            
            _questWindow.SetActive(true);

            if (_clickObject.tag == "QuestButton")
            {
                Text _buttonTitle = _clickObject.transform.Find("Title").GetComponent<Text>();

                _questTitle.text = _buttonTitle.text;

                if (_clickObject.name == "몬스터 퇴치")
                {
                    _questInfo.text = "중앙섬에 있는 몬스터를 모두 퇴치하자 \n\n 보상: 100골드";
                }

                else if (_clickObject.name == "바위 부수기")
                {
                    _questInfo.text = "마지막 섬을 막는 바위를 부숴라 \n\n 보상: 1000골드";
                }
            }
        }


    }
}