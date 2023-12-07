using Godot;
using System;

public partial class sworddanny : CharacterBody2D
{
	[Export]
	public float Speed = 125.0f;
	
	[Export]
	public float JumpVelocity = -300.0f;
	
	// Double jump
	[Export]
	public float DoubleJumpVelocity = -150.0f;
	
	[Export]
	public int health = 3;
	
	[Export]
	public Vector2 initalPos = new Vector2(250,500);
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	public bool has_double_jump = false;
	
	// Locks animition when in jump
	public bool animation_locked = false;
	
	// Insures idle animation is played on no key press
	public Vector2 direction = Vector2.Zero;
	
	private AnimatedSprite2D _animatedSprite;
	
	// Used in land to stop jump animation
	public bool was_in_air = false;
	
	public Vector2 velocity;
	
	public string[] array = { "jump_end", "jump_start", "jump_double" };
	
	// Checks if sword is swinging so player can't move
	public bool is_swinging = false;
	
	// Used for sword collision
	public bool facing_left = false;
	
	private String _sceneName;
	
	public override void _Ready()
	{
		// Access to animation globally
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sceneName = GetTree().CurrentScene.Name;
	}

	public override void _PhysicsProcess(double delta)
	{
		//velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			was_in_air = true;
			//GD.Print(_sceneName);
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
		// If player is in air they can swing and move
		if(_sceneName != "jump_level")
		{
			if (direction != Vector2.Zero && (!is_swinging || !IsOnFloor()))
			{
				velocity.X = direction.X * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}
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
		{
			facing_left = false;
			_animatedSprite.FlipH = false;
		}
			
		else if (direction.X < 0)
		{
			facing_left = true;
			_animatedSprite.FlipH = true;
		}
			
	}
	public void jump()
	{
		velocity.Y = JumpVelocity;
		animation_locked = true;
	}
	public void land()
	{
		_animatedSprite.Play("idle");
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
		var node = GetNode<CollisionShape2D>("SwordArea/CollisionShape2D");
		// Sword starts to the right
		node.Position = node.right;
		if (Input.IsActionJustPressed("fight"))
		{
			// Lock movement and animation
			is_swinging = true;
			_animatedSprite.Play("fight");
			animation_locked = true;
			if(facing_left)
				node.Position = node.left;
			// Renable sword area hitbox
			node.Disabled = false;
		}
		else
			node.Disabled = true;
	}
	private void _on_animated_sprite_2d_animation_looped()
	{
		// Clear fight animation
		animation_locked = false;
		is_swinging = false;
	}
	private void _on_area_hitbox_body_entered(Node2D body)
	{
		if(body.IsInGroup("Mobs"))
		{
			//var health2 = GetNode<Sprite2D>("Health1/Health2/Health2"); //TODO there is a clear way to implement healt bars
			if(health < 3)
			{
				GetNode<Sprite2D>("Health1/Health2").Hide();
			}
			GetNode<Sprite2D>("Health1/Health2/Health3").Hide();
			//GetNode<Label>("Health").SetText("New text"); // Remove health
			_animatedSprite.Play("idle_death");
			animation_locked = true;
			GD.Print(body.Name);
			// Restart scene on danny collision
			health--;
			if (health <= 0)
			{
				Position = initalPos;
				GetTree().ReloadCurrentScene();
			}
				
		}
	}
	// Player fell down pit
	public void _on_fall_area_area_entered(Area2D area)
	{
			GetTree().ReloadCurrentScene();
	}
	// Player went through portal
	public void _on_portal_body_entered(Node2D body)
	{
		GD.Print(body.Name);
		if(body.Name == "AreaHitbox")
			GetTree().ChangeSceneToFile("res://Levels/level1.tscn");
	}
	// Restart game
	private void restart_portal(Node2D body)
	{
		if(body.Name == "AreaHitbox")
			GetTree().ChangeSceneToFile("res://Levels/jump_level.tscn");
	}
	public void _on_level2_finish_door_area_entered(Node2D body)
	{
		if(body.Name == "AreaHitbox")
			GetTree().ChangeSceneToFile("res://Levels/menu.tscn");// TODO next level
		
	}
}
