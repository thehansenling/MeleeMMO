using UnityEngine;
using System;

class BackAirPlayerState : AerialAttackPlayerState
{
	public override string name_ { get { return "BACK_AIR"; } }
	public override int duration_frames_ { get { return 20; } }
	public BackAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.BackAir);
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
        var y_direction = contact_direction.y * 20;
        if (player_.facing_right_)
        {
            return new Vector2(-20, y_direction);
        }

        return new Vector2(20, y_direction);
    }
}