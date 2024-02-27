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

        public AudioSource _objectSound; // ������Ʈ ������ҽ�

        public AudioClip _keySound; // ���� ȹ�� ȿ����
        public AudioClip _boxSound; // �������� ȹ�� ȿ����
        public AudioClip _rockSound; // ���� �ı� ȿ����

        // Ȱ�� ������Ʈ�� ���� ȿ���� ���
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