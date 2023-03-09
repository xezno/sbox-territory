﻿namespace Territory.Weapons;

[Prefab]
public partial class Aim : WeaponComponent, ISingletonComponent
{
	protected override bool CanStart( Player player )
	{
		if ( !Input.Down( InputButton.SecondaryAttack ) ) return false;
		if ( !Weapon.CanFire( player ) ) return false;

		return true;
	}

	protected override void OnStart( Player player )
	{
		base.OnStart( player );

		player?.SetAnimParameter( "b_aiming", true );

		if ( Game.IsServer )
		{
			DoAimInEffects( To.Single( player ) );
		}
	}

	protected override void OnStop( Player player )
	{
		base.OnStop( player );

		player?.SetAnimParameter( "b_aiming", false );

		if ( Game.IsServer )
		{
			DoAimOutEffects( To.Single( player ) );
		}
	}

	[ClientRpc]
	public static void DoAimInEffects()
	{
		Game.AssertClient();
		WeaponViewModel.Current?.SetAnimParameter( "aiming", true );
	}

	[ClientRpc]
	public static void DoAimOutEffects()
	{
		Game.AssertClient();
		WeaponViewModel.Current?.SetAnimParameter( "aiming", false );
	}
}
