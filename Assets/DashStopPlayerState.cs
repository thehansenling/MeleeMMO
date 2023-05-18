using UnityEngine;

class DashStopPlayerState : PlayerState {
	public override string name_ { get { return "DASH_STOP"; } }
	public override int duration_frames_ { get { return 10; } }
	public DashStopPlayerState(Character player) : base(player) {}

    protected override PlayerState onControlStickHeld(Inputs inputs)
    {
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (frame_ <= 2 &&
            ((player_.move_direction_right_ && move.x < -STICK_DEADZONE_THRESHOLD) ||
            (!player_.move_direction_right_ && move.x > STICK_DEADZONE_THRESHOLD)))
        {
            return new DashStartPlayerState(player_);
        }
        return this;
    }

    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (frame_ <= 2 &&
            ((player_.move_direction_right_ && move.x < -STICK_DEADZONE_THRESHOLD) ||
            (!player_.move_direction_right_ && move.x > STICK_DEADZONE_THRESHOLD)))
        {
            return new DashStartPlayerState(player_);
        }
        return this;
    }

    public override PlayerState defaultNextState(Inputs inputs) {
		return new NeutralPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
		return;
	}
}