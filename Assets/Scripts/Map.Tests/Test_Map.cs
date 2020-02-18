using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Map
{
    [Test]
    public void MapBounds_ShouldBeEqualTo_MapSize()
    {
        //var map = new Map(3, 5);
        //Assert.AreEqual(new Vector2Int(0, 0), map.BoundsMin);
        //Assert.AreEqual(new Vector2Int(3, 5), map.BoundsMax);

        var cell1 = new Cell(new Vector2Int());
        cell1.Position.x = 33;

        var cell2 = cell1;
        cell2.Position.x = 5555;
        cell2.UnavaliableNeighbours.Add(new Vector2Int(888, 0));

        Debug.Log("cell1.Position : " + cell1.Position);
        Debug.Log("cell2.Position : " + cell2.Position);
        Debug.Log("cell1.UnavaliableNeighbours.Length: " + cell1.UnavaliableNeighbours.Count);
        Debug.Log("cell2.UnavaliableNeighbours.Length: " + cell2.UnavaliableNeighbours.Count);
    }

    [Test]
    public void CreateMap()
    {
        var map = new Map(5,5);
        var path = new Pathfinder(map);
        path.Map = map;
        
        path.GenerateRandomPath(5);
    }

    [Test]
    public void CoroutineTest()
    {
    }
}
