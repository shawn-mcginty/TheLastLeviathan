namespace TheLastLeviathan.Player;

using Godot;

public partial class PlayerLogic {
	public static class Output {
		public static class Animations {
			public readonly record struct Idle;
			public readonly record struct Walk;
		}

		public readonly record struct MovementComputed(Vector2 Velocity);
	}
}
