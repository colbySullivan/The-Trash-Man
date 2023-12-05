using Godot;
using System;

public partial class CollisionShape2D : Godot.CollisionShape2D
{
	[Export]
	public Vector2 right = new Vector2(20,-2);
	
	[Export]
	public Vector2 left = new Vector2(-20,-2);
}
