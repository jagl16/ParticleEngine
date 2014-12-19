using Java.Util;
using Java.Lang;

namespace AViewParticleEngine
{
	public class AccelerationInitializer : IParticleInitializer
	{
		private readonly float mMinValue;
		private readonly float mMaxValue;
		private readonly int mMinAngle;
		private readonly int mMaxAngle;

		public AccelerationInitializer(float minAcceleration, float maxAcceleration, int minAngle, int maxAngle) {
			mMinValue = minAcceleration;
			mMaxValue = maxAcceleration;
			mMinAngle = minAngle;
			mMaxAngle = maxAngle;
		}
		#region ParticleInitializer implementation

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			float angle = mMinAngle;
			if (mMaxAngle != mMinAngle) {
				angle = r.NextInt(mMaxAngle - mMinAngle) + mMinAngle;
			}
			float angleInRads = (float) (angle*Math.Pi/180f);
			float value = (r.NextFloat () * (mMaxValue - mMinValue) + mMinValue);
			p.AccelerationX = (float) (value * Math.Cos(angleInRads));
			p.AccelerationY = (float) (value * Math.Sin(angleInRads));
		}

		#endregion
	}
}

