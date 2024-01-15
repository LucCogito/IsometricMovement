using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        {
            var unit = Instantiate(_unitPrefab, new Vector3(i, 1, 3-i), Quaternion.identity, transform);
            unit.Initialize(_teamData);
            _units.Add(unit);
        }
        _selectedUnit = _units[0];
        OnUnitsCreated?.Invoke(_units);
    }

    private void MoveUnits(Vector2 positon)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(positon), out RaycastHit hit))
            MoveToPosition(new Vector2 (hit.point.x, hit.point.z));
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
        PathfindingManager.Instance.RequestPath(_selectedUnit.transform.position, new Vector3(position.x, 0, position.y), SetPaths);
    }

    private void SetPaths(Vector3[] path)
    {
        _selectedUnit.SetPath(path);
        foreach (var unit in _units)
            if (unit != _selectedUnit)
                unit.SetFollowTarget(_selectedUnit);
    }
}
