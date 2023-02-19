namespace Territory;

public enum MercType
{
	Medic,
	Assault,
	Ranged,
	Engineer,
	FireSupport
}

public static class MercTypeExtensions
{
	public static string ToDisplayString( this MercType mercType )
	{
		var original = mercType.ToString();
		var result = "";

		for ( int i = 0; i < original.Length; ++i )
		{
			char c = original[i];
			if ( i != 0 && char.IsUpper( c ) )
				result += " ";

			result += c;
		}

		return result;
	}

	public static string ToIcon( this MercType mercType )
	{
		// Returns a material icon name for this merc
		switch ( mercType )
		{
			case MercType.Medic:
				return "medication";
			case MercType.Assault:
				return "swap_vert";
			case MercType.Ranged:
				return "data_saver_on";
			case MercType.Engineer:
				return "construction";
			case MercType.FireSupport:
				return "local_fire_department";
		}

		return "warning";
	}
}
