using Godot;
using System.Collections.Generic;

public partial class SettingsManager : Node
{
	public StateMachine StateMachine = new StateMachine();
	public CanvasLayer SettingsContainer;

	[Signal]
	public delegate void FOVChangedEventHandler(float value);
	[Signal]
	public delegate void ViewDistanceChangedEventHandler(float value);
	[Signal]
	public delegate void BrightnessChangedEventHandler(float value);
	[Signal]
	public delegate void RenderScaleChangedEventHandler(float value);
	[Signal]
	public delegate void LODThresholdChangedEventHandler(float value);
	[Signal]
	public delegate void AntialiasingChangedEventHandler(float value);
	[Signal]
	public delegate void MasterVolumeChangedEventHandler(float value);
	[Signal]
	public delegate void MusicVolumeChangedEventHandler(float value);
	[Signal]
	public delegate void UIVolumeChangedEventHandler(float value);
	[Signal]
	public delegate void AccessInventoryChangedEventHandler(string value);
	[Signal]
	public delegate void MoveForwardChangedEventHandler(string value);
	[Signal]
	public delegate void MoveBackwardChangedEventHandler(string value);
	[Signal]
	public delegate void MoveLeftChangedEventHandler(string value);
	[Signal]
	public delegate void MoveRightChangedEventHandler(string value);
	[Signal]
	public delegate void SprintChangedEventHandler(string value);
	[Signal]
	public delegate void CrouchChangedEventHandler(string value);
	[Signal]
	public delegate void JumpChangedEventHandler(string value);


	public Dictionary<string, string> Settings = new Dictionary<string, string>()
	{
		{"FOV", "90"},
		{"ViewDistance", "4000"},
		{"Brightness", "1"},
		{"RenderScale", "1"},
		{"LODThreshold", "1"},
		{"Antialiasing", "1"},
		{"MasterVolume", "50"},
		{"MusicVolume", "100"},
		{"UIVolume", "100"},
		{"AccessInventory", "Tab"},
		{"MoveForward", "W"},
		{"MoveBackward", "S"},
		{"MoveLeft", "A"},
		{"MoveRight", "D"},
		{"Sprint", "Shift"},
		{"Crouch", "Ctrl"},
		{"Jump", "Space"}
	};

	Persistence Persistence;

	public override void _Ready()
	{
		Persistence = GetNode<Persistence>("/root/Persistence");
		Persistence.LoadSettings();

		SetFOV(float.Parse(Settings["FOV"]));
		SetViewDistance(float.Parse(Settings["ViewDistance"]));
		SetBrightness(float.Parse(Settings["Brightness"]));
		SetRenderScale(float.Parse(Settings["RenderScale"]));
		SetLODThreshold(float.Parse(Settings["LODThreshold"]));
		SetAntialiasing(float.Parse(Settings["Antialiasing"]));
		SetMasterVolume(float.Parse(Settings["MasterVolume"]));
		SetUIVolume(float.Parse(Settings["UIVolume"]));
		SetAccessInventory(Key.Parse<Key>(Settings["AccessInventory"]));
		SetMoveForward(Key.Parse<Key>(Settings["MoveForward"]));
		SetMoveBackward(Key.Parse<Key>(Settings["MoveBackward"]));
		SetMoveLeft(Key.Parse<Key>(Settings["MoveLeft"]));
		SetMoveRight(Key.Parse<Key>(Settings["MoveRight"]));
		SetSprint(Key.Parse<Key>(Settings["Sprint"]));
		SetCrouch(Key.Parse<Key>(Settings["Crouch"]));
		SetJump(Key.Parse<Key>(Settings["Jump"]));

		StateMachine.ChangeState(new SettingsState_InterfaceHidden(this));
	}

	public override void _Process(double delta)
	{
		StateMachine.Update();
	}

	public override void _Input(InputEvent @event)
	{
		StateMachine._Input(@event);
	}

	public void SetFOV(float value)
	{
		Settings["FOV"] = value.ToString();
		GetTree().Root.GetCamera3D().Fov = value;
		EmitSignal(SignalName.FOVChanged, value);
	}

	public void SetViewDistance(float value)
	{
		Settings["ViewDistance"] = value.ToString();
		GetTree().Root.GetCamera3D().Far = value;
		EmitSignal(SignalName.ViewDistanceChanged, value);
	}

	public void SetBrightness(float value)
	{
		Settings["Brightness"] = value.ToString();
		GetTree().Root.GetCamera3D().Environment.AdjustmentBrightness = value;
		EmitSignal(SignalName.BrightnessChanged, value);
	}

	public void SetRenderScale(float value)
	{
		Settings["RenderScale"] = value.ToString();
		GetTree().Root.Scaling3DScale = value;
		EmitSignal(SignalName.RenderScaleChanged, value);
	}

	public void SetLODThreshold(float value)
	{
		Settings["LODThreshold"] = value.ToString();
		GetTree().Root.MeshLodThreshold = value;
		EmitSignal(SignalName.LODThresholdChanged, value);
	}

	public void SetAntialiasing(float value)
	{
		Settings["Antialiasing"] = value.ToString();
		GetTree().Root.Msaa3D = (Viewport.Msaa)(int)value;
		EmitSignal(SignalName.AntialiasingChanged, value);
	}

	public void SetMasterVolume(float value)
	{
		Settings["MasterVolume"] = value.ToString();
		AudioServer.SetBusVolumeDb(0, value);
		EmitSignal(SignalName.MasterVolumeChanged, value);
	}

	public void SetMusicVolume(float value)
	{
		Settings["MusicVolume"] = value.ToString();
		AudioServer.SetBusVolumeDb(1, value);
		EmitSignal(SignalName.MusicVolumeChanged, value);
	}

	public void SetUIVolume(float value)
	{
		Settings["UIVolume"] = value.ToString();
		AudioServer.SetBusVolumeDb(2, value);
		EmitSignal(SignalName.UIVolumeChanged, value);
	}

	public void SetAccessInventory(Key value)
	{
		Settings["AccessInventory"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("UI_Access_Inventory", value);
		EmitSignal(SignalName.AccessInventoryChanged, value.ToString());
	}

	public void SetMoveForward(Key value)
	{
		Settings["MoveForward"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Forward", value);
		EmitSignal(SignalName.MoveForwardChanged, value.ToString());
	}

	public void SetMoveBackward(Key value)
	{
		Settings["MoveBackward"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Backward", value);
		EmitSignal(SignalName.MoveBackwardChanged, value.ToString());
	}

	public void SetMoveLeft(Key value)
	{
		Settings["MoveLeft"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Left", value);
		EmitSignal(SignalName.MoveLeftChanged, value.ToString());
	}

	public void SetMoveRight(Key value)
	{
		Settings["MoveRight"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Right", value);
		EmitSignal(SignalName.MoveRightChanged, value.ToString());
	}

	public void SetSprint(Key value)
	{
		Settings["Sprint"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Sprint", value);
		EmitSignal(SignalName.SprintChanged, value.ToString());
	}

	public void SetCrouch(Key value)
	{
		Settings["Crouch"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Crouch", value);
		EmitSignal(SignalName.CrouchChanged, value.ToString());
	}

	public void SetJump(Key value)
	{
		Settings["Jump"] = value.ToString();
		StateMachine.ChangeState(new SettingsState_InterfaceVisible(this));
		UpdateAction("Movement_Jump", value);
		EmitSignal(SignalName.JumpChanged, value.ToString());
	}

	public void UpdateAction(string actionName, Key key)
	{
		InputMap.ActionEraseEvents(actionName);
		InputEventKey newEvent = new InputEventKey();
		newEvent.Keycode = key;
		InputMap.ActionAddEvent(actionName, newEvent);
	}
}

public class SettingsState_InterfaceVisible : IState
{
	SettingsManager owner;

	public SettingsState_InterfaceVisible(SettingsManager owner) { this.owner = owner; }

	public void Enter(StateMachine stateMachine)
	{
		owner.SettingsContainer.Show();
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	public void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("UI_Back"))
		{
			owner.StateMachine.ChangeState(new SettingsState_InterfaceHidden(owner));
		}
	}

}

public class SettingsState_InterfaceHidden : IState
{
	SettingsManager owner;

	public SettingsState_InterfaceHidden(SettingsManager owner) { this.owner = owner; }

	public void Enter(StateMachine stateMachine)
	{
		owner.SettingsContainer.Hide();
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	public void _Input(InputEvent @event)
	{
	}

}

public class SettingsState_KeybindEdit : IState
{
	SettingsManager owner;
	public string ActionToEdit;

	public SettingsState_KeybindEdit(SettingsManager owner) { this.owner = owner; }

	public void Enter(StateMachine stateMachine)
	{
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	public void _Input(InputEvent @event)
	{
	}
}
