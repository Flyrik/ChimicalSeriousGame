using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;
    public GameObject[] barrelPrefabs;

    [Tooltip("Temps entre 2 spawns")]
    public float spawnInterval = 2.0f;

    [Tooltip("Nombre max de barils présents en même temps")]
    public int maxAlive = 5;

    [Tooltip("Parent pour ranger les barils dans la hiérarchie")]
    public Transform spawnedParent;

    [Header("Anti-overlap (évite de spawn si la place est occupée)")]
    public float checkRadius = 0.25f;
    public LayerMask overlapMask = ~0;

    private readonly List<GameObject> alive = new();

    private void Start()
    {
        if (spawnedParent == null) spawnedParent = this.transform;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            CleanupNulls();

            if (alive.Count < maxAlive && CanSpawn())
            {
                SpawnOne();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private bool CanSpawn()
    {
        if (spawnPoint == null) return false;
        // Vérifie qu'il n'y a pas déjà un objet au point de spawn
        return !Physics.CheckSphere(spawnPoint.position, checkRadius, overlapMask, QueryTriggerInteraction.Ignore);
    }

    private void SpawnOne()
    {
        if (barrelPrefabs == null || barrelPrefabs.Length == 0 || spawnPoint == null) return;

        var prefab = barrelPrefabs[Random.Range(0, barrelPrefabs.Length)];
        var go = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, spawnedParent);

        alive.Add(go);
    }

    private void CleanupNulls()
    {
        for (int i = alive.Count - 1; i >= 0; i--)
        {
            if (alive[i] == null) alive.RemoveAt(i);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnPoint.position, checkRadius);
    }
}
