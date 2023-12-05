using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // Check if all enemies have been killed, show the complete screen.
        if (gameObject.activeInHierarchy && transform.childCount == 0)
        {
            Debug.Log("You won!");
        }
    }
}
