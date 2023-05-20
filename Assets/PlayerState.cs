using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public abstract class PlayerState
{
    public double STICK_DASH_THRESHOLD = .4;
	public double STICK_DEADZONE_THRESHOLD = .2;

    protected Character player_;
	// Constructor shared by all PlayerStates
	public PlayerState(Character player) {
		player_ = player;
	}
	// Attributes shared by all PlayerStates
	public int frame_ = 0;

	// Attributes custom per PlayerState
	public abstract string name_ {get; }
	public virtual int duration_frames_ {get; protected set;}

	// Functions used to calculate next state in different cases
	public virtual PlayerState processOnCollision(Inputs inputs) { return this; }
	public PlayerState processInputs(Inputs inputs)
	{
		ButtonInputAction shieldAction = (ButtonInputAction) inputs.shield_.getInputAction();
		PlayerState newState;
		switch(shieldAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onShieldPushed(inputs);
				inputs.shield_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onShieldHeld();
				inputs.shield_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}

		ButtonInputAction jumpAction = (ButtonInputAction) inputs.jump_.getInputAction();
		switch(jumpAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onJumpPushed();
				inputs.jump_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onJumpHeld();
				inputs.jump_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}

		ButtonInputAction attackAction = (ButtonInputAction) inputs.attack_.getInputAction();
		switch(attackAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onAttackPushed();
				inputs.attack_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onAttackHeld();
				inputs.attack_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}

		StickInputAction attackStickAction = (StickInputAction) inputs.attack_stick_.getInputAction();
		switch(attackStickAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onAttackStickPushed();
				inputs.attack_stick_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onAttackStickHeld();
				inputs.attack_stick_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}

		ButtonInputAction grabAction = (ButtonInputAction) inputs.grab_.getInputAction();
		switch(grabAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onGrabPushed();
				inputs.grab_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onGrabHeld();
				inputs.grab_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}

		ButtonInputAction specialAction = (ButtonInputAction) inputs.special_.getInputAction();
		switch(specialAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onSpecialPushed();
				inputs.special_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onSpecialHeld();
				inputs.special_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}
		
		StickInputAction controlStickAction = (StickInputAction) inputs.control_stick_.getInputAction();
		switch(controlStickAction.input_action_state_) {
			case InputActionState.PUSHED:
				newState = onControlStickPushed(inputs);
				inputs.control_stick_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.HELD:
				newState = onControlStickHeld(inputs);
				inputs.control_stick_.consumed_ = true;
				return newState.processInputs(inputs);
			case InputActionState.NOT_PUSHED:
				newState = onControlStickNotPushed(inputs);
				inputs.control_stick_.consumed_ = true;
				return newState.processInputs(inputs);
			default:
				break;
		}
		return this;
	}
	public virtual PlayerState defaultNextState(Inputs inputs) { return null; }

	public PlayerState processOnHit(Inputs inputs) {
		return this;
	}

	// Function called from game loop
	public void execute(Inputs inputs) {
        player_.rigid_body_.gravityScale = 1;
        onExecute(inputs);
		frame_++;
	}
	protected abstract void onExecute(Inputs inputs);

	// On Input functions
	protected virtual PlayerState onShieldPushed(Inputs inputs) { return this; }
	protected virtual PlayerState onShieldHeld() { return this; }
	protected virtual PlayerState onJumpPushed() { return this; }
	protected virtual PlayerState onJumpHeld() { return this; }
	protected virtual PlayerState onAttackPushed() { return this; }
	protected virtual PlayerState onAttackHeld() { return this; }
	protected virtual PlayerState onAttackStickPushed() { return this; }
	protected virtual PlayerState onAttackStickHeld() { return this; }
	protected virtual PlayerState onGrabPushed() { return this; }
	protected virtual PlayerState onGrabHeld() { return this; }
	protected virtual PlayerState onSpecialPushed() { return this; }
	protected virtual PlayerState onSpecialHeld() { return this; }
	protected virtual PlayerState onControlStickPushed(Inputs inputs) { return this; }
	protected virtual PlayerState onControlStickHeld(Inputs inputs) { return this; }
	protected virtual PlayerState onControlStickNotPushed(Inputs inputs) { return this; }
}
