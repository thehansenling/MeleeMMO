using UnityEngine;
class JumpSquatPlayerState : PlayerState {
	public override string name_ { get { return "JUMPSQUAT"; } }
	public override int duration_frames_ { get { return 4; } }
    private int jump_force = 20;
	public JumpSquatPlayerState(Character player) : base(player) {}
    public override PlayerState defaultNextState(Inputs inputs) {
        InputActionState jump_value = inputs.jump_.getInputAction().input_action_state_;
        if (jump_value == InputActionState.HELD || jump_value == InputActionState.PUSHED)
        {
            jump_force = 30;
        }
        return new JumpPlayerState(player_, jump_force);
	}
}