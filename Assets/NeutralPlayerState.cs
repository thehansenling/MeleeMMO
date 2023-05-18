using UnityEngine;
using System;

class NeutralPlayerState : PlayerState {
	public override string name_ { get { return "NEUTRAL"; } }
	public override int duration_frames_ { get { return -1; } }
	public NeutralPlayerState(Character player) : base(player) {}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (Math.Abs(move.x) < .2)
		{
			return this;
		}
		return new TurnaroundPlayerState(player_, inputs); //should be walk
	}

	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
		return new DashStartPlayerState(player_);
	}

    protected override PlayerState onControlStickNotPushed(Inputs inputs)
    {
        return this;

    }

    protected override PlayerState onJumpPushed() {
		return new JumpSquatPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
		return;
	}
}