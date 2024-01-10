using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector2> OnMoveUnits;
    public event Action<Unit> OnChooseUnit;

    public void MoveUnits(Vector2 position) => OnMoveUnits?.Invoke(position);

    public void ChooseUnit(Unit unit) => OnChooseUnit?.Invoke(unit);
}
