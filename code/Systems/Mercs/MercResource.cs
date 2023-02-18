namespace Territory;

[GameResource( "Mercenary", "merc", "Defines a mercenary." )]
public class MercResource : GameResource
{
	public string Name { get; set; } = "Mercenary";
	public int Health { get; set; } = 100;
	public int BaseSpeed { get; set; } = 320;
}
