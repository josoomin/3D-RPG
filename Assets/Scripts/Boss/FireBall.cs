using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class FireBall : MonoBehaviour
    {
        float _moveSpeed = 2f;
        float _fireBallDamage = 10f;

        void Start()
        {

        }

        void Update()
        {
            transform.Translate(new Vector3(0, 0, -_moveSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            gameObject.transform.position = gameObject.transform.parent.transform.position;

            if (other.tag == "Player")
            {
                Player _PL = other.GetComponent<Player>();

                if (other.tag == "Player" && _PL._hp > 0)
                {
                    if (_PL._defand)
                    {
                        float _dmg = _fireBallDamage - _PL._DEF;

                        if (_dmg < 0)
                            _PL.TakeDamage(0);

                        else
                            _PL.TakeDamage(_dmg);
                    }

                    else
                        _PL.TakeDamage(_fireBallDamage);
                }
            }
        }
    }
}