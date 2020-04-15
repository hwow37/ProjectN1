using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAreaSystem : MonoBehaviour
{
    // RestAreas
    public GameObject restArea_01;
    public GameObject restArea_02;
    public GameObject restArea_03;

    // Check unvisited area
    //public static bool[] unlocked_restareas = new bool[2];

    // Get RestArea Position
    public Vector2 GetPosition_RestArea(int areaNum)
    {
        switch (areaNum)
        {
            case 0:
                return restArea_01.transform.position;
            case 1:
                return restArea_02.transform.position;
            case 2:
                return restArea_03.transform.position;
            default:
                return restArea_01.transform.position;
        }
    }

    // Get RestArea Number
    public int GetNum_RestArea(string areaName)
    {
       switch (areaName)
        {
            case "RestArea_01":
                return 0;
            case "RestArea_02":
                return 1;
            case "RestArea_03":
                return 2;
            default:
                return 0;
        }
    }
}
