namespace TheLastLeviathan.Game;

using Chickensoft.LogicBlocks;
using Chickensoft.LogicBlocks.Generator;

public interface IGameLogic : ILogicBlock<GameLogic.IState> { }

[StateMachine]
public partial class GameLogic : LogicBlock<GameLogic.IState>, IGameLogic {
	public override IState GetInitialState(IContext context) => new State(context);

	public GameLogic(IAppRepo appRepo) {
		Set(appRepo);
	}
}
