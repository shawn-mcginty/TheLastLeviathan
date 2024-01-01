namespace TheLastLeviathan.App;

public partial class AppLogic {
	public partial record State {
		public record LeavingMenu : State, IGet<Input.FadeOutFinished> {
			public LeavingMenu(IContext context) : base(context) {
				var appRepo = Context.Get<IAppRepo>();
				OnEnter<LeavingMenu>(
					(previous) => Context.Output(new Output.FadeOut())
				);
			}

			public IState On(Input.FadeOutFinished input) => new PlayingGame(Context);
		}
	}
}
