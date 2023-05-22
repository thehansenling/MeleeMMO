using UnityEngine;
using System;

class WalkPlayerState : PlayerState
{
    public override string name_ { get { return "WALK"; } }
    public override int duration_frames_ { get { return -1; } }
    public WalkPlayerState(Character player) : base(player) { }

    protected override PlayerState onControlStickNotPushed(Inputs inputs)
    {
		return new NeutralPlayerState(player_);
    }

    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick_value.y < -STICK_DASH_THRESHOLD || Math.Abs(stick_value.x) < STICK_DEADZONE_THRESHOLD)
        {
            return new NeutralPlayerState(player_);
        }
        return new DashPlayerState(player_);
    }

    protected override PlayerState onControlStickHeld(Inputs inputs)
    {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick_value.y < -STICK_DASH_THRESHOLD || Math.Abs(stick_value.x) < STICK_DEADZONE_THRESHOLD)
        {
            return new NeutralPlayerState(player_);
        }
        return this;
    }

    protected override PlayerState onJumpPushed() {
        return new JumpSquatPlayerState(player_);
    }

    protected override void onExecute(Inputs inputs)
    {
        if (frame_ == 0)
        {
            Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
            if (stick_value.x < 0)
            {
                player_.facing_right_ = false;
            }
            else
            {
                player_.facing_right_ = true;
            }
        }
        player_.MoveCharacter(inputs, player_.WALK_SPEED);
        return;
    }
}
