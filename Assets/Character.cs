
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Character : MonoBehaviour
{

    public int MAX_SPEED;
    public int WALK_SPEED;
    public int MAX_FALL_SPEED;
    bool in_air_;
    public Rigidbody2D rigid_body_;
    public int jumps_;
    public float directional_influence_force_;
    public bool facing_right_ = true;
    public bool air_dodge_;
    public Vector2 air_dodge_velocity_;


    bool shield_ = false;
	bool jump_ = false;
	bool attack_ = false;
	Vector2 attack_stick_ = new Vector2(0, 0);
	bool grab_ = false;
	bool special_ = false;
	Vector2 control_stick_ = new Vector2(0, 0);	

	PlayerState current_state_;
	bool hit_;
	//bool collision_;
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
    }

    public void MoveCharacter(Inputs input, float speed = 16)
    {
		Vector2 stick_value = ((StickInputAction)input.control_stick_.getInputAction()).value_;
		float move_x = 0;
		if (stick_value.x >= 0) move_x = 1;
		if (stick_value.x < 0) move_x = -1;
		rigid_body_.velocity = new Vector2(move_x * speed, rigid_body_.velocity.y);
    }

    public void MoveCharacterFacing(float speed = 16)
    {
        float move_x = 0;
        if (facing_right_) 
        {
            move_x = 1;
        }
        else
        {
            move_x = -1;
        } 
        rigid_body_.velocity = new Vector2(move_x * speed, rigid_body_.velocity.y);
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

        //collision_ = false;
	}

    void Start()
    {
        Application.targetFrameRate = 60;
        jumps_ = 2;
        in_air_ = false;
        rigid_body_ = GetComponent<Rigidbody2D>();
        inputs_ = new Inputs();
        current_state_ = new NeutralPlayerState(this);
        MAX_FALL_SPEED = 30;
        MAX_SPEED = 16;
        WALK_SPEED = 6;
        air_dodge_ = false;
        air_dodge_velocity_ = new Vector2(0, 0);
    }

    private void Awake()
    {
    }

    private void OnMove(InputValue value)
    {
        control_stick_ = value.Get<Vector2>();
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
		print("Current State: " + current_state_.name_);
    	print("Stick Input: " + ((StickInputAction)inputs_.control_stick_.getInputAction()).value_);
        print("Current Velocity: " + rigid_body_.velocity);
		// If state is ending based on number of frames
		if (current_state_.frame_ >= current_state_.duration_frames_ && current_state_.duration_frames_ != -1)
		{
			current_state_ = current_state_.defaultNextState(inputs_);
		}
        print("Post Default Velocity: " + rigid_body_.velocity);

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
        print("Post Collision Velocity: " + rigid_body_.velocity);

        current_state_.execute(inputs_);
        print("Post Execute Velocity: " + rigid_body_.velocity);

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
        else if (current_state_.name_ == "TURNAROUND")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            print("ENTER COLLISION DETECTED");
            //collision_ = true;
            in_air_ = false;
            jumps_ = 1;
        }
    }
}
