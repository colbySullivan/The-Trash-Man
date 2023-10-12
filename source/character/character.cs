using Godot;
using System;

public partial class character : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f;
	
	[Export]
	public float JumpVelocity = -300.0f;
	
	// Double jump
	[Export]
	public float DoubleJumpVelocity = -150.0f;
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public bool has_double_jump = false;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		AnimatedSprite2D _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		else
			has_double_jump = false;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
			if (IsOnFloor())
				//Normal jump
				velocity.Y = JumpVelocity;
			else if (!has_double_jump)
			{
				// Double jump from air
				velocity.Y = DoubleJumpVelocity;
				has_double_jump = true;
			}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			if(Input.IsActionPressed("left"))
				_animatedSprite.Play("left");
			if(Input.IsActionPressed("right"))
				_animatedSprite.Play("right");
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
