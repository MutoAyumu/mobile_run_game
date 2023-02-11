using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using StateBase = StatePatternBase<EnemyController>.StateBase;

public partial class EnemyController
{
    [Header("Idle")]
    [SerializeField] float _interval = 2f;

    public class EnemyIdleState : StateBase
    {
        #region 変数
        Timer _intervalTimer = new Timer();
        #endregion

        #region プロパティ
        #endregion

        public override void Init()
        {
            _intervalTimer.Setup(Owner._interval);
        }
        public override void OnUpdate()
        {
            if (_intervalTimer.RunTimer())
            {
                StatePattern.ChangeState((int)StateType.Attack);
            }
        }
    }
}