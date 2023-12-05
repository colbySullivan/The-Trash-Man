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
	
	Random r = new Random();
	
	[Export]
	public Vector2 initalPos = new Vector2(621,540);
	
	//public override void _Ready()
	//{
		// Spawn more class instances
		//danny mob = MobScene.Instantiate<danny>();
		//var dannyScene = ResourceLoader.Load("res://character/danny.tscn");
		//instance.SetPos = initalPos;
		// Store the reference to the SpawnLocation node.
		//var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		//for(int i = 4; i < 4; i++){
			//mobSpawnLocation.ProgressRatio = GD.Randf();
			//Vector2 playerPosition = GetNode<CharacterBody2D>("Character").Position;
			//mob.Initialize(mobSpawnLocation.Position, playerPosition);
			// Spawn the mob by adding it to the Main scene.
			//mob.playerPos = playerPosition;
			//GD.Print("created new danny instance");
			//AddChild(mob);
		//}
	//}
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
		AnimatedSprite2D _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// User outside of fight area
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
		// User entered left area
		else if(attackState == "fightLeft")
		{
			_animatedSprite.Play("left");
			velocity.X = SpeedLeft;
		}
		// User entered right area
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
		if(body.Name == "sworddanny") // Insures that other collisons don't count
			attackState = "fightLeft";	
	}
	private void _on_interaction_area_body_exited(Node2D body)
	{
		if(body.Name == "sworddanny")
			// Return to random state when user is outside zone
			attackState = "wonder";
	}	
	// Character on right of danny
	private void _on_attack_range_body_entered_right(Node2D body)
	{
		if(body.Name == "sworddanny")
			attackState = "fightRight";
	}	
	private void _on_stomp_area_body_entered(Node2D body)
	{
		// Character stomped on Danny
		if(body.Name == "sworddanny")
		{
			QueueFree();
		}
			
	}
}

