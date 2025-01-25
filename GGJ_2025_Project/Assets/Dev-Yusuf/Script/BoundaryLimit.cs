using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryLimit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bubble"))
        {
            Bubble bubble = collision.GetComponent<Bubble>();
            if (bubble != null)
            {
                bubble.StopVerticalForce();
                Debug.Log("Bubble triggered BoundaryLimit.");
            }
        }
    }
}

