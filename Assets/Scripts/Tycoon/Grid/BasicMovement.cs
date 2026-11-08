using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovement : MonoBehaviour
{
    [SerializeField] [Min(0.0f)] Vector2 moveSpeed = new(5.0f, 5.0f);
    [SerializeField] SmoothedValueConfig inputSmoothConfig;
    SmoothedVector2 smoothedInput;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        smoothedInput = new SmoothedVector2(inputSmoothConfig)
        {
            SmoothedValue = Vector2.zero,
            TargetValue = Vector2.zero
        };
    }

    private void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;
        UpdateInput(deltaTime);
        UpdateMovement(deltaTime);
    }
    

    void UpdateInput(float deltaTime)
    {
        smoothedInput.Smooth(deltaTime);
    }
    void UpdateMovement(float deltaTime)
    {
        rb.MovePosition(rb.position + deltaTime * Vector2.Scale(smoothedInput.SmoothedValue, moveSpeed));
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        smoothedInput.TargetValue = context.ReadValue<Vector2>();
    }
}
