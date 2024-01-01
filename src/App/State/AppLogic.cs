namespace TheLastLeviathan.App;

using Chickensoft.LogicBlocks;
using Chickensoft.LogicBlocks.Generator;

public interface IAppLogic : ILogicBlock<AppLogic.IState> { }

[StateMachine]
public partial class AppLogic : LogicBlock<AppLogic.IState>, IAppLogic {
	public override IState GetInitialState(IContext context) => new State.StartMenu(context);

	public AppLogic(IAppRepo appRepo) {
		Set(appRepo);
	}
}
