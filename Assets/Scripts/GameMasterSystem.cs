using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterSystem : MonoBehaviour
{
    public Player player;
    
    void Start()
    {
        // New game
        if (MainMenu.mainSelect == 1){}
        // Load game
        else if(MainMenu.mainSelect == 2)
        {
            player.LoadPlayer();
        }
    }
}
