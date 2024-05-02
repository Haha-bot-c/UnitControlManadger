using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    private const string DefaultLayer = "Default";

    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private int _maxNumberOfResources = 20;
    [SerializeField] private float _spawnInterval = 5f;

    private int _currentNumberOfResources = 0;
    private Terrain _terrain;

    private void Start()
    {
        _terrain = Terrain.activeTerrain;
        StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnInterval);

        while (_currentNumberOfResources < _maxNumberOfResources)
        {
            SpawnResource();
            yield return wait;
        }
    }

    private void SpawnResource()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();
        Instantiate(_resourcePrefab, spawnPosition, Quaternion.identity);
        _currentNumberOfResources++;
    }

    private Vector3 GetValidSpawnPosition()
    {
        float terrainWidth = _terrain.terrainData.size.x;
        float terrainLength = _terrain.terrainData.size.z;

        Vector3 spawnPosition = Vector3.zero;

        float minDistanceFromEdge = 15f;
        float minDistanceFromObjects = 3; 

        while (true)
        {
            spawnPosition = GenerateRandomPosition(terrainWidth, terrainLength, minDistanceFromEdge);
            spawnPosition = AdjustPositionToTerrain(spawnPosition);

            if (HasOtherCollisions(spawnPosition, minDistanceFromObjects) == false)
            {
                break;
            }
        }

        return spawnPosition;
    }

    private Vector3 GenerateRandomPosition(float terrainWidth, float terrainLength, float minDistanceFromEdge)
    {
        float randomX = Random.Range(minDistanceFromEdge, terrainWidth - minDistanceFromEdge);
        float randomZ = Random.Range(minDistanceFromEdge, terrainLength - minDistanceFromEdge);

        return new Vector3(randomX, 0f, randomZ);
    }

    private Vector3 AdjustPositionToTerrain(Vector3 position)
    {
        float terrainHeight = _terrain.SampleHeight(position);
        float heightAboveTerrain = 5f;

        position.y = terrainHeight + heightAboveTerrain;

        return position;
    }

    private bool HasOtherCollisions(Vector3 position, float minDistanceFromObjects)
    {
        Collider[] colliders = Physics.OverlapSphere(position, minDistanceFromObjects, LayerMask.GetMask(DefaultLayer));

        return colliders.Length > 0; 
    }
}
