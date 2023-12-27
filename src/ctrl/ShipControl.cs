using Godot;
using System;

public partial class ShipControl : Node
{
	private RigidBody3D rbody;
	private bool isControlled = true;

	private Vector2 mouseInput;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rbody = GetParent<RigidBody3D>();
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isControlled) {
			ApplyThrust(GetThrust());
			ApplyYaw(GetYaw());
			ApplyPitch(GetPitch());
		}
		GD.Print($"{rbody.Transform.Origin}, {rbody.LinearVelocity.Normalized()}, {mouseInput}");
		mouseInput = Vector2.Zero;
	}

	private void ApplyThrust(float thrust) {
		rbody.ApplyCentralForce(rbody.GlobalTransform.Basis.Z * -thrust);
	}

	private void ApplyYaw(float yaw) {
		rbody.ApplyTorque(rbody.Basis.Y * yaw * -0.0001f);
	}

	private void ApplyPitch(float pitch) {
		rbody.ApplyTorque(rbody.Basis.X * pitch * -0.0001f);
	}

	private void ApplyRoll(float roll) {
		rbody.ApplyTorque(rbody.Basis.Z * -roll * 0.0001f);
	}

	private float GetThrust() => Input.GetAxis("thrustNeg", "thrustPos");
	private float GetYaw() => mouseInput.X;
	private float GetRoll() => Input.GetAxis("rollNeg", "rollPos");
	private float GetPitch() => mouseInput.Y;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion) {
			InputEventMouseMotion e = @event as InputEventMouseMotion;

			mouseInput = e.Relative;
		}
    }

    public override void _ExitTree()
    {
		GetTree().Quit();
        base._ExitTree();
    }
}
