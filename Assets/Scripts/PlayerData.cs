using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] last_saved_position;

    // Check unvisited area
    //public bool[] unlocked_restarea;

    public PlayerData (Player player)
    {
        level = player.level;
        health = player.health;

        last_saved_position = new float[3];
        last_saved_position[0] = RestArea.lastSavedPosition.x;
        last_saved_position[1] = RestArea.lastSavedPosition.y;
        last_saved_position[2] = RestArea.lastSavedPosition.z;

        //unlocked_restarea = new bool[2];
    }
}
