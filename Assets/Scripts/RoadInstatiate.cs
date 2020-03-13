using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using egrath.tools.algo;
using System.Linq;
using System;

public class RoadInstatiate : MonoBehaviour
{
    public GameObject roadInstantiate;
    public GameObject destination;
    private GameObject tempRoad;
    public GameObject character;

    public List<RoadData> side = new List<RoadData>();
    
    private Vector3 currentPos;

    public int squareSize;

    PathSideSelect pss;
    CreateMap cm;
    RouletteWheel rw;

    [HideInInspector]
    public string lastDirection, roadSpite ="Up";
    

    public int levelStepSize;
    public int a = 0;

    public struct RoadData
    {
        public Vector3 pos { get; set; }
        public string lastDirection { get; set; }
    }

    void Start()
    {
        pss = FindObjectOfType<PathSideSelect>();
        cm = FindObjectOfType<CreateMap>();
        rw = new RouletteWheel();
        currentPos = roadInstantiate.transform.position;

        var rd = new RoadData();
        rd.pos = new Vector3(character.transform.position.x, 0.07f, character.transform.position.z+squareSize);
        rd.lastDirection = "Up";
        side.Add(rd);
        
        ChosePoint();
    }

    private List<float> GetDistanceAvilablePoints(List<Vector3> points)
    {
        List<float> distances = new List<float>();

        for (int i = 0; i < points.Count; i++)
        {
            distances.Add(Vector3.Distance(points[i], destination.transform.position));
        }

        return distances;
    }

    void ChosePoint()
    {
        List<Vector3> avilablePoints = new List<Vector3>();
        avilablePoints = GetAvilablePoints(currentPos);

        
        //List<float> dist = new List<float>();
        //List<float> tempDist = new List<float>();

        //dist = GetDistanceAvilablePoints(avilablePoints);
        //tempDist = dist;

        //for (int i = 0; i < tempDist.Count; i++)
        //{
        //    tempDist[i] = 1 / tempDist[i];
        //}

        //tempDist.Sort();

        //float probility = rw.SelectFittesIndex(tempDist);

        //Debug.Log("probility:" +probility);
        //Debug.Log(" 1/ probility:" +1/probility);

        //var chosenSide = dist.FindIndex(element => element == (1/probility));
        //Debug.Log(chosenSide);
        var chosenSide = UnityEngine.Random.Range(0, avilablePoints.Count);

        AddRoadToList(avilablePoints[chosenSide]);
    }

    private bool ReachedDestination(Vector3 current)
    {
        return current == destination.transform.position ? true : false;
    }

    private bool GridControll(Vector3 current)
    {
        return side.FindIndex(element => element.pos == current) < 0 ? true : false;
    }

    private List<Vector3> GetAvilablePoints(Vector3 current)
    {
        List<Vector3> avilablePoints = new List<Vector3>();
        List<Vector3> allPoints = new List<Vector3>();

        allPoints.Add( new Vector3(current.x - squareSize, current.y, current.z) );
        allPoints.Add( new Vector3(current.x + squareSize, current.y, current.z) );
        allPoints.Add( new Vector3(current.x, current.y, current.z + squareSize) );
        allPoints.Add( new Vector3(current.x, current.y, current.z - squareSize) );

        for (int i = 0; i < allPoints.Count; i++)
        {
            if (GridControll(allPoints[i]))
            {
                if (Mathf.Abs(destination.transform.position.x - allPoints[i].x) <= squareSize && allPoints[i].z - destination.transform.position.z <= squareSize && allPoints[i].z >= roadInstantiate.transform.position.z)
                {
                    avilablePoints.Add(allPoints[i]);
                }
            }
        }

        return avilablePoints;
        
    }

    private void AddRoadToList(Vector3 nextPosition)
    {
        a++;
        if (a > 50)
        {
            Debug.Log("A: " + a);
            InstantiateRoad();
            return;
        }

        currentPos = nextPosition;
        var rd = new RoadData();
        rd.pos = nextPosition;
        rd.lastDirection = lastDirection;
        side.Add(rd);

        if (ReachedDestination(nextPosition))
        {
            Debug.Log("Reached");
            Debug.Log("A: " + a);
            InstantiateRoad();
            cm.MapCreate(squareSize);
        }
        else
        {
            ChosePoint();
        }
    }

    public void InstantiateRoad()
    {
        for (int i = 0; i < side.Count; i++)
        {
            if (i == side.Count - 1)
            {
                roadSpite = "UpUp";
            }
            else if (side[i].lastDirection == "Up" && side[i + 1].lastDirection == "Left")
            {
                roadSpite = "UpLeft";
            }
            else if (side[i].lastDirection == "Up" && side[i + 1].lastDirection == "Right")
            {
                roadSpite = "UpRight";
            }
            else if (side[i].lastDirection == "Left" && side[i + 1].lastDirection == "Left")
            {
                roadSpite = "LeftLeft";
            }
            else if (side[i].lastDirection == "Left" && side[i + 1].lastDirection == "Up")
            {
                roadSpite = "LeftUp";
            }
            else if (side[i].lastDirection == "Right" && side[i + 1].lastDirection == "Right")
            {
                roadSpite = "RightRight";
            }
            else if (side[i].lastDirection == "Right" && side[i + 1].lastDirection == "Up")
            {
                roadSpite = "RightUp";
            }
            else if (side[i].lastDirection == "Up" && side[i + 1].lastDirection == "Up")
            {
                roadSpite = "UpUp";
            }

            tempRoad = pss.RoadSideSelect(roadSpite);

            GameObject node = Instantiate(tempRoad, side[i].pos, tempRoad.transform.rotation);
            //node.transform.parent = GameObject.Find("Roads").transform;
            node.name = roadSpite;
        }

    }
}