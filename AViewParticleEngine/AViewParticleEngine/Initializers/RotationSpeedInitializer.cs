using Java.Util;


namespace JG.ParticleEngine.Initializers
{
	public class RotationSpeedInitializer : IParticleInitializer
	{
		private readonly float mMinRotationSpeed;
		private readonly float mMaxRotationSpeed;

		public RotationSpeedInitializer(float minRotationSpeed,	float maxRotationSpeed) {
			mMinRotationSpeed = minRotationSpeed;
			mMaxRotationSpeed = maxRotationSpeed;
		}

		#region IParticleInitializer implementation

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			float rotationSpeed = r.NextFloat()*(mMaxRotationSpeed-mMinRotationSpeed) + mMinRotationSpeed;
			p.RotationSpeed = rotationSpeed;
		}

		#endregion
	}
}

