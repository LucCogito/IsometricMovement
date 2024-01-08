using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField] private TeamData _teamData;
    [SerializeField] private Unit _unitPrefab;

    private List<Unit> _units = new List<Unit>();

    public event Action<List<Unit>> OnUnitsCreated;

    private void Start()
    {
        for (int i = 0; i < _teamData.UnitsCount; i++)
            _units.Add(Instantiate(_unitPrefab, transform));
        OnUnitsCreated?.Invoke(_units);
    }
}
