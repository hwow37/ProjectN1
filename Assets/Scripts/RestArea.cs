using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestArea : MonoBehaviour
{

    public Player player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //충돌한 물체 tag가 Player
        if (other.transform.tag == "PlayerToSave")
        {
            player.SavePlayer();
            Debug.Log("enter collider");
        }   

        /*GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);
        Destroy(gameObject);*/
    }
}
