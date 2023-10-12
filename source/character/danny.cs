using Godot;
using System;

public partial class danny : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public int state = 3;
	
	public override void _PhysicsProcess(double delta)
	{
		AnimatedSprite2D _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;


		if(state == 1){
			_animatedSprite.Play("left");
			velocity.X = -Speed;
		}
		if(state == 2){
			_animatedSprite.Play("right");
			velocity.X = Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	public void getRandom()
	{
		Random r = new Random();
		state = r.Next(0, 4);
	}
}
