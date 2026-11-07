using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    protected int maxHealth;
    protected float speed;

    protected int currentHealth;
    protected Rigidbody2D body;

    Vector3 _dirToPlayer;

    bool calculatedThisFrame = false;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateEnemy();
    }

    public virtual void UpdateEnemy()
    {
        //KillIfDead();
    }

    private void LateUpdate()
    {
        calculatedThisFrame = false;
    }

    public Vector3 GetDirToPlayer()
    {
        Vector3 dirToPlayer;

        if (!calculatedThisFrame)
        {
            dirToPlayer = Utility.GetPlayerObject().transform.position - transform.position;
            _dirToPlayer = dirToPlayer;
            calculatedThisFrame = true;
        }
        else
        {
            dirToPlayer = _dirToPlayer;
        }

        return dirToPlayer;
    }

    public void RotateTowardsPlayer(Vector3 dir)
    {
        float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SeekPlayer(Vector3 dirToPlayer)
    {
        body.linearVelocity = speed * dirToPlayer.normalized;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
