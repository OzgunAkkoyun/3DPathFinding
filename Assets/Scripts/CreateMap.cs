using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject[] mapObjects;
    RoadInstatiate ri;
    Vector3 setPos;
    void Start()
    {
        //ri = FindObjectOfType<RoadInstatiate>();
        Debug.Log("CreateMApScript");
    }

    public void MapCreate(int stepSize)
    {
        ri = FindObjectOfType<RoadInstatiate>();
        for (int i = -24; i < 20; i+= stepSize)
        {
            for (int j = -24; j < 20; j+= stepSize)
            {
                setPos = new Vector3(i, 0.07f, j);
                Debug.Log(ri.side.FindIndex(element => element.pos == setPos));
                //Debug.Log(ri.squareSize);
                if ( ri.side.FindIndex(element => element.pos==setPos)<0 )
                {
                    Instantiate(mapObjects[0], new Vector3(i, 0, j), mapObjects[0].transform.rotation);
                }
               
            }
            
        }
    }

}
