using UnityEngine;
using System;

class DashStartPlayerState : DashPlayerState
{
    public override string name_ { get { return "DASH"; } }
    public override int duration_frames_ { get { return 6; } }
    public DashStartPlayerState(Character player) : base(player) { }

    protected override PlayerState onControlStickNotPushed(Inputs inputs)
    {
        return this;
    }

    public override PlayerState defaultNextState(Inputs inputs)
    {
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if ((player_.move_direction_right_ && move.x > STICK_DEADZONE_THRESHOLD) ||
            (!player_.move_direction_right_ && move.x < -STICK_DEADZONE_THRESHOLD))
        {
            return new DashPlayerState(player_);
        }
        return new NeutralPlayerState(player_);
    }
}
