using UnityEngine;
using System;

class ForwardAirPlayerState : PlayerState {
	public override string name_ { get { return "FORWARD_AIR"; } }
	public override int duration_frames_ { get { return -1; } }
	public ForwardAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.ForwardAir);
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 12);
    }
}