using Java.Util;


namespace AViewParticleEngine
{
	public class SpeeddByComponentsInitializer : IParticleInitializer
	{
		private readonly float mMinSpeedX;
		private readonly float mMaxSpeedX;
		private readonly float mMinSpeedY;
		private readonly float mMaxSpeedY;

		public SpeeddByComponentsInitializer(float speedMinX, float speedMaxX, float speedMinY, float speedMaxY) {
			mMinSpeedX = speedMinX;
			mMaxSpeedX = speedMaxX;
			mMinSpeedY = speedMinY;
			mMaxSpeedY = speedMaxY;
		}

		#region IParticleInitializer implementation

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			p.SpeedX =  r.NextFloat()*(mMaxSpeedX-mMinSpeedX)+mMinSpeedX;
			p.SpeedY = r.NextFloat()*(mMaxSpeedY-mMinSpeedY)+mMinSpeedY;
		}

		#endregion
	}
}

