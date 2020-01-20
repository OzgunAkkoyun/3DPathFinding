using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInputs : MonoBehaviour
{
    [HideInInspector]
    public List<KeyCode> inputs = new List<KeyCode>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputs.Add(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputs.Add(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputs.Add(KeyCode.RightArrow);
        }
    }
}
