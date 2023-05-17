using UnityEngine;
using System;

class RunTurnPlayerState : PlayerState {
	public override string name_ { get { return "RUN_TURN"; } }
	public override int duration_frames_ { get { return 20; } }
	public RunTurnPlayerState(Character player) : base(player) {}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        return this;
    }

	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
        return this;
    }

    protected override PlayerState onJumpPushed() {
        return new JumpSquatPlayerState(player_);
    }

	public override PlayerState defaultNextState(Inputs inputs) {
        //if the input snaps to not pushed from run, we go through here to runstop to neutral, not just neutral
        return new RunPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
        if (frame_ == 0)
        {
            Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
            if (!(inputs.control_stick_.stickVelocity().x > STICK_DASH_THRESHOLD) && 
                (inputs.control_stick_.stickVelocity().x < -STICK_DASH_THRESHOLD || 
                move.x < -STICK_DEADZONE_THRESHOLD))
            {
                player_.move_direction_right_ = false;
            }
            else
            {
                player_.move_direction_right_ = true;
            }
        }
        return;
	}
}