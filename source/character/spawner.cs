using Godot;
using System;

public partial class spawner : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create new instances of mobs
		//node.Position = GetNode<CharacterBody2D>("Character").Position;
		var scene = GD.Load<PackedScene>("res://character/danny.tscn"); // Will load when the script is instanced.
		for(int i = 0; i < 5; i++){
			var node = scene.Instantiate();
			AddChild(node);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// Player entered a portal
	// TODO can clean this up by simply using the scence names
	// concated into calls
	private void _on_portal_body_entered(Node2D body)
	{
		String _sceneName = GetTree().CurrentScene.Name;
		if(body.Name == "AreaHitbox")
			switch(_sceneName) 
			{
				default:
					GetTree().ChangeSceneToFile("res://Levels/level1.tscn");
				break;
				case "level1":
					GetTree().ChangeSceneToFile("res://Levels/level2.tscn");
				break;	
				case "level2":
					GetTree().ChangeSceneToFile("res://Levels/level3.tscn");
				break;
				case "level3":
					GetTree().ChangeSceneToFile("res://Levels/menu.tscn");
				break;
			}
	}
	
	// Player fell through hole
	private void _on_fall_area_area_entered(Area2D area)
	{
		GetTree().ReloadCurrentScene();
	}
}
