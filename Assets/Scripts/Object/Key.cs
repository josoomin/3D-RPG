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
            ObjectSound.I.PlaySound("KEY");
            base.ActiveObject(Player);

            Player _Player = Player.GetComponent<Player>();

            if (gameObject.name == "Key")
            {
                base.AddInven(Player, _key);
            }
        }
    }
}