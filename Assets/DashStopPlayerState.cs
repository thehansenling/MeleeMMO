class DashStopPlayerState : PlayerState {
	public override string name_ { get { return "RUN_STOP"; } }
	public override int duration_frames_ { get { return 10; } }
	public DashStopPlayerState(Character player) : base(player) {}

	protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        return this;
    }

	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
        return this;
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		return new NeutralPlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
		return;
	}
}