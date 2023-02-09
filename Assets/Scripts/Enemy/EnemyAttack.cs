using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(ArrangementObjectGenerator))]
public class EnemyAttack : MonoBehaviour
{
    #region 変数
    [SerializeField] float _power = 1f;
    [SerializeField] float _interval = 2f;
    [SerializeField] Transform[] _setPositions;
    ArrangementObjectGenerator _generator;

    Timer _attackTimer = new Timer();
    #endregion
    
    #region プロパティ
    #endregion

    public void Init(EnemyController enemy)
    {
        enemy.OnUpdateSub.Subscribe(_ => OnUpdate()).AddTo(this);

        _attackTimer.Setup(_interval);
        TryGetComponent(out _generator);
    }
    void OnUpdate()
    {
        if(_attackTimer.RunTimer())
        {
            OnAttack();
        }
    }
    void OnAttack()
    {
        var r = Random.Range(0, _setPositions.Length);
        var obj = _generator.OnCreate();
        obj.position = _setPositions[r].position;
    }
}
