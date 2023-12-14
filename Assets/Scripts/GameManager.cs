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
        public AudioSource _BossSound;

        public GameObject _player;
        Player _playerCscript;

        public GameObject _mapObject;
        public List<GameObject> _mapObjectList;

        public GameObject _sponePoints;
        public List<GameObject> _sponePointList;

        public GameObject _monster;
        public List<GameObject> _monsterList;

        public GameObject _whiteParticleSystem;

        public int _breakRockCount;

        public GameObject _gameOverUI;

        public GameObject _gameClearUI;
        public bool _gameClear;

        void Start()
        {
            Application.targetFrameRate = 60;

            _playerCscript = _player.GetComponent<Player>();

            _whiteParticleSystem.SetActive(false);

            #region 스폰 포인트 게임 오브젝트를 배열에 추가
            for (int i = 0; i < _sponePoints.transform.childCount; i++)
            {
                GameObject _Point = _sponePoints.transform.GetChild(i).gameObject;
                _sponePointList.Add(_Point);
            }
            #endregion

            #region 몬스터를 각각의 스폰 포인트에 배치 후 이름 지정
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
            #endregion

            #region 게임 오브젝트를 게임오브젝트 배열에 추가
            for (int i = 0; i < _mapObject.transform.childCount; i++)
            {
                GameObject _Item = _mapObject.transform.GetChild(i).gameObject;
                _mapObjectList.Add(_Item);

                if (_mapObjectList[i].name == "Key")
                {
                    _mapObjectList[i].SetActive(false);
                }
            }
            #endregion

            _gameClearUI.SetActive(false);
            _gameOverUI.SetActive(false);
            _gameClear = false;
        }

        public void SetSoundLevel(float BGMLevel, float SFXLevel)
        {
            _BGM.volume = BGMLevel;

            _objectSound.volume = SFXLevel;
            _playerSound.volume = SFXLevel;
            _playerstepSound.volume = SFXLevel;
            _BossSound.volume = SFXLevel;

            for (int i = 0; i < _monsterList.Count; i++)
            {
                _monsterSound = _monsterList[i].GetComponent<AudioSource>();
                _monsterSound.volume = SFXLevel * 0.5f;
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

        public void ClearQuest(int credit)
        {
            _playerCscript._money += credit;
        }

        public void ReStart()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }

        public void GameClear()
        {
            _gameClear = true;
            _gameClearUI.SetActive(true);
        }

        public void GameOver()
        {
            _gameOverUI.SetActive(true);
        }
    }
}