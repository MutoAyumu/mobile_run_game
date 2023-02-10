using UnityEngine;
using UniRx;
using StateBase = StatePatternBase<PlayerController>.StateBase;

public partial class PlayerController
{
    [Header("Move")]
    [SerializeField] float _moveSpeed = 1;

    public class PlayerMove : StateBase
    {
        bool _isGroundChecked;
        LayerMask _groundLayer;
        float _currentSpeed;
        Vector2 _dir;

        const string JUMP_PARAM = "IsJump";
        const string GROUND_LAYER_NAME = "Ground";

        public override void Init(PlayerController owner)
        {
            _currentSpeed = owner._moveSpeed;
            _groundLayer = LayerMask.GetMask(GROUND_LAYER_NAME);
            owner.Input.MoveVector.Subscribe(SetDirection).AddTo(owner);
            owner.Input.JumpSub.Subscribe(_ => OnJump()).AddTo(owner);
        }

        public override void OnUpdate()
        {
            OnMove();
        }
        void OnMove()
        {
            if (_dir != Vector2.zero)
            {
                var vel = new Vector3(_dir.x, 0, 0).normalized * _currentSpeed;
                vel.y = Owner._rb.velocity.y;
                Owner._rb.velocity = vel;
            }

            _isGroundChecked = IsGroundCheck();
            Owner._anim.SetBool(JUMP_PARAM, !_isGroundChecked);
        }
        void OnJump()
        {
            if (!_isGroundChecked) return;

            StatePattern.ChangeState((int)StateType.Jump);
        }

        bool IsGroundCheck()
        {
            var check = false;
            var hit = Physics.OverlapSphere(Owner._thisTransform.position, Owner._groundCheckRadius, _groundLayer);

            if (hit.Length > 0)
            {
                check = true;
            }

            return check;
        }

        void SetDirection(Vector2 vec)
        {
            _dir = new Vector2(vec.x, vec.y);
        }
    }
    private void OnDrawGizmosSelected()
    {
        var pos = Application.isPlaying ? _thisTransform.position : this.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, _groundCheckRadius);
    }
}
