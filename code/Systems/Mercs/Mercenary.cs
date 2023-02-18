namespace Territory;

public class Mercenary : EntityComponent<Player>, ISingletonComponent
{
	public MercResource Resource { get; set; }

	public Mercenary()
	{
		Resource = ResourceLibrary.Get<MercResource>( "data/valkyrie.merc" );
	}

	public void Simulate( IClient cl )
	{
		int line = 10;
		if ( Game.IsClient )
			line += 10;

		var realm = (Game.IsClient) ? "CL" : "SV";

		DebugOverlay.ScreenText( $"[{realm}] Current merc: {Resource.Name}", line );
	}
}
