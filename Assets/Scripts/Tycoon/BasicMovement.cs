using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] [Min(0.0f)] Vector2 moveSpeed = new(5.0f, 5.0f);
    [SerializeField] SmoothedValueConfig inputSmoothConfig;
    SmoothedVector2 smoothedInput;

    private void Awake()
    {
        smoothedInput = new SmoothedVector2(inputSmoothConfig)
        {
            SmoothedValue = Vector2.zero,
            TargetValue = Vector2.zero
        };
    }

    private void Update()
    {
        UpdateInput(Time.deltaTime);
        UpdateMovement(Time.deltaTime);
    }
    

    void UpdateInput(float deltaTime)
    {
        smoothedInput.Smooth(deltaTime);
    }
    void UpdateMovement(float deltaTime)
    {
        transform.position += (Vector3)(deltaTime * Vector2.Scale(smoothedInput.SmoothedValue, moveSpeed));
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        smoothedInput.TargetValue = context.ReadValue<Vector2>();
    }
}
