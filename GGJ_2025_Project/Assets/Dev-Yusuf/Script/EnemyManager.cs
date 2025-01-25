using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private bool isTrapped = false;

    public void SetTrapped(bool trapped)
    {
        isTrapped = trapped;
        if (trapped)
        {
            // Disable enemy functionality
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            // Re-enable enemy functionality
            GetComponent<Collider2D>().enabled = true;
        }
    }
}
