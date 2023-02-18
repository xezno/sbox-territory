using Territory.Mechanics;

namespace Territory;

public partial class PlayerCamera : EntityComponent<Player>, ISingletonComponent
{
	private float _interpolatedFovMultiplier;

	public virtual void Update( Player player )
	{
		float fieldOfView = EvaluateFieldOfView( player );

		Camera.Position = player.EyePosition;
		Camera.Rotation = player.EyeRotation;
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( fieldOfView );
		Camera.FirstPersonViewer = player;
		Camera.ZNear = 0.5f;

		// Post Processing
		var pp = Camera.Main.FindOrCreateHook<Sandbox.Effects.ScreenEffects>();
		pp.Sharpen = 0.05f;
		pp.Vignette.Intensity = 0.60f;
		pp.Vignette.Roundness = 1f;
		pp.Vignette.Smoothness = 0.3f;
		pp.Vignette.Color = Color.Black.WithAlpha( 1f );
		pp.MotionBlur.Scale = 0f;
		pp.Saturation = 1f;
		pp.FilmGrain.Response = 1f;
		pp.FilmGrain.Intensity = 0.01f;
		pp.ChromaticAberration.Scale = 0.025f;
	}

	/// <summary>
	/// Calculate a field of view based on the player's current state.
	/// This will multiply against the field of view specified in the user's preferences.
	/// </summary>
	private float EvaluateFieldOfView( Player player )
	{
		float fovMultiplier = 1.0f;

		if ( player.Controller.IsMechanicActive<SprintMechanic>() )
			fovMultiplier = 1.2f;

		// Interpolate so that FOV transitions smoothly
		_interpolatedFovMultiplier = _interpolatedFovMultiplier.LerpTo( fovMultiplier, 10f * Time.Delta );

		float fieldOfView = Game.Preferences.FieldOfView * _interpolatedFovMultiplier;
		return fieldOfView;
	}
}
