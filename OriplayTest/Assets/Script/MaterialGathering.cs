using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    private bool isInConstructionZone = false;
    private Coroutine collectingCoroutine;
    private Coroutine deliveringCoroutine;

    [SerializeField] AudioSource boxesPlaced;

    private List<GameObject> spawnedBoxes = new List<GameObject>(); // Lista pentru a reține cutiile instanțiate
    private Animator animator; // Referință la Animator

    private void Start()
    {
        UpdateUI();
        animator = GetComponent<Animator>(); // Obține componenta Animator
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

        // Oprește livrarea cutiilor dacă jucătorul a ieșit din ConstructionZone
        if (!isInConstructionZone && deliveringCoroutine != null)
        {
            StopCoroutine(deliveringCoroutine);
            deliveringCoroutine = null;
        }

        if (currentBoxes > 0) // Verificăm dacă playerul are cutii
        {
            if (playerController.IsMoving())
            {
                Debug.Log("Se activează animația de mișcare");
                animator.SetBool("IsCarryingBoxes", true); // Activează animația de transport
                animator.SetBool("IdleWithItem", false); // Dezactivează animația de stat pe loc
            }
            else
            {
                Debug.Log("Se activează animația de stat pe loc");
                animator.SetBool("IdleWithItem", true);
                animator.SetBool("IsCarryingBoxes", false); // Dezactivează animația de transport
                 // Activează animația de stat pe loc
            }
        }
        else
        {
            // Dacă nu are cutii, asigură-te că dezactivezi ambele animații
            Debug.Log("Nu are cutii");
            animator.SetBool("IsCarryingBoxes", false);
            animator.SetBool("IdleWithItem", false);
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
            isInConstructionZone = true;
            boxesPlaced.Play();
            ConstructionZone constructionZone = other.GetComponent<ConstructionZone>();
            if (constructionZone != null && deliveringCoroutine == null)
            {
                deliveringCoroutine = StartCoroutine(DeliverBoxes(constructionZone));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MaterialZone"))
        {
            isInMaterialZone = false;
        }
        if (other.CompareTag("ConstructionZone"))
        {
            isInConstructionZone = false;
            if (deliveringCoroutine != null)
            {
                StopCoroutine(deliveringCoroutine);
                deliveringCoroutine = null;
            }
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
        Vector3 spawnPosition = playerHand.position + Vector3.up * 0.185f * currentBoxes; // `0.3f` reprezintă distanța între cutii
        GameObject spawnedBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
        spawnedBox.transform.SetParent(playerHand); // Atașează cutia la mâna player-ului
        spawnedBox.transform.localEulerAngles = new Vector3(0, 0, 0);
        spawnedBoxes.Add(spawnedBox); // Adaugă cutia în lista de cutii
    }

    private IEnumerator DeliverBoxes(ConstructionZone constructionZone)
    {
        while (currentBoxes > 0 && isInConstructionZone)
        {
            int remainingBoxes = constructionZone.AddBoxes(1);
            currentBoxes--;
            UpdateUI();

            if (spawnedBoxes.Count > 0)
            {
                // Distruge ultima cutie din listă și o elimină din listă
                GameObject lastBox = spawnedBoxes[spawnedBoxes.Count - 1];
                spawnedBoxes.RemoveAt(spawnedBoxes.Count - 1);
                Destroy(lastBox);
            }

            if (remainingBoxes <= 0)
            {
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
