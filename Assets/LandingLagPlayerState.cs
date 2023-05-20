using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

class LandingLagPlayerState : PlayerState {
	public override string name_ { get { return "LANDING_LAG"; } }
	//public override int duration_frames_;// { get { return duration_frames_; } }
	public LandingLagPlayerState(Character player, int duration_frames = 20) : base(player) 
	{
        if (player_.air_dodge_)
        {
            //print("Initial x velocity: " + player_.rigid_body_.velocity);
            float max_wave_dash_speed = 16;
            float min_wave_dash_speed = 4;
            float wave_dash_speed = player_.air_dodge_velocity_.x;
            if (wave_dash_speed > 0)
            {
                wave_dash_speed = Mathf.Min(wave_dash_speed, max_wave_dash_speed);
                wave_dash_speed = Mathf.Max(wave_dash_speed, min_wave_dash_speed);
            }
            if (wave_dash_speed < 0)
            {
                wave_dash_speed = Mathf.Max(wave_dash_speed, -max_wave_dash_speed);
                wave_dash_speed = Mathf.Min(wave_dash_speed, -min_wave_dash_speed);
            }
            player_.rigid_body_.velocity = new Vector2(wave_dash_speed, 0);
            //print("after wavedash velocity: " + rigid_body_.velocity);
        }
        player_.air_dodge_ = false;
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