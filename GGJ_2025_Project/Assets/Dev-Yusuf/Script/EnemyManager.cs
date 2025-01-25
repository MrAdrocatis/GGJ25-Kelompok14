using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private bool isTrapped = false;

    public void SetTrapped(bool trapped)
    {
        isTrapped = trapped;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();

        if (trapped)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            collider.enabled = false;
        }
        else
        {
            rb.isKinematic = false;
            collider.enabled = true;
        }
    }
}
