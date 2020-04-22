using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlgorithmWalker : MonoBehaviour
{
    public Map Map;
    [Space(40f)]
    public Pathfinder Pathfinder;
    [Space(40f)]
    public int ExpectedPathLength = 1000;
    [Space(40f)]
    public int RepeatedWalkerRepeats = 10;
    IEnumerator Enumerator;
    public int mapSize;
    public GameObject road;
    public GameObject grass;
    public GameObject target;
    public GameObject[] trees;

    public CreateLine Line;

    private int howManyTries=0;

    public void Awake()
    {
        Map = new Map(mapSize, mapSize);
        Pathfinder = new Pathfinder(Map);
         
        StartToGeneratePath();

        GenaretePathInStart();
    }

    private void GenaretePathInStart()
    {
        Pathfinder.EnableLogging = false;
        StartToGeneratePath();
        howManyTries++;
        for (int i = 0; i < ExpectedPathLength; i++)
        {
            var shouldContinue = Enumerator.MoveNext();
            
            if (!shouldContinue)
            {
                if (Pathfinder.Path.Count <= 10)
                {
                    Debug.Log("Finished. Length: " + Pathfinder.PathLength);
                    Debug.Log("Finished. Trys: " + howManyTries);
                    GenerateMap();
                    break;
                }
                else
                {
                    GenaretePathInStart();
                }
            }
        }
    }
    private void StartToGeneratePath()
    {
        Pathfinder.Reset();
        Enumerator = Pathfinder.GenerateRandomPath(ExpectedPathLength);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var shouldContinue = Enumerator.MoveNext();

            var convertedPath = Pathfinder.Path
                .Select(item => item.Position.ToVector3XZ())
                .ToArray();
            Line.DrawLine(convertedPath);

            if (!shouldContinue)
            {
                Debug.Log("Finished..");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
           StartCoroutine("WalkerAutomatic");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            RepeatedWalker(RepeatedWalkerRepeats);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartToGeneratePath();
            var tryCount = 0;
            while (Pathfinder.PathLength != 9)
            {
                if (tryCount++ > 1000)
                {
                    Debug.LogWarning("Failed.");
                    break;
                }
                QuickWalker();
            }
            Debug.Log("Tries: " + tryCount);
        }
    }

    private IEnumerator WalkerAutomatic()
    {
        Pathfinder.EnableLogging = true;
        //StartToGeneratePath();

        for (int i = 0; i < ExpectedPathLength; i++)
        {
            var shouldContinue = Enumerator.MoveNext();

            var convertedPath = Pathfinder.Path
                .Select(item => item.Position.ToVector3XZ())
                .ToArray();
            Line.DrawLine(convertedPath);
            yield return new WaitForSeconds(0f);

            if (!shouldContinue)
            {
                Debug.Log("Finished. Length: " + Pathfinder.PathLength);
                GenerateMap();
                yield break;
            }
        }

        Debug.LogWarning("Failed.");
    }

    private void GenerateMap()
    {
        for (int i = -5; i < mapSize; i++)
        {
            for (int j = -5; j < mapSize; j++)
            {
                
                if (Pathfinder.Path.FindIndex(element => element.Position == new Vector2Int(i, j)) < 0 )
                {
                    Instantiate(grass, new Vector3(i , 0, j ), Quaternion.identity);
                    Instantiate(trees[Random.Range(0, 3)], new Vector3(i , 1, j ), trees[0].transform.rotation);
                }
                else
                {
                    if(Map.TargetPoint != new Vector2Int(i,j))
                        Instantiate(road, new Vector3(i , 0, j ), Quaternion.identity);
                    else
                        Instantiate(target, new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }
    }

    private void QuickWalker()
    {
        Pathfinder.EnableLogging = false;
        StartToGeneratePath();

        for (int i = 0; i < ExpectedPathLength; i++)
        {
            var shouldContinue = Enumerator.MoveNext();

            if (!shouldContinue)
            {
                var convertedPath = Pathfinder.Path
                    .Select(item => item.Position.ToVector3XZ())
                    .ToArray();
                Line.DrawLine(convertedPath);
            }
        }
    }

    private void RepeatedWalker(int repeatCount)
    {
        Pathfinder.EnableLogging = false;
        
        var totalSteps = 0;
        var totalStepBacks = 0;
        var totalSuccessfulRuns = 0;
        var stepsToFinish = "";

        for (int iRepeat = 0; iRepeat < repeatCount; iRepeat++)
        {
            StartToGeneratePath();

            for (int i = 0; i < ExpectedPathLength; i++)
            {
                var shouldContinue = Enumerator.MoveNext();

                if (!shouldContinue)
                {
                    totalSteps += Pathfinder.TotalSteps;
                    totalStepBacks += Pathfinder.TotalStepBacks;
                    stepsToFinish += Pathfinder.PathLength.ToString()+", ";
                    totalSuccessfulRuns++;
                    Pathfinder.Reset();
                    break;
                }
            }
        }

        Debug.Log($"Total successful runs: " + totalSuccessfulRuns);
        Debug.Log($"Average step backs: " + ((float)totalStepBacks / totalSuccessfulRuns));
        Debug.Log($"Average steps: " + ((float)totalSteps / totalSuccessfulRuns));
        Debug.Log($"Steps Text: " + stepsToFinish);
        WriteTextFile(stepsToFinish);
        //Debug.Log($"Average step back ratio: " + ((float)StepBackCount / TotalSteps).ToString("P2"));
    }

    private void WriteTextFile(string histogram)
    {
        string path = "C:/wamp64/www/AbidinKodlamaHistogram/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(histogram);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Map.StartingPoint.ToVector3XZ(), 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Map.TargetPoint.ToVector3XZ(), 0.26f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Map.BoundsCenterXZ, Map.BoundsSizeXZ);

    }
}
