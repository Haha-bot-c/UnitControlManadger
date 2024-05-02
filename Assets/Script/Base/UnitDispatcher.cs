using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitDispatcher : MonoBehaviour
{
    private const int MinCountUnits = 1;
    public List<Movement> AllUnits => _allUnits;

    [SerializeField] private List<Movement> _allUnits = new List<Movement>();
    [SerializeField] private BaseWarehouse _baseWarehouse;

    private Queue<Resource> _foundResources = new Queue<Resource>();
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);

    private void Start()
    {
        StartCoroutine(DispatchUnits());
    }

    private IEnumerator DispatchUnits()
    {
        while (true)
        {
            Movement freeUnit = GetFreeUnit();
            SendUnitToResource(freeUnit);
            yield return _waitForSeconds;
        }
    }

    private Movement GetFreeUnit()
    {
        foreach (var unit in _allUnits)
        {
            if (unit.CurrentResource == null)
            {
                return unit;
            }
        }
        return null;
    }

    private void SendUnitToResource(Movement unit)
    {
        if (_foundResources.Count > 0 && unit != null)
        {
            Resource resource = _foundResources.Dequeue();
            unit.InitiateMoveToResource(resource);
            _baseWarehouse.AddIndexResirse(resource.ResourceIndex);
        }
    }

    public void AddFoundResource(Resource resource)
    {
        _foundResources.Enqueue(resource);
    }


    public Movement GetFreeUnitForColonize()
    {
        foreach (var unit in _allUnits)
        {
            if (unit.CurrentResource == null && _allUnits.Count > MinCountUnits)
            {
                _allUnits.Remove(unit);
                return unit;
            }
        }

        return null;
    }

    public void RegisterUnit(Movement unit)
    {
        _allUnits.Add(unit);
    }
}
