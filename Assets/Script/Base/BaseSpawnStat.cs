using UnityEngine;

[RequireComponent(typeof(UnitDispatcher), typeof(BaseColonizeStat))]

public class BaseSpawnStat : MonoBehaviour
{
    private const int UnitSpawnResourceThreshold = 3;
    private const float SpawnRadius = 5f;
    private const int MaxCountUnit = 3;

    [SerializeField] private Movement _unitPrefab;
    [SerializeField] private BaseWarehouse _baseWarehouse;

    private UnitDispatcher _unitDispatcher;
    private BaseColonizeStat _baseColonizeStat;
    
    private void Start()
    {
        _unitDispatcher = GetComponent<UnitDispatcher>();
        _baseColonizeStat = GetComponent<BaseColonizeStat>();
    }

    private void OnEnable()
    {
        _baseWarehouse.CollectedResourcesChanged += CollectResources;
    }

    private void CollectResources(int count)
    {
        if (count >= UnitSpawnResourceThreshold && _baseColonizeStat.IsColonizing == false && _unitDispatcher.AllUnits.Count <= MaxCountUnit)
        {
            _baseWarehouse.SpendResources(UnitSpawnResourceThreshold);
            SpawnUnit();
        }  
    }

    private void SpawnUnit()
    {
        Vector3 randomOffset = Random.insideUnitSphere * SpawnRadius;
        Vector3 spawnPosition = transform.position + randomOffset;
        Movement newUnit = Instantiate(_unitPrefab, spawnPosition, Quaternion.identity);
        _unitDispatcher.RegisterUnit(newUnit);
    }
    private void OnDisable()
    {
        _baseWarehouse.CollectedResourcesChanged -= CollectResources;
    }

    
}