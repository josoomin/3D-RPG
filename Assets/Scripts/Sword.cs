using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Sword : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Monster"))
            {
                Monster _monsterC = other.GetComponent<Monster>();
                _monsterC.TakeDamage(10f);
            }
        }
    }
}