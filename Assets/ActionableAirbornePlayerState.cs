using UnityEngine;
using System;

class ActionableAirbornePlayerState : PlayerState {
    private bool fast_fall = false;
    public override string name_ { get { return "ACTIONABLE_AIRBORNE"; } }
	public override int duration_frames_ { get { return -1; } }
	
	public ActionableAirbornePlayerState(Character player) : base(player) 
	{
        player_.animator_.SetInteger("state", (int)CharacterState.ActionableAirborne);
    }
	public override PlayerState processOnCollision(Inputs inputs) {
        
        float ground_raycast = Physics2D.Raycast(player_.GetComponent<Collider2D>().bounds.min, new Vector2(0, -1)).distance;
		if (ground_raycast < .5)
		{
			if (frame_ > 1)
			{
				return new LandingLagPlayerState(player_, 8);
			}
		}
		return this;
	}
    public override PlayerState processOnHit(Inputs inputs)
    {
        return new HitStunPlayerState(player_);
    }
    protected override PlayerState onShieldPushed(Inputs inputs) {
		return new AirdodgePlayerState(player_, inputs);
	}

    protected override PlayerState onJumpPushed() {
    	if (player_.jumps_ > 0)
    	{
            player_.rigid_body_.velocity = new Vector2(0, 0);
            player_.jumps_--;
    		return new JumpPlayerState(player_, 30);
    	}
		return this;
	}
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
    private PlayerState processSpecialDirection(Vector2 stick)
    {
        if (Math.Abs(stick.x) > STICK_DEADZONE_THRESHOLD && Math.Abs(stick.x) > Math.Abs(stick.y))
        {
            if ((stick.x > 0 && player_.facing_right_) ||
                (stick.x < 0 && !player_.facing_right_))
            {
                return new SideSpecialPlayerState(player_);
            }
            return new SideSpecialPlayerState(player_);
        }
        else if (Math.Abs(stick.y) > STICK_DEADZONE_THRESHOLD && Math.Abs(stick.y) > Math.Abs(stick.x))
        {
            if (stick.y > 0)
            {
                return new UpSpecialPlayerState(player_);
            }
            return new DownSpecialPlayerState(player_);
        }
        return new NeutralSpecialPlayerState(player_);
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
    protected override PlayerState onSpecialPushed(Inputs inputs)
    {
        Vector2 stick = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        return processSpecialDirection(stick);
    }
    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 stick = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick.y < -STICK_DASH_THRESHOLD && player_.rigid_body_.velocity.y < 0)
		{
			fast_fall = true;
		}

		return this;
    }

    protected override void onExecute(Inputs inputs) {
		float max_fall_speed = player_.MAX_FALL_SPEED;

        float move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_.x;
        float di_force = move * player_.directional_influence_force_;
		float potential_x_speed = player_.rigid_body_.velocity.x + di_force;
		float speed_sign = potential_x_speed >= 0 ? 1 : -1;
		float x_speed = Math.Min(player_.MAX_SPEED, Math.Abs(potential_x_speed));

		float clamped_fall_speed = Math.Max(-max_fall_speed, player_.rigid_body_.velocity.y);
        player_.rigid_body_.velocity = new Vector2(speed_sign * x_speed, clamped_fall_speed);

        if (fast_fall)
        {
            player_.rigid_body_.velocity = new Vector2(speed_sign * x_speed, -player_.MAX_FALL_SPEED);
        }

		return;
	}
}
