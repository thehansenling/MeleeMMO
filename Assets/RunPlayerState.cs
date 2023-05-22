using UnityEngine;
using System;

class RunPlayerState : PlayerState
{
    public override string name_ { get { return "RUN"; } }
    public override int duration_frames_ { get { return -1; } }
    public RunPlayerState(Character player) : base(player) { }

    protected override PlayerState onControlStickHeld(Inputs inputs)
    {
		StickInputAction stick_action = (StickInputAction)inputs.control_stick_.getInputAction();
        Vector2 stick_value = stick_action.value_;
		Vector2 stick_velocity = inputs.control_stick_.stickVelocity();

        if (stick_value.y < -STICK_DASH_THRESHOLD)
		{
            player_.rigid_body_.velocity = new Vector2(0, player_.rigid_body_.velocity.y);
            return new NeutralPlayerState(player_);
		}
		else if (player_.facing_right_)
		{
			if (stick_value.x < -STICK_DASH_THRESHOLD || stick_velocity.x < -STICK_DASH_THRESHOLD)
			{
				return new RunTurnPlayerState(player_);
			}
		}
		else
		{
			if (stick_value.x > STICK_DASH_THRESHOLD || stick_velocity.x > STICK_DASH_THRESHOLD)
			{
				return new RunTurnPlayerState(player_);
			}
		}
        return this;
    }

    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (stick_value.y < -STICK_DASH_THRESHOLD)
        {
            player_.rigid_body_.velocity = new Vector2(0, player_.rigid_body_.velocity.y);
            return new NeutralPlayerState(player_);
        }
        else if (player_.facing_right_)
        {
            if (stick_value.x < -STICK_DASH_THRESHOLD)
            {
                return new RunTurnPlayerState(player_);
            }
        }
        else
        {
            if (stick_value.x > STICK_DASH_THRESHOLD)
            {
                return new RunTurnPlayerState(player_);
            }
        }
        return this;
    }

    protected override PlayerState onControlStickNotPushed(Inputs inputs)
    {
		return new RunStopPlayerState(player_);
    }

    protected override PlayerState onJumpPushed()
    {
        return new JumpSquatPlayerState(player_);
    }

    public override PlayerState defaultNextState(Inputs inputs)
    {
        return new RunStopPlayerState(player_);
    }

    protected override void onExecute(Inputs inputs)
    {
        player_.MoveCharacter(inputs);
        if (frame_ == 0)
        {
            Vector2 stick_value = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
			if (stick_value.x < 0)
			{
				player_.facing_right_ = false;
			}
			else
			{
                player_.facing_right_ = true;
            }
        }
        return;
    }
}
