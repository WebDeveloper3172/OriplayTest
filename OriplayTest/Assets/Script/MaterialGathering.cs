using UnityEngine;
using TMPro;
using System.Collections;

public class MaterialGathering : MonoBehaviour
{
    public int maxBoxes = 5;
    public int currentBoxes = 0;
    public TMP_Text collectedBoxesText;
    public float collectionInterval = 1.0f; // Intervalul în secunde pentru a adăuga o cutie
    public GameObject boxPrefab; // Prefab-ul cutiei de transportat
    public Transform playerHand; // Locul unde cutiile vor fi "puse" în mâna player-ului
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
            ConstructionZone constructionZone = other.GetComponent<ConstructionZone>();
            if (constructionZone != null)
            {
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
            SpawnBox(); // Creează cutia în mâna player-ului
            UpdateUI();

            if (currentBoxes >= maxBoxes)
            {
                currentBoxes = maxBoxes;
                break;
            }

            yield return new WaitForSeconds(collectionInterval);
        }
    }

    private void SpawnBox()
    {
        // Creează cutia la poziția mâinii player-ului, cu un mic offset vertical în funcție de numărul cutiilor
        Vector3 spawnPosition = playerHand.position + Vector3.up * 0.2f * currentBoxes; // `0.3f` reprezintă distanța între cutii
        GameObject spawnedBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
        spawnedBox.transform.SetParent(playerHand); // Atașează cutia la mâna player-ului
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
