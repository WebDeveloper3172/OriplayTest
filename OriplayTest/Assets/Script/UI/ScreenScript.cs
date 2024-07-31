using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScreenScript : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> componentsToDisable = new List<MonoBehaviour>();
    [SerializeField] private GameObject alwayscomponent;

    public void Initial()
    {
        Time.timeScale = 0;
        alwayscomponent.SetActive(false);
        SetComponentsEnabled(false);
    }
    public void ReloadLevelButton()
    {
        //PlayerLife.NumberEnemy = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        SetComponentsEnabled(true);
    }
 public void NextLevelButton()
    {
        //PlayerLife.NumberEnemy = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
        SetComponentsEnabled(true);
    }
    public void MainMenuButton()
    {
        //PlayerLife.NumberEnemy = 0;
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
