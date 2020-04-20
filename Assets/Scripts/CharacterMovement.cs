﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    bool isAnimStarted = false;
    GetInputs getInputs;
    
    Vector3 inputVector;

    void Start()
    {
        getInputs = FindObjectOfType<GetInputs>();
        inputVector = transform.position;
    }

    IEnumerator ExecuteAnimation()
    {
        for (int i = 0; i < getInputs.inputs.Count; i++)
        {
            if (getInputs.inputs[i] == KeyCode.LeftArrow)
            {
                inputVector.x -= 1;
            }
            else if (getInputs.inputs[i] == KeyCode.RightArrow)
            {
                inputVector.x += 1;
            }
            else if (getInputs.inputs[i] == KeyCode.UpArrow)
            {
                inputVector.z += 1;
            }

            if (isAnimStarted) yield break; // exit function
            isAnimStarted = true;

            var relativePos = new Vector3(inputVector.x, transform.position.y, inputVector.z) - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos);

            for (float t = 0f; t < 1f; t += Time.deltaTime * 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation , t);
                yield return null;
            }
            for (float t = 0f; t < 1f; t += Time.deltaTime * 1f)
            {
                transform.position = Vector3.Lerp(transform.position, inputVector, t);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
                yield return null;
            }
            transform.position = inputVector;
            isAnimStarted = false;
        }
    }
}
