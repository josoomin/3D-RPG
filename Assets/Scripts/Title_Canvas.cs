using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace josoomin
{
    public class Title_Canvas : MonoBehaviour
    {
        public GameObject _htp;

        private void Start()
        {
            _htp.SetActive(false);
        }

        // ���� ����
        public void GameStart()
        {
            SceneManager.LoadScene(1);
        }

        // �÷��� ���â Ȱ��ȭ
        public void OpenHowToPlay()
        {
            _htp.SetActive(true);
        }

        // �÷��� ���â ��Ȱ��ȭ
        public void CloseHowToPlay()
        {
            _htp.SetActive(false);
        }

        // ���� ����
        public void ShutDown()
        {
            Application.Quit();
        }
    }
}
