namespace TheLastLeviathan.Player;

using Godot;

public partial class PlayerLogic {
	public static class Input {
		public readonly record struct Enable;
		public readonly record struct PhysicsTick(double Delta);
		public readonly record struct Moved(Vector2 GlobalPosition);
		public readonly record struct StartedMoving;
		public readonly record struct StoppedMoving;
		public readonly record struct ChangeDirection(Vector2 Direction);
	}
}
