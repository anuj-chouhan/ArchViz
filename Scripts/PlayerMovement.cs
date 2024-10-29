using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform; // Reference to the player's camera
    [SerializeField] private CharacterController controller; // CharacterController component

    [Header("Player Properties")]
    [SerializeField] private float speed; // Movement speed of the player

    public static PlayerMovement instance; // Singleton instance for easy access

    // Private variables for movement and physics
    [SerializeField] private float _xMove = 0f; // Rotation around the X-axis (pitch)
    [SerializeField] private float _yMove = 0f; // Rotation around the Y-axis (yaw)
    private float _gravity = -20f; // Gravity applied to the player
    private float _heightOffset = 3.6576f; // Offset for camera height
    private Vector3 _velocity; // Velocity for physics calculations
    private Vector3 _Move; // Movement direction vector
    private bool _IsGrounded; // Grounded state check
    private bool _isGyroOn; // Gyroscope status flag

    public bool isDroneModeOn; // Flag to toggle drone mode
    public bool isPlayerLookingAtMe; // Flag to check if player is looking at an object

    private void OnValidate()
    {
        controller = GetComponent<CharacterController>(); // Ensure CharacterController is assigned
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Singleton Error Here"); // Ensure only one instance exists
        }

        instance = this;
    }

    private void Update()
    {
        PlayerMove(); // Handle player movement
        CameraLook(); // Handle camera rotation
        IsPlayerLookingAtSomething(); // Check if player is looking at an object
    }

    private void PlayerMove()
    {
        if (isDroneModeOn)
        {
            controller.height = 0;
            controller.center = new Vector3(controller.center.x, _heightOffset, controller.center.z);

            // Ignore collisions with default layer in drone mode
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Default"), true);

            Vector2 inputVector = InputManager.instance.MoveInputVector();

            // Calculate movement based on camera and input direction
            _Move = (transform.right * inputVector.x + cameraTransform.forward * inputVector.y);

            controller.Move(_Move * speed * Time.deltaTime); // Move player
        }
        else
        {
            controller.height = _heightOffset;
            controller.center = new Vector3(controller.center.x, _heightOffset / 2, controller.center.z);

            // Enable collisions with default layer in regular mode
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Default"), false);

            Vector2 InputVector = InputManager.instance.MoveInputVector();

            _IsGrounded = Physics.Raycast(transform.position, Vector3.down, .1f); // Ground check

            if (_IsGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // Reset velocity if grounded
            }

            _Move = (transform.right * InputVector.x + transform.forward * InputVector.y);

            controller.Move(_Move * speed * Time.deltaTime); // Move player

            _velocity.y += _gravity * Time.deltaTime; // Apply gravity
            controller.Move(_velocity * Time.deltaTime);
        }
    }

    public bool IsPlayerLookingAtSomething()
    {
        // Cast a ray from the camera to detect objects in view
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.TryGetComponent(out AutoDoorOpenClose AutoDoorOpenClose))
            {
                isPlayerLookingAtMe = true; // Player is looking at an interactable object
            }
            else
            {
                isPlayerLookingAtMe = false;
            }
        }
        else
        {
            isPlayerLookingAtMe = false;
        }

        return isPlayerLookingAtMe;
    }

    private void CameraLook()
    {
        // Get look input and apply it to camera and player rotation
        Vector2 LookInputVector = InputManager.instance.LookInputVector() * Time.deltaTime;

        _xMove -= LookInputVector.y; // Pitch
        _yMove += LookInputVector.x; // Yaw

        _xMove = Mathf.Clamp(_xMove, -90, 90); // Limit pitch rotation

        // Rotate camera based on look input
        cameraTransform.transform.rotation = Quaternion.Euler(_xMove, _yMove, 0);

        // Position camera at player height offset
        cameraTransform.transform.position = transform.position + Vector3.up * _heightOffset;

        // Rotate player based on yaw input
        transform.rotation = Quaternion.Euler(0, _yMove, 0);
    }
}
