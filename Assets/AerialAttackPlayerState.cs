using UnityEngine;
using System;
using System.Collections.Generic;

class AerialAttackPlayerState : PlayerState {
    private bool fast_fall = false;
    public override string name_ { get { return "BACK_AIR"; } }
	public override int duration_frames_ { get { return -1; } }
    public HashSet<int> hit_ids_;
	public AerialAttackPlayerState(Character player) : base(player) 
    {
    }
    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 stick = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick.y < -STICK_DASH_THRESHOLD && player_.rigid_body_.velocity.y < 0)
        {
            fast_fall = true;
        }

        return this;
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 12);
    }
    protected override void onExecute(Inputs inputs)
    {
        if (fast_fall)
        {
            player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x, -player_.MAX_FALL_SPEED);
        }
    }
}