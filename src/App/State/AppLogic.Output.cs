namespace TheLastLeviathan.App;

public partial class AppLogic {
	public static class Output {
		public readonly record struct StartGame;
		public readonly record struct CaptureMouse(bool IsMouseCaptured);
		public readonly record struct LoadGame;
		public readonly record struct ShowStartMenu;
		public readonly record struct FadeOut;
		public readonly record struct ShowPauseMenu;
		public readonly record struct HidePauseMenu;
		public readonly record struct ShowGame;
		public readonly record struct DisablePauseMenu;
	}
}
