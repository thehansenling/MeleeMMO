class JumpSquatPlayerState : PlayerState {
	public override string name_ { get { return "JUMPSQUAT"; } }
	public override int duration_frames_ { get { return 4; } }
	public JumpSquatPlayerState(Character player) : base(player) {}

	public override PlayerState processOnCollision(Inputs inputs) {
		return this;
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		return new JumpPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
		return;
	}
}