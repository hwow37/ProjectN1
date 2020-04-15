using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelMenu : MonoBehaviour
{
    public GameObject travelMenuUI;
    public GameObject playerUI;
    public RestAreaSystem restAreaSystem;
    public Player player;
    public Player playerWeapon;

    public static bool inTravelMenu = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inTravelMenu)
        {
            SetTravelMenu(false);
        }
    }

    // Travel to the next RestArea
    public void Select_NextArea(int areaNum)
    {
        if (areaNum != RestArea.lastRestAreaNum)
        {
            Vector2 position = restAreaSystem.GetPosition_RestArea(areaNum);

            player.transform.position = position;
            position.y = position.y - 0.161f;
            playerWeapon.transform.position = position;
        }
    }

    public void SetTravelMenu(bool b)
    {
        inTravelMenu = b;
        travelMenuUI.SetActive(b);
        playerUI.SetActive(!b);
    }
}
