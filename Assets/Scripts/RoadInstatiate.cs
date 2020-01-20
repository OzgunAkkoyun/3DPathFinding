using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInstatiate : MonoBehaviour
{
    public GameObject roadInstantiate;
    public GameObject destination;
    private GameObject tempRoad;
    public List<Transform> roads = new List<Transform>();

    public List<RoadData> side = new List<RoadData>();

    private Vector3 zeroVector;
    public int squareSize;
    PathSideSelect pss;
    CreateMap cm;
    [HideInInspector]
    public string lastDirection, tempDirection, roadSpite;

    public bool isLeft, isRight = true;

    public int levelStepSize;

    public struct RoadData
    {
        public Vector3 pos { get; set; }
        public string lastDirection { get; set; }
    }

    void Start()
    {
        pss = FindObjectOfType<PathSideSelect>();
        cm = FindObjectOfType<CreateMap>();
        zeroVector = roadInstantiate.transform.position;
        RoadSpawn();
        InstantiateRoad();
        cm.MapCreate();
    }

    void Update()
    {
        
    }

    void RoadSpawn()
    {
        int i = 0;
        do
        {
            var verticalOrHorizantal = Random.Range(0, 2); //0 is horizantal 1 is Vertical

            if (i == 0)
            {
                lastDirection = "Up";
            }
            else
            {
                if (side[i-1].pos.z == destination.transform.position.z) //if y position is equal to destination, road can be created only horizantal
                {
                    verticalOrHorizantal = 0;
                }

                if (Mathf.Floor(verticalOrHorizantal) == 0)
                {
                    var leftOrRight = 0;

                    if (side[i - 1].pos.z == destination.transform.position.z)//if y position is equal to destination, road can be created only horizantal
                    {
                        if (side[i - 1].pos.x - destination.transform.position.x < 0)// and the node is left or right from to destination created only left or right
                        {
                            leftOrRight = 1;
                            isRight = true;
                        }
                        else if (side[i - 1].pos.x - destination.transform.position.x > 0)
                        {
                            leftOrRight = 0;
                            isLeft = true;
                        }
                    }
                    else
                    {
                        if (lastDirection == "Up")
                        {
                            leftOrRight = Random.Range(0, 2); //0 is Left 1 is Right
                        }
                        else if (lastDirection == "Left")
                        {
                            leftOrRight = 0;
                        }
                        else
                        {
                            leftOrRight = 1;
                        }
                    }

                    if (Mathf.Floor(leftOrRight) == 0 && isLeft)
                    {
                        isRight = false;
                        zeroVector.x -= squareSize;
                        lastDirection = "Left";

                    }
                    else if (Mathf.Floor(leftOrRight) == 1 && isRight)
                    {
                        isLeft = false;
                        zeroVector.x += squareSize;
                        lastDirection = "Right";
                    }

                }
                else
                {
                    isLeft = true;
                    isRight = true;
                    zeroVector.z += squareSize;
                    lastDirection = "Up";
                }
            }

            if (destination.transform.position.x - zeroVector.x > levelStepSize)
            {
                zeroVector.x += squareSize;
                isLeft = false;
                lastDirection = "Up";
                zeroVector.z += squareSize;
            }
            else if (destination.transform.position.x - zeroVector.x < -levelStepSize)
            {
                zeroVector.x -= squareSize;
                isRight = false;
                lastDirection = "Up";
                zeroVector.z += squareSize;
            }

            var rd = new RoadData();
            rd.pos = zeroVector;
            rd.lastDirection = lastDirection;
            side.Add(rd);
            i++;

            if (i > 50)
            {
                break;
            }

        } while (side[i - 1].pos.x != destination.transform.position.x || side[i - 1].pos.z != destination.transform.position.z);
    }

    public void InstantiateRoad()
    {        
        for (int i = 0; i < side.Count; i++)
        {
            if (i == side.Count - 1)
            {
                roadSpite = "UpUp";
            }
            else if (side[i].lastDirection == "Up" && side[i+1].lastDirection == "Left")
            {
                roadSpite = "UpLeft";
            }
            else if (side[i].lastDirection == "Up" && side[i+1].lastDirection == "Right")
            {
                roadSpite = "UpRight";
            }
            else if (side[i].lastDirection == "Left" && side[i+1].lastDirection == "Left")
            {
                roadSpite = "LeftLeft";
            }
            else if (side[i].lastDirection == "Left" && side[i+1].lastDirection == "Up")
            {
                roadSpite = "LeftUp";
            }
            else if (side[i].lastDirection == "Right" && side[i+1].lastDirection == "Right")
            {
                roadSpite = "RightRight";
            }
            else if (side[i].lastDirection == "Right" && side[i+1].lastDirection == "Up")
            {
                roadSpite = "RightUp";
            }
            else if (side[i].lastDirection == "Up" && side[i+1].lastDirection == "Up")
            {
                roadSpite = "UpUp";
            }

            tempRoad = pss.RoadSideSelect(roadSpite);
            
            GameObject node = Instantiate(tempRoad, side[i].pos, tempRoad.transform.rotation);
            node.name = roadSpite;

            /*tempRoad = node;
            tempDirection = lastDirection;*/
        }
        
    }
}
