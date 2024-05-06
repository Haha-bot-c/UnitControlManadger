using System.Collections;
using UnityEngine;

public class BaseColonizationStatus : MonoBehaviour
{
    public bool IsColonizing { get; private set; } = false;

    private const int MinCountUnit = 2;
    private const int ÑolonizeResourceCount = 2;

    [SerializeField] private BaseWarehouse _baseWarehouse;
    [SerializeField] private UnitDispatcher _unitDispatcher;
    [SerializeField] private BaseInteractor _baseInterctionr;
    [SerializeField] private float _colonizationDelay = 3f;

    private int _collectedResources;
    private Coroutine _colonizeCorotine;
    private Mover _unit; 

    private void OnEnable()
    {
        _baseWarehouse.CollectedResourcesChanged += CollectResources;
        _baseInterctionr.FlagSet += ColonizeStart;
    }
    private void OnDisable()
    {
        _baseWarehouse.CollectedResourcesChanged -= CollectResources;
        _baseInterctionr.FlagSet -= ColonizeStart;
    }

    private void ColonizeStart(Vector3 positionFlag)
    {
        if (_colonizeCorotine == null 
            && _unitDispatcher.GetTotalUnitCount() > MinCountUnit)
        {
            IsColonizing = true;
            StartCoroutine(Colonize(positionFlag));
        }
    }
    private void CollectResources(int count)
    {
        _collectedResources = count;
    }

    private IEnumerator Colonize(Vector3 positionFlag)
    {
        WaitForSeconds _waitForSeconds = new WaitForSeconds(_colonizationDelay);

        while (true)
        {
            if(_collectedResources >= ÑolonizeResourceCount)
            {
                _unit = _unitDispatcher.GetFreeUnitForColonize();

                if (_unit != null)
                {
                    _unit.StartColonizing(positionFlag);
                    _baseWarehouse.SpendResources(ÑolonizeResourceCount);
                    IsColonizing = false;
                    _colonizeCorotine = null;

                    yield break;
                }
            }
           
            yield return _waitForSeconds; 
        }
    }

}
