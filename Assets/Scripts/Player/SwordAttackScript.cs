using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordAttackScript : SkillObject
{
    [SerializeField] float _rotateSpeed = 1f;
    [SerializeField] float _speed = 3f;
    [SerializeField] Vector3 _endRotation;
    [SerializeField] Transform _root;

    int _damage;

    private void Awake()
    {
        _root.DOLocalRotate(_endRotation, _rotateSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetRelative(true);
    }

    private void Update()
    {
        this.transform.Translate(0, 0, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
