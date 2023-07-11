using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class TreasureBox : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>()._closeObject = gameObject;
                UI_Canvas.I.CloseTreasureBox(true);
            }
        }


        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>()._closeObject = null;
                UI_Canvas.I.CloseTreasureBox(false);
            }
        }

        public void OpenMe(GameObject Player)
        {
            Player _Player = Player.GetComponent<Player>();

            Destroy(gameObject);
            UI_Canvas.I.CloseTreasureBox(false);
            _Player._inven.Add("망치");
            _Player._closeObject = null;
        }
    }
}