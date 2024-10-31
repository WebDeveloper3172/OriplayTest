using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30f; // Timpul total de joc (în secunde)
    public TextMeshProUGUI timerText; // Referința la UI-ul Text pentru afișarea timpului rămas
    public GameObject losePopup; // Referința la panelul de tip pop-up "You Lose"

    private bool timerIsRunning = false;

    void Start()
    {
        // Inițializăm timer-ul
        timerIsRunning = true;
        losePopup.SetActive(false); // Ascundem pop-up-ul la început
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // Scădem timpul rămas și actualizăm UI-ul
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                // Când timpul a expirat, afișăm pop-up-ul și oprim timer-ul
                timeRemaining = 0;
                timerIsRunning = false;
                losePopup.SetActive(true); // Afișăm pop-up-ul "You Lose"
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        // Asigurăm că timpul afișat nu este negativ
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        // Calculăm minutele și secundele rămase
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Afișăm timpul rămas în format MM:SS
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
