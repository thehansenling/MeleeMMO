
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;



public class Character : MonoBehaviour
{
    protected bool DEBUG_PRINT = false;

    public int MAX_SPEED;
    public int WALK_SPEED;
    public int MAX_FALL_SPEED;
    public int MAX_FAST_FALL_SPEED;
    public bool in_air_;
    protected bool last_in_air_;
    public Rigidbody2D rigid_body_;
    public Animator animator_;
    public int jumps_;
    public float directional_influence_force_;
    public bool facing_right_ = true;
    public bool air_dodge_;
    public Vector2 air_dodge_velocity_;
    protected int id_ = 1;


    bool shield_ = false;
	bool jump_ = false;
	bool attack_ = false;
	Vector2 attack_stick_ = new Vector2(0, 0);
	bool grab_ = false;
	bool special_ = false;
	Vector2 control_stick_ = new Vector2(0, 0);	

	PlayerState current_state_;
	protected bool hit_;
	//bool collision_;
    Inputs inputs_;
    public int GetID() { return id_; }
    void DetectCollisionExit()
    {
        print("EXITING");
        CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        coll.OverlapCollider(filter, results);
        bool on_ground = false;
        foreach (Collider2D result in results)
        {
            print(result.tag);
            if (result.gameObject.tag == "Ground")
            {
                on_ground = true;
            }
        }
        in_air_ = !on_ground;
    }

    protected void OnGround()
    {
        RaycastHit2D raycast = Physics2D.Raycast(GetComponent<Collider2D>().bounds.min, new Vector2(0, -1), Mathf.Infinity, 1 << 9);
        float ground_raycast = raycast.distance;
        if (ground_raycast > .01)
        {
            in_air_ = true;
        }
        else
        {
            in_air_ = false;
        }
    }
    public void OnHit(Vector2 hit_force)
    {
        print("HIT HIT HIT");
        rigid_body_.velocity = hit_force;
        hit_ = true;
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
        jumps_ = 1;
        in_air_ = false;
        rigid_body_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        inputs_ = new Inputs();
        current_state_ = new NeutralPlayerState(this);
        MAX_FALL_SPEED = 30;
        MAX_FAST_FALL_SPEED = 50;
        MAX_SPEED = 16;
        WALK_SPEED = 6;
        air_dodge_ = false;
        air_dodge_velocity_ = new Vector2(0, 0);
        directional_influence_force_ = 1f;
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
        jump_ = !jump_;
    }

    private void OnTrigger(InputValue value)
    {
        shield_ = !shield_;
    }

    private void OnAttack(InputValue value)
    {
        attack_ = !attack_;
    }

    private void OnAttackStick(InputValue value)
    {
        attack_stick_ = value.Get<Vector2>();
    }
    private void OnSpecial(InputValue value)
    {
        special_ = !special_;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //collision_ = true;
            in_air_ = false;
            jumps_ = 1;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        print(enemy.GetID());
        if (!current_state_.CheckHitID(enemy.GetID()))
        {
            var hit_direction = current_state_.HitForce(collision);
            print(hit_direction);
            enemy.OnHit(hit_direction);
            current_state_.AddHitID(enemy.GetID());
        }
    }
}
