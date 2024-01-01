namespace TheLastLeviathan.Game;

public partial class GameLogic {
	public static class Output {
		public readonly record struct MouseCaptured(bool IsMouseCaptured);
		public readonly record struct FocusPlayer;
		public readonly record struct SetPauseMode(bool IsPaused);
	}
}
