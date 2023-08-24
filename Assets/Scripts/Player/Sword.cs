using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Sword : MonoBehaviour
    {
        public Player _player;
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