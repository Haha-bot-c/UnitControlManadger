using UnityEngine;
using System;

[RequireComponent(typeof(Terrain))]
public class TerrainClick : MonoBehaviour
{
    [SerializeField] private LayerMask occupiedLayer;
    [SerializeField] private float checkRadius = 1f;
    [SerializeField] private Game _game;

    private Terrain _terrain;

    public event Action<Vector3> OnClik;

    private void Start()
    {
        _terrain = GetComponent<Terrain>();
    }

    private void OnMouseDown()
    {
        Vector3? clickedPosition = CheckClickToTerrain();
      
        if (clickedPosition.HasValue)
        {
            OnClik?.Invoke(clickedPosition.Value);
        }    
    }

    private Vector3? CheckClickToTerrain()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_terrain.GetComponent<Collider>().Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 position = hit.point;

            if (hit.collider.gameObject == _terrain.gameObject)
            {
                if (IsPositionOccupied(position) == false)
                {
                    return position;
                }
            }
        }
        return null;
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius, occupiedLayer);

        return colliders.Length > 0;
    }
}
