using UnityEngine;
using System;

class UpAirPlayerState : PlayerState {
	public override string name_ { get { return "UP_AIR"; } }
	public override int duration_frames_ { get { return -1; } }
	public UpAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.UpAir);
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 12);
    }
}