
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
// enum PlayerState {
//     NEUTRAL = 0,
//     WALK = 1,
//     RUN = 2,
//     DASH = 3,
//     TURN = 4,
//     TURNRUN = 5,
//     AIRDODGE = 6,
//     JUMP = 7,
//     JUMPSQUAT = 8,
//     STOP = 9,
//     DASH_STOP = 10,
//     RUN_STOP = 11,
// }



public class Character : MonoBehaviour
{
    int TURNRUN_FRAMES = 60;
    int DASH_FRAMES = 30;
    int AIR_DODGE_FRAMES = 40;
    int STOP_FRAMES = 30;
    int DASH_STOP_FRAMES = 10;
    int RUN_STOP_FRAMES = 20;
    int JUMPSQUAT_FRAMES = 4;
    float DI_MODIFIER = 1f;
    float MAX_SPEED = .8f;
    int air_dodges = 0;
    bool in_air_;
    public Rigidbody2D rigid_body_;
    
    float move_speed_;
    public int jumps_;
    Vector2 direction_;
    Vector2 air_dodge_direction_;
    Vector2 directional_influence_;
    Vector2 move_velocity_;
    PlayerStateOld state_;
    int state_frames_;
    bool invulnerable_;
    int inactionable_frames_;
    bool actionable_;
    float directional_influence_force_;
    public bool from_air_ = false;

    public bool move_direction_right_ = true;
    // Start is called before the first frame update
    Vector2 move;

	bool shield_ = false;
	bool jump_ = false;
	bool attack_ = false;
	Vector2 attack_stick_ = new Vector2(0, 0);
	bool grab_ = false;
	bool special_ = false;
	Vector2 control_stick_ = new Vector2(0, 0);	

	PlayerState current_state_;
	PlayerState last_state_;
	bool hit_;
	bool collision_;

    Inputs inputs_;

    void DetectCollisionExit()
    {
        CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        coll.OverlapCollider(filter, results);
        bool on_ground = false;
        foreach (Collider2D result in results)
        {
            if (result.gameObject.tag == "Ground")
            {
                on_ground = true;
            }
        }
        in_air_ = !on_ground;
        from_air_ = true;
    }

    public void MoveCharacter(Inputs input, float speed = 16)
    {
		Vector2 move = ((StickInputAction)input.control_stick_.getInputAction()).value_;
        print(move);
		float move_x = 0;
		if (move.x >= 0) move_x = 1;
		if (move.x < 0) move_x = -1;
		// this.rigid_body_.AddForce(new Vector2(move_x * 200, 0));
		rigid_body_.velocity = new Vector2(move_x * speed, rigid_body_.velocity.y);
        print(rigid_body_.velocity);
    }

    public void MoveCharacterFacing(float speed = 16)
    {
        float move_x = 0;
        if (move_direction_right_) 
        {
            move_x = 1;
        }
        else
        {
            move_x = -1;
        } 
        // this.rigid_body_.AddForce(new Vector2(move_x * 200, 0));
        rigid_body_.velocity = new Vector2(move_x * speed, rigid_body_.velocity.y);
        print(rigid_body_.velocity);
    }

	void UpdateInputs()
	{
		inputs_.shield_.setButton(shield_);
		inputs_.jump_.setButton(jump_);
		inputs_.attack_.setButton(attack_);
		inputs_.attack_stick_.setStick(attack_stick_);
		inputs_.grab_.setButton(grab_);
		inputs_.special_.setButton(special_);
		inputs_.control_stick_.setStick(control_stick_);	
	}

	void ResetInputs()
	{
 		shield_ = false;
		jump_ = false;
		attack_ = false;
		grab_ = false;
		special_ = false;

        collision_ = false;
	}

    void Start()
    {
        Application.targetFrameRate = 60;
        jumps_ = 2;
        in_air_ = false;
        rigid_body_ = GetComponent<Rigidbody2D>();
        move_speed_ = 10;
        directional_influence_ = new Vector2(0, 0);
        direction_ = new Vector2(0, 0);
        move_velocity_ = new Vector2(0, 0);
        state_ = PlayerStateOld.NEUTRAL;
        invulnerable_ = false;
        inactionable_frames_ = 0;
        actionable_ = true;
        directional_influence_force_ = 5;
        air_dodge_direction_ = new Vector2(0, 0);
        inputs_ = new Inputs();
        current_state_ = new NeutralPlayerState(this);
        last_state_ = new NeutralPlayerState(this);

    }

    private void Awake()
    {
    }

    private void OnMove(InputValue value)
    {

        control_stick_ = value.Get<Vector2>();
        //print("controlstick");
        //print(control_stick_);
    }

    private void OnJump(InputValue value)
    {
        jump_ = true;
    }

    private void OnTrigger(InputValue value)
    {
        shield_ = true;
    }


    // Update is called once per frame
    void Update()
    {
        DetectCollisionExit();
    	UpdateInputs();
		print(current_state_.name_);
        // print(((ButtonInputAction)inputs_.shield_.getInputAction()).input_action_state_);
    	print(((StickInputAction)inputs_.control_stick_.getInputAction()).value_);
		// If state is ending based on number of frames
		if (current_state_.frame_ >= current_state_.duration_frames_ && current_state_.duration_frames_ != -1)
		{
			current_state_ = current_state_.defaultNextState(inputs_);
		}

        current_state_ = current_state_.processInputs(inputs_);
        
        if (hit_)
		{
			current_state_ = current_state_.processOnHit(inputs_);
		}
		// if (collision_)
		if (!in_air_)
		{
			current_state_ = current_state_.processOnCollision(inputs_);
		}

		current_state_.execute(inputs_);

		last_state_ = current_state_;
        //end
        DisplayState();
 		ResetInputs();
    }


    private void DisplayState()
    {
        if (current_state_.name_ == "NEUTRAL")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (current_state_.name_ == "DASH")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (current_state_.name_ == "RUN")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (current_state_.name_ == "RUN_TURN")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (current_state_.name_ == "AIR_DODGE")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        else if (current_state_.name_ == "JUMP")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (current_state_.name_ == "JUMPSQUAT")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        else if (current_state_.name_ == "RUN_STOP")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (current_state_.name_ == "LANDING_LAG")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (current_state_.name_ == "WALK")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            collision_ = true;
            in_air_ = false;
            jumps_ = 1;
            actionable_ = true;
            if (state_ == PlayerStateOld.AIRDODGE)
            {
                state_ = PlayerStateOld.NEUTRAL;
            }
            else 
            {
            	state_ = PlayerStateOld.STOP;
            }
            rigid_body_.velocity = new Vector2(rigid_body_.velocity.x, 0);
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    print("EXITING");
    //    // if (collision.gameObject.tag == "Ground")
    //    // {
    //        in_air_ = true;
    //    // }
    //}
}
