using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestArea : MonoBehaviour
{
    public Player player;
    public TravelMenu travelMenu;
    public RestAreaSystem restAreaSystem;

    // for Saving last SavePoint
    public static Vector3 lastSavedPosition;
    public static int lastRestAreaNum;
    public static bool inRestArea = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the tag of the crashed object is player
        if (other.transform.tag == "PlayerToSave")
        {
            lastSavedPosition = this.transform.position;
            lastRestAreaNum = restAreaSystem.GetNum_RestArea(gameObject.name);
            inRestArea = true;
            player.SavePlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the tag of the crashed object is player
        if (other.transform.tag == "PlayerToSave")
        {
            travelMenu.SetTravelMenu(false);
            inRestArea = false;
        }
    }
}
