using UnityEngine;
using System;

class NeutralAirPlayerState : AerialAttackPlayerState
{
	public override string name_ { get { return "NEUTRAL_AIR"; } }
	public override int duration_frames_ { get { return 20; } }
	public NeutralAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.NeutralAir);
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
        var x_direction = contact_direction.x;
        if (x_direction > 0)
        {
            return new Vector2(20, y_direction);
        }

        return new Vector2(-20, y_direction);
    }
}