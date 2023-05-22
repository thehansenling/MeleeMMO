using UnityEngine; 

class DashTurnaroundPlayerState : PlayerState {
	public override string name_ { get { return "DASH_TURNAROUND"; } }
	public override int duration_frames_ { get { return 10; } }
	public DashTurnaroundPlayerState(Character player) : base(player) {}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
		Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
		if (frame_ == 1 && 
			((player_.facing_right_ && stick_value.x < -STICK_DEADZONE_THRESHOLD) || 
			(!player_.facing_right_ && stick_value.x > STICK_DEADZONE_THRESHOLD)))
		{
			return new DashPlayerState(player_);
		}
        return this;
    }

	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
		Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
		if (frame_ == 1 && 
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
}