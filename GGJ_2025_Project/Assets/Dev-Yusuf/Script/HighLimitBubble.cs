using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLimitBubble : MonoBehaviour
{
    [Header("Boundary Settings")]
    public Vector2 size = new Vector2(10f, 5f);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bubble"))
        {
            Bubble bubble = collision.GetComponent<Bubble>();
            if (bubble != null && bubble.IsTrappingEnemy)
            {
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop bubble
            }
        }
    }
}
