using Java.Util;
using Java.Lang;


namespace JG.ParticleEngine.Initializers
{
	public class SpeeddModuleAndRangeInitializer : IParticleInitializer
	{
		private float mSpeedMin;
		private float mSpeedMax;
		private int mMinAngle;
		private int mMaxAngle;

		public SpeeddModuleAndRangeInitializer(float speedMin, float speedMax, int minAngle, int maxAngle) {
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
			float speed =  r.NextFloat()*(mSpeedMax-mSpeedMin) + mSpeedMin;
			int angle;
			if (mMaxAngle == mMinAngle) {
				angle = mMinAngle;
			}
			else {
				angle = r.NextInt(mMaxAngle - mMinAngle) + mMinAngle;
			}
			float angleInRads = (float) (angle*Math.Pi/180f);
			p.SpeedX = (float) (speed * Math.Cos(angleInRads));
			p.SpeedY = (float) (speed * Math.Sin(angleInRads));
		}

		#endregion
	}
}

