using UnityEngine;

class JumpPlayerState : ActionableAirbornePlayerState {
	public override string name_ { get { return "JUMP"; } }
	public override int duration_frames_ { get { return 10; } }
	public JumpPlayerState(Character player) : base(player) {}

	public override PlayerState defaultNextState(Inputs inputs) {
		return new ActionableAirbornePlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
		if (frame_ == 0) {
			player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x, 30);
		}
	}
}