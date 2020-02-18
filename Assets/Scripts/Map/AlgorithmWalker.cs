using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlgorithmWalker : MonoBehaviour
{
    public Map Map;
    [Space(40f)]
    public Pathfinder Pathfinder;
    [Space(40f)]
    public int ExpectedPathLength = 1000;
    IEnumerator Enumerator;

    public CreateLine Line;

    public void Awake()
    {
        Map = new Map(10, 10);
        Pathfinder = new Pathfinder(Map);

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
                Debug.Log("Finished.");
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
    }

    private IEnumerator WalkerAutomatic()
    {

        for (int i = 0; i < ExpectedPathLength; i++)
        {
            var shouldContinue = Enumerator.MoveNext();

            var convertedPath = Pathfinder.Path
                .Select(item => item.Position.ToVector3XZ())
                .ToArray();
            Line.DrawLine(convertedPath);

            if (!shouldContinue)
            {
                Debug.Log("Finished.");
            }
            yield return new WaitForSeconds(0f);
        }
        
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
