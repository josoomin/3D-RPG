using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace josoomin
{
    public class Title_Canvas : MonoBehaviour
    {
        public GameObject _settingMenu;

        private void Start()
        {
            _settingMenu.SetActive(false);
        }

        public void GameStart()
        {
            SceneManager.LoadScene(1);
        }

        public void OpenSettingMenu()
        {
            _settingMenu.SetActive(true);
        }

        public void ShutDown()
        {
            Application.Quit();
        }
    }
}
