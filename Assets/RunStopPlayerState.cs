using UnityEngine;

class RunStopPlayerState : PlayerState {
	public override string name_ { get { return "RUN_STOP"; } }
	public override int duration_frames_ { get { return 10; } }
	public RunStopPlayerState(Character player) : base(player) {}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
		Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
		if (move.y < -STICK_DASH_THRESHOLD)
		{
			return new NeutralPlayerState(player_);
		}
		else if ((move.x < -STICK_DEADZONE_THRESHOLD && player_.move_direction_right_) || 
			(move.x > STICK_DEADZONE_THRESHOLD && !player_.move_direction_right_))
		{
			return new RunTurnPlayerState(player_);
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
		else if ((move.x < -STICK_DEADZONE_THRESHOLD && player_.move_direction_right_) || 
			(move.x > STICK_DEADZONE_THRESHOLD && !player_.move_direction_right_))
		{
			return new RunTurnPlayerState(player_);
		}
        return this;
	}

    protected override PlayerState onJumpPushed() {
		return new JumpSquatPlayerState(player_);
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		return new NeutralPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
        player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x * .8f, player_.rigid_body_.velocity.y);
        return;
	}
}