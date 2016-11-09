using System;

namespace JG.ParticleEngine.Initializers
{
	public class SpeeddByComponentsInitializer : IParticleInitializer
	{
		readonly float mMinSpeedX;
		readonly float mMaxSpeedX;
		readonly float mMinSpeedY;
		readonly float mMaxSpeedY;

		public SpeeddByComponentsInitializer(float speedMinX, float speedMaxX, float speedMinY, float speedMaxY) {
			mMinSpeedX = speedMinX;
			mMaxSpeedX = speedMaxX;
			mMinSpeedY = speedMinY;
			mMaxSpeedY = speedMaxY;
		}

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			p.SpeedX = (float)(r.NextDouble () * (mMaxSpeedX - mMinSpeedX) + mMinSpeedX);
			p.SpeedY = (float)(r.NextDouble () * (mMaxSpeedY - mMinSpeedY) + mMinSpeedY);
		}
	}
}
