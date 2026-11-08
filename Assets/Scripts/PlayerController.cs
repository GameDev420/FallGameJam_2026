using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int speed;

    [Header("Sprites")]
    [SerializeField] private GameObject body;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private Vector2 boxCastSize;
    [SerializeField] private float boxCastDuration;
    [SerializeField] private LayerMask enemyLayer;

    public Rigidbody2D rb;
    private PlayerControls pc;
    private bool isMoving;
    private Coroutine activeBoxCastCoroutine;
    public Transform aimPos;

    private Vector2 currentMovementInput;

    private void Awake()
    {
        pc = new PlayerControls();
        InitInputActions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed += Mathf.CeilToInt(SaveSystem.Instance.Data.SpeedAdditive);
    }

    private void OnEnable()
    {
        pc.Enable();
    }

    private void OnDisable()
    {
        pc.Disable();
    }

    private void InitInputActions()
    {
        pc.Default.Movement.performed += ctx => MovePlayer(ctx.ReadValue<Vector2>());
        pc.Default.Movement.canceled += ctx => MovePlayer(new Vector2());

        pc.Default.Aiming.performed += ctx => AimPlayer(ctx.ReadValue<Vector2>());

        pc.Default.Attack.started += ctx => StartBoxCastAttack();
    }

    private void MovePlayer(Vector2 MovementDir)
    {
        currentMovementInput = MovementDir;
        isMoving = MovementDir != Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        rb.linearVelocity = currentMovementInput * speed;
    }

    private void AimPlayer(Vector2 AimPos)
    {
        Vector2 lookAtDir;

        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(body.transform.position);
        lookAtDir = AimPos - new Vector2(playerScreenPoint.x, playerScreenPoint.y);
        lookAtDir.Normalize();

        if (lookAtDir.normalized != lookAtDir)
        {
            Debug.LogWarning("LookAtDir not normalized");
        }

        float aimAngle = Mathf.Atan2(-lookAtDir.x, lookAtDir.y) * Mathf.Rad2Deg;
        body.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
    }

    private void StartBoxCastAttack()
    {
        // Cancel any existing coroutine
        if (activeBoxCastCoroutine != null)
        {
            StopCoroutine(activeBoxCastCoroutine);
        }

        activeBoxCastCoroutine = StartCoroutine(BoxCastAttackCoroutine());
    }

    private IEnumerator BoxCastAttackCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < boxCastDuration)
        {
            Vector2 attackOrigin = aimPos.position;
            Vector3 attackDirection = body.transform.up;
            float boxAngle = body.transform.eulerAngles.z;

            PerformBoxCast(attackOrigin, attackDirection, boxAngle);

            yield return null;
            elapsedTime += Time.deltaTime; 
        }

        activeBoxCastCoroutine = null;
    }

    private void PerformBoxCast(Vector2 origin, Vector2 direction, float angle)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            origin,
            boxCastSize,
            angle,
            direction,
            0f,
            enemyLayer
        );

        //foreach (RaycastHit2D hit in hits)
        //{
        //    if (hit.collider != null)
        //    {
        //        GameObject enemy = hit.collider.gameObject;
        //        Health enemyHealth = enemy.GetComponent<Health>();
        //        Debug.Log($"Hit enemy: {enemy.name}");
        //        enemyHealth.TakeDamage(3);
        //    }
        //}
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying && activeBoxCastCoroutine != null)
        {
            Gizmos.color = Color.red;

            Vector2 origin = aimPos.position;
            Vector2 direction = body.transform.up;
            float boxAngle = body.transform.eulerAngles.z;

            Gizmos.matrix = Matrix4x4.TRS(
                origin,
                Quaternion.Euler(0, 0, boxAngle),
                Vector3.one
            );
            Gizmos.DrawWireCube(Vector3.zero, boxCastSize);
            Gizmos.matrix = Matrix4x4.identity;

            Gizmos.DrawLine(origin, origin + direction * 2f);

            Gizmos.DrawWireSphere(origin, 0.1f);
        }
    }
}