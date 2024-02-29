using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class FireBall : MonoBehaviour
    {
        public Transform _boss; // 보스 게임 오브젝트 윗치값

        public Transform _firePool; // 화염구 오브젝트 풀링 위치
        public List<GameObject> _fireBallList; // 화염구 풀링 피스트

        float _moveSpeed = 10f; // 이동 속도
        float _fireBallDamage = 10f; // 화염구가 주는 데미지

        void Start()
        {
            transform.rotation = _boss.rotation;
        }

        void Update()
        {
            transform.Translate(new Vector3(0, 0, _moveSpeed * Time.deltaTime));
        }

        
        private void OnTriggerEnter(Collider other)
        {
            // 플레이어에 닿으면 플레이어에게 대미지를 준다
            if (other.tag == "Player")
            {
                Player _player = other.GetComponent<Player>();

                if (other.tag == "Player" && _player._hp > 0)
                {
                    _player.TakeDamage(_fireBallDamage);
                }
            }

            // 플레이어 나 벽에 닿으면 오브젝트 풀링 위치로 이동
            if (other.tag == "Player" || other.tag == "Wall")
            {
                gameObject.transform.position = _firePool.position;
                gameObject.transform.parent = _firePool;
                _fireBallList.Add(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}