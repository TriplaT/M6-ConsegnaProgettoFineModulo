using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Gameplay";

    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private bool isLoading = false;

    private void Awake()
    {
        if (playButton == null) Debug.LogWarning("MainMenu: playButton non assegnato!");
        if (exitButton == null) Debug.LogWarning("MainMenu: exitButton non assegnato!");

        SetButtonsInteractable(true);
    }

    public void PlayGame()
    {
        if (isLoading) return;

        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadGameSceneAsync()
    {
        isLoading = true;
        SetButtonsInteractable(false);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        if (isLoading) return;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetButtonsInteractable(bool state)
    {
        if (playButton != null) playButton.interactable = state;
        if (exitButton != null) exitButton.interactable = state;
    }
}
