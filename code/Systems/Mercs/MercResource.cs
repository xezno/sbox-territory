namespace Territory;

[GameResource( "Mercenary", "merc", "Defines a mercenary." )]
public class MercResource : GameResource
{
	public string Name { get; set; } = "Mercenary";
	public string Description { get; set; } = "Lorem Ipsum Dolor Sit Amet";
	public int Health { get; set; } = 100;
	public int BaseSpeed { get; set; } = 320;

	public MercType Type { get; set; } = MercType.Assault;

	public Prefab PrimaryWeapon { get; set; }
	public Prefab SecondaryWeapon { get; set; }
}
