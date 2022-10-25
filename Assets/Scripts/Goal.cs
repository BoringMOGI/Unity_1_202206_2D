using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player p = collision.GetComponent<Player>();
            p.OnGoal();

            GameUI.Instance.SwitchClearPanel(true);
            Debug.Log("ÇÃ·¹ÀÌ¾î¶û ºÎµúÃÆ´Ù.");
        }
    }
}
