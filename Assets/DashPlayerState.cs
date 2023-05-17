using UnityEngine;
using System;

class DashPlayerState : PlayerState {
	public override string name_ { get { return "DASH"; } }
	public override int duration_frames_ { get { return 20; } }
	public DashPlayerState(Character player) : base(player) {}

	
	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (move.y < -STICK_DASH_THRESHOLD)
        {
            return new NeutralPlayerState(player_);
        } else if (((player_.move_direction_right_ && move.x < -STICK_DEADZONE_THRESHOLD) || 
            (!player_.move_direction_right_ && move.x > STICK_DEADZONE_THRESHOLD))) {
            return new DashTurnaroundPlayerState(player_);
        }
        return this;
	}
		
	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (move.y < -STICK_DASH_THRESHOLD)
        {
            return new NeutralPlayerState(player_);
        }
        else if (player_.move_direction_right_)
        {
            if (move.x < -STICK_DEADZONE_THRESHOLD)
            {
                return new DashTurnaroundPlayerState(player_);
            }
        }
        else
        {
            if (move.x > STICK_DEADZONE_THRESHOLD)
            {
                return new DashTurnaroundPlayerState(player_);
            }
        }
        return new DashPlayerState(player_);
	}

    // protected override PlayerState onControlStickNotPushed(Inputs inputs) {

    //     return new NeutralPlayerState(player_);
    // }

	protected override PlayerState onJumpPushed() {
		return new JumpSquatPlayerState(player_);
	}

	public override PlayerState defaultNextState(Inputs inputs) {
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if ((player_.move_direction_right_ && move.x > STICK_DEADZONE_THRESHOLD) || 
            (!player_.move_direction_right_ && move.x < -STICK_DEADZONE_THRESHOLD))
        {
            return new RunPlayerState(player_);
        }
		return new NeutralPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (frame_ == 0)
		{
            if (move.x < 0)
            {
                player_.move_direction_right_ = false;
            }
            else
            {
                player_.move_direction_right_ = true;
            }
        }
        player_.MoveCharacterFacing();
		return;
	}
}
