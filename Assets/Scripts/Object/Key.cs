using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Key : MapObject
    {
        public GameObject _key; // 열쇠 아이템 게임오브젝트

        // 열쇠 획득 사운드 재생 및 인벤토리에 추가
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