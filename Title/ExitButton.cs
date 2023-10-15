using Godot;

public partial class ExitButton : Button
{
	public override void _Pressed()
	{
		GetTree().Quit();
	}
}
