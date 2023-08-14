using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class FireBall : MonoBehaviour
    {
        public Transform _boss;

        public Transform _firePool;
        public List<GameObject> _fireBallList;

        float _moveSpeed = 10f;
        float _fireBallDamage = 10f;

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