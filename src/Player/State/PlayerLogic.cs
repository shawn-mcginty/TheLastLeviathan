namespace TheLastLeviathan.Player;

using Chickensoft.LogicBlocks;
using Chickensoft.LogicBlocks.Generator;
using TheLastLeviathan.Game;

public interface IPlayerLogic : ILogicBlock<PlayerLogic.IState> { }

[StateMachine]
public partial class PlayerLogic : LogicBlock<PlayerLogic.IState>, IPlayerLogic {
	public override IState GetInitialState(IContext context) => new State.Disabled(Context);

	public PlayerLogic(IPlayer player, Settings settings, IAppRepo appRepo, IGameRepo gameRepo) {
		Set(player);
		Set(settings);
		Set(appRepo);
		Set(gameRepo);
		Set(new Data());
	}
}
