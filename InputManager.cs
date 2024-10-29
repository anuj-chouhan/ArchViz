using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick; // Joystick reference for movement input

    public float lookSensitivity = 100; // Sensitivity for looking (traditional input)
    public float gyroSensitivity = 100; // Sensitivity for looking (gyroscope input)
    [HideInInspector] public bool isGyroOn; // Flag to check if gyroscope input is enabled

    private Gyroscope gyro; // Gyroscope instance
    private bool isGyroSupported; // Flag for gyroscope support check
    public static InputManager instance; // Singleton instance for easy access

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Singleton Error Here"); // Ensure only one instance exists
        }

        instance = this;
    }

    private void Start()
    {
        isGyroSupported = EnableGyroscope(); // Initialize gyroscope support check
    }

    public Vector2 MoveInputVector()
    {
        Vector2 move;

#if UNITY_EDITOR
        // Move with keys when in editor for testing
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        if (move == Vector2.zero)
        {
            // Use joystick if no key is pressed
            move.x = joystick.Horizontal;
            move.y = joystick.Vertical;
        }
#else
        // Use joystick movement when not in the editor
        move.x = joystick.Horizontal;
        move.y = joystick.Vertical;
#endif

        return move;
    }

    public Vector2 LookInputVector()
    {
        if (isGyroOn)
        {
            Vector2 LookInput = new Vector2();

            // Traditional input for looking (joystick or mouse)
            LookInput.x = SimpleInput.GetAxis("LookX") * lookSensitivity;
            LookInput.y = SimpleInput.GetAxis("LookY") * lookSensitivity;

            // Gyroscope input (for mobile)
            if (isGyroSupported)
            {
                // Apply gyroscope input
                LookInput.x += -gyro.rotationRateUnbiased.y * gyroSensitivity; // Horizontal gyroscope input
                LookInput.y += gyro.rotationRateUnbiased.x * gyroSensitivity;  // Vertical gyroscope input
            }

            return LookInput;
        }
        else
        {
            // Return traditional input without gyroscope
            Vector2 LookInput = new Vector2();
            LookInput.x = SimpleInput.GetAxis("LookX") * lookSensitivity;
            LookInput.y = SimpleInput.GetAxis("LookY") * lookSensitivity;
            return LookInput;
        }
    }

    private bool EnableGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true; // Enable gyroscope
            return true;
        }
        return false;
    }
}
