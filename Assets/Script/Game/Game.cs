using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField] private List<BaseFlagSet> _bases = new();
    [SerializeField] private TerrainClick _terrain;

    private void OnEnable()
    {
        _terrain.OnClik += GetPositionClik;
    }  

    private void GetPositionClik(Vector3 vector3)
    {
        foreach(var bas in _bases)
        {
            if (bas.IsActiv)
            {
                if(vector3 != null)
                {
                    bas.HandleBaseClick(vector3);
                } 
            }
        }  
    }

    private void OnDisable()
    {
        _terrain.OnClik -= GetPositionClik;
    }

    public void RegisterBase(UnitDispatcher dispatcher)
    {
        var baseFlagSet = dispatcher.GetComponent<BaseFlagSet>();
        _bases.Add(baseFlagSet);
    }
}