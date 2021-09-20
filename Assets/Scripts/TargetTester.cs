using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTester : MonoBehaviour
{
    public GameObject targetMarker;

    void Start()
    {
        Instantiate(targetMarker, CircleXY(45, 1.5f), Quaternion.identity);
    }

    Vector2 CircleXY(float angle, float distance)
    {
        float lengtUnit = 1.5f;   // fitted for my Radar on screen
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians) * distance * lengtUnit;
        float y = Mathf.Sin(radians) * distance * lengtUnit;
        Vector2 pos = new Vector2(x*-1, y);                     // *-1 to flip the x axis

        return pos;
    }
}
