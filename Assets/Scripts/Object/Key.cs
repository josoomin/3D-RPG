using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Key : MapObject
    {
        public GameObject _key;

        public override void ActiveObject(GameObject Player)
        {
            base.ActiveObject(Player);

            Player _Player = Player.GetComponent<Player>();

            if (gameObject.name == "Key")
            {
                _key.transform.parent = _Player._inven.transform;
                _key.SetActive(true);
                _Player._invenList.Add(gameObject.name);
            }
        }
    }
}