using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Renderer _renderer;

    private float _stopMargin, _stopMarginSqr;
    private float _updateUnitFollowInterval, _currentUnitFollowTime;
    private float _speed;
    private Vector3[] _path;
    private int _targetIndex;
    private bool _waitingForPath;
    private Coroutine _followUnitCoroutine, _followPathCoroutine;

    public bool IsMoving {get; private set;}

    public Color Color { get; private set; }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    public void Initialize(TeamData teamData )
    {
        _speed = Random.Range(teamData.SpeedRange.x, teamData.SpeedRange.y);
        _updateUnitFollowInterval = Random.Range(teamData.FollowIntervalRange.x, teamData.FollowIntervalRange.y);
        _stopMargin = Random.Range(teamData.StopMarginRange.x, teamData.StopMarginRange.y);
        _stopMarginSqr = _stopMargin * _stopMargin;
        Color = Random.ColorHSV(0, 1, 1, 1, .8f, .8f, 1, 1);
        var mpt = new MaterialPropertyBlock();
        mpt.SetColor("_Color", Color);
        _renderer.SetPropertyBlock(mpt);
    }

    public void SetFollowTarget(Unit target)
    {
        IsMoving = true;
        if (_followUnitCoroutine != null)
            StopCoroutine(_followUnitCoroutine);
        _followUnitCoroutine = StartCoroutine(FollowUnit(target));
    }

    public void SetPath(Vector3[] path, bool stopFollowingUnit = true)
    {
        if (_followUnitCoroutine != null)
            StopCoroutine(_followUnitCoroutine);
        SetPath(path);
    }

    private void SetPath(Vector3[] path)
    {
        _waitingForPath = false;
        if (path.Length > 0)
        {
            IsMoving = true;
            _path = path;
            _targetIndex = 0;
            if (_followPathCoroutine != null)
                StopCoroutine(_followPathCoroutine);
            _followPathCoroutine = StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];
        Vector2 distanceVector;
        while (true)
        {
            distanceVector = new Vector2(transform.position.x - currentWaypoint.x, transform.position.z - currentWaypoint.z);
            if (distanceVector.sqrMagnitude <= _stopMarginSqr)
            {
                if (++_targetIndex >= _path.Length)
                {
                    IsMoving = false;
                    _path = null;
                    yield break;
                }
                currentWaypoint = _path[_targetIndex];
            }
            var waypointPos = new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z);
            var targetPos = Vector3.MoveTowards(transform.position, waypointPos, _speed * Time.deltaTime);
            _rigidbody.MovePosition(targetPos);
            yield return null;
        }
    }

    private IEnumerator FollowUnit(Unit unit)
    {
        _currentUnitFollowTime = _updateUnitFollowInterval;
        while (unit.IsMoving)
        {
            _currentUnitFollowTime -= Time.deltaTime;
            if ((_path == null || _currentUnitFollowTime <= 0) && !_waitingForPath)
            {
                _currentUnitFollowTime = _updateUnitFollowInterval;
                PathfindingManager.Instance.RequestPath(transform.position, unit.transform.position, SetPath);
                _waitingForPath = true;
            }
            yield return null;
        }
        PathfindingManager.Instance.RequestPath(transform.position, unit.transform.position, SetPath);
    }
}
