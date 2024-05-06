using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BaseOperatorFlagSet : MonoBehaviour
{
    [SerializeField] private List<BaseInteractor> _bases = new();
    [SerializeField] private TerrainClick _terrain;

    private void OnEnable()
    {
        _terrain.OnClik += GetPositionClik;
    }

    private void GetPositionClik(Vector3 position)
    {
        _bases.Where(bas => bas.IsActiv).ToList().ForEach(bas => bas.HandleBaseClick(position));
    }

    private void OnDisable()
    {
        _terrain.OnClik -= GetPositionClik;
    }

    public void RegisterBase(UnitDispatcher dispatcher)
    {
        dispatcher.TryGetComponent(out BaseInteractor baseFlagSet);
        _bases.Add(baseFlagSet);
    }
}