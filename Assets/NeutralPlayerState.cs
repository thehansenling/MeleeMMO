using UnityEngine;
using System;

class NeutralPlayerState : PlayerState {
	public override string name_ { get { return "NEUTRAL"; } }
	public override int duration_frames_ { get { return -1; } }
	public NeutralPlayerState(Character player) : base(player) {}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (Math.Abs(stick_value.x) < STICK_DEADZONE_THRESHOLD)
		{
			return this;
		}
		else if ((stick_value.x > STICK_DEADZONE_THRESHOLD && !player_.facing_right_ ) ||
                (stick_value.x < -STICK_DEADZONE_THRESHOLD && player_.facing_right_))
		{
            return new TurnaroundPlayerState(player_, inputs); //should be walk
        }
		return new WalkPlayerState(player_);
	}

	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
		return new DashStartPlayerState(player_);
	}

    protected override PlayerState onJumpPushed() {
		return new JumpSquatPlayerState(player_);
	}
}