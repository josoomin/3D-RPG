using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace josoomin
{
    public class UI_Canvas : MonoBehaviour
    {
        public static UI_Canvas I;

        Text _alarmText;
        Text _fKeyText;

        [SerializeField] GameObject _fKey;
        [SerializeField] GameObject _talkWindow;
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
            _questWindow = transform.Find("QuestWindow").gameObject;
            _fKeyText = transform.Find("TalkKey/Text").GetComponent<Text>();
            _alarmText = transform.Find("AlarmText").GetComponent<Text>();

            _fKey.SetActive(false);
            _talkWindow.SetActive(false);
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
            yield return new WaitForSeconds(1);
            while (_alarmText.color.a > 0)
            {
                //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
                color.a -= (.05f * Time.deltaTime);

                _alarmText.color = color;
                //wait for a frame
                //yield return null;
            }

            _alarmText.enabled = false;
        }

        public void ActiveTalkWindow()
        {
            _talk = true;
            _talkWindow.SetActive(true);
        }

        public void ActiveQuestWindow()
        {
            _questWindow.SetActive(true);
        }

        public void DeActiveTalkWindow()
        {
            _talk = false;
            _talkWindow.SetActive(false);
            _questWindow.SetActive(false);
        }
    }
}