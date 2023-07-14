using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class MapObject : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>()._closeObject = gameObject;
                UI_Canvas.I.CloseMapObject(gameObject.name, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>()._closeObject = null;
                UI_Canvas.I.CloseMapObject(gameObject.name, false);
            }
        }

        public void ActiveObject(GameObject Player)
        {
            Player _Player = Player.GetComponent<Player>();

            if (gameObject.name == "Key")
            {
                _Player._inven.Add(gameObject.name);
            }
            else if(gameObject.name == "TreasureBox")
            {
                _Player._inven.Add("망치");
            }
            
            Destroy(gameObject);
            UI_Canvas.I.CloseMapObject(gameObject.name, false);
            _Player._closeObject = null;
        }
    }
}