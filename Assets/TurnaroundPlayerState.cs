using UnityEngine;
using System;

class TurnaroundPlayerState : NeutralPlayerState {
	public override string name_ { get { return "TURNAROUND"; } }
	public override int duration_frames_ { get { return 7; } }
	private bool facing_right_ = true;
	public TurnaroundPlayerState(Character player, Inputs inputs) : base(player) 
	{
		Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
		facing_right_ = stick_value.x >= 0;
	}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
		return this;
	}

   	public override PlayerState defaultNextState(Inputs inputs)
	{
		player_.facing_right_ = 
			facing_right_;
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (Math.Abs(stick_value.x) > STICK_DEADZONE_THRESHOLD)
		{	
			return new WalkPlayerState(player_);	
		}
		return new NeutralPlayerState(player_);
	}
}