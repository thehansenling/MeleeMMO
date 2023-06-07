using UnityEngine;
using System;

class DownAirPlayerState : PlayerState {
	public override string name_ { get { return "DOWN_AIR"; } }
	public override int duration_frames_ { get { return -1; } }
	public DownAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.DownAir);
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 12);
    }
}