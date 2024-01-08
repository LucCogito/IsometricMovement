using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsPanel : MonoBehaviour
{
    [SerializeField] private UnitCell _unitCellPrefab;
    [SerializeField] private TeamController _teamController;

    private List<UnitCell> _cells = new List<UnitCell>();

    private void Awake()
    {
        _teamController.OnUnitsCreated += AddUnitCells;
    }

    private void AddUnitCells(List<Unit> units)
    {
        foreach (var unit in units)
        {
            var cell = Instantiate(_unitCellPrefab, transform);
            cell.Initialize(unit);
            cell.OnClicked += ChooseUnit;
            _cells.Add(cell);
        }
    }

    private void ChooseUnit(Unit unit)
    {
        
    }
}
