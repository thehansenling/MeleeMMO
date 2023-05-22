using UnityEngine;
using System;

class DashStartPlayerState : DashPlayerState
{
    public override string name_ { get { return "DASH_START"; } }
    public override int duration_frames_ { get { return 6; } }
    public DashStartPlayerState(Character player) : base(player) { }

    public override PlayerState defaultNextState(Inputs inputs)
    {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;

        bool facing_right = true;
        if (stick_value.x < 0)
        {
            facing_right = false;
        }

        if ((player_.facing_right_ && stick_value.x > STICK_DEADZONE_THRESHOLD) ||
            (!player_.facing_right_ && stick_value.x < -STICK_DEADZONE_THRESHOLD))
        {
            player_.facing_right_ = facing_right;
            return new DashPlayerState(player_);
        }
        player_.facing_right_ = facing_right;
        return new DashStopPlayerState(player_);
    }
}
