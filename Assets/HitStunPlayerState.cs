using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

class HitStunPlayerState : PlayerState
{
    public override string name_ { get { return "HIT_STUN"; } }
    public override int duration_frames_ { get { return 20; } }
    private bool fast_fall = false;
    public HitStunPlayerState(Character player, int duration_frames = 20) : base(player)
    {
        duration_frames_ = duration_frames;
        player.animator_.SetInteger("state", (int)CharacterState.HitStun);
    }
    public override PlayerState defaultNextState(Inputs inputs)
    {
        if (player_.in_air_)
        {
            return new ActionableAirbornePlayerState(player_);
        }
        return new NeutralPlayerState(player_);
    }
    protected override void onExecute(Inputs inputs)
    {
        RaycastHit2D raycast = Physics2D.Raycast(player_.GetComponent<Collider2D>().bounds.min, new Vector2(0, -1), Mathf.Infinity, 1 << 9);
        float ground_raycast = raycast.distance;
        Debug.Log(ground_raycast);
        if (ground_raycast < .02)
        {
            Debug.Log(player_.rigid_body_.velocity.y);
            if (player_.rigid_body_.velocity.y < -60)
            {
                player_.rigid_body_.velocity = new Vector2(player_.rigid_body_.velocity.x, -player_.rigid_body_.velocity.y);
            }
            else
            {
                return;// new HitStunPlayerState(player_, 10);
            }
        }
        return;
    }
}