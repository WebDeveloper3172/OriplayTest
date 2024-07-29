using UnityEngine;
using TMPro;

public class ConstructionZone : MonoBehaviour
{
    public int totalBoxesNeeded = 5;
    private int boxesDelivered = 0;

    public GameObject[] largeConstructionStages;
    public GameObject[] smallConstructionStages;

    public TMP_Text boxesRemainingText; // UI text to display remaining boxes

    private void Start()
    {
        UpdateUI();
    }

    public int AddBoxes(int boxCount)
    {
        int boxesToUse = Mathf.Min(boxCount, totalBoxesNeeded - boxesDelivered);
        boxesDelivered += boxesToUse;
        UpdateConstruction();
        UpdateUI();
        return boxCount - boxesToUse;
    }

    private void UpdateConstruction()
    {
        float progress = (float)boxesDelivered / totalBoxesNeeded;
     

        int largeStagesToShow = Mathf.FloorToInt(progress * largeConstructionStages.Length);
        for (int i = 0; i < largeConstructionStages.Length; i++)
        {
            largeConstructionStages[i].SetActive(i < largeStagesToShow);
        }

        // Etapele de progres pentru obiectele mici, activarea acestora doar dacă toate obiectele mari sunt afișate
        if (largeStagesToShow == largeConstructionStages.Length)
        {
            int smallStagesToShow = Mathf.FloorToInt(progress * smallConstructionStages.Length);
            for (int i = 0; i < smallConstructionStages.Length; i++)
            {
                smallConstructionStages[i].SetActive(i < smallStagesToShow);
            }
        }

        if (boxesDelivered >= totalBoxesNeeded)
        {
            CompleteConstruction();
        }
    }

    private void CompleteConstruction()
    {
        Debug.LogError("Construction complete");
        // Additional logic for completed construction
        // E.g., deactivating the construction zone, playing a sound, etc.
        gameObject.SetActive(false);
        boxesRemainingText.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        int boxesRemaining = totalBoxesNeeded - boxesDelivered;
        boxesRemainingText.text = "Boxes Remaining: " + boxesRemaining;
    }
}
