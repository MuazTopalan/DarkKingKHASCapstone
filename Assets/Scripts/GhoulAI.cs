using UnityEditor;
using UnityEngine;

public class GhoulAI : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed;

    public float ghoulDetectionRange = 2f;
    public float attackCooldown = 3f;
    public static float ghoulAttackDamage = 15;

    public float xOffset; // X offset for box cast
    public float yOffset; // Y offset for box cast

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    private Transform player;
    private bool isPlayerInRange = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    [SerializeField] private CircleCollider2D ghouldMeleeCollider;

    private int direction = 1; // 1 for right, -1 for left

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);

        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag
    }

    private void Update()
    {
        CheckPlayerInRange(); // Check if the player is in range

        if (!isPlayerInRange && !isAttacking)
        {
            MoveToNextPoint();
        }

        if (isPlayerInRange && !isAttacking)
        {
            StopAndAttack();
        }

        if (isAttacking)
        {
            HandleAttackCooldown();
        }
    }

    private void MoveToNextPoint()
    {
        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform)
        {
            if (point.x <= 0)
            {
                flip();
                currentPoint = pointA.transform;
            }
        }
        else
        {
            if (point.x >= 0)
            {
                flip();
                currentPoint = pointB.transform;
            }
        }

        Vector2 movementDirection = (currentPoint.position - transform.position).normalized;
        rb.velocity = movementDirection * speed;
    }

    private void CheckPlayerInRange()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, ghoulDetectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range");
        }
        else
        {
            isPlayerInRange = false;
            Debug.Log("Player is not in range");
        }
    }

    private void StopAndAttack()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("isAttacking");
        isAttacking = true;
        attackTimer = attackCooldown;
        Debug.Log("Ghoul Attacking");

        //FindObjectOfType<AudioManager>().Play("GhoulAttack");

    }

    private void HandleAttackCooldown()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            isAttacking = false;
        }
    }

    private void flip()
    {
        direction *= -1; // Update the direction
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxSize = new Vector2(ghoulDetectionRange, ghoulDetectionRange);
        Vector2 boxCenter = new Vector2(transform.position.x + xOffset * direction, transform.position.y + yOffset);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    public void GhoulMeleeColliderActivate()
    {
        ghouldMeleeCollider.gameObject.SetActive(true);
    }
    public void GhoulMeleeColliderDeActivate()
    {
        ghouldMeleeCollider.gameObject.SetActive(false);
    }
}
