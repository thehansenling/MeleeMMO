using UnityEngine;

class DashStopPlayerState : PlayerState {
	public override string name_ { get { return "DASH_STOP"; } }
	public override int duration_frames_ { get { return 10; } }
	public DashStopPlayerState(Character player) : base(player) {}

    protected override PlayerState onControlStickHeld(Inputs inputs)
    {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (frame_ <= 2 &&
            ((player_.facing_right_ && stick_value.x < -STICK_DEADZONE_THRESHOLD) ||
            (!player_.facing_right_ && stick_value.x > STICK_DEADZONE_THRESHOLD)))
        {
            return new DashPlayerState(player_);
        }
        return this;
    }
    protected override PlayerState onJumpPushed()
    {
        return new JumpSquatPlayerState(player_);
    }

    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (frame_ <= 2 &&
            ((player_.facing_right_ && stick_value.x < -STICK_DEADZONE_THRESHOLD) ||
            (!player_.facing_right_ && stick_value.x > STICK_DEADZONE_THRESHOLD)))
        {
            return new DashPlayerState(player_);
        }
        return this;
    }

    public override PlayerState defaultNextState(Inputs inputs) {
		return new NeutralPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
        player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x * .8f, player_.rigid_body_.velocity.y);
		return;
	}
}