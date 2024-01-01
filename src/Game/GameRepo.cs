namespace TheLastLeviathan.Game;

using System;
using Chickensoft.GoDotCollections;
using Godot;

public interface IGameRepo : IDisposable {
	IAutoProp<Vector2> PlayerGlobalPosition { get; }

	void SetPlayerGlobalPosition(Vector2 playerGlobalPosition);
}

public class GameRepo : IGameRepo {
	public IAutoProp<Vector2> PlayerGlobalPosition => _playerGlobalPosition;

	private readonly AutoProp<Vector2> _playerGlobalPosition;

	private bool _disposedValue;

	public GameRepo() {
		_playerGlobalPosition = new AutoProp<Vector2>(Vector2.Zero);
	}

	internal GameRepo(AutoProp<Vector2> playerGlobalPosition) {
		_playerGlobalPosition = playerGlobalPosition;
	}

	public void SetPlayerGlobalPosition(Vector2 playerGlobalPosition) => _playerGlobalPosition.OnNext(playerGlobalPosition);

	public void Dispose(bool disposing) {
		if (!_disposedValue) {
			if (disposing) {
				_playerGlobalPosition.Dispose();
			}
			_disposedValue = true;
		}
	}

	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
