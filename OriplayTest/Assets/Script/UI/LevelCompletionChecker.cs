using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionChecker : MonoBehaviour
{
    [SerializeField] private AudioSource winSound;
    [SerializeField] private GameObject finishPopup;
    [SerializeField] private ScreenScript finishPopUpScript;
    [SerializeField] private List<GameObject> objectsToCheck;

    private bool levelFinished = false;

    void Start()
    {
        finishPopup.SetActive(false);
    }

    void Update()
    {
        if (!levelFinished && AllObjectsDeactivated())
        {
            FinishLevel();
        }
    }

    private bool AllObjectsDeactivated()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private void FinishLevel()
    {
        levelFinished = true;
        //winSound.Play();
        finishPopUpScript.Initial();
        finishPopup.SetActive(true);
        UnlockedNewLevel();
        Debug.LogError($"<color=yellow>A finisat metoda FinishLevel</color>");
    }

    private void UnlockedNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
