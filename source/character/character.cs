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
	public bool was_in_air = false;
	public Vector2 velocity;
	public string[] array = { "jump_end", "jump_start", "jump_double" };
	public bool is_swinging = false;
	
	public override void _Ready()
	{
		// Access to animation globally
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		//velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			was_in_air = true;
		}
			
		else
		{
			has_double_jump = false;
			
			if (was_in_air == true){
				land();
			}		
			was_in_air = false;
		}
			

		// Handle Jump and double jump
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor())
			{
				// Normal jump
				jump();
			}
				
			else if (!has_double_jump)
			{
				// Double jump from air
				velocity.Y = DoubleJumpVelocity;
				has_double_jump = true;
			}
		}
			

		// Get the input direction and handle the movement/deceleration.
		// Need vector to satisify animation updating
		direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero && (!is_swinging || !IsOnFloor()))
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
		swing_sword();
		_on_animated_sprite_2d_animation_finished();
		
		
	}
	public void update_animation()
	{
		if (!animation_locked){
			if (direction.X != 0)
				_animatedSprite.Play("run");
			else
				_animatedSprite.Play("idle");
		}
	}
	public void update_facing_direction()
	{
		if (direction.X > 0)
			_animatedSprite.FlipH = false;
		else if (direction.X < 0)
			_animatedSprite.FlipH = true;
	}
	public void jump()
	{
		velocity.Y = JumpVelocity;
		_animatedSprite.Play("jump_start");
		animation_locked = true;
	}
	public void land()
	{
		_animatedSprite.Play("jump_end");
		animation_locked = true;
	}
	
	public void _on_animated_sprite_2d_animation_finished()
	{
		if(Array.Exists(array, element => element == _animatedSprite.Animation) && IsOnFloor()){
			animation_locked = false;
			is_swinging = false;
		}
				
	}
	public void swing_sword()
	{
		if (Input.IsActionJustPressed("fight"))
		{
			is_swinging = true;
			_animatedSprite.Play("fight");
			animation_locked = true;
		}
	}
	private void _on_animated_sprite_2d_animation_looped()
	{
		// Clear fight animation
		animation_locked = false;
		is_swinging = false;
	}
}



