using UnityEngine;

class AirdodgePlayerState : PlayerState {
	public override string name_ { get { return "AIR_DODGE"; } }
	public override int duration_frames_ { get { return -1; } }
	public AirdodgePlayerState(Character player) : base(player) { }

    public AirdodgePlayerState(Character player, Inputs inputs) : base(player) 
	{
		if (frame_ == 0) {
			Vector2 direction = ((StickInputAction)inputs.control_stick_.getInputAction()).value_.normalized;
			player_.rigid_body_.velocity = direction * 30;
		}
	}

	public override PlayerState processOnCollision(Inputs inputs) {
		return new LandingLagPlayerState(player_);
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		return this;
	}

	protected override void onExecute(Inputs inputs) {
		return;
	}
}