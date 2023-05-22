using UnityEngine;
using System;

class RunTurnPlayerState : PlayerState {
	public override string name_ { get { return "RUN_TURN"; } }
	public override int duration_frames_ { get { return 26; } }
	public RunTurnPlayerState(Character player) : base(player) {}

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
            Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
            if (!(inputs.control_stick_.stickVelocity().x > STICK_DASH_THRESHOLD) && 
                (inputs.control_stick_.stickVelocity().x < -STICK_DASH_THRESHOLD ||
                stick_value.x < -STICK_DEADZONE_THRESHOLD))
            {
                player_.facing_right_ = false;
            }
            else
            {
                player_.facing_right_ = true;
            }
        }
        return;
	}
}