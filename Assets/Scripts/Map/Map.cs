using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Random = System.Random;

// Rider or ReSharper
// float errors
// list pooling
// class vs struct
// Without MonoBehaviour
// Test
// Render
// History tree
// SOLID principles

// ReSharper shortcuts
//   Ctrl + T
//   F12
//   Ctrl + D
//   Ctrl + R + R   (Rename)

// Walking algorithm with IEnumerator 

// F12 decompile

// LINQ

// Extension methods
// deneme
// Tools before implementation

// High precision time
// ALT + SHIFT + PageDown/PageUp
// Method 'ref' parameters

// TODO:
// Stats
// Istedigimiz uzunlukta yol olusturabilmesi
// Alternatif yollar sunabilmesi
// Histogram

public class Timer
{
    private float StartTime;
    public float Duration;

    public Timer()
    {
        StartTime = Time.realtimeSinceStartup;
    }

    public void Finish(bool log)
    {
        var endTime = Time.realtimeSinceStartup;
        Duration = endTime - StartTime;

        if (log)
        {
            Debug.Log($"Calculations took {TimeSpan.FromSeconds(Duration).ToString("g")}");
        }
    }
}


[Serializable]
public struct Cell : IEquatable<Cell>
{
    public Vector2Int Position;
    public List<Vector2Int> UnavaliableNeighbours;

    #region Initialization

    public Cell(Vector2Int position)
    {
        Position = position;
        UnavaliableNeighbours = new List<Vector2Int>();
    }

    #endregion

    #region Equality

    public bool Equals(Cell other)
    {
        return Position == other.Position;
    }

    public static bool operator ==(Cell obj1, Cell obj2)
    {
        return obj1 == obj2;
    }

    public static bool operator !=(Cell obj1, Cell obj2)
    {
        return !(obj1 == obj2);
    }

    #endregion

    public List<Cell> GetNeighbours()
    {
        return new List<Cell>
        {
            new Cell(new Vector2Int(Position.x - 1, Position.y)),
            new Cell(new Vector2Int(Position.x + 1, Position.y)),
            new Cell(new Vector2Int(Position.x, Position.y - 1)),
            new Cell(new Vector2Int(Position.x, Position.y + 1))
        };
    }
}

[Serializable]
public class Map
{
    public Vector2Int BoundsMin;
    public Vector2Int BoundsMax;

    public Bounds Bounds;

    public Vector3 BoundsCenterXZ => (BoundsMin + BoundsMax).ToVector3XZ() / 2;
    public Vector3 BoundsSizeXZ => (BoundsMax - BoundsMin).ToVector3XZ();

    public Vector2Int StartingPoint = new Vector2Int(1, 1);
    public Vector2Int TargetPoint = new Vector2Int(3, 3);

    public Map(int sizeX, int sizeY)
    {
        BoundsMin = new Vector2Int(0, 0);
        BoundsMax = new Vector2Int(sizeX , sizeY );

        Bounds = new Bounds(BoundsCenterXZ, BoundsSizeXZ);
    }
}

[Serializable]
public class Pathfinder
{
    [HideInInspector]
    public Map Map;
    [Space(15f)]
    public List<Cell> Path = new List<Cell>();
    public int PathLength => Path.Count;
    //[Space(15f)]
    //public List<Cell> UnavaliableCells = new List<Cell>();

    public int TotalStepBacks = 0;
    public int TotalSteps = 0;

    public Pathfinder(Map map)
    {
        Map = map;
    }

    public void Reset()
    {
        Path.Clear();
        TotalSteps = 0;
        TotalStepBacks = 0;
    }

    public IEnumerator GenerateRandomPath(int expectedPathLength)
    {
        var timer = new Timer();

        // TODO: Check for expectedPathLength
        Path.Add(new Cell(Map.StartingPoint));
        var currentPathIndex = 0;

        while (currentPathIndex < expectedPathLength)
        {
            var currentCell = Path[currentPathIndex];
            var neighbours = GetAvailableNeighbours(currentCell);
            yield return null;

            if (IsTargetReached(neighbours))
            {
                Path.Add(new Cell(Map.TargetPoint));
                timer.Finish(EnableLogging);
                //Debug.Log($"Step backs: " + StepBackCount);
                //Debug.Log($"Steps: " + TotalSteps);
                //Debug.Log($"Step back ratio: " + ((float)StepBackCount / TotalSteps).ToString("P2"));
                yield break;
            }

            if (SelectNextCell(neighbours, out Cell selectedNeighbour))
            {
                TotalSteps++;
                //Debug.Log("Current TotalSteps: " + TotalSteps);
                Path.Add(selectedNeighbour);
                currentPathIndex++;
                //CheckRoadLenght(currentCell);
            }
            else
            {
                TotalStepBacks++;
                //Debug.Log("------------------------- Current TotalStepBacks: " + TotalStepBacks);
                OneStepBackinList(currentCell, ref currentPathIndex);
            }
            
        }

        timer.Finish(EnableLogging);

    }

    //Go one step back in the Path list
    private void OneStepBackinList(Cell currentCell, ref int currentPathIndex)
    {
        //UnavaliableCells.Add(currentCell);
        Path.RemoveAt(currentPathIndex);
        currentPathIndex--;
        Path[currentPathIndex].UnavaliableNeighbours.Add(currentCell.Position);
    }

    //private void CheckRoadLength(Cell currentCell, int expectedPathLength)
    //{
    //    if (Path.Count > expectedPathLength)
    //    {
    //        OneStepBackinList(currentCell);
    //    }
    //}

    private List<Cell> GetAvailableNeighbours(Cell cell)
    {
        var neighbours = cell.GetNeighbours();
        List<Cell> availableCells = new List<Cell>();

        foreach (var neighbour in neighbours)
        {

            if (IsCellOnPath(neighbour))
            {
                //Log("Cell On Path");
            }
            else
            {
                if (CellinBounds(neighbour))
                {
                    if (CellUnavaliableNeighboursGet(neighbour,cell))
                    {
                        //Log("Cell UnavaliableNeighbours");
                    }
                    else
                    {
                        availableCells.Add(neighbour);
                    }
                }
                else
                {
                    //Log("Cell Not in bounds");
                }
                
            }
        }
        return availableCells;
    }

    #region BoolFunctions

    public bool CellUnavaliableNeighboursGet(Cell neighbours, Cell cell) => cell.UnavaliableNeighbours.Contains(neighbours.Position);

    public bool CellinBounds(Cell neighbours) => Map.Bounds.Contains(neighbours.Position.ToVector3XZ());

    public bool IsCellOnPath(Cell neighbours) => Path.Contains(neighbours);

    private bool SelectNextCell(List<Cell> neighbours, out Cell selectedNeighbour)
    {
        if (neighbours.Count > 0)
        {
            var rnd = UnityEngine.Random.Range(0, neighbours.Count);
            selectedNeighbour = neighbours[rnd];
            return true;
        }
        else
        {
            selectedNeighbour = new Cell();
            return false;
        }

    }

    private bool IsTargetReached(List<Cell> location)
    {
        var reached = false;
        for (int i = 0; i < location.Count; i++)
        {
            if (Map.TargetPoint == location[i].Position)
            {
                reached = true;
                break;
            }
            else
            {
                reached = false;
            }
        }
        return reached;
    }

    #endregion

    #region Logging

    public bool EnableLogging = true;

    private void Log(string message)
    {
        if (EnableLogging)
        {
            Debug.Log(message);
        }
    }

    #endregion

}