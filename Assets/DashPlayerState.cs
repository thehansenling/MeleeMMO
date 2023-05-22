using UnityEngine;
using System;

class DashPlayerState : PlayerState {
	public override string name_ { get { return "DASH"; } }
	public override int duration_frames_ { get { return 10; } }
	public DashPlayerState(Character player) : base(player) {}

	
	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick_value.y < -STICK_DASH_THRESHOLD)
        {
            player_.rigid_body_.velocity = new Vector2(0, player_.rigid_body_.velocity.y);
            return new NeutralPlayerState(player_);
        } else if (((player_.facing_right_ && stick_value.x < -STICK_DEADZONE_THRESHOLD) || 
            (!player_.facing_right_ && stick_value.x > STICK_DEADZONE_THRESHOLD))) {
            return new DashStopPlayerState(player_);
        }
        return this;
	}
		
	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick_value.y < -STICK_DASH_THRESHOLD)
        {
            player_.rigid_body_.velocity = new Vector2(0, player_.rigid_body_.velocity.y);
            return new NeutralPlayerState(player_);
        }
        else if (player_.facing_right_)
        {
            if (stick_value.x < -STICK_DEADZONE_THRESHOLD)
            {
                return new DashStopPlayerState(player_);
            }
        }
        else
        {
            if (stick_value.x > STICK_DEADZONE_THRESHOLD)
            {
                return new DashStopPlayerState(player_);
            }
        }
        return new DashPlayerState(player_);
	}

    protected override PlayerState onControlStickNotPushed(Inputs inputs)
    {
        return new DashStopPlayerState(player_);
    }

    protected override PlayerState onJumpPushed() {
		return new JumpSquatPlayerState(player_);
	}

	public override PlayerState defaultNextState(Inputs inputs) {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if ((player_.facing_right_ && stick_value.x > STICK_DEADZONE_THRESHOLD) || 
            (!player_.facing_right_ && stick_value.x < -STICK_DEADZONE_THRESHOLD))
        {
            return new RunPlayerState(player_);
        }
		return new DashStopPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (frame_ == 0)
		{
            if (stick_value.x < 0)
            {
                player_.facing_right_ = false;
            }
            else
            {
                player_.facing_right_ = true;
            }
        }
        player_.MoveCharacterFacing();
		return;
	}
}
