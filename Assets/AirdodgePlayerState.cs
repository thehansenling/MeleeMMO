using UnityEngine;

class AirdodgePlayerState : PlayerState {
	public override string name_ { get { return "AIR_DODGE"; } }
	public override int duration_frames_ { get { return 24; } }
	public AirdodgePlayerState(Character player) : base(player) { }

    public AirdodgePlayerState(Character player, Inputs inputs) : base(player) 
	{
		Debug.Log("AIRDODGING HERE");
		Vector2 direction = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
		if (direction.x < STICK_DEADZONE_THRESHOLD && direction.x > -STICK_DEADZONE_THRESHOLD) { direction.x = 0; }
		direction = direction.normalized;
        player_.rigid_body_.velocity = direction * 25;
        player_.air_dodge_velocity_ = player_.rigid_body_.velocity;
        player_.air_dodge_ = true;
	}

	public override PlayerState processOnCollision(Inputs inputs) {
        Debug.Log("LANDING LAG");
        return new LandingLagPlayerState(player_, 12);
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		player_.air_dodge_ = false;
		player_.rigid_body_.velocity = new Vector2(0, 0);
        player_.rigid_body_.gravityScale = 1;
		Debug.Log("DEFAULT INACTIONABLE");
        return new InactionableAirbornePlayerState(player_);
	}

	protected override void onExecute(Inputs inputs) {
		player_.rigid_body_.gravityScale = 0;
		int fast_frames = 10;
		if (frame_ < fast_frames)
		{
			player_.rigid_body_.velocity = player_.rigid_body_.velocity * .95f;
		}
		else
		{
            player_.rigid_body_.velocity = player_.rigid_body_.velocity * .7f;
        }
        player_.air_dodge_velocity_ = player_.rigid_body_.velocity;
		return;
	}
}