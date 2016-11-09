
namespace JG.ParticleEngine.Modifiers
{
	public interface IParticleModifier
	{
		/// <summary>
		/// Modifies the specific value of a particle given the current millis.
		/// </summary>
		/// <param name="particle">Particle.</param>
		/// <param name="millis">Millis.</param>
		void Apply(Particle particle, long millis);
	}
}

