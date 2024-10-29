using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject GameControlsUI;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TMP_Dropdown playerModeDropdown;
    [SerializeField] private TMP_Dropdown graphicsQualityDropdown;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Slider gyroscopeSensitivitySlider;
    [SerializeField] private Slider touchPadSensitivitySlider;
    [SerializeField] private Slider fieldOfViewSlider;
    [SerializeField] private Toggle gyroscopeToggle;
    [SerializeField] private Toggle backgroundMusicToggle;

    public static GameUIController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Signleton Error Here");
        }

        instance = this;
    }

    private void Start()
    {
        // Initialize UI with saved player settings and add listeners
        LoadPlayerSettings();
        SetupUIListeners();
    }

    /// <summary>
    /// Loads all saved player settings from PlayerPrefs and applies them to the UI and game.
    /// </summary>
    private void LoadPlayerSettings()
    {
        // Gyroscope Toggle (0 = off, 1 = on)
        bool isGyroEnabled = PlayerPrefsManager.LoadInt(PlayerPrefsManager.Settings.GyroscopeToggle) == 1;
        InputManager.instance.isGyroOn = isGyroEnabled;
        gyroscopeToggle.isOn = isGyroEnabled;

        // Music Toggle (0 = off, 1 = on)
        bool isMusicEnabled = PlayerPrefsManager.LoadInt(PlayerPrefsManager.Settings.MusicToggle) == 1;
        if (isMusicEnabled)
        {
            backgroundMusicSource.Play();
            backgroundMusicToggle.isOn = true;
        }
        else
        {
            backgroundMusicSource.Stop();
            backgroundMusicToggle.isOn = false;
        }

        // Load and apply other settings
        gyroscopeSensitivitySlider.value = PlayerPrefsManager.LoadFloat(PlayerPrefsManager.Settings.GyroscopeSensitivity);
        touchPadSensitivitySlider.value = PlayerPrefsManager.LoadFloat(PlayerPrefsManager.Settings.TouchSensitivity);
        fieldOfViewSlider.value = PlayerPrefsManager.LoadFloat(PlayerPrefsManager.Settings.FOVValue);
        graphicsQualityDropdown.value = PlayerPrefsManager.LoadInt(PlayerPrefsManager.Settings.Graphics);

        // Apply the loaded settings
        ApplySettings();
    }

    /// <summary>
    /// Applies the loaded settings to the game environment (e.g., camera settings, quality levels).
    /// </summary>
    private void ApplySettings()
    {
        InputManager.instance.gyroSensitivity = gyroscopeSensitivitySlider.value;
        InputManager.instance.lookSensitivity = touchPadSensitivitySlider.value;
        mainCamera.fieldOfView = fieldOfViewSlider.value;
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.value);
    }

    /// <summary>
    /// Adds event listeners to the UI elements for handling player interactions.
    /// </summary>
    private void SetupUIListeners()
    {
        // Toggle and slider listeners
        gyroscopeToggle.onValueChanged.AddListener(OnGyroscopeToggleChanged);
        backgroundMusicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        gyroscopeSensitivitySlider.onValueChanged.AddListener(OnGyroscopeSensitivityChanged);
        touchPadSensitivitySlider.onValueChanged.AddListener(OnTouchPadSensitivityChanged);
        fieldOfViewSlider.onValueChanged.AddListener(OnFieldOfViewChanged);
        graphicsQualityDropdown.onValueChanged.AddListener(OnGraphicsQualityChanged);

        // Button listeners
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        resetButton.onClick.AddListener(ResetAllPlayerSettings);
        playerModeDropdown.onValueChanged.AddListener(OnPlayerModeChanged);
        mainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scenes.MainMenu));
    }

    /// <summary>
    /// Changes the player mode between First-Person and Drone mode.
    /// </summary>
    private void OnPlayerModeChanged(int modeIndex)
    {
        if (modeIndex == 0)
        {
            // First-Person Mode
            PlayerMovement.instance.isDroneModeOn = false;
        }
        else if (modeIndex == 1)
        {
            // Drone Mode
            PlayerMovement.instance.isDroneModeOn = true;
        }
        else
        {
            Debug.LogError("Invalid player mode selected in dropdown.");
        }
    }

    /// <summary>
    /// Updates the graphics quality based on the dropdown selection and saves it.
    /// </summary>
    private void OnGraphicsQualityChanged(int qualityLevel)
    {
        PlayerPrefsManager.SaveInt(PlayerPrefsManager.Settings.Graphics, qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    /// <summary>
    /// Pauses the game and shows the pause menu.
    /// </summary>
    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        SetGameplayUIActive(false);
    }

    /// <summary>
    /// Resumes the game and hides the pause menu.
    /// </summary>
    private void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        SetGameplayUIActive(true);
    }

    /// <summary>
    /// Toggles visibility of gameplay-related UI elements like joysticks and player mode dropdown.
    /// </summary>
    private void SetGameplayUIActive(bool isActive)
    {
        GameControlsUI.SetActive(isActive);
    }

    /// <summary>
    /// Resets all player settings to their default values and reloads the UI.
    /// </summary>
    private void ResetAllPlayerSettings()
    {
        PlayerPrefsManager.ResetAllSettings();
        LoadPlayerSettings();  // Re-load settings and apply them
    }

    // UI Event Handler Methods

    private void OnGyroscopeToggleChanged(bool isEnabled)
    {
        InputManager.instance.isGyroOn = isEnabled;
        PlayerPrefsManager.SaveInt(PlayerPrefsManager.Settings.GyroscopeToggle, isEnabled ? 1 : 0);
    }

    private void OnMusicToggleChanged(bool isEnabled)
    {
        if (isEnabled) backgroundMusicSource.Play();
        else backgroundMusicSource.Stop();

        PlayerPrefsManager.SaveInt(PlayerPrefsManager.Settings.MusicToggle, isEnabled ? 1 : 0);
    }

    private void OnGyroscopeSensitivityChanged(float newSensitivity)
    {
        InputManager.instance.gyroSensitivity = newSensitivity;
        PlayerPrefsManager.SaveFloat(PlayerPrefsManager.Settings.GyroscopeSensitivity, newSensitivity);
    }

    private void OnTouchPadSensitivityChanged(float newSensitivity)
    {
        InputManager.instance.lookSensitivity = newSensitivity;
        PlayerPrefsManager.SaveFloat(PlayerPrefsManager.Settings.TouchSensitivity, newSensitivity);
    }

    private void OnFieldOfViewChanged(float newFOV)
    {
        mainCamera.fieldOfView = newFOV;
        PlayerPrefsManager.SaveFloat(PlayerPrefsManager.Settings.FOVValue, newFOV);
    }
}