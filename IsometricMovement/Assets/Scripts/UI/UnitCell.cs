using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCell : MonoBehaviour, IPointerClickHandler
{
    private Unit _assignedUnit;

    public event Action<Unit> OnClicked;

    public void Initialize(Unit unit) => _assignedUnit = unit;

    public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(_assignedUnit);
}
