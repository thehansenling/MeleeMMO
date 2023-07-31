using UnityEngine;

class JumpPlayerState : ActionableAirbornePlayerState {
	public override string name_ { get { return "JUMP"; } }
	public override int duration_frames_ { get { return 10; } }

	private int jump_force_;
	public JumpPlayerState(Character player, int jump_force = 20) : base(player) 
	{
		jump_force_ = jump_force;
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		return new ActionableAirbornePlayerState(player_);
	}

    protected override void onExecute(Inputs inputs) {
		base.onExecute(inputs);
		if (frame_ == 0) {
			player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x, jump_force_);
		}
	}
}