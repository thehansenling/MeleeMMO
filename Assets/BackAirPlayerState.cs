using UnityEngine;
using System;

class BackAirPlayerState : PlayerState {
	public override string name_ { get { return "BACK_AIR"; } }
	public override int duration_frames_ { get { return -1; } }
	public BackAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.BackAir);
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 12);
    }
}