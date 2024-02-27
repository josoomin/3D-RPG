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

        // 게임 실행
        public void GameStart()
        {
            SceneManager.LoadScene(1);
        }

        // 플레이 방법창 활성화
        public void OpenHowToPlay()
        {
            _htp.SetActive(true);
        }

        // 플레이 방법창 비활성화
        public void CloseHowToPlay()
        {
            _htp.SetActive(false);
        }

        // 게임 종료
        public void ShutDown()
        {
            Application.Quit();
        }
    }
}
