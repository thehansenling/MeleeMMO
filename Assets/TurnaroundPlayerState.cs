using UnityEngine;
using System;

class TurnaroundPlayerState : NeutralPlayerState {
	public override string name_ { get { return "TURNAROUND"; } }
	public override int duration_frames_ { get { return 7; } }
	private bool facing_right_ = true;
	public TurnaroundPlayerState(Character player, Inputs inputs) : base(player) 
	{
		Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
		facing_right_ = move.x >= 0;
	}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
		return this;
	}

   	public override PlayerState defaultNextState(Inputs inputs)
	{
		player_.move_direction_right_ = 
			facing_right_;
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (Math.Abs(move.x) > .2)
		{	
			return new WalkPlayerState(player_);	
		}
		return new NeutralPlayerState(player_);
	}


	protected override void onExecute(Inputs inputs) {
		return;
	}
}