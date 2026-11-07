using System.Collections;
using UnityEngine;

public class BasicEnemy : EnemyBehaviour
{
    [SerializeField] private FieldOfView enemyView;
    [SerializeField] private float basicSpeed = 7f;

    [Header("Melee Attack")]
    [SerializeField] private int meleeDamage = 1;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private float meleeWindup = 0.2f;
    [SerializeField] private Vector2 meleeBoxCastSize = new Vector2(1f, 1f);
    [SerializeField] private LayerMask playerLayer;

    private bool isAttacking;

    protected override void Awake()
    {
        speed = basicSpeed;
        base.Awake();
    }

    public override void UpdateEnemy()
    {
        base.UpdateEnemy();

        if (enemyView != null && enemyView.canSeePlayer)
        {
            Vector3 dirToPlayer = GetDirToPlayer();
            RotateTowardsPlayer(dirToPlayer);

            float distanceToPlayer = dirToPlayer.magnitude;

            if (distanceToPlayer <= meleeRange && !isAttacking)
            {
                StartCoroutine(MeleeAttack());
            }
            else if (distanceToPlayer > meleeRange)
            {
                SeekPlayer(dirToPlayer);
            }
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;

        body.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(meleeWindup);

        PerformMeleeBoxCast();

        isAttacking = false;
    }

    private void PerformMeleeBoxCast()
    {
        Vector2 attackOrigin = (Vector2)transform.position + (Vector2)transform.up * (meleeBoxCastSize.y / 2f);
        Vector2 attackDirection = transform.up;
        float boxAngle = transform.eulerAngles.z;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            attackOrigin,
            meleeBoxCastSize,
            boxAngle,
            attackDirection,
            0f,
            playerLayer
        );

        Debug.Log($"Box cast performed. Hits: {hits.Length}");

        //foreach (RaycastHit2D hit in hits)
        //{
        //    if (hit.collider != null)
        //    {
        //        Debug.Log($"Hit: {hit.collider.name}");
        //        Health health = hit.collider.GetComponentInParent<Health>();
        //        if (health != null)
        //        {
        //            health.TakeDamage(meleeDamage);
        //            Debug.Log($"Enemy hit player for {meleeDamage} damage!");
        //        }
        //    }
        //}
    }

    private void OnDrawGizmosSelected()
    {
        //draws melee range circle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        //draws melee box cast area
        Gizmos.color = Color.red;
        Vector2 attackOrigin = (Vector2)transform.position + (Vector2)transform.up * (meleeBoxCastSize.y / 2f);
        float boxAngle = transform.eulerAngles.z;

        Gizmos.matrix = Matrix4x4.TRS(
            attackOrigin,
            Quaternion.Euler(0, 0, boxAngle),
            Vector3.one
        );
        Gizmos.DrawWireCube(Vector3.zero, meleeBoxCastSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
}