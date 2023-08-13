using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace josoomin
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I;

        private void Awake()
        {
            I = this;
        }

        public AudioSource _BGM;

        public AudioSource _objectSound;
        public AudioSource _playerSound;
        public AudioSource _playerstepSound;
        public AudioSource _monsterSound;

        public GameObject _player;
        Player _playerCscript;

        public GameObject _mapObject;
        public List<GameObject> _mapObjectList;

        public GameObject _sponePoints;
        public List<GameObject> _sponePointList;

        public GameObject _monster;
        public List<GameObject> _monsterList;

        public GameObject _whiteParticleSystem;

        public int _deathMonsterCount;
        public int _breakRockCount;

        void Start()
        {
            Application.targetFrameRate = 60;

            _playerCscript = _player.GetComponent<Player>();

            _whiteParticleSystem.SetActive(false);

            //��� ���� ����Ʈ �迭�� �߰�
            for (int i = 0; i < _sponePoints.transform.childCount; i++)
            {
                GameObject _Point = _sponePoints.transform.GetChild(i).gameObject;
                _sponePointList.Add(_Point);
            }

            //���� �� ���� ����Ʈ�� ����
            for (int i = 0; i < _sponePointList.Count; i++)
            {
                if (_sponePointList[i].name != "KeySponePoint")
                {
                    GameObject _cloneMon = Instantiate(_monster);
                    _cloneMon.transform.position = _sponePointList[i].transform.position;
                    _cloneMon.name = "Monster" + i;
                    _monsterList.Add(_cloneMon);
                }
            }

            //�ʿ� ������Ʈ �ڽİ�ü �迭�� �߰�
            for (int i = 0; i < _mapObject.transform.childCount; i++)
            {
                GameObject _Item = _mapObject.transform.GetChild(i).gameObject;
                _mapObjectList.Add(_Item);

                if (_mapObjectList[i].name == "Key")
                {
                    _mapObjectList[i].SetActive(false);
                }
            }
        }

        public void SetSoundLevel(float BGMLevel, float SFXLevel)
        {
            _BGM.volume = BGMLevel;

            _objectSound.volume = SFXLevel;
            _playerSound.volume = SFXLevel;
            _playerstepSound.volume = SFXLevel;

            for (int i = 0; i < _monsterList.Count; i++)
            {
                _monsterSound = _monsterList[i].GetComponent<AudioSource>();
                _monsterSound.volume = SFXLevel;
            }
        }

        public void DropKey()
        {
            for (int i = 0; i < _mapObjectList.Count; i++)
            {
                if (_mapObjectList[i].name == "Key")
                {
                    _mapObjectList[i].SetActive(true);

                    for (int j = 0; j < _sponePointList.Count; j++)
                    {
                        if (_sponePointList[j].name == "KeySponePoint")
                        {
                            Vector3 _SP = _sponePointList[j].transform.position;

                            _whiteParticleSystem.SetActive(true);
                            _whiteParticleSystem.transform.position = _SP;
                            _mapObjectList[i].transform.position = _SP;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void ClearMonsterQuest()
        {
            _playerCscript._money += 500;
        }

        public void ClearRockQuest()
        {
            _playerCscript._money += 1000;
        }

        public void ReStart()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }
    }
}