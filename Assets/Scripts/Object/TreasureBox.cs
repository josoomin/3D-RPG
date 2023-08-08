using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class TreasureBox : MapObject
    {
        public List<GameObject> _inItem;

        public void start()
        {
            for (int i = 0; i < _inItem.Count; i++)
            {
                _inItem[i].SetActive(false);
            }
        }

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