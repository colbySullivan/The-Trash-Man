using Godot;
using System;

public partial class spawner : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create new instances of mobs
		var scene = GD.Load<PackedScene>("res://character/danny.tscn"); // Will load when the script is instanced.
		var node = scene.Instantiate();
		//node.Position = GetNode<CharacterBody2D>("Character").Position;
		for(int i = 0; i < 4; i++){
			AddChild(node);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
