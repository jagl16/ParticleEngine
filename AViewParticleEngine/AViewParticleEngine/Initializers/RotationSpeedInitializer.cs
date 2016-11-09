
using System;

namespace JG.ParticleEngine.Initializers
{
	public class RotationSpeedInitializer : IParticleInitializer
	{
		readonly float mMinRotationSpeed;
		readonly float mMaxRotationSpeed;

		public RotationSpeedInitializer(float minRotationSpeed,	float maxRotationSpeed) {
			mMinRotationSpeed = minRotationSpeed;
			mMaxRotationSpeed = maxRotationSpeed;
		}

		#region IParticleInitializer implementation

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			p.RotationSpeed = (float)(r.NextDouble () * (mMaxRotationSpeed - mMinRotationSpeed) + mMinRotationSpeed);
		}

		#endregion
	}
}

