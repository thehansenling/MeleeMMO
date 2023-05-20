using UnityEngine;
using System;

class InactionableAirbornePlayerState : PlayerState
{
    public override string name_ { get { return "INACTIONABLE_AIRBORNE"; } }
    public override int duration_frames_ { get { return -1; } }
    public InactionableAirbornePlayerState(Character player) : base(player) 
    {
    }


    public override PlayerState processOnCollision(Inputs inputs)
    {
        if (frame_ > 1)
        {
            return new LandingLagPlayerState(player_, 12);
        }
        return this;
    }

    protected override void onExecute(Inputs inputs)
    {
        //player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x, player_.rigid_body_.velocity.y - 1);
        //player_.directional_influence_ = ((StickInputAction)inputs.control_stick_.getInputAction()).value_; //can replace all moves with this

       

        return;
    }
}
