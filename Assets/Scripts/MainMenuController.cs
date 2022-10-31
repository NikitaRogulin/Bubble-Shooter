using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    private bool isWantExit;
    private void OnEnable()
    {
        mainMenu.startButton.onClick.AddListener(LoadingGamePlayScene);
        mainMenu.additionalExitButton.onClick.AddListener(Quit);
        mainMenu.additionalButton.onClick.AddListener(LoadingInfoScene);
        mainMenu.mainExitButton.onClick.AddListener(SwapButtons);
        mainMenu.stayButton.onClick.AddListener(SwapButtons);
    }
    private void OnDisable()
    {
        mainMenu.startButton.onClick.RemoveListener(LoadingGamePlayScene);
        mainMenu.additionalExitButton.onClick.RemoveListener(Quit);
        mainMenu.additionalButton.onClick.RemoveListener(LoadingInfoScene);
        mainMenu.mainExitButton.onClick.RemoveListener(SwapButtons);
        mainMenu.stayButton.onClick.RemoveListener(SwapButtons);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadingGamePlayScene()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadingInfoScene()
    {
        SceneManager.LoadScene(2);
    }
    private void Quit()
    {
            Application.Quit();
            Debug.Log("!");
    }
    private void SwapButtons()
    {
        mainMenu.mainButtons.SetActive(!mainMenu.mainButtons.activeInHierarchy);
        mainMenu.additionalButtons.SetActive(!mainMenu.additionalButtons.activeInHierarchy);
    }
}
