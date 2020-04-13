using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject[] mapObjects;
    RoadInstatiate ri;
    Vector3 setPos;
 
    public void MapCreate(int stepSize)
    {
        Debug.Log("Map creation");
        ri = FindObjectOfType<RoadInstatiate>();
        for (int i = -20; i < 24; i+= stepSize)
        {
            for (int j = -24; j < 24; j+= stepSize)
            {
                setPos = new Vector3(i, 0.07f, j);

                if ( ri.side.FindIndex(element => element.pos==setPos) < 0  )
                {
                    if (setPos != new Vector3(0, 0.07f, -24))
                    {
                        Random.seed = System.DateTime.Now.Millisecond;
                        var olasilik = (int)Mathf.Round(Random.Range(0, mapObjects.Length));
                        Instantiate(mapObjects[olasilik], new Vector3(i, mapObjects[olasilik].transform.position.y, j), mapObjects[olasilik].transform.rotation);
                    }
                }
               
            }
            
        }
    }

}
