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
        #region �ϐ�
        Timer _intervalTimer = new Timer();
        #endregion

        #region �v���p�e�B
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