namespace Territory;

public partial class Mercenary : EntityComponent<Player>, ISingletonComponent
{
	[Net] public MercResource Resource { get; set; }

	public Mercenary()
	{
		Resource = ResourceLibrary.Get<MercResource>( "data/valkyrie.merc" );
	}

	public void Simulate( IClient cl )
	{
		//
	}

	public void SwitchMercenary( string mercPath )
	{
		Resource = ResourceLibrary.Get<MercResource>( mercPath );
	}
}
