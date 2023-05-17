using UnityEngine;

class LandingLagPlayerState : PlayerState {
	public override string name_ { get { return "LANDING_LAG"; } }
	//public override int duration_frames_;// { get { return duration_frames_; } }
	public LandingLagPlayerState(Character player, int duration_frames = 20) : base(player) 
	{
		duration_frames_ = duration_frames;
	}

	public override PlayerState defaultNextState(Inputs inputs) {
		return new NeutralPlayerState(player_);
	}

    protected override void onExecute(Inputs inputs)
    {
        //player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x, 0);
        return;
    }
}