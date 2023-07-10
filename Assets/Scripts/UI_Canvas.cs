using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace josoomin
{
    public class UI_Canvas : MonoBehaviour
    {
        public static UI_Canvas I;

        [SerializeField] GameObject _fKey;
        [SerializeField] GameObject _talkWindow;
        public Text _fKeyText;

        [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;

        public bool _closeNPC;
        public bool _closeKey;

        public bool _talk;

        private void Awake()
        {
            I = this;
        }

        void Start()
        {
            _fKey = transform.Find("TalkKey").gameObject;
            _talkWindow = transform.Find("TalkWindow").gameObject;
            _fKeyText = transform.Find("TalkKey/Text").GetComponent<Text>();

            _fKey.SetActive(false);
            _talkWindow.SetActive(false);
        }

        public void CloseNPC(bool Player)
        {
            if (Player == true)
            {
                _fKeyText.text = "대화";
                _fKey.SetActive(true);
                _closeNPC = true;
            }
            else if (Player == false)
            {
                _fKey.SetActive(false);
                _closeNPC = false;
            }
        }

        public void CloseKey(bool Player)
        {
            if (Player == true)
            {
                _fKeyText.text = "줍기";
                _fKey.SetActive(true);
                _closeKey = true;
            }
            else if(Player == false)
            {
                _fKey.SetActive(false);
                _closeKey = false;
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
        }
    }
}