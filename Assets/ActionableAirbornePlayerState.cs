using UnityEngine;
using System;

class ActionableAirbornePlayerState : PlayerState {
	public override string name_ { get { return "ACTIONABLE_AIRBORNE"; } }
	public override int duration_frames_ { get { return -1; } }
	public ActionableAirbornePlayerState(Character player) : base(player) {}

	private PlayerState processAttackDirection(Vector2 stick)
	{
        if (Math.Abs(stick.x) > STICK_DEADZONE_THRESHOLD && Math.Abs(stick.x) > Math.Abs(stick.y))
		{
			if ((stick.x > 0 && player_.facing_right_) || 
				(stick.x < 0 && !player_.facing_right_))
			{
				return new ForwardAirPlayerState(player_);
			}
			return new BackAirPlayerState(player_);
		}
		else if (Math.Abs(stick.y) > STICK_DEADZONE_THRESHOLD && Math.Abs(stick.y) > Math.Abs(stick.x))
		{
			if (stick.y > 0)
			{
				return new UpAirPlayerState(player_);
			}
			return new DownAirPlayerState(player_);
		}
		return new NeutralAirPlayerState(player_);
	}

	public override PlayerState processOnCollision(Inputs inputs) {
		if (frame_ > 1) {
			return new LandingLagPlayerState(player_, 12);
		}
		return this;
	}

	protected override PlayerState onShieldPushed(Inputs inputs) {
		return new AirdodgePlayerState(player_, inputs);
	}

    protected override PlayerState onJumpPushed() {
    	if (player_.jumps_ > 0)
    	{
            player_.rigid_body_.velocity = new Vector2(0, 0);
            player_.jumps_--;
    		return new JumpPlayerState(player_);
    	}
		return this;
	}

    protected override PlayerState onAttackPushed(Inputs inputs)
    {
        Vector2 stick = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        return processAttackDirection(stick);
    }
    protected override PlayerState onAttackStickPushed(Inputs inputs)
    {
        Vector2 stick = ((StickInputAction)inputs.attack_stick_.getInputAction()).value_;
        return processAttackDirection(stick);
    }

    protected override void onExecute(Inputs inputs) {
        float move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_.x;
        float di_force = move * player_.directional_influence_force_;
		float potential_x_speed = player_.rigid_body_.velocity.x + di_force;
		float speed_sign = potential_x_speed >= 0 ? 1 : -1;
		float x_speed = Math.Min(player_.MAX_SPEED, Math.Abs(potential_x_speed));
		float clamped_fall_speed = Math.Max(-player_.MAX_FALL_SPEED, player_.rigid_body_.velocity.y);

        player_.rigid_body_.velocity = new Vector2(speed_sign * x_speed, clamped_fall_speed);

		return;
	}
}
