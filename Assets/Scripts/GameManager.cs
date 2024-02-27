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

        public AudioSource _BGM; // 배경음 오디오소스

        public AudioSource _objectSound; // 오브젝트 효과음 오디오 소스
        public AudioSource _playerSound; // 플레이어 효과음 오디오 소스
        public AudioSource _playerstepSound; // 플레이어 발소리 효과음 오디오 소스
        public AudioSource _monsterSound; // 몬스터 효과음 오디오 소스
        public AudioSource _BossSound; // 보스 효과음 오디오 소스

        public GameObject _player; // 플레이어 게임 오브젝트
        Player _playerCscript; // 플레이어 스크립트

        public GameObject _mapObject; // 맵 게임오브젝트
        public List<GameObject> _mapObjectList; // 맵 게임 오브젝트 리스트
         
        public GameObject _sponePoints; // 몬스터 스폰 포인트 게임오브젝트
        public List<GameObject> _sponePointList; // 몬스터 스폰 포인트 리스트

        public GameObject _monster; // 몬스터 게임오브젝트 
        public List<GameObject> _monsterList; // 몬스터 게임 오브젝트 리스트

        public GameObject _whiteParticleSystem; // 열쇠 파티클 게임 오브젝트

        public int _breakRockCount; // 부순 바위의 갯수

        public GameObject _gameOverUI; // 게임오버 UI

        public GameObject _gameClearUI; // 게임클리어 UI
        public bool _gameClear; // 게임 클리어 판단값

        void Start()
        {
            Application.targetFrameRate = 60; // 게임 60프레임 고정

            _playerCscript = _player.GetComponent<Player>();

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

            // 실행시 불필요한 게임오브젝트를 false로 초기화
            _whiteParticleSystem.SetActive(false);
            _gameClearUI.SetActive(false);
            _gameOverUI.SetActive(false);
            _gameClear = false;
        }

        // 사운드 슬라이더 값에 따른 배경음, 효과음 값 적용
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

        // 몬스터 전부 처치시 열쇠를 필드에 드롭
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

        // 퀘스트 클리어 시 돈 추가
        public void ClearQuest(int credit)
        {
            _playerCscript._money += credit;
        }

        // 게임 재시작
        public void ReStart()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }

        // 게임 클리어 UI 활성화
        public void GameClear()
        {
            _gameClear = true;
            _gameClearUI.SetActive(true);
        }

        // 게임 오버 UI 활성화
        public void GameOver()
        {
            _gameOverUI.SetActive(true);
        }
    }
}