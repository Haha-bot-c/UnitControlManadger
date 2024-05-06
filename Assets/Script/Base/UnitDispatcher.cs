using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class UnitDispatcher : MonoBehaviour
{
    private const int MinCountUnits = 1;

    [SerializeField] private List<Mover> _allUnits = new List<Mover>();
    [SerializeField] private BaseWarehouse _baseWarehouse;

    private Queue<Resource> _pendingResources = new Queue<Resource>();
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);

    private void Start()
    {
        StartCoroutine(DispatchUnits());
    }

    private IEnumerator DispatchUnits()
    {
        while (true)
        {
            Mover freeUnit = GetFreeUnit();
            SendUnitToResource(freeUnit);
            yield return _waitForSeconds;
        }
    }

    private Mover GetFreeUnit()
    {
        return _allUnits.FirstOrDefault(unit => unit.CurrentResource == null);
    }

    private void SendUnitToResource(Mover unit)
    {
        if (_pendingResources.Count > 0 && unit != null)
        {
            Resource resource = _pendingResources.Dequeue();
            unit.InitiateMoveToResource(resource);
            _baseWarehouse.AddIndexResirse(resource.ResourceIndex);
        }
    }

    public void AddFoundResource(Resource resource)
    {
        _pendingResources.Enqueue(resource);
    }

    public Mover GetFreeUnitForColonize()
    {
        Mover freeUnit = _allUnits.FirstOrDefault(unit => unit.CurrentResource == null && _allUnits.Count > MinCountUnits);
        _allUnits.RemoveAll(unit => unit == freeUnit);
        return freeUnit;
    }

    public void RegisterUnit(Mover unit)
    {
        _allUnits.Add(unit);
    }

    public int GetTotalUnitCount()
    {
        return _allUnits.Count;
    }
}
