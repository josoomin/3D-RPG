using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class BossAtkTri : MonoBehaviour
    {
        float _ATK = 20f;

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