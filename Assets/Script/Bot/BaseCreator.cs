using UnityEngine;

[RequireComponent(typeof(Mover))]
public class BaseCreator : MonoBehaviour
{
    [SerializeField] private UnitDispatcher _basePrefab;
    [SerializeField] private Mover _unit;

    private void OnEnable()
    {
        _unit.ColonizingPositionReached += OnColonizingPositionReached;  
    }

    private void OnColonizingPositionReached(Vector3 position)
    {
        UnitDispatcher dispatcher = Instantiate(_basePrefab, position, Quaternion.identity);
        dispatcher.RegisterUnit(_unit);
        _unit.AssignBase(dispatcher);
    }

    private void OnDisable()
    {
        _unit.ColonizingPositionReached -= OnColonizingPositionReached;  
    }  
}
