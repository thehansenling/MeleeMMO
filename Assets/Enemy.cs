
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Runtime.Serialization.Json;

public class Enemy : Character
{
    bool PRINT_DEBUG = false;

    bool shield_ = false;
	bool jump_ = false;
	bool attack_ = false;
	Vector2 attack_stick_ = new Vector2(0, 0);
	bool grab_ = false;
	bool special_ = false;
	Vector2 control_stick_ = new Vector2(0, 0);	

	PlayerState current_state_;
	//bool collision_;
    Inputs inputs_;

    void DetectCollisionExit()
    {
        Collider2D coll = GetComponent<Collider2D>();
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
        animator_ = GetComponent<Animator>();
        inputs_ = new Inputs();
        current_state_ = new NeutralPlayerState(this);
        MAX_FALL_SPEED = 30;
        MAX_SPEED = 16;
        WALK_SPEED = 6;
        air_dodge_ = false;
        air_dodge_velocity_ = new Vector2(0, 0);
        directional_influence_force_ = 1f;
        id_ = 2;
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

    private void OnAttack(InputValue value)
    {
        attack_ = true;
    }

    private void OnAttackStick(InputValue value)
    {
        attack_stick_ = value.Get<Vector2>();
    }


    // Update is called once per frame
    void Update()
    {

        //DetectCollisionExit();
        OnGround();

        UpdateInputs();
        if (DEBUG_PRINT)
        {
            print("Current State: " + current_state_.name_);
            print("Stick Input: " + ((StickInputAction)inputs_.control_stick_.getInputAction()).value_);
            print("Current Velocity: " + rigid_body_.velocity);
            print(facing_right_);
        }
        // If state is ending based on number of frames
        if (current_state_.frame_ >= current_state_.duration_frames_ && current_state_.duration_frames_ != -1)
        {
            current_state_ = current_state_.defaultNextState(inputs_);
        }
        if (DEBUG_PRINT) print("Post Default Velocity: " + rigid_body_.velocity);

        current_state_ = current_state_.processInputs(inputs_);

        if (hit_)
        {
            print("HIT HIT HIT HIT");
            print(current_state_.name_);
            current_state_ = current_state_.processOnHit(inputs_);
        }
        // if (collision_)
        if (!last_in_air_ && in_air_)
        {
            current_state_ = current_state_.processOnEnvironment(inputs_);
        }
        if (!in_air_)
        {
            current_state_ = current_state_.processOnCollision(inputs_);
        }
        if (DEBUG_PRINT) print("Post Collision Velocity: " + rigid_body_.velocity);

        current_state_.execute(inputs_);
        if (DEBUG_PRINT) print("Post Execute Velocity: " + rigid_body_.velocity);

        //end

        DisplayState();
        last_in_air_ = in_air_;
        hit_ = false;
        //ResetInputs();
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
        else if (current_state_.name_ == "HIT_STUN")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    print("COLLIDER THING DOING TRIGGER");
    //    print(collision.)
    //    rigid_body_.AddForce(new Vector2(0, 1000));
    //    //if hitbox out 
    //    //state.hit direction and power
    //    //if (current_state_)
    //    //{

    //    //}
    //}
}
