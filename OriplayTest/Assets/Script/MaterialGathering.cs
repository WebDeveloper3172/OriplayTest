using UnityEngine;
using TMPro;
using System.Collections;

public class MaterialGathering : MonoBehaviour
{
    public int maxBoxes = 5;
    public int currentBoxes = 0;
    public TMP_Text collectedBoxesText;
    public float collectionInterval = 1.0f; // Interval in seconds to add a box

    public PlayerController playerController; // Referință la componenta PlayerController
    private bool isInMaterialZone = false;
    private Coroutine collectingCoroutine;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (isInMaterialZone && !playerController.IsMoving() && currentBoxes < maxBoxes)
        {
            if (collectingCoroutine == null)
            {
                collectingCoroutine = StartCoroutine(CollectBoxes());
            }
        }
        else if (collectingCoroutine != null)
        {
            StopCoroutine(collectingCoroutine);
            collectingCoroutine = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MaterialZone"))
        {
            isInMaterialZone = true;
        }
        if (other.CompareTag("ConstructionZone") && currentBoxes > 0)
        {
            Debug.LogError("Entered ConstructionZone with boxes");
            ConstructionZone constructionZone = other.GetComponent<ConstructionZone>();
            if (constructionZone != null)
            {
                Debug.LogError("ConstructionZone component found");
                currentBoxes = constructionZone.AddBoxes(currentBoxes);
                UpdateUI();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MaterialZone"))
        {
            isInMaterialZone = false;
        }

      
    }

    private IEnumerator CollectBoxes()
    {
        while (currentBoxes < maxBoxes)
        {
            currentBoxes++;
            UpdateUI();

            if (currentBoxes >= maxBoxes)
            {
                currentBoxes = maxBoxes;
                break;
            }

            yield return new WaitForSeconds(collectionInterval);
        }
    }

    private void UpdateUI()
    {
        collectedBoxesText.text = "Boxes Collected: " + currentBoxes;
    }
}
