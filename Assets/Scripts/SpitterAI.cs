using UnityEngine;

public class SpitterAI : MonoBehaviour
{
    public GameObject spitPrefab;
    public Transform spitSpawnPoint;
    public float spitSpeed;
    public float spitCooldown = 2f;
    public float spitDetectionRange = 5f;

    public float xOffset; // X offset for boxcast
    public float yOffset; // Y offset for boxcast
    public float width = 1f; // Width of the boxcast
    public float length = 1f; // Length of the boxcast

    public float walkDistance = 2f; // Distance to walk in either direction
    public float walkSpeed = 2f; // Walking speed

    private Transform player;
    private bool isPlayerInRange = false;
    private bool canSpit = true;

    private Vector3 initialPosition;
    private bool isWalkingRight = true;

    [SerializeField] private Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag

        initialPosition = transform.position;
    }

    private void Update()
    {
        CheckPlayerInRange(); // Check if the player is in range

        if (isPlayerInRange && canSpit)
        {
            anim.SetTrigger("isAttacking");
        }

        MoveSideToSide();
    }

    private void CheckPlayerInRange()
    {
        Vector2 boxSize = new Vector2(width, length);
        Vector2 boxCenter = new Vector2(transform.position.x + xOffset * (isWalkingRight ? 1f : -1f), transform.position.y + yOffset);

        Collider2D playerCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f, LayerMask.GetMask("Player"));

        isPlayerInRange = playerCollider != null;
    }


    public void Spit()
    {
        FindObjectOfType<AudioManager>().Play("Spit");
        // Instantiate the spit projectile from the spitSpawnPoint position and rotation
        GameObject spit = Instantiate(spitPrefab, spitSpawnPoint.position, Quaternion.identity);

        // Calculate the direction from the spitter to the player
        Vector3 direction = (player.position - spitSpawnPoint.position).normalized;

        // Set the spit projectile's velocity based on the direction
        Rigidbody2D spitRigidbody = spit.GetComponent<Rigidbody2D>();
        spitRigidbody.velocity = direction * spitSpeed;

        // Start the spit cooldown
        canSpit = false;
        Invoke("ResetSpitCooldown", spitCooldown);
    }


    private void ResetSpitCooldown()
    {
        canSpit = true;
    }

    private void MoveSideToSide()
    {
        Vector3 targetPosition;
        if (isWalkingRight)
        {
            targetPosition = initialPosition + new Vector3(walkDistance, 0f, 0f);
            FlipSprite(false); // Face right
        }
        else
        {
            targetPosition = initialPosition;
            FlipSprite(true); // Face left
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isWalkingRight = !isWalkingRight;
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
        Gizmos.color = Color.blue;

        Vector2 boxSize = new Vector2(width, length);
        Vector2 boxCenter = new Vector2(transform.position.x + xOffset * (isWalkingRight ? 1f : -1f), transform.position.y + yOffset);

        if (!isWalkingRight)
        {
            boxCenter.x -= width; // Adjust the box center for left-facing spitter
        }

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

}
