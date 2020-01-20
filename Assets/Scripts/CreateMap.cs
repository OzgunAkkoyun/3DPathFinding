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
        ri = FindObjectOfType<RoadInstatiate>();
        Debug.Log("dağ:"+mapObjects[0].transform.position);
    }

    public void MapCreate()
    {
        for (int i = -24; i < 20; i+=ri.squareSize)
        {
            for (int j = -24; j < 20; j+=ri.squareSize)
            {
                //setPos = new Vector3(i, 0, j);
                //if (ri.side.Find(element => element.pos==setPos) == null)
                Instantiate(mapObjects[0], new Vector3(i, 0, j), mapObjects[0].transform.rotation);
                //ri.side.Find(element => element.pos == setPos)
            }
            
        }
    }

}
