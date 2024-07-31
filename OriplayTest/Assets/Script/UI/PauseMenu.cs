using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private List<MonoBehaviour> componentsToDisable = new List<MonoBehaviour>();

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        SetComponentsEnabled(false);
    }
 
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        SetComponentsEnabled(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        SetComponentsEnabled(true);
    }
    public void Home()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
        SetComponentsEnabled(true);
    }
    private void SetComponentsEnabled(bool isEnabled)
    {
        foreach (var component in componentsToDisable)
        {
            if (component != null)
            {
                component.enabled = isEnabled;
            }
        }
    }
}
