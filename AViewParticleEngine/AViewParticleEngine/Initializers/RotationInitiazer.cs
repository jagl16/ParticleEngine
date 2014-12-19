using Java.Util;


namespace AViewParticleEngine
{
	public class RotationInitiazer : IParticleInitializer
	{
		private readonly int mMinAngle;
		private readonly int mMaxAngle;

		public RotationInitiazer(int minAngle, int maxAngle) {
			mMinAngle = minAngle;
			mMaxAngle = maxAngle;
		}
			
		public void InitParticle(Particle p, Random r) {
			int value = r.NextInt(mMaxAngle-mMinAngle)+mMinAngle;
			p.InitialRotation = value;
		}
	}
}

