using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject aboutMenu;
    [SerializeField] private GameObject contactMenu;

    [Space]

    [Header("Main Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button aboutButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button contactButton;

    [Space]

    [Header("Start Menu Buttons")]
    [SerializeField] private Button interiorSceneButton;
    [SerializeField] private Button exteriorSceneButton;
    [SerializeField] private Button startMenuBackButton;

    [Space]

    [Header("About Menu Buttons")]
    [SerializeField] private Button aboutMenuBackButton;
    
    [Space]

    [Header("Contact Menu Buttons")]
    [SerializeField] private Button contactMenuBackButton;


    private void Start()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);

        startButton.onClick.AddListener(StartButton);
        aboutButton.onClick.AddListener(AboutButton);
        exitButton.onClick.AddListener(ExitButton);
        interiorSceneButton.onClick.AddListener(InteriorSceneButton);
        exteriorSceneButton.onClick.AddListener(ExteriorSceneButton);
        startMenuBackButton.onClick.AddListener(BackButtonMenu);
        aboutMenuBackButton.onClick.AddListener(BackButtonMenu);
        tutorialButton.onClick.AddListener(TutorialButton);
        contactButton.onClick.AddListener(ContactButton);
        contactMenuBackButton.onClick.AddListener(BackButtonMenu);
    }
    private void StartButton()
    {
        startMenu.SetActive(true);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        contactMenu.SetActive(false);
    }

    private void AboutButton()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
        contactMenu.SetActive(false);
    }

    private void BackButtonMenu()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        contactMenu.SetActive(false);
    }

    private void ContactButton()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        contactMenu.SetActive(true);
    }

    private void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }

    private void InteriorSceneButton()
    {
        Loader.Load(Loader.Scenes.Interior);
    }

    private void ExteriorSceneButton()
    {
        Loader.Load(Loader.Scenes.Exterior);
    }

    private void TutorialButton()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=vfnAYipqj1k&list=LL&index=30");
    }
}
