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
            base.ActiveObject(Player);

            Player _Player = Player.GetComponent<Player>();

            if (gameObject.name == "TreasureBox")
            {
                for (int i = 0; i < _inItem.Count; i++)
                {
                    _inItem[i].transform.parent = _Player._inven.transform;
                    _inItem[i].SetActive(true);
                    _Player._invenList.Add(_inItem[i].name);
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
                    if (_Player._inven.transform.GetChild(i).name == "Key")
                    {
                        Destroy(_Player._inven.transform.GetChild(i));
                    }
                }
            }
        }
    }
}