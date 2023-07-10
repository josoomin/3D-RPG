using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{

    public class GameManager : MonoBehaviour
    {
        public List<GameObject> _sponePointList;
        public List<GameObject> _monsterList;

        public GameObject _monster;

        void Start()
        {
            for (int i = 0; i < _sponePointList.Count; i++)
            {
                Instantiate(_monster);
                _monsterList.Add(_monster);
                _monsterList[i].transform.position = _sponePointList[i].transform.position;
            }
        }

        void Update()
        {

        }
    }
}