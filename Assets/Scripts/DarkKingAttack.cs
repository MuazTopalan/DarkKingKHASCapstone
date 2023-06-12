using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKingAttack : MonoBehaviour
{
    private float attackCooldownTimer;
    private float slamCooldownTimer;

    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackCooldown = 0.5f;

    [SerializeField] private bool canSlam = true;
    [SerializeField] private bool isSlamming = false;
    [SerializeField] private float slamCooldown = 5f;

    public static float kingAttackDamage = 10;
    public static float kingSlamDamage = 20;

    [SerializeField] private CircleCollider2D meleeCollider;
    [SerializeField] private BoxCollider2D slamCollider;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private DarkKingMovement movementScript;

    private void Start()
    {
        meleeCollider.gameObject.SetActive(false);
        slamCollider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!canAttack)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0)
            {
                canAttack = true;
            }
        }

        if (!canSlam)
        {
            slamCooldownTimer -= Time.deltaTime;
            if (slamCooldownTimer <= 0)
            {
                canSlam = true;
            }
        }

        if (isAttacking || isSlamming)
        {
            return;
        }

        Attack();
        Slam();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack && movementScript.IsGrounded())
        {
            anim.SetTrigger("isAttacking");
            Debug.Log("King Attacked");
            FindObjectOfType<AudioManager>().Play("KingAttack");
            isAttacking = true;
            canAttack = false;
            attackCooldownTimer = attackCooldown;
        }
    }

    private void Slam()
    {
        if (Input.GetMouseButtonDown(1) && canSlam && movementScript.IsGrounded())
        {
            anim.SetTrigger("isSlamming");
            Debug.Log("King Slammed");
            FindObjectOfType<AudioManager>().Play("KingSlam");
            isSlamming = true;
            canSlam = false;
            slamCooldownTimer = slamCooldown;
        }
    }

    public void MeleeColliderActivate()
    {
        meleeCollider.gameObject.SetActive(true);
    }

    public void MeleeColliderDeActivate()
    {
        meleeCollider.gameObject.SetActive(false);
        isAttacking = false;
    }

    public void SlamColliderActivate()
    {
        slamCollider.gameObject.SetActive(true);
    }

    public void SlamColliderDeActivate()
    {
        slamCollider.gameObject.SetActive(false);
        isSlamming = false;
    }
}
