using Godot;
using System;

public partial class CharacterCameraController : Camera3D
{
	[Export]
	public float RotationSpeed = 2.0f;

	[Export]
	public float VerticalRotationLimit = Mathf.Pi / 4;

	[Export]
	public float DistanceFromCharacter = 5.0f;

	[Export]
	public Vector3 CameraOffset = new Vector3(0, 2, 0); // Adjust vertical offset

	private Node3D character;
	private float horizontalAngle = 0;
	private float verticalAngle = 0;

	public override void _Ready()
	{
		// Get the parent node (assumed to be the character)
		character = GetParent<Node3D>();

		// Initialize angles
		horizontalAngle = Rotation.Y;
		verticalAngle = 0;
	}

	public override void _Process(double delta)
	{
		// Rotate camera based on input
		float horizontalRotation = 0;
		float verticalRotation = 0;

		if (Input.IsActionPressed("pl_rotate_camera_left"))
			horizontalRotation -= 1;
		if (Input.IsActionPressed("pl_rotate_camera_right"))
			horizontalRotation += 1;
		if (Input.IsActionPressed("pl_rotate_camera_up"))
			verticalRotation -= 1;
		if (Input.IsActionPressed("pl_rotate_camera_down"))
			verticalRotation += 1;

		// Update angles based on input
		horizontalAngle += (float)delta * horizontalRotation * RotationSpeed;
		verticalAngle = Mathf.Clamp(
			verticalAngle + (float)delta * verticalRotation * RotationSpeed,
			-VerticalRotationLimit,
			VerticalRotationLimit
		);

		// Calculate new position using spherical coordinates
		Vector3 cameraPosition = new Vector3(
			Mathf.Cos(horizontalAngle) * Mathf.Cos(verticalAngle),
			Mathf.Sin(verticalAngle),
			Mathf.Sin(horizontalAngle) * Mathf.Cos(verticalAngle)
		) * DistanceFromCharacter;

		// Set the camera's position relative to the character
		GlobalPosition = character.GlobalPosition + CameraOffset + cameraPosition;

		// Make the camera look at the character
		LookAt(character.GlobalPosition + CameraOffset, Vector3.Up);
	}
}
