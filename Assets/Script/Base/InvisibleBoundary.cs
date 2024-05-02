using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InvisibleBoundary : MonoBehaviour
{
    private const int MaxDimensionCount = 3;
    private const float BoundaryDivisionFactor = 2f;

    [SerializeField] private Vector3 _boundarySize = new Vector3(10f, 10f, 10f);
    [SerializeField] private BaseWarehouse _baseWarehouse;

    private List<Resource> resources;

    private void OnEnable()
    {
        _baseWarehouse.QueueChanged += AddReserseToArray;
    }

    private void OnDisable()
    {
        _baseWarehouse.QueueChanged -= AddReserseToArray;
    }

    private void Update()
    {
        
        ClampResourcePositions();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _boundarySize);
    }

    private void ClampCubePosition(GameObject cube)
    {
        Vector3 clampedPosition = cube.transform.position;

        for (int i = 0; i < MaxDimensionCount; i++)
        {
            float boundaryHalfSize = _boundarySize[i] / BoundaryDivisionFactor;
            float boundaryMin = transform.position[i] - boundaryHalfSize;
            float boundaryMax = transform.position[i] + boundaryHalfSize;
            clampedPosition[i] = Mathf.Clamp(clampedPosition[i], boundaryMin, boundaryMax);
        }

        cube.transform.position = clampedPosition;
    }

    private void ClampResourcePositions()
    {
        if (resources != null)
        {
            for (int i = resources.Count - 1; i >= 0; i--)
            {
                Resource resource = resources[i];

                if (resource != null)
                {
                    ClampCubePosition(resource.gameObject);
                }
                else
                {
                    resources.RemoveAt(i);
                }
            }
        }
    }



    private void AddReserseToArray(Queue<Resource> collectedResources)
    {
        resources = collectedResources.ToList();
    }
}