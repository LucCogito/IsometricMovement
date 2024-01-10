using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField] private TeamData _teamData;
    [SerializeField] private Unit _unitPrefab;

    private InputManager _inputManager;
    private List<Unit> _units = new List<Unit>();
    private Unit _selectedUnit;
    private Vector2 _currentMovementTarget;

    public event Action<List<Unit>> OnUnitsCreated;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _inputManager.OnMoveUnits += MoveUnits;
        _inputManager.OnChooseUnit += ChooseUnit;
    }

    private void OnDestroy()
    {
        if (_inputManager != null)
        {
            _inputManager.OnMoveUnits -= MoveUnits;
            _inputManager.OnChooseUnit -= ChooseUnit;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _teamData.UnitsCount; i++)
            _units.Add(Instantiate(_unitPrefab, transform));
        OnUnitsCreated?.Invoke(_units);
    }

    private void MoveUnits(Vector2 positon)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(positon), out RaycastHit hit))
            MoveToPosition(hit.point);
    }

    private void ChooseUnit(Unit unit)
    {
        if (_selectedUnit != unit)
        {
            _selectedUnit = unit;
            if (_selectedUnit.IsMoving)
                MoveToPosition(_currentMovementTarget);
        }
    }

    private void MoveToPosition(Vector2 position)
    {
        _currentMovementTarget = position;
    }
}
