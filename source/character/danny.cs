using Godot;
using System;

public partial class danny : CharacterBody2D
{	
	public const float SpeedRight = 100.0f;
	public const float SpeedLeft = -100.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Default attack state is standing still
	public int state = 1;
	
	// Used for simple attack state machine
	public bool timer = true;
	public double time = 0;
	
	// Wonder is the same as idle
	public String attackState = "wonder";
	
	// When mob is in attack state it will move towards user
	public Vector2 playerPos = new Vector2(0,0);
	
	public AnimatedSprite2D _animatedSprite;
	//public AnimatedSprite2D _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
	Random r = new Random();
	
	[Export]
	public Vector2 initalPos = new Vector2(621,540);
	
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play("idle");
	}
	public override void _PhysicsProcess(double delta)
	{
		//var dannynode = GetTree().GetRoot().GetNode("Character");
		//onready var player := get_tree().get_root().get_node("Main2D").get_node("Player")
		//GD.Print(dannynode.Position);
		//Vector2 buffer = dannynode.Position;
		if (timer) {
		  time += delta;
		  // Change danny state every 2 seconds
		  if (time > 2f) {
			state = r.Next(0, 4);
			time = 0;
		  }
	  	}
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// User outside of fight area
		if(attackState == "wonder")
		{
			if(state == 3){
				velocity.X = SpeedLeft;
			}
			if(state == 2){
				velocity.X = SpeedRight;
			}
			else if (state == 1)
			{
				_animatedSprite.Play("idle");
				velocity.X = 0;
			}
			
		}
		// User entered left area
		else if(attackState == "fightLeft")
		{
			//_animatedSprite.Play("left"); // New garbage bag mobs only have one animation
			//_animatedSprite.FlipH = false;
			velocity.X = SpeedLeft;
		}
		// User entered right area
		else if(attackState == "fightRight")
		{
			// _animatedSprite.Play("right");
			//_animatedSprite.FlipH = true;
			velocity.X = SpeedRight;
		}
			

		Velocity = velocity;
		MoveAndSlide();
	}
	private void _on_interaction_area_area_shape_entered(Rid area_rid, Area2D area, long area_shape_index, long local_shape_index)
	{
		if(area.Name == "SwordArea")
		{
			GD.Print("Danny ded");
			QueueFree();
		}
	}
	// Character on left of danny
	private void _on_attack_range_body_entered_left(Node2D body)
	{
		if(body.Name == "sworddanny") // Insures that other collisons don't count
		{
			_animatedSprite.Play("fight");
			attackState = "fightLeft";	
		} 
			
	}
	private void _on_interaction_area_body_exited(Node2D body)
	{
		if(body.Name == "sworddanny")
		{
			_animatedSprite.Play("fight");
			attackState = "wonder";
		}
		// Return to random state when user is outside zone	
	}	
	// Character on right of danny
	private void _on_attack_range_body_entered_right(Node2D body)
	{
		if(body.Name == "sworddanny")
		{
			_animatedSprite.Play("fight");
			attackState = "fightRight";
		}
			
	}	
	private void _on_stomp_area_body_entered(Node2D body)
	{
		// Character stomped on Danny
		//GD.Print(body.Name); // Leave for debugging
		if(body.Name == "sworddanny")
		{
			_animatedSprite.Play("fight");
			QueueFree();
		}
			
	}
}

