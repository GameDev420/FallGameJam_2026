using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("FOV Settings")]
    public float radius;
    [Range(0, 360)]
    public float angle;

    [Header("References")]
    public GameObject playerRef;

    [Header("Layers")]
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("State")]
    public bool canSeePlayer;

    private void Start()
    {
        playerRef = Utility.GetPlayerObject();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.up, directionToTarget);

            if (angleToTarget < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    directionToTarget,
                    distanceToTarget,
                    obstructionMask
                );

                if (hit.collider == null)
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = canSeePlayer ? Color.green : Color.red;

        Vector2 forward = transform.up;
        float halfAngle = angle / 2;

        Gizmos.DrawWireSphere(transform.position, radius);

        Quaternion leftRotation = Quaternion.Euler(0, 0, -halfAngle);
        Quaternion rightRotation = Quaternion.Euler(0, 0, halfAngle);

        Vector2 leftDirection = leftRotation * forward;
        Vector2 rightDirection = rightRotation * forward;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + leftDirection * radius);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + rightDirection * radius);

        Gizmos.DrawLine((Vector2)transform.position + leftDirection * radius,
                        (Vector2)transform.position + rightDirection * radius);
    }
}