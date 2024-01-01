namespace TheLastLeviathan.App;

using Godot;

public partial class AppLogic {
	public partial record State {
		public record StartMenu : State, IGet<Input.StartGame> {
			public StartMenu(IContext context) : base(context) {
				var appRepo = Context.Get<IAppRepo>();
				OnEnter<StartMenu>(
					(previous) => {
						GD.Print("StartMenu.OnEnter");
						appRepo.OnStartMenuEntered();
						Context.Output(new Output.LoadGame());
						Context.Output(new Output.ShowStartMenu());
					}
				);
			}

			public IState On(Input.StartGame input) {
				GD.Print("StartMenu.OnStartGame");
				return new LeavingMenu(Context);
			}
		}
	}
}
