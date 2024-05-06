using UnityEngine;

[RequireComponent(typeof(UnitDispatcher), typeof(BaseColonizationStatus))]
public class UnitSpawner : MonoBehaviour
{
    private const int UnitSpawnResourceThreshold = 3;
    private const float SpawnRadius = 5f;
    private const int MaxCountUnit = 3;

    [SerializeField] private Mover _unitPrefab;
    [SerializeField] private BaseWarehouse _baseWarehouse;

    private UnitDispatcher _unitDispatcher;
    private BaseColonizationStatus _baseColonizationStatus;

    private void Start()
    {
        _unitDispatcher = GetComponent<UnitDispatcher>();
        _baseColonizationStatus = GetComponent<BaseColonizationStatus>();
    }

    private void OnEnable()
    {
        _baseWarehouse.CollectedResourcesChanged += CollectResources;
    }
    private void OnDisable()
    {
        _baseWarehouse.CollectedResourcesChanged -= CollectResources;
    }

    private void CollectResources(int count)
    {
        if (count >= UnitSpawnResourceThreshold 
            && _baseColonizationStatus.IsColonizing == false
            && _unitDispatcher.GetTotalUnitCount() < MaxCountUnit)
        {
            _baseWarehouse.SpendResources(UnitSpawnResourceThreshold);
            SpawnRandomUnit();
        }
    }

    private void SpawnRandomUnit()
    {
        Vector3 randomOffset = Random.insideUnitSphere * SpawnRadius;
        Vector3 spawnPosition = transform.position + randomOffset;
        Mover newUnit = Instantiate(_unitPrefab, spawnPosition, Quaternion.identity);
        _unitDispatcher.RegisterUnit(newUnit);
        newUnit.AssignBase(_unitDispatcher);
    }
}
