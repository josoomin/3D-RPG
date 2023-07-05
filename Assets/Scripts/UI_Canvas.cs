using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class UI_Canvas : MonoBehaviour
    {
        public static UI_Canvas I;

        [SerializeField] GameObject _fKey;
        [SerializeField] GameObject _talkWindow;

        [SerializeField] GameObject _player;
        [SerializeField] GameObject _npc;

        [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;

        bool _F;

        public static bool _talk;
        public static bool _playerEnter;

        private void Awake()
        {
            I = this;
        }

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").gameObject;
            _npc = GameObject.FindGameObjectWithTag("NPC").gameObject;
            _fKey = transform.Find("TalkKey").gameObject;
            _talkWindow = transform.Find("TalkWindow").gameObject;

            _fKey.SetActive(false);
            _talkWindow.SetActive(false);
        }

        void Update()
        {
            CloseNPC();
            if (_F && Input.GetKeyDown(KeyCode.F) && _talk == false)
            {
                ActiveTalkWindow();
            }
        }

        void CloseNPC()
        {
            if (_playerEnter == true)
            {
                _fKey.SetActive(true);
                _F = true;
            }
            else
            {
                _fKey.SetActive(false);
                _F = false;
            }
        }

        void ActiveTalkWindow()
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