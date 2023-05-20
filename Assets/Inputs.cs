using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActionState {
	HELD,
	PUSHED,
	RELEASED,
	NOT_PUSHED,
	CONSUMED,
}

public abstract class InputAction {
	public InputActionState input_action_state_;
}

public class ButtonInputAction : InputAction {

	public ButtonInputAction(InputActionState input_action_state) {
		input_action_state_ = input_action_state;
	}
}

public class StickInputAction : InputAction {
	public Vector2 value_;
	public StickInputAction(InputActionState input_action_state, Vector2 value) {
		input_action_state_ = input_action_state;
		value_ = value;
	}
}

public abstract class Input {
	public bool consumed_ = false;
	public abstract InputAction getInputAction();
}

public class ButtonInput : Input {
	private bool is_depressed_;
	private bool was_depressed_;

	public override InputAction getInputAction() {
		if (consumed_) {
			return new ButtonInputAction(InputActionState.CONSUMED);
		}

		if (is_depressed_ && !was_depressed_) {
			return new ButtonInputAction(InputActionState.PUSHED);
		} else if (is_depressed_ && was_depressed_) {
			return new ButtonInputAction(InputActionState.HELD);
		} else if (!is_depressed_ && was_depressed_) {
			return new ButtonInputAction(InputActionState.RELEASED);
		} else {
			return new ButtonInputAction(InputActionState.NOT_PUSHED);			
		}
	}

	public void setButton(bool newButtonValue) {
		was_depressed_ = is_depressed_;
		is_depressed_ = newButtonValue;
		consumed_ = false;
	}
}

public class StickInput : Input {
	private Vector2 stick_;
	private Vector2 stick_previous_;

	public override InputAction getInputAction() {
		if (consumed_) {
			return new StickInputAction(InputActionState.CONSUMED, stick_);
		}

		if ((stick_ - stick_previous_).magnitude > .4 && stick_.magnitude > .4) {
			return new StickInputAction(InputActionState.PUSHED, stick_);
		} else if (stick_.magnitude > .2) {
			return new StickInputAction(InputActionState.HELD, stick_);
		} else {
			return new StickInputAction(InputActionState.NOT_PUSHED, stick_);
		}
	}
	public Vector2 stickVelocity()
	{
		return stick_ - stick_previous_;

    }
	public void setStick(Vector2 newStickValue) {
		stick_previous_ = stick_;
		stick_ = newStickValue;
		consumed_ = false;
	}
}

public class Inputs {

	public ButtonInput shield_ = new ButtonInput();
	public ButtonInput jump_ = new ButtonInput();
	public ButtonInput attack_ = new ButtonInput();
	public StickInput attack_stick_ = new StickInput();
	public ButtonInput grab_ = new ButtonInput();
	public ButtonInput special_ = new ButtonInput();
	public StickInput control_stick_ = new StickInput();

}