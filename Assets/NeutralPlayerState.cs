using UnityEngine;
using System;

class NeutralPlayerState : PlayerState {
	public override string name_ { get { return "NEUTRAL"; } }
	public override int duration_frames_ { get { return -1; } }
	public NeutralPlayerState(Character player) : base(player) 
	{
		player_.animator_.SetInteger("state", (int)CharacterState.Neutral);
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
    protected override PlayerState onControlStickHeld(Inputs inputs)
	{
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (Math.Abs(stick_value.x) < STICK_DEADZONE_THRESHOLD)
		{
			return this;
		}
		else if ((stick_value.x > STICK_DEADZONE_THRESHOLD && !player_.facing_right_ ) ||
                (stick_value.x < -STICK_DEADZONE_THRESHOLD && player_.facing_right_))
		{
            return new TurnaroundPlayerState(player_, inputs); //should be walk
        }
		return new WalkPlayerState(player_);
	}

	protected override PlayerState onControlStickPushed(Inputs inputs)
	{
		return new DashStartPlayerState(player_);
	}
    protected override PlayerState onSpecialPushed(Inputs inputs)
    {
        Vector2 stick = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        return processSpecialDirection(stick);
    }
    public override PlayerState processOnHit(Inputs inputs)
    {
        return new HitStunPlayerState(player_);
    }
    protected override PlayerState onJumpPushed() {
		return new JumpSquatPlayerState(player_);
	}
}