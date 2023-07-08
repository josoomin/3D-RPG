using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Sword : MonoBehaviour
    {
        void Start()
        {
            
        }

        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            Monster _monsterC = other.GetComponent<Monster>();

            if (other.tag == "Monster")
            {
                _monsterC.TakeDamage(10f);
            }
        }
    }
}