using System;

namespace JG.ParticleEngine.Initializers
{
	public class RotationInitiazer : IParticleInitializer
	{
		readonly int mMinAngle;
		readonly int mMaxAngle;

		public RotationInitiazer(int minAngle, int maxAngle) {
			mMinAngle = minAngle;
			mMaxAngle = maxAngle;
		}
			
		public void InitParticle(Particle p, Random r) {
			p.InitialRotation = r.Next (mMaxAngle - mMinAngle) + mMinAngle; ;
		}
	}
}

