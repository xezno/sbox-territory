namespace Territory.Mechanics;

/// <summary>
/// The basic sprinting mechanic for players.
/// It shouldn't, though.
/// </summary>
public partial class SprintMechanic : PlayerControllerMechanic
{
	/// <summary>
	/// Sprint has a higher priority than other mechanics.
	/// </summary>
	public override int SortOrder => 10;

	private float SprintMultiplier => 1.25f;
	public override float? WishSpeed => Player.Mercenary.Resource.BaseSpeed * SprintMultiplier;

	protected override bool ShouldStart()
	{
		if ( !Input.Down( InputButton.Run ) ) return false;
		if ( Player.MoveInput.Length == 0 ) return false;

		return true;
	}
}
