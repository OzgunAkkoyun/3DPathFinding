using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CharacterMovement character;

    void Start()
    {
        character = FindObjectOfType<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            character.StartCoroutine( "ExecuteAnimation" );
        }
    }
}
