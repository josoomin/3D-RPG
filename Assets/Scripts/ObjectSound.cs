using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class ObjectSound : MonoBehaviour
    {
        public static ObjectSound I;

        private void Awake()
        {
            I = this;
        }

        public AudioSource _objectSound; // 오브젝트 오디오소스

        public AudioClip _keySound; // 열쇠 획득 효과음
        public AudioClip _boxSound; // 보물상자 획득 효과음
        public AudioClip _rockSound; // 바위 파괴 효과음

        // 활성 오브젝트에 따라 효과음 재생
        public void PlaySound(string action)
        {
            switch (action)
            {
                case "KEY":
                    _objectSound.clip = _keySound;
                    break;
                case "BOX":
                    _objectSound.clip = _boxSound;
                    break;
                case "ROCK":
                    _objectSound.clip = _rockSound;
                    break;
            }
            _objectSound.Play();
        }
    }
}