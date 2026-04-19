/*
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifeTime = 2f;

    private float direction;
    private ulong ownerId;
    private bool initialized;

    public void Initialize(float newDirection, ulong shooterId)
    {
        if (!IsServer) return;

        direction = newDirection;
        ownerId = shooterId;
        initialized = true;

        CancelInvoke();
        Invoke(nameof(DespawnProjectile), lifeTime);
    }

    void Update()
    {
        if (!IsServer || !initialized) return;

        transform.position += Vector3.right * direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("ArenaBoundary") || other.CompareTag("Ground"))
        {
            DespawnProjectile();
            return;
        }

        PlayerNetworkController player = other.GetComponent<PlayerNetworkController>();

        if (player != null && player.OwnerClientId != ownerId)
        {
            player.TakeDamage(damage, ownerId);
            DespawnProjectile();
        }
    }

    void DespawnProjectile()
    {
        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
*/

using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifeTime = 2f;

    private float direction;
    private ulong ownerId;
    private bool initialized;
    private float timer;

    public void Initialize(float newDirection, ulong shooterId)
    {
        direction = newDirection;
        ownerId = shooterId;
        initialized = true;
        timer = lifeTime;
    }

    void Update()
    {
        if (!initialized) return;

        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ResetProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ArenaBoundary") || other.CompareTag("Ground"))
        {
            ResetProjectile();
            return;
        }

        PlayerNetworkController player = other.GetComponent<PlayerNetworkController>();

        if (player != null && player.OwnerClientId != ownerId)
        {
            player.TakeDamage(damage, ownerId);
            ResetProjectile();
        }
    }

    void ResetProjectile()
    {
        initialized = false;
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }
}