using Godot;
using System;
using System.Threading.Tasks;

public partial class SceneManager : Node
{
	[Signal]
	public delegate void SceneChangedEventHandler(string path);

	public async Task ChangeScene(string path)
	{
		GetTree().ChangeSceneToFile(path);
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
		EmitSignal(SignalName.SceneChanged, path);
	}
}
