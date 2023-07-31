using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

class DownAirPlayerState : AerialAttackPlayerState
{
    public override string name_ { get { return "DOWN_AIR"; } }
    public override int duration_frames_ { get { return 30; } }
    public DownAirPlayerState(Character player) : base(player)
    {
        player.animator_.SetInteger("state", (int)CharacterState.DownAir);
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 1);
    }

    public override PlayerState defaultNextState(Inputs inputs)
    {
        return new ActionableAirbornePlayerState(player_);
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

        return new Vector2(x_direction, -20);
    }

}