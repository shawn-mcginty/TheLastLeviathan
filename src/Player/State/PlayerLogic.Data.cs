namespace TheLastLeviathan.Player;

using System.Collections.Generic;
using Godot;

public partial class PlayerLogic {
	public record Data {
		public Vector2 LastStrongDirection { get; set; } = Vector2.Right;
		public Vector2 LastVelocity { get; set; } = Vector2.Zero;

		public List<Player.MovementInputs> MovementInputs { get; set; } = new List<Player.MovementInputs>();
		public List<Player.MovementInputs> LastMovementInputs { get; set; } = new List<Player.MovementInputs>();
		public Player.MovementInputs? LastStrongInput { get; set; }
		public Player.MovementInputs? StrongInput { get; set; }
	}
}
