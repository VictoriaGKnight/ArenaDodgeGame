using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerNetworkController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootCooldown = 0.35f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private float shootTimer;

    private NetworkVariable<int> currentHealth = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
{
    Debug.Log("Player spawned | OwnerClientId: " + OwnerClientId +
              " | LocalClientId: " + NetworkManager.Singleton.LocalClientId +
              " | IsOwner: " + IsOwner);

    rb = GetComponent<Rigidbody2D>();

    if (spriteRenderer != null)
    {
        if (OwnerClientId == 0)
        {
            spriteRenderer.color = Color.cyan;
        }
        else
        {
            spriteRenderer.color = Color.magenta;
        }
    }

    if (IsServer)
    {
        currentHealth.Value = maxHealth;

        if (OwnerClientId == 0)
        {
            transform.position = new Vector3(-6f, 2f, 0f);
        }
        else
        {
            transform.position = new Vector3(6f, 2f, 0f);
        }

        Debug.Log("Server positioned player " + OwnerClientId + " at " + transform.position);
    }

    currentHealth.OnValueChanged += OnHealthChanged;

    if (IsOwner && GameManager.Instance != null)
    {
        GameManager.Instance.SetLocalHealth(currentHealth.Value);
    }
}

    public override void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
        base.OnDestroy();
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if (IsOwner && GameManager.Instance != null)
        {
            GameManager.Instance.SetLocalHealth(newValue);
        }
    }

    void Update()
{
    if (!IsOwner) return;

    if (PauseMenuManager.IsPausedLocal)
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        return;
    }

    if (Input.GetKeyDown(KeyCode.P))
    {
        Debug.Log("Local owner controlling player with OwnerClientId: " + OwnerClientId);
    }

    shootTimer -= Time.deltaTime;

    float moveInput = Input.GetAxisRaw("Horizontal");
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

    if (moveInput > 0.01f)
    {
        facingRight = true;
        spriteRenderer.flipX = false;
    }
    else if (moveInput < -0.01f)
    {
        facingRight = false;
        spriteRenderer.flipX = true;
    }

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && shootTimer <= 0f)
    {
        Debug.Log("Shoot key detected");

        shootTimer = shootCooldown;

        float direction = facingRight ? 1f : -1f;
        ShootServerRpc(direction);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShootSound();
        }
    }
}

    [ServerRpc]
    private void ShootServerRpc(float direction)
{
    if (shootPoint == null) return;

    GameObject projectileObj = ProjectilePool.Instance.GetProjectile();
    projectileObj.transform.position = shootPoint.position;

    Projectile projectile = projectileObj.GetComponent<Projectile>();
    projectile.Initialize(direction, OwnerClientId);
}

    public void TakeDamage(int amount, ulong attackerId)
    {
        if (!IsServer) return;

        currentHealth.Value -= amount;

        if (currentHealth.Value < 0)
            currentHealth.Value = 0;

        if (currentHealth.Value <= 0 && NetworkMatchManager.Instance != null)
        {
            NetworkMatchManager.Instance.HandlePlayerDeath(attackerId, OwnerClientId);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}