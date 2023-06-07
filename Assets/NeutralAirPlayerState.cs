using UnityEngine;
using System;

class NeutralAirPlayerState : PlayerState {
	public override string name_ { get { return "NEUTRAL_AIR"; } }
	public override int duration_frames_ { get { return -1; } }
	public NeutralAirPlayerState(Character player) : base(player) 
    {
        player.animator_.SetInteger("state", (int)CharacterState.NeutralAir);
    }
    public override PlayerState processOnCollision(Inputs inputs)
    {
        return new LandingLagPlayerState(player_, 12);
    }
}