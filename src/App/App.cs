namespace TheLastLeviathan.App;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.PowerUps;
using Godot;
using SuperNodes.Types;
using TheLastLeviathan.Utils;

public interface IApp : ICanvasLayer, IProvide<IAppRepo> { }

[SuperNode(typeof(AutoSetup), typeof(AutoNode), typeof(Provider))]
public partial class App : CanvasLayer, IApp {
	public override partial void _Notification(int what); // dunno, this is for chickensoft magic

	#region Constants
	public const string GAME_SCENE_PATH = "res://src/Game/Game.tscn";
	#endregion

	#region Externals
	public Game.IGame Game { get; set; } = default!;
	public IInstantiator Instantiator { get; set; } = default!;
	#endregion

	#region State
	public IAppRepo AppRepo { get; set; } = default!;
	public IAppLogic AppLogic { get; set; } = default!;
	public AppLogic.IBinding AppBinding { get; set; } = default!;
	#endregion

	#region Provisions
	public IAppRepo Value() => AppRepo;
	#endregion

	#region Nodes
	[Node]
	public IStartMenu StartMenu { get; set; } = default!;

	[Node]
	public ISubViewport GameViewport { get; set; } = default!;
	#endregion

	public void Setup() {
		GD.Print("App.Setup");
		AppRepo = new AppRepo();
		AppLogic = new AppLogic(AppRepo);
		Instantiator = new Instantiator(GetTree());

		StartMenu.Start += OnStart;

		Provide();
	}

	public void OnReady() {
		AppBinding = AppLogic.Bind();

		AppBinding
			.Handle<AppLogic.Output.LoadGame>((output) => {
				GD.Print("App Handle LoadGame");
				Game = Instantiator.LoadAndInstantiate<Game.Game>(GAME_SCENE_PATH);
				Game.Hide();
				GameViewport.AddChildEx(Game);
				Instantiator.SceneTree.Paused = false;
			})
			.Handle<AppLogic.Output.ShowStartMenu>((output) => {
				GD.Print("App Handle ShowStartMenu");
				StartMenu.Show();
				FadeIn();
			})
			.Handle<AppLogic.Output.FadeOut>((output) => {
				GD.Print("App Handle FadeOut");
				FadeOut();
			})
			.Handle<AppLogic.Output.ShowGame>((output) => {
				GD.Print("App Handle ShowGame");
				Game.Show();
				StartMenu.Hide();
				FadeIn();
			});

		AppLogic.Start();
	}

	private void FadeOut() {
		GD.Print("FadeOut -- Implement this later");
		AppLogic.Input(new AppLogic.Input.FadeOutFinished());
	}
	private void FadeIn() {
		GD.Print("FadeIn -- Implement this later");
		AppLogic.Input(new AppLogic.Input.FadeInFinished());
	}
	private void OnStart() {
		GD.Print("App.OnStart");
		AppLogic.Input(new AppLogic.Input.StartGame());
	}

	public void OnExitTree() {
		AppLogic.Stop();
		AppBinding.Dispose();
		AppRepo.Dispose();

		StartMenu.Start -= OnStart;
	}
}
