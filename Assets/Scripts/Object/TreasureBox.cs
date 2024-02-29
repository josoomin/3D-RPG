using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class TreasureBox : MapObject
    {
        public List<GameObject> _inItem; // 안에 들어간 아이템 리스트

        public void start()
        {
            // 상자 안에 아이템 비활성화
            for (int i = 0; i < _inItem.Count; i++)
            {
                _inItem[i].SetActive(false);
            }
        }

        // 열쇠 보유 상태에서 보물상자와 상호작용 시 아이템 획득
        public override void ActiveObject(GameObject Player)
        {
            ObjectSound.I.PlaySound("BOX");
            base.ActiveObject(Player);

            Player _Player = Player.GetComponent<Player>();

            if (gameObject.name == "TreasureBox")
            {
                for (int i = 0; i < _inItem.Count; i++)
                {
                    base.AddInven(Player, _inItem[i]);
                }

                for (int i = 0; i < _Player._invenList.Count; i++)
                {
                    if (_Player._invenList[i] == "Key")
                    {
                        _Player._invenList.RemoveAt(i);
                    }
                }

                for (int i = 0; i < _Player._inven.transform.childCount; i++)
                {
                    Transform _Key = _Player._inven.transform.GetChild(i);

                    if (_Key.name == "Key")
                    {
                        _Key.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}