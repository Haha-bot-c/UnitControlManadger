using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(BaseOperatorFlagSet))]
public class BaseScanner : MonoBehaviour
{
    [SerializeField] private List<UnitDispatcher> _bases = new List<UnitDispatcher>();
    [SerializeField] private float _scanRadius = 10f;
    [SerializeField] private WaitForSeconds _scanInterval = new WaitForSeconds(2f);

    private BaseOperatorFlagSet _baseOperator;
    private List<Resource> _foundedResources = new List<Resource>();
    private int _resourceIndex;

    private void Start()
    {
        _baseOperator = GetComponent<BaseOperatorFlagSet>();
        StartCoroutine(PeriodicScan());
    }

    private IEnumerator PeriodicScan()
    {
        int currentBaseIndex = 0;

        while (true)
        {
            UnitDispatcher currentBase = _bases[currentBaseIndex];

            ScanForResourcesAndBase(currentBase);

            currentBaseIndex = ++currentBaseIndex % _bases.Count;

            yield return _scanInterval;
        }
    }

    private void ScanForResourcesAndBase(UnitDispatcher currentBase)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Resource resourceComponent) && _foundedResources.Contains(resourceComponent) == false)
            {
                AddResourceToBase(resourceComponent, currentBase);
            }
            else if(collider.TryGetComponent(out UnitDispatcher dispatcher) && _bases.Contains(dispatcher) == false)
            {
                _bases.Add(dispatcher);
                _baseOperator.RegisterBase(dispatcher);
            }
        }
    }
    private void AddResourceToBase(Resource resourceComponent, UnitDispatcher dispather)
    {
        _resourceIndex++;
        resourceComponent.AssignResourceIndex(_resourceIndex);
        dispather.AddFoundResource(resourceComponent);
        _foundedResources.Add(resourceComponent);
    }
}
