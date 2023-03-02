namespace Territory.Mechanics;

/// <summary>
/// The wall jump mechanic for players.
/// </summary>
public partial class WallJumpMechanic : PlayerControllerMechanic
{
	public override int SortOrder => 26;
	private float Gravity => 700f;
	private float WallJumpImpulse => 100f;
	private float WallJumpUpwardImpulse => 300f;
	private const int MaxWallJumps = 4;
	private const float BboxSize = 20f;
	private const float BboxZOffset = 20f;
	private const float BboxHeight = 16f;

	private Vector3 Mins => new Vector3( -BboxSize, -BboxSize, BboxZOffset );
	private Vector3 Maxs => new Vector3( BboxSize, BboxSize, BboxZOffset + BboxHeight );

	private int WallJumpsLeft { get; set; }

	TraceResult DoTrace( Vector3 direction )
	{
		float dist = 48f;
		var ray = new Ray( Position + Vector3.Up * 32f, direction );
		var tr = Trace.Ray( ray, dist ).Ignore( Player ).WorldOnly().Run();
		return tr;
	}

	private TraceResult TraceWallJump()
	{
		//var pos = Player.Position;
		//var tr = Trace.Ray( pos, pos )
		//	.WithAnyTags( "solid", "glass" )
		//	.WithoutTags( "player" )
		//	.Ignore( Player )
		//	.Size( Mins, Maxs )
		//	.Run();

		//if ( tr.Hit )
		//{
		//	var normal = tr.Normal;

		//	DebugOverlay.Sphere( tr.EndPosition, 16f, Color.Blue, 5f, false );
		//	DebugOverlay.Sphere( tr.HitPosition, 4f, Color.Green, 5f, false );
		//	DebugOverlay.Line( tr.EndPosition, tr.EndPosition + normal * 128f, 5f, false );
		//	DebugOverlay.Line( tr.StartPosition, tr.EndPosition, 5f, false );
		//	DebugOverlay.Text( $"Trace normal: {normal}\nTrace entity: {tr.Entity}", tr.EndPosition, 5f );
		//}

		//return tr;

		var trLeft = DoTrace( Player.EyeRotation.Left );
		var trRight = DoTrace( Player.EyeRotation.Right );

		var shortest = (trLeft.Distance > trRight.Distance) ? trRight : trLeft;
		var longest = (trLeft.Distance > trRight.Distance) ? trLeft : trRight;

		if ( shortest.Hit )
		{
			return shortest;
		}
		else if ( longest.Hit )
		{
			return longest;
		}
		else
		{
			return default;
		}
	}

	protected override bool ShouldStart()
	{
		if ( Controller.GroundEntity.IsValid() )
		{
			WallJumpsLeft = MaxWallJumps;
			return false;
		}

		if ( Controller.TimeSinceJumped < 0.1f )
			return false;

		if ( !Input.Pressed( InputButton.Jump ) )
			return false;

		if ( WallJumpsLeft <= 0 )
			return false;

		var tr = TraceWallJump();

		if ( !tr.Hit )
			return false;

		return true;
	}

	protected override void OnStart()
	{
		WallJumpsLeft--;
		var tr = TraceWallJump();

		var velocity = Velocity.WithZ( 0 );
		var direction = tr.Normal.WithZ( 0 ).Normal;

		direction = direction.LerpTo( Player.EyeRotation.Forward, 0.5f );

		Velocity = velocity.Length * direction;
		Velocity += WallJumpImpulse * direction;
		Velocity = Velocity.LerpTo( velocity, 0.5f );

		Velocity += Vector3.Up * WallJumpUpwardImpulse;

		Velocity -= new Vector3( 0, 0, Gravity * 0.5f ) * Time.Delta;
		Controller.GetMechanic<WalkMechanic>().ClearGroundEntity();

		Controller.TimeSinceJumped = 0;
	}

	protected override void Simulate()
	{
		base.Simulate();

		if ( Controller.GroundEntity.IsValid() )
		{
			WallJumpsLeft = MaxWallJumps;
		}
	}

	protected override void OnActivate()
	{
		WallJumpsLeft = MaxWallJumps;
	}
}
