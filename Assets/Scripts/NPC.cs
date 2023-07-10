using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class NPC : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                UI_Canvas.I.CloseNPC(true);
            }
        }


        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                UI_Canvas.I.CloseNPC(false);
            }
        }
    }
}