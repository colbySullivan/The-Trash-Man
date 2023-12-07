using Godot;
using System;

public partial class menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Can select with ui arrows
		GetNode<Button>("Start").GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_start_pressed()
	{
		GetTree().ChangeSceneToFile("res://Levels/jump_level.tscn");
	}


	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}

}
