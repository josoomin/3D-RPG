using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I;

        private void Awake()
        {
            I = this;
        }

        public GameObject _mapObject;
        public List<GameObject> _mapObjectList;

        public GameObject _sponePoints;
        public List<GameObject> _sponePointList;
        public List<string> _monsterList;

        public GameObject _monster;

        void Start()
        {
            //모든 스폰 포인트 배열에 추가
            for (int i = 0; i < _sponePoints.transform.childCount; i++)
            {
                GameObject _Point = _sponePoints.transform.GetChild(i).gameObject;
                _sponePointList.Add(_Point);
            }

            //몬스터 각 스폰 포인트에 생성
            for (int i = 0; i < _sponePointList.Count; i++)
            {
                if (_sponePointList[i].name != "KeySponePoint")
                {
                    Instantiate(_monster);
                    _monster.transform.position = _sponePointList[i].transform.position;
                    _monster.name = "Monster" + i;
                    _monsterList.Add(_monster.name + "(Clone)");
                }
            }

            //맵에 오브젝트 자식개체 배열에 추가
            for (int i = 0; i < _mapObject.transform.childCount; i++)
            {
                GameObject _Item = _mapObject.transform.GetChild(i).gameObject;
                _mapObjectList.Add(_Item);
            }
        }

        void Update()
        {

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
                            _mapObjectList[i].transform.position = _sponePointList[j].transform.position;
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }
}