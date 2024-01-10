using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _image;

    private Unit _assignedUnit;

    public event Action<Unit> OnClicked;

    public void Initialize(Unit unit)
    {
        _assignedUnit = unit;
        _image.color = unit.Color;
    }

    public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(_assignedUnit);
}
