namespace TheLastLeviathan.Utils;

using Godot;

public interface IInstantiator {
	public SceneTree SceneTree { get; }

	T LoadAndInstantiate<T>(string path) where T : Node;
}

public class Instantiator : IInstantiator {
	public SceneTree SceneTree { get; }

	public Instantiator(SceneTree sceneTree) {
		SceneTree = sceneTree;
	}

	public T LoadAndInstantiate<T>(string path) where T : Node {
		var scene = GD.Load<PackedScene>(path);
		return scene.Instantiate<T>();
	}
}
