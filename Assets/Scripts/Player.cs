using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int level = 1;
    public int health = 30;
    private float[] last_saved_position;

    // Save
    public void SavePlayer()
    {
        last_saved_position = new float[3];
        last_saved_position[0] = RestArea.lastSavedPosition.x;
        last_saved_position[1] = RestArea.lastSavedPosition.y;
        last_saved_position[2] = RestArea.lastSavedPosition.z;
        SaveSystem.SavePlayer(this);
    }

    // Load
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        health = data.health;

        Vector3 position;
        position.x = data.last_saved_position[0];
        position.y = data.last_saved_position[1];
        position.z = data.last_saved_position[2];
        transform.position = position;
    }
}
