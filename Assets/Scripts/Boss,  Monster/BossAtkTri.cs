using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class BossAtkTri : MonoBehaviour
    {
        float _ATK = 20f; // 보스 공격력
        
        // 보스 공격 콜라이더에 닿은 플레이어에게 데미지
        private void OnTriggerEnter(Collider other)
        {
            Player _player = other.GetComponent<Player>();

            if (other.tag == "Player" && _player._hp > 0)
            {
                _player.TakeDamage(_ATK);
            }
        }
    }
}