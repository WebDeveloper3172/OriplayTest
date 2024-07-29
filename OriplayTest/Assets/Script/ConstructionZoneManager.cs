using UnityEngine;

public class ConstructionZoneManager : MonoBehaviour
{
    public GameObject[] constructionStages;
    private int currentStageIndex = 0;

    private void Start()
    {
        // Dezactivează toate obiectele inițial
        foreach (var stage in constructionStages)
        {
            stage.SetActive(false);
        }

        // Activează primul obiect
        if (constructionStages.Length > 0)
        {
            constructionStages[0].SetActive(true);
        }
    }

    public void ActivateNextStage()
    {
        // Dezactivează obiectul curent
        if (currentStageIndex < constructionStages.Length)
        {
            constructionStages[currentStageIndex].SetActive(false);
        }

        // Activează următorul obiect dacă există
        currentStageIndex++;
        if (currentStageIndex < constructionStages.Length)
        {
            constructionStages[currentStageIndex].SetActive(true);
        }
    }
}
