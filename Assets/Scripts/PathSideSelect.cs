using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSideSelect : MonoBehaviour
{
    public GameObject[] roadSide;
    int spriteIndex;

    public GameObject RoadSideSelect(string direction)
    {
        if (direction == "UpLeft")
        {
            spriteIndex = 0;
        }
        else if (direction == "UpRight")
        {
            spriteIndex = 1;
        }
        else if (direction == "LeftLeft")
        {
            spriteIndex = 2;
        }
        else if (direction == "LeftUp")
        {
            spriteIndex = 3;
        }
        else if (direction == "RightRight")
        {
            spriteIndex = 4;
        }
        else if (direction == "RightUp")
        {
            spriteIndex = 5;
        }
        else if (direction == "UpUp")
        {
            spriteIndex = 6;
        }
        return roadSide[spriteIndex];
    }
}
