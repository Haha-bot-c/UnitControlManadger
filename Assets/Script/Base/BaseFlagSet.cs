using System;
using UnityEngine;

public class BaseFlagSet : MonoBehaviour
{
    private static BaseFlagSet _activeBase;
    public bool IsActiv { get; private set; }

    [SerializeField] private GameObject _flagPrefab;
    [SerializeField] private DisplayText _displayText;
    private GameObject _flagInstance;
    
    public event Action<Vector3> FlagSet;

    private void OnMouseDown()
    {
        if (IsActiv)
        {
            Deactivate();
        }
        else
        {
            if (_activeBase != null)
            {
                _activeBase.Deactivate();
            }

            Activate();
        };
    }

    private void Activate()
    {
        _activeBase = this;
        IsActiv = true;
        _displayText.TextShowActive(GetInstanceID().ToString());
    }

    private void Deactivate()
    {
        IsActiv = false;
        _displayText.TextShowDeactivated(GetInstanceID().ToString());
    }

    private void SpawnFlag(Vector3 position)
    {
        if (_flagInstance == null && _flagPrefab != null)
        {
            _flagInstance = Instantiate(_flagPrefab, position, Quaternion.identity);
        }
        else
        {
            _flagInstance.transform.position = position;
        }
    }

    public void HandleBaseClick(Vector3 position)
    {
        _displayText.UpdateClickPositionText(position);
        SpawnFlag(position);
        FlagSet?.Invoke(position);
    }
}
