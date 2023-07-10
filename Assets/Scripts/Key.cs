using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Key : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>()._closeObject = gameObject;
                UI_Canvas.I.CloseKey(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>()._closeObject = null;
                UI_Canvas.I.CloseKey(false);
            }
        }

        public void DestroyMe(GameObject Player)
        {
            Destroy(gameObject);
            UI_Canvas.I.CloseKey(false);
            Player.GetComponent<Player>()._inven.Add(gameObject.name);
        }
    }
}