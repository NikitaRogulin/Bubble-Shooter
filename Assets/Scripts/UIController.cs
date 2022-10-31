using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Button mainMenu;

    private void OnEnable()
    {
        mainMenu.onClick.AddListener(LoadingMainMenu);
    }
    private void OnDisable()
    {
        mainMenu.onClick.RemoveListener(LoadingMainMenu);
    }
    public void AddScore(int points)
    {
        score.text += points.ToString();   
    }
    private void LoadingMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}