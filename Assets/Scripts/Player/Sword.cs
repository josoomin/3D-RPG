using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Sword : MonoBehaviour
    {
        public Player _player; // 플레이어 스크립트

        // 검 콜라이더에 닿는 물체의 스크립트를 가져와 데미지 적용
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Monster"))
            {
                Monster _monsterC = other.GetComponent<Monster>();
                _monsterC.TakeDamage(_player._ATK);
            }

            else if (other.CompareTag("Boss"))
            {
                Boss _BC = other.GetComponent<Boss>();
                _BC.TakeDamage(_player._ATK);
            }
        }
    }
}