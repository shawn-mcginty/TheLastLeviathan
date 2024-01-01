namespace TheLastLeviathan;

using System;
using Chickensoft.GoDotCollections;

public interface IAppRepo : IDisposable {
	public IAutoProp<bool> IsMouseCaptured { get; }
	void OnStartGame();
	void Pause();
	void Resume();
	void OnStartMenuEntered();
	event Action? GameStarting;
	event Action? GamePaused;
	event Action? GameResumed;
	event Action? StartMenuEntered;
}

public class AppRepo : IAppRepo {
	public IAutoProp<bool> IsMouseCaptured => _isMouseCaptured;
	private readonly AutoProp<bool> _isMouseCaptured;
	public event Action? GameStarting;
	public event Action? GamePaused;
	public event Action? GameResumed;
	public event Action? StartMenuEntered;


	public AppRepo() {
		_isMouseCaptured = new AutoProp<bool>(false);
	}

	internal AppRepo(AutoProp<bool> isMouseCaptured) {
		_isMouseCaptured = isMouseCaptured;
	}

	public void OnStartGame() {
		_isMouseCaptured.OnNext(true);
		GameStarting?.Invoke();
	}

	public void Pause() {
		_isMouseCaptured.OnNext(false);
		GamePaused?.Invoke();
	}

	public void Resume() {
		_isMouseCaptured.OnNext(true);
		GameResumed?.Invoke();
	}

	public void OnStartMenuEntered() => StartMenuEntered?.Invoke();

	protected void Dispose(bool disposing) {
		if (disposing) {
			GameStarting = null;
			_isMouseCaptured.Dispose();
		}
	}

	public void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
