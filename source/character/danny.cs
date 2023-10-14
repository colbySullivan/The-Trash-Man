using Godot;
using System;

public partial class danny : CharacterBody2D
{
	public const float SpeedRight = 100.0f;
	public const float SpeedLeft = -100.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public int state = 3;
	
	public bool timer = true;
	public double time = 0;
	
	public override void _PhysicsProcess(double delta)
	{
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

		Velocity = velocity;
		MoveAndSlide();
	}
}