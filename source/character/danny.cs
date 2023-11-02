using Godot;
using System;

public partial class danny : CharacterBody2D
{
	public const float SpeedRight = 100.0f;
	public const float SpeedLeft = -100.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public int state = 1;
	
	public bool timer = true;
	public double time = 0;
	
	public String attackState = "wonder";
	
	public Vector2 playerPos = new Vector2(0,0);
	
	public override void _PhysicsProcess(double delta)
	{
		//var dannynode = GetTree().GetRoot().GetNode("Character");
		//onready var player := get_tree().get_root().get_node("Main2D").get_node("Player")
		//GD.Print(dannynode.Position);
		//Vector2 buffer = dannynode.Position;
		if (timer) {
		  time += delta;
		  if (time > 2f) {
			// 2 seconds has passed, do your stuff
			Random r = new Random();
			state = r.Next(0, 4);
			time = 0;
		  }
	  	}
		AnimatedSprite2D _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		if(attackState == "wonder")
		{
			if(state == 3){
			_animatedSprite.Play("left");
			velocity.X = SpeedLeft;
			}
			if(state == 2){
			_animatedSprite.Play("right");
			velocity.X = SpeedRight;
			}
			else if (state == 1)
			velocity.X = 0;
		}
		else if(attackState == "fightLeft")
		{
			_animatedSprite.Play("left");
			velocity.X = SpeedLeft;
		}
		else if(attackState == "fightRight")
		{
			_animatedSprite.Play("right");
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
		if(body.Name == "Character") // Insures that other collisons don't count
			attackState = "fightLeft";	
	}
	private void _on_interaction_area_body_exited(Node2D body)
	{
		if(body.Name == "Character")
			// Return to random state when user is outside zone
			attackState = "wonder";
	}	
	// Character on right of danny
	private void _on_attack_range_body_entered_right(Node2D body)
	{
		if(body.Name == "Character")
			attackState = "fightRight";
	}	
}
