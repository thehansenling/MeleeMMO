using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

class DownSpecialPlayerState : PlayerState
{
    public override string name_ { get { return "DOWN_SPECIAL"; } }
    public override int duration_frames_ { get { return 20; } }
    private bool fast_fall = false;
    public DownSpecialPlayerState(Character player) : base(player)
    {
        player.animator_.SetInteger("state", (int)CharacterState.DownSpecial);
    }
    public override PlayerState defaultNextState(Inputs inputs)
    {
        if (player_.in_air_)
        {
            return new ActionableAirbornePlayerState(player_);
        }
        return new NeutralPlayerState(player_);
    }
    protected override PlayerState onJumpPushed()
    {
        if (player_.jumps_ > 0)
        {
            player_.rigid_body_.velocity = new Vector2(0, 0);
            player_.jumps_--;
            return new JumpPlayerState(player_, 30);
        }
        return this;
    }
    public override Vector2 HitForce(Collider2D collision)
    {
        var transform = player_.transform;
        var contact_direction = collision.ClosestPoint(transform.position) - new Vector2(transform.position.x, transform.position.y);
        Debug.Log(contact_direction);
        var x_direction = contact_direction.x * 20;
        if (x_direction > 0)
        {
            x_direction = Mathf.Min(20, x_direction);
        }
        else
        {
            x_direction = Mathf.Max(-20, x_direction);
        }

        return new Vector2(x_direction, 30);
    }
    protected override void onExecute(Inputs inputs)
    {
        if (player_.in_air_)
        {
            player_.rigid_body_.velocity = new Vector2(0, -.2f);
        }
        return;
    }
}