using UnityEngine;

public class GhoulAI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float walkDistance;
    public float ghoulDetectionRange = 2f;
    public float attackCooldown = 3f;
    public static float ghoulAttackDamage = 15;
    public float xOffset; // X offset for box cast
    public float yOffset; // Y offset for box cast

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private bool isPlayerInRange = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    [SerializeField] private CircleCollider2D ghouldMeleeCollider;
    private int direction = 1; // 1 for right, -1 for left
    private Vector3 initialPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag
        initialPosition = transform.position;
    }

    private void Update()
    {
        CheckPlayerInRange(); // Check if the player is in range

        if (!isPlayerInRange && !isAttacking)
        {
            MoveSideToSide();
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

    private void MoveSideToSide()
    {
        Vector3 targetPosition;
        if (direction == 1)
        {
            targetPosition = initialPosition + new Vector3(walkDistance, 0f, 0f);
            FlipSprite(false); // Face right
        }
        else
        {
            targetPosition = initialPosition;
            FlipSprite(true); // Face left
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            direction *= -1; // Reverse the direction
        }
    }

    private void CheckPlayerInRange()
    {
        Vector2 boxSize = new Vector2(ghoulDetectionRange, ghoulDetectionRange);
        Vector2 boxCenter = new Vector2(transform.position.x + xOffset * direction, transform.position.y + yOffset * direction);
        Collider2D playerCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f, LayerMask.GetMask("Player"));

        isPlayerInRange = playerCollider != null;
    }


    private void StopAndAttack()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("isAttacking");
        FindObjectOfType<AudioManager>().Play("GhoulAttack");
        isAttacking = true;
        attackTimer = attackCooldown;
        Debug.Log("Ghoul Attacking");
    }

    private void HandleAttackCooldown()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            isAttacking = false;
        }
    }

    private void FlipSprite(bool faceLeft)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = faceLeft ? -1f : 1f;
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
