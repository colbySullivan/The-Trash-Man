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
	public bool animation_locked = false;
	public Vector2 direction = Vector2.Zero;
	private AnimatedSprite2D _animatedSprite;
	
	public override void _Ready()
	{
		// Access to animation globally
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		else
			has_double_jump = false;

		// Handle Jump and double jump
		if (Input.IsActionJustPressed("jump"))
			if (IsOnFloor())
				// Normal jump
				velocity.Y = JumpVelocity;
			else if (!has_double_jump)
			{
				// Double jump from air
				velocity.Y = DoubleJumpVelocity;
				has_double_jump = true;
			}

		// Get the input direction and handle the movement/deceleration.
		// Need vector to satisify animation updating
		direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		
		Velocity = velocity;
		MoveAndSlide();
		update_animation();
		update_facing_direction();
	}
	public void update_animation()
	{
		if (!animation_locked)
			if (direction.X != 0)
				_animatedSprite.Play("run");
			else
				_animatedSprite.Play("idle");
	}
	public void update_facing_direction()
	{
		if (direction.X > 0)
			_animatedSprite.FlipH = false;
		else if (direction.X < 0)
			_animatedSprite.FlipH = true;
	}
}
