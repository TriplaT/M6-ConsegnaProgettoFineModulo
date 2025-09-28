using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float rotationSmoothTime = 0.1f;

    [Header("Jump Settings")]
    public float gravity = -8f;
    public float jumpHeight = 1.5f;

    [Header("References")]
    public Animator animator;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float rotationVelocity;

    private float smoothSpeed;
    private bool isJumpingInternal = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("ThirdPersonMovement: manca CharacterController!");

        if (animator == null)
            Debug.LogWarning("ThirdPersonMovement: manca Animator!");

        if (cameraTransform == null)
            Debug.LogWarning("ThirdPersonMovement: manca cameraTransform!");
    }

    void OnEnable()
    {
        ResetAnimator();
    }

    public void ResetAnimator()
    {
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
            animator.SetFloat("speed", 0f);
            animator.SetBool("isJumping", false);
        }
        smoothSpeed = 0f;
        isJumpingInternal = false;
        velocity = Vector3.zero;
    }


    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        float inputMagnitude = Mathf.Clamp01(direction.magnitude);

        smoothSpeed = Mathf.Lerp(smoothSpeed, inputMagnitude, Time.deltaTime * 15f);
        if (animator != null)
            animator.SetFloat("speed", smoothSpeed);

        if (inputMagnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumpingInternal = true;
            if (animator != null)
                animator.SetBool("isJumping", true);
        }

        if (controller.isGrounded && isJumpingInternal && velocity.y <= 0f)
        {
            isJumpingInternal = false;
            if (animator != null)
                animator.SetBool("isJumping", false);
        }
    }
}