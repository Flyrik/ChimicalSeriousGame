using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawnerPool : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;
    public GameObject[] barrelPrefabs; 
    public GameObject BoutonClique;
    [Tooltip("Temps entre 2 spawns")]
    public float spawnInterval = 2.0f;

    [Tooltip("Nombre max de barils présents en même temps")]
    public int maxAlive = 3;

    [Tooltip("Parent pour ranger les barils dans la hiérarchie")]
    public Transform spawnedParent;
    public GameObject BoutonIndique;

    [Header("Anti-overlap")]
    public float checkRadius = 0.25f;
    public LayerMask overlapMask = ~0;

    [Header("Limite de spawns")]
    public int maxSpawns = 10; 
    private int spawnCount = 0;
    public AudioSource audioSource;

    private List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        if (spawnedParent == null) spawnedParent = this.transform;

        // Crée le pool : 1 objet de chaque type
        foreach (var prefab in barrelPrefabs)
        {
            GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, spawnedParent);
            obj.SetActive(false);
            pool.Add(obj);
        }

        // Ne rien spawner automatiquement ici
    }

    // Cette fonction sera appelée par ton bouton OnClick
    public void StartSpawning()
    {
        spawnCount = 0; // reset compteur
        StartCoroutine(SpawnLoop());
        BoutonIndique.SetActive(false);
        BoutonClique.SetActive(false);
        audioSource.Play();

    }

    private IEnumerator SpawnLoop()
    {
        while (spawnCount < maxSpawns)
        {
            if (GetAliveCount() < maxAlive && CanSpawn())
            {
                SpawnOneFromPoolRandom();
                spawnCount++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Spawn terminé !");
    }

    private bool CanSpawn()
    {
        if (spawnPoint == null) return false;
        return !Physics.CheckSphere(spawnPoint.position, checkRadius, overlapMask, QueryTriggerInteraction.Ignore);
    }

    private void SpawnOneFromPoolRandom()
    {
        List<GameObject> inactiveObjects = pool.FindAll(obj => !obj.activeInHierarchy);

        if (inactiveObjects.Count == 0)
        {
            Debug.LogWarning("Tous les objets du pool sont actifs !");
            return;
        }

        GameObject objToSpawn = inactiveObjects[Random.Range(0, inactiveObjects.Count)];
        objToSpawn.transform.position = spawnPoint.position;
        objToSpawn.transform.rotation = spawnPoint.rotation;
        objToSpawn.SetActive(true);
    }

    private int GetAliveCount()
    {
        int count = 0;
        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy) count++;
        }
        return count;
    }
}
