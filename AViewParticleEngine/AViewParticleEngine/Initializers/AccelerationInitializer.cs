using System;

namespace JG.ParticleEngine.Initializers
{
	public class AccelerationInitializer : IParticleInitializer
	{
		readonly float mMinValue;
		readonly float mMaxValue;
		readonly int mMinAngle;
		readonly int mMaxAngle;

		public AccelerationInitializer(float minAcceleration, float maxAcceleration, int minAngle, int maxAngle) {
			mMinValue = minAcceleration;
			mMaxValue = maxAcceleration;
			mMinAngle = minAngle;
			mMaxAngle = maxAngle;
		}

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			var angle = mMinAngle;
			if (mMaxAngle != mMinAngle) {
				angle = r.Next(mMaxAngle - mMinAngle) + mMinAngle;
			}
			var angleInRads = angle*Math.PI/180f;
			var value = (r.NextDouble () * (mMaxValue - mMinValue) + mMinValue);
			p.AccelerationX = (float) (value * Math.Cos(angleInRads));
			p.AccelerationY = (float) (value * Math.Sin(angleInRads));
		}

	}
}
