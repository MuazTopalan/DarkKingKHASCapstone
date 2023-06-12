using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DarkKingHp : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    
    
    [SerializeField] private Animator anim;
    private DarkKingMovement movementScript;
    private DarkKingAttack attackScript;
    private Rigidbody2D rb;
    public HealthBarScript healthBar;
    private Collider2D playerCollider;

    private void Start()
    {
        movementScript = GetComponent<DarkKingMovement>();
        attackScript = GetComponent<DarkKingAttack>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("Hurt");

        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ghouldMeleeCollider"))
        {
            Debug.Log("ghoul hit king");
            currentHealth -= GhoulAI.ghoulAttackDamage;
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("Hurt");
            }
            healthBar.SetHealth(currentHealth);
            Debug.Log("Player health : " + currentHealth);
        
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        if (other.CompareTag("spit"))
        {
            Debug.Log("spitter hit king with a spit");
            currentHealth -= Spit.spitDamage;
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("Hurt");
            }
            healthBar.SetHealth(currentHealth);
            Debug.Log("Player health : " + currentHealth);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        //if (other.CompareTag("StalagmiteTipCollider"))
        //{
        //    health -= StalagmiteTip.stalagmiteTipDamage;
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager != null)
        //    {
        //        audioManager.Play("KingHurt");
        //    }
        //    healthBar.SetHealth(health);
        //    Debug.Log("Player health : " + health);
        //    if (health <= 0f)
        //    {
        //        Die();
        //    }
        //}

        //if (other.CompareTag("bossTongueAttackCollider"))
        //{
        //    health -= BossAttack.bossTongueColliderDamage;
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager != null)
        //    {
        //        audioManager.Play("KingHurt");
        //    }
        //    healthBar.SetHealth(health);
        //    Debug.Log("Player health : " + health);
        //    if (health <= 0f)
        //    {
        //        Die();
        //    }
        //}
        //
        //if (other.CompareTag("bossMeleeAttackCollider"))
        //{
        //    health -= BossAttack.bossMeleeColliderDamage;
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager != null)
        //    {
        //        audioManager.Play("KingHurt");
        //    }
        //    healthBar.SetHealth(health);
        //    Debug.Log("Player health : " + health);
        //    if (health <= 0f)
        //    {
        //        Die();
        //    }
        //}
    }

    private void Die()
    {
        anim.SetTrigger("die");

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play("KingDeath");
        }


        // Disable movement script
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // Disable attack script
        if (attackScript != null)
        {
            attackScript.enabled = false;
        }

        // "Disable" Rigidbody2D
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f; // Set gravity scale to zero
        }

        // Disable Collider2D
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
    }

    private void LoadEndGame()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Stop("OST");
            audioManager.Stop("BossMusic");
        }
        SceneManager.LoadScene(3);
        //audioManager.Play("EndGameMusic");
    }
}
