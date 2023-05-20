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
        Vector2 move = stick_action.value_;
		Vector2 stick_velocity = inputs.control_stick_.stickVelocity();

        if (move.y < -STICK_DASH_THRESHOLD)
		{
            player_.rigid_body_.velocity = new Vector2(0, player_.rigid_body_.velocity.y);
            return new NeutralPlayerState(player_);
		}
		else if (player_.move_direction_right_)
		{
			if (move.x < -STICK_DASH_THRESHOLD || stick_velocity.x < -STICK_DASH_THRESHOLD)
			{
				return new RunTurnPlayerState(player_);
			}
		}
		else
		{
			if (move.x > STICK_DASH_THRESHOLD || stick_velocity.x > STICK_DASH_THRESHOLD)
			{
				return new RunTurnPlayerState(player_);
			}
		}

		Debug.Log("HERE MAYBE");
		Debug.Log(move);
        return this;
    }

    protected override PlayerState onControlStickPushed(Inputs inputs)
    {
        Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
        if (move.y < -STICK_DASH_THRESHOLD)
        {
            player_.rigid_body_.velocity = new Vector2(0, player_.rigid_body_.velocity.y);
            return new NeutralPlayerState(player_);
        }
        else if (player_.move_direction_right_)
        {
            if (move.x < -STICK_DASH_THRESHOLD)
            {
                Debug.Log("HERE MAYBE 3");
                Debug.Log(move);
                return new RunTurnPlayerState(player_);
            }
        }
        else
        {
            if (move.x > STICK_DASH_THRESHOLD)
            {
                Debug.Log("HERE MAYBE 4");
                Debug.Log(move);
                return new RunTurnPlayerState(player_);
            }
        }
        Debug.Log("HERE MAYBE 2");
        Debug.Log(move);
		Debug.Log(player_.move_direction_right_);
        return this;
    }

    protected override PlayerState onControlStickNotPushed(Inputs inputs)
    {
		// if (inputs.control_stick_.stickVelocity().magnitude > .45)
		// {
		// 	return new RunTurnPlayerState(player_);
		// }
		return new RunStopPlayerState(player_);
    }

    protected override PlayerState onJumpPushed()
    {
        return new JumpSquatPlayerState(player_);
    }

    public override PlayerState defaultNextState(Inputs inputs)
    {
        Debug.Log("THIS GUY 3");
        return new RunStopPlayerState(player_);
    }

    protected override void onExecute(Inputs inputs)
    {
        player_.MoveCharacter(inputs);
        if (frame_ == 0)
        {
            Vector2 move = ((StickInputAction)inputs.control_stick_.getInputAction()).value_;
			if (move.x < 0)
			{
				player_.move_direction_right_ = false;
			}
			else
			{
                player_.move_direction_right_ = true;
            }
        }
        return;
    }
}
