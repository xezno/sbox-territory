namespace GameTemplate.Mechanics;

/// <summary>
/// The jump mechanic for players.
/// </summary>
public partial class JumpMechanic : PlayerControllerMechanic
{
	public override int SortOrder => 25;
	private float Gravity => 700f;
	private float LongJumpImpulse => 240f;
	private float JumpStrength => 300f;

	protected override bool ShouldStart()
	{
		if ( !Input.Pressed( InputButton.Jump ) ) return false;
		if ( !Controller.GroundEntity.IsValid() ) return false;
		return true;
	}

	protected override void OnStart()
	{
		float flGroundFactor = 1.0f;
		float flMul = JumpStrength;
		float startz = Velocity.z;

		Velocity = Velocity.WithZ( startz + flMul * flGroundFactor );
		Velocity -= new Vector3( 0, 0, Gravity * 0.5f ) * Time.Delta;

		Controller.GetMechanic<WalkMechanic>()
			.ClearGroundEntity();

		if ( Controller.IsMechanicActive<CrouchMechanic>() )
		{
			// This is a long jump
			var lookDir = Player.EyeRotation.Forward;
			lookDir = lookDir.WithZ( 0 ).Normal;

			// Impulse - shoot player forwards for a long jump
			Velocity += lookDir * LongJumpImpulse;
		}
	}
}
