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
    private Coroutine deliveringCoroutine;

    [SerializeField] AudioSource boxesPlaced;

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
            boxesPlaced.Play();
            //Debug.LogError("Entered ConstructionZone with boxes");
            ConstructionZone constructionZone = other.GetComponent<ConstructionZone>();
            if (constructionZone != null)
            {
                //Debug.LogError("CurentBoxes after: " + currentBoxes);
                //currentBoxes = constructionZone.AddBoxes(currentBoxes);
                //Debug.LogError("CurentBoxes before: " + currentBoxes);
                //UpdateUI();
                if (deliveringCoroutine == null)
                {
                    deliveringCoroutine = StartCoroutine(DeliverBoxes(constructionZone));
                }
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
    private IEnumerator DeliverBoxes(ConstructionZone constructionZone)
    {
        int boxesToDeliver = currentBoxes;
        while (boxesToDeliver > 0)
        {
            int remainingBoxes = constructionZone.AddBoxes(1);
            currentBoxes--;
            boxesToDeliver = remainingBoxes;
            UpdateUI();
            if (currentBoxes <= 0)
            {
                currentBoxes = 0;
                break;
            }
            yield return new WaitForSeconds(collectionInterval);
        }
        deliveringCoroutine = null;
    }

    private void UpdateUI()
    {
        collectedBoxesText.text = "Boxes Collected: " + currentBoxes;
    }
}
