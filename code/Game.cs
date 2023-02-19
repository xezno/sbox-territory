global using Sandbox;
global using Sandbox.UI;
global using System;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Linq;
//
global using Territory.UI;

namespace Territory;

public partial class TerritoryGameManager : GameManager
{
	public List<MercResource> MercResources { get; private set; } = new();

	public static TerritoryGameManager Instance { get; private set; }

	public TerritoryGameManager()
	{
		Instance = this;

		MercResources = ResourceLibrary.GetAll<MercResource>().ToList();

		if ( Game.IsClient )
		{
			Game.RootPanel = new Hud();
		}
		else
		{
			Game.TickRate = 30;
		}
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var pawn = new Player();
		client.Pawn = pawn;
		pawn.Respawn();

		// Get all of the spawnpoints
		var spawnpoints = Entity.All.OfType<SpawnPoint>();

		// chose a random one
		var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		// if it exists, place the pawn there
		if ( randomSpawnPoint != null )
		{
			var tx = randomSpawnPoint.Transform;
			tx.Position = tx.Position + Vector3.Up * 10.0f; // raise it up
			pawn.Transform = tx;
		}

		Chat.AddChatEntry( To.Everyone, client.Name, "joined the game", client.SteamId, true );
	}

	public override void ClientDisconnect( IClient client, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( client, reason );
		Chat.AddChatEntry( To.Everyone, client.Name, "left the game", client.SteamId, true );
	}
}
