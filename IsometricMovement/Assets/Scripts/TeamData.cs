using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TeamData")]
public class TeamData : ScriptableObject
{
    [field: SerializeField] public int UnitsCount { get; private set; }
    [field: SerializeField] public Vector2 SpeedRange { get; private set; }
    [field: SerializeField] public Vector2 FollowIntervalRange { get; private set; }
    [field: SerializeField] public Vector2 StopMarginRange { get; private set; }
}
