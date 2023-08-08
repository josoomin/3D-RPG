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

        public virtual void ActiveObject(GameObject Player)
        {
            Player _Player = Player.GetComponent<Player>();
            
            Destroy(gameObject);
            UI_Canvas.I.CloseMapObject(gameObject.name, false);
            _Player._closeObject = null;

            if (gameObject.name == "Rock")
            {
                ObjectSound.I.PlaySound("ROCK");
                UI_Canvas.I._breakRock = true;
            }
        }

        public virtual void AddInven(GameObject Player, GameObject Item)
        {
            Player _Player = Player.GetComponent<Player>();

            Item.transform.parent = _Player._inven.transform;
            Item.SetActive(true);
            _Player._invenList.Add(Item.name);
        }
    }
}