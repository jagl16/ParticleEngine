using System;

namespace JG.ParticleEngine.Initializers
{
	/// <summary>
	/// Particle modifier for angles and peed 
	/// </summary>
	public class SpeedModuleAndRangeInitializer : IParticleInitializer
	{
		readonly float mSpeedMin;
		readonly float mSpeedMax;
		readonly int mMinAngle;
		readonly int mMaxAngle;

		public SpeedModuleAndRangeInitializer(float speedMin, float speedMax, int minAngle, int maxAngle) {
			mSpeedMin = speedMin;
			mSpeedMax = speedMax;
			mMinAngle = minAngle;
			mMaxAngle = maxAngle;
			// Make sure the angles are in the [0-360) range
			while (mMinAngle < 0) {
				mMinAngle+=360;
			}
			while (mMaxAngle < 0) {
				mMaxAngle+=360;
			}
			// Also make sure that mMinAngle is the smaller
			if (mMinAngle > mMaxAngle) {
				int tmp = mMinAngle;
				mMinAngle = mMaxAngle;
				mMaxAngle = tmp;
			}
		}

		#region IParticleInitializer implementation

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			var speed = r.NextDouble()*(mSpeedMax-mSpeedMin) + mSpeedMin;
			int angle;
			if (mMaxAngle == mMinAngle) {
				angle = mMinAngle;
			}
			else {
				angle = r.Next(mMaxAngle - mMinAngle) + mMinAngle;
			}
			var angleInRads = angle*Math.PI/180f;
			p.SpeedX = (float) (speed * Math.Cos(angleInRads));
			p.SpeedY = (float) (speed * Math.Sin(angleInRads));
		}

		#endregion
	}
}

