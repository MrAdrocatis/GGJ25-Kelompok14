using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private bool isTrapped = false;
    private List<MonoBehaviour> activeComponents = new List<MonoBehaviour>();

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

            // Disable all MonoBehaviour scripts except this one
            foreach (var component in GetComponents<MonoBehaviour>())
            {
                if (component != this && component.enabled)
                {
                    activeComponents.Add(component);
                    component.enabled = false;
                }
            }

            transform.localPosition = Vector2.zero; // Center the enemy in the bubble
            Debug.Log($"Enemy {gameObject.name} is now trapped.");
        }
        else
        {
            rb.isKinematic = false;
            collider.enabled = true;

            // Re-enable previously active components
            foreach (var component in activeComponents)
            {
                component.enabled = true;
            }
            activeComponents.Clear();

            Debug.Log($"Enemy {gameObject.name} is released from bubble.");
        }
    }
}
