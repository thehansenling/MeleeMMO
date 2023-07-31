using UnityEngine;
using System;

class UpAirPlayerState : AerialAttackPlayerState
{
	public override string name_ { get { return "UP_AIR"; } }
	public override int duration_frames_ { get { return 15; } }
	public UpAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.UpAir);
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
        if (player_.facing_right_)
        {
            return new Vector2(5, 15);
        }

        return new Vector2(-5, 15);
    }
}