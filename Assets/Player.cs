using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerStateOld {
    NEUTRAL = 0,
    WALK = 1,
    RUN = 2,
    DASH = 3,
    TURN = 4,
    TURNRUN = 5,
    AIRDODGE = 6,
    JUMP = 7,
    JUMPSQUAT = 8,
    STOP = 9,
    DASH_STOP = 10,
    RUN_STOP = 11,
}

public class Player : MonoBehaviour
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
    int jumps_;
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
    // Start is called before the first frame update
    PlayerInput controls;
    Vector2 move;

    bool jump_button_;
    bool a_button_;
    bool trigger_button_;


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
        

    }

    private void Awake()
    {
        // controls = GetComponent<PlayerInput>();
        // controls.actions["Move"].Enable();
        // controls.actions["Move"].performed += ctx => move = Vector2.zero;

        // controls.GamecubeMap.R.performed += ctx => r_button_ = true;
        // controls.GamecubeMap.R.canceled += ctx => r_button_ = false;

        // controls.GamecubeMap.L.performed += ctx => l_button_ = true;
        // controls.GamecubeMap.L.canceled += ctx => l_button_ = false;

        // controls.GamecubeMap.X.performed += ctx => x_button_ = true;
        // controls.GamecubeMap.X.canceled += ctx => x_button_ = false;

        // controls.GamecubeMap.A.performed += ctx => a_button_ = true;
        // controls.GamecubeMap.A.canceled += ctx => a_button_ = false;
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        jump_button_ = true;
    }

    private void OnTrigger(InputValue value)
    {
        print("TRIGGER");
        trigger_button_ = true;
    }


    // Update is called once per frame
    void Update()
    {

        float x_input = move.x;//Input.GetAxis("Horizontal");
        float y_input = move.y;//Input.GetAxis("Vertical");
        
    
        if (Mathf.Abs(move.x) < .1)
        {
            move.x = 0;
        }
        if (Mathf.Abs(move.y) < .1)
        {
            move.y = 0;
        }

        Vector2 air_dodge_force = new Vector2(0, 0);
        if (actionable_)
        {
            if (in_air_ || state_ == PlayerStateOld.JUMP)
            {

                if (trigger_button_)
                {
                    
                    air_dodges++;
                    state_frames_ = 0;

                    air_dodge_direction_ = new Vector2(move.x * .3f, move.y * .3f);

                    state_ = PlayerStateOld.AIRDODGE;
                    actionable_ = false;
                }

                direction_ = new Vector2(move.x, move.y);
                //rigid_body_.velocity -= directional_influence_;
                directional_influence_ = new Vector2(directional_influence_force_ * move.x, directional_influence_force_ * move.y);
                //move_velocity_ = new Vector2(rigid_body_.velocity.x, rigid_body_.velocity.y);
                //rigid_body_.AddForce(air_dodge_force);

            }
            else
            {
                
                if (Mathf.Abs(move.x) > .1)
                {
                    //print("FIRST");
                    float ground_move_x = (Mathf.Abs(move.x) / move.x) * MAX_SPEED;
                    if (state_ == PlayerStateOld.NEUTRAL)
                    {
                        state_frames_ = 0;
                        state_ = PlayerStateOld.DASH;
                        direction_ = new Vector2(move.x, move.y);
                    }
                    else if (state_ == PlayerStateOld.RUN_STOP)
                    {
                        state_frames_++;

                        if (state_frames_ > RUN_STOP_FRAMES)
                        {

                            state_frames_ = 0;
                            state_ = PlayerStateOld.STOP;
                            
                        }
                        else if ((direction_.x < 0 && move.x > 0) ||
                            (direction_.x > 0 && move.x < 0))
                        {
                            state_frames_ = 0;
                            state_ = PlayerStateOld.TURNRUN;
                        }
                        else
                        {
                            state_frames_ = 0;
                            state_ = PlayerStateOld.RUN;
                            direction_ = new Vector2(move.x, move.y);
                        }
                    }
                    else if (state_ == PlayerStateOld.DASH_STOP)
                    {
                        state_frames_++;

                        state_frames_ = 0;
                        state_ = PlayerStateOld.DASH;

                        direction_ = new Vector2(move.x, move.y);
                    }
                    else if (state_ == PlayerStateOld.DASH)
                    {
                        print("DASH FRAMES");
                        print(state_frames_);
                        state_frames_++;
                        if ((direction_.x < 0 && move.x > 0) ||
                            (direction_.x > 0 && move.x < 0))
                        {
                            state_frames_ = 0;
                            state_ = PlayerStateOld.DASH;
                        }
                        else
                        {
                            if (state_frames_ > DASH_FRAMES)
                            {
                                state_frames_ = 0;
                                state_ = PlayerStateOld.RUN;
                            }
                        }
                        direction_ = new Vector2(ground_move_x, move.y);


                    }
                    else if (state_ == PlayerStateOld.RUN)
                    {
                        print("RUNNING");
                        print(direction_.x);
                        print(move.x);
                        if ((direction_.x < 0 && move.x > 0) ||
                            (direction_.x > 0 && move.x < 0))
                        {
                            print("GOING INTO TURNRUN");
                            state_ = PlayerStateOld.TURNRUN;
                            state_frames_ = 0;
                            direction_ = new Vector2(ground_move_x, move.y);
                        }
                        print(move_velocity_);
                    }
                    else if (state_ == PlayerStateOld.TURNRUN)
                    {
                        if ((direction_.x < 0 && move.x > 0) ||
                            (direction_.x > 0 && move.x < 0))
                        {
                            state_frames_ = 0;
                            direction_ = new Vector2(move.x, move.y);

                        }
                        else
                        {
                            state_frames_++;
                            if (state_frames_ >= TURNRUN_FRAMES)
                            {
                                state_frames_ = 0;
                                state_ = PlayerStateOld.RUN;
                                direction_ = new Vector2(move.x, move.y);
                            }
                        }
                    }
                    else if (state_ == PlayerStateOld.STOP)
                    {
                        if (move.x != 0)
                        {
                            state_frames_ = 0;
                            state_ = PlayerStateOld.DASH;
                            direction_ = new Vector2(move.x, move.y);
                        }
                    }

                    if (state_ == PlayerStateOld.TURNRUN)
                    {
                        print("WHAT");
                        move_velocity_ = new Vector2(0, 0);
                    }
                    else if (state_ == PlayerStateOld.STOP)
                    {
                        print("AHHHH");
                        move_velocity_ = new Vector2(move_velocity_.x / 2, 0);
                    }

                    else
                    {
                        print("THIRD");
                        move_velocity_ = new Vector2(ground_move_x, 0);
                    }
                }
                else
                {
                    //print("SECOND");
                    print((move_velocity_, move.x)) ;
                    if (state_ == PlayerStateOld.DASH)
                    {
                        state_ = PlayerStateOld.DASH_STOP;
                        state_frames_ = 0;
                        move_velocity_ = new Vector2(move_velocity_.x, move_velocity_.y);
                    }
                    else if (state_ == PlayerStateOld.RUN)
                    {
                        state_ = PlayerStateOld.RUN_STOP;
                        state_frames_ = 0;
                        move_velocity_ = new Vector2(move_velocity_.x, move_velocity_.y);
                        print("RUNSDG");
                        print(move_velocity_);
                    }
                    else if (state_ == PlayerStateOld.RUN_STOP)
                    {
                        state_frames_ += 1;
                        float x_velocity = move_velocity_.x - MAX_SPEED / (float) RUN_STOP_FRAMES;
                        if (Mathf.Abs(move_velocity_.x) <= MAX_SPEED / (float)RUN_STOP_FRAMES)
                        {
                            print("ZEROING");
                            move_velocity_.x = 0;
                        }
                        move_velocity_ = new Vector2(x_velocity, move_velocity_.y);
                        print("RUNSTOPPINGSDGDS");
                        print(MAX_SPEED / (float)RUN_STOP_FRAMES);
                        print(move_velocity_);
                        if (state_frames_ >= RUN_STOP_FRAMES)
                        {
                            state_ = PlayerStateOld.STOP;
                            state_frames_ = 0;
                            print(">STOPFRAMES");
                            move_velocity_ = new Vector2(0, 0);
                        }
                    }
                    else if (state_ == PlayerStateOld.DASH_STOP)
                    {
                        
                        state_frames_ += 1;
                        move_velocity_ = new Vector2(move_velocity_.x, move_velocity_.y);
                        if (state_frames_ >= DASH_STOP_FRAMES)
                        {
                            state_ = PlayerStateOld.STOP;
                            state_frames_ = 0;
                            print("NOWAY");
                            move_velocity_ = new Vector2(0, 0);
                        }
                    }
                    else if (state_ != PlayerStateOld.STOP &&
                        state_ != PlayerStateOld.NEUTRAL)
                    {
                        state_ = PlayerStateOld.STOP;
                        state_frames_ = 0;
                        move_velocity_ = new Vector2(0, 0);
                        print("STOP VELCOITY");
                    }
                    else if (state_ == PlayerStateOld.STOP)
                    {
                        state_frames_++;
                        if (state_frames_ > STOP_FRAMES)
                        {
                            state_frames_ = 0;
                            state_ = PlayerStateOld.NEUTRAL;
                            move_velocity_ = new Vector2(0, 0);
                        }
                        move_velocity_ = new Vector2(move_velocity_.x / 2, 0);
                        print("STOP VELCOITY31");
                    }
                    else
                    {
                       
                        state_ = PlayerStateOld.NEUTRAL;
                        move_velocity_ = new Vector2(0, 0);
                        //print("STOP VELCOITY2");
                    }
                    //rigid_body_.velocity -= directional_influence_;
                    directional_influence_ = new Vector2(0, 0);
                }
                directional_influence_ = new Vector2(0, 0);
            }
        }

        //directional_influence_ = new Vector2(0, 0);
        if (state_ == PlayerStateOld.JUMPSQUAT)
        {
            move_velocity_ = new Vector2(0, 0);
            state_frames_++;
            if (state_frames_ > JUMPSQUAT_FRAMES)
            {
                state_frames_ = 0;
                state_ = PlayerStateOld.JUMP;

                in_air_ = true;
                actionable_ = true;
            }
        } 
        else if (state_ == PlayerStateOld.JUMP)
        {
        	state_frames_++;
        	if (state_frames_ == 1)
        	{
                jumps_--;
                Vector2 jump_force = new Vector2(0, 30);
                Vector2 move_velocity = new Vector2(direction_.x * MAX_SPEED, 0);
                //print(move_velocity);
                //rigid_body_.velocity = move_velocity;
                rigid_body_.velocity = new Vector2(rigid_body_.velocity.x, jump_force.y);
                state_ = PlayerStateOld.NEUTRAL;
                state_frames_ = 0;	
            }

        }
        else if (state_ == PlayerStateOld.AIRDODGE)
        {
            
            state_frames_++;
            if (state_frames_ > AIR_DODGE_FRAMES)
            {
            	rigid_body_.velocity = new Vector2(0, 0);
                air_dodge_direction_ = new Vector2(0, 0);
                state_ = PlayerStateOld.NEUTRAL;
            }
            else if (state_frames_ > AIR_DODGE_FRAMES / 4)
            {
            	rigid_body_.velocity = new Vector2(0, 0); 
                air_dodge_direction_ = new Vector2(0, 0);
                move_velocity_ = new Vector2(0, 0);
            }
            float air_dodge_speed = 100;
            rigid_body_.velocity = new Vector2(air_dodge_direction_.x* air_dodge_speed, air_dodge_direction_.y * air_dodge_speed);
            print("AIR DODGE");
            print((air_dodge_direction_.x, air_dodge_direction_.y));
            
        }
        else if (actionable_ && jump_button_  && jumps_ > 0)
        {
            //jumps_--;
            //Vector2 jump_force = new Vector2(0, 1500);
            //Vector2 move_velocity = new Vector2(direction_.x * 8, 0);
            //rigid_body_.velocity = move_velocity;
            //rigid_body_.AddForce(jump_force);
            if (in_air_)
            {
                state_frames_ = 0;
                state_ = PlayerStateOld.JUMP;
                jumps_--;
                Vector2 jump_force = new Vector2(0, 30);

                //rigid_body_.velocity = new Vector2(rigid_body_.velocity.x, 0);
                rigid_body_.velocity = new Vector2(rigid_body_.velocity.x, jump_force.y);
                //actionable_ = true;

            }
            else
            {
                state_ = PlayerStateOld.JUMPSQUAT;
                actionable_ = false;
            }

            //state_ = PlayerStateOld.JUMP; this should happen before velocity set with other actions when jumpsquat frames implemented
        }
		else
        {
            directional_influence_.x = directional_influence_.x * DI_MODIFIER;
            directional_influence_.y = directional_influence_.y * DI_MODIFIER;
            directional_influence_.y = Mathf.Min(0, directional_influence_.y);
            move_velocity_.x += directional_influence_.x;
            float capped_x = Mathf.Min(move_velocity_.x, MAX_SPEED);
            capped_x = Mathf.Max(capped_x, -MAX_SPEED);
            move_velocity_.x = capped_x;
            move_velocity_.x = move_velocity_.x * .12f;

            transform.Translate(new Vector3(move_velocity_.x, move_velocity_.y, 0));
        }
        print(state_);
        //print(air_dodges);

        //print("HEY: " + in_air_);
        //print(directional_influence_);
        //print("TEST");
        //print(move);
        //print(directional_influence_);
        //print(move_velocity_);
        //print(capped_x);
        
        //print(move_velocity_);
        
        //transform.position.Set(transform.position.x + move_velocity_.x, transform.position.y, transform.position.z);
        //rigid_body_.velocity = move_velocity_ + directional_influence_ ;

        //rigid_body_.velocity = new Vector2(capped_x, rigid_body_.velocity.y);
        

        jump_button_ = false;
        a_button_ = false;
        trigger_button_ = false;
        DisplayState();

        // if (state_ == PlayerStateOld.JUMP)
        // {
       	// 	Application.targetFrameRate = 1;
       	// 	//Time.timeScale = 0;
        // } else {
        // 	//Application.targetFrameRate = 60;
        // }
    }

    //NEUTRAL = 0,
    //WALK = 1,
    //RUN = 2,
    //DASH = 3,
    //TURN = 4,
    //TURNRUN = 5,
    //NEUTRALAIRDODGE = 6,
    //DIRECTIONALAIRDODGE = 7,
    //JUMP = 8,
    //STOP = 9,

    private void DisplayState()
    {
        if (state_ == PlayerStateOld.NEUTRAL)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (state_ == PlayerStateOld.DASH)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (state_ == PlayerStateOld.RUN)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (state_ == PlayerStateOld.TURN)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
        else if (state_ == PlayerStateOld.TURNRUN)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (state_ == PlayerStateOld.AIRDODGE)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        else if (state_ == PlayerStateOld.JUMP)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (state_ == PlayerStateOld.STOP)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (state_ == PlayerStateOld.JUMPSQUAT)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        else if (state_ == PlayerStateOld.RUN_STOP)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            in_air_ = false;
            jumps_ = 2;
            actionable_ = true;
            if (state_ == PlayerStateOld.AIRDODGE)
            {
                
                state_ = PlayerStateOld.NEUTRAL;
            }
            else 
            {
            	state_ = PlayerStateOld.STOP;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            in_air_ = true;
        }
    }
}
