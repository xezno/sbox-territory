namespace Territory.Weapons;

[Prefab]
public partial class Reload : WeaponComponent, ISingletonComponent
{
	[Net, Prefab, Category( "Weapon" )] public int Clip { get; set; } = 90;
	[Net, Prefab, Category( "Weapon" )] public int Ammo { get; set; } = 30;
	[Net, Prefab, Category( "Weapon" )] public float ReloadTime { get; set; } = 1f;

	TimeSince _timeSinceReload;

	protected override bool CanStart( Player player )
	{
		if ( !Input.Down( InputButton.Reload ) ) return false;
		return true;
	}

	protected override void OnStart( Player player )
	{
		base.OnStart( player );

		_timeSinceReload = 0;

		// Send clientside effects to the player.
		if ( Game.IsServer )
		{
			DoReloadEffects( To.Single( player ) );
		}

		// Reload the weapon
		Entity.Ammo -= Clip;
		Entity.Clip = Clip;
	}

	[ClientRpc]
	public static void DoReloadEffects()
	{
		Game.AssertClient();
		WeaponViewModel.Current?.SetAnimParameter( "reload", true );
	}
}
