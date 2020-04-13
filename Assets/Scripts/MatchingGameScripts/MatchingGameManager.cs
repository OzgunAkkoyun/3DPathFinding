using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingGameManager : MonoBehaviour
{
    public int cellSize = 16;
    public GameObject cellPrefab;
    public GameObject grid;

    void Awake()
    {
        for (int i = 0; i < cellSize; i++)
        {
            var cell = Instantiate(cellPrefab, grid.transform.position, Quaternion.identity);
            cell.name = i.ToString();
            cell.transform.parent = grid.transform;
            cell.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
