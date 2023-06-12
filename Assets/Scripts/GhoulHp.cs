using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulHp : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;
    private Collider2D ghoulCollider;
    private GhoulAI ghoulAIScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("kingMeleeCollider"))
        {
            health -= DarkKingAttack.kingAttackDamage;
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("Hurt");
            }
            Debug.Log("Enemy health: " + health);

            if (health <= 0f)
            {
                Die();
            }
        }
        if (other.CompareTag("kingSlamCollider"))
        {
            health -= DarkKingAttack.kingSlamDamage;
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("Hurt");
            }
            if (health <= 0f)
            {
                Die();
            }
        }
    }


    private void Die()
    {
        anim.SetTrigger("die");
        FindObjectOfType<AudioManager>().Play("KingDeath");

        // Disable movement script
        if (ghoulAIScript != null)
        {
            ghoulAIScript.enabled = false;
        }


        // "Disable" Rigidbody2D
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f; // Set gravity scale to zero
        }

        // Disable Collider2D
        if (ghoulCollider != null)
        {
            ghoulCollider.enabled = false;
        }
    }

    private void RemoveObjectFromScene()
    {
        Destroy(gameObject);
    }
}
