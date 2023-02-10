using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using StateBase = StatePatternBase<PlayerController>.StateBase;

public partial class PlayerController
{
    [Header("Jump")]
    [SerializeField] float _jumpPower = 1;
    [Header("IsGroundCheck")]
    [SerializeField] float _groundCheckRadius = 1f;

    public class PlayerJump : StateBase
    {

        public override void OnEnter()
        {
            OnJump();
            StatePattern.ChangeState((int)StateType.Move);
        }

        void OnJump()
        {
            var vel = Owner._rb.velocity;
            vel.y = 0;
            Owner._rb.velocity = vel;
            Owner._rb.AddForce(Vector3.up * Owner._jumpPower, ForceMode.VelocityChange);
        }
    }
}