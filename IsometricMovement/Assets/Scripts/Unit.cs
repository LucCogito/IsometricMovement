using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Renderer _renderer;

    private float _stopMargin = .5f, _stopMarginSqr;
    private float _followStopMargin = 1.5f, _followStopMarginSqr;
    private float _speed = 3;
    private Vector2 _targetPosition;
    private Unit _followTarget;

    public bool IsMoving {get; private set;}

    public Color Color { get; private set; }

    private void Awake()
    {
        _stopMarginSqr = _stopMargin * _stopMargin;
        _followStopMarginSqr = _followStopMargin * _followStopMargin;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
        MoveTick();
    }

    public void Initialize(float speed)
    {
        _speed = speed;
        Color = Random.ColorHSV(0, 1, 1, 1, .8f, .8f, 1, 1);
        var mpt = new MaterialPropertyBlock();
        mpt.SetColor("_Color", Color);
        _renderer.SetPropertyBlock(mpt);
    }

    public void SetPath(Vector2 target)
    {
        IsMoving = true;
        _followTarget = null;
        _targetPosition = target;
    }

    public void Follow(Unit unit)
    {
        IsMoving = true;
        _followTarget = unit;
    }

    private void MoveTick()
    {
        if (IsMoving)
        {
            if (_followTarget != null)
            {
                var targetPosition = _followTarget.transform.position;
                var direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.z - transform.position.z);
                if (direction.sqrMagnitude <= _followStopMarginSqr)
                {
                    if(!_followTarget.IsMoving)
                        IsMoving = false;
                }
                else
                {
                    var moveVector = direction.normalized * Time.fixedDeltaTime * _speed;
                    _rigidbody.MovePosition(transform.position + new Vector3(moveVector.x, 0, moveVector.y));
                }
            }
            else
            {
                var direction = _targetPosition - new Vector2(transform.position.x, transform.position.z);
                if (direction.sqrMagnitude <= _stopMarginSqr)
                    IsMoving = false;
                else
                {
                    var moveVector = direction.normalized * Time.fixedDeltaTime * _speed;
                    _rigidbody.MovePosition(transform.position + new Vector3(moveVector.x, 0, moveVector.y));
                }
            }
        }
    }
}
