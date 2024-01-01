namespace TheLastLeviathan.Game;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.PowerUps;
using Godot;
using SuperNodes.Types;
using TheLastLeviathan.Player;
using TheLastLeviathan.PlayerCamera;

public interface IGame : INode2D, IProvide<IGameRepo> { }

[SuperNode(typeof(Provider), typeof(Dependent), typeof(AutoNode))]
public partial class Game : Node2D, IGame {
	public override partial void _Notification(int what); // dunno, this is for chickensoft magic

	#region State
	public IGameRepo GameRepo { get; set; } = default!;
	public IGameLogic GameLogic { get; set; } = default!;
	public GameLogic.IBinding GameBinding { get; set; } = default!;
	#endregion

	#region Nodes
	[Node]
	public IPlayer Player { get; set; } = default!;
	[Node]
	public IPlayerCamera PlayerCamera { get; set; } = default!;
	#endregion

	#region Provisions
	IGameRepo IProvide<IGameRepo>.Value() => GameRepo;
	#endregion

	#region Dependencies
	[Dependency]
	public IAppRepo AppRepo => DependOn<IAppRepo>();
	#endregion

	public void Setup() {
		GameRepo = new GameRepo();
		GameLogic = new GameLogic(AppRepo);

		Provide();
	}

	public void OnResolved() {
		GD.Print("Game.OnResolved");
		GameBinding = GameLogic.Bind();

		GameBinding
			.Handle<GameLogic.Output.FocusPlayer>((output) => {
				GD.Print("Game Handle FocusPlayer");
				PlayerCamera.UsePLayerCamera();
			})
			.Handle<GameLogic.Output.SetPauseMode>((output) => {
				GD.Print("Game Handle SetPauseMode");
				GetTree().Paused = output.IsPaused;
			});

		GameLogic.Start();

		GameLogic.Input(new GameLogic.Input.Initialize());
	}


	public void OnExitTree() {
		GameLogic.Stop();
		GameBinding.Dispose();
		GameRepo.Dispose();
	}
}
