using Android.Views.Animations;

namespace AViewParticleEngine
{
	public class ScaleModifier : IParticleModifier
	{

		private readonly float mInitialValue;
		private readonly float mFinalValue;
		private readonly long mEndTime;
		private readonly long mStartTime;
		private readonly long mDuration;
		private readonly float mValueIncrement;
		private readonly IInterpolator mInterpolator;

		public ScaleModifier (float initialValue, float finalValue, long startMilis, long endMilis, IInterpolator interpolator) {
			mInitialValue = initialValue;
			mFinalValue = finalValue;
			mStartTime = startMilis;
			mEndTime = endMilis;
			mDuration = mEndTime - mStartTime;
			mValueIncrement = mFinalValue-mInitialValue;
			mInterpolator = interpolator;
		}

		public ScaleModifier (float initialValue, float finalValue, long startMilis, long endMilis) : this (initialValue, finalValue, startMilis, endMilis, new LinearInterpolator()){
		}


		public void Apply(Particle particle, long miliseconds) {
			if (miliseconds < mStartTime) {
				particle.Scale = mInitialValue;
			}
			else if (miliseconds > mEndTime) {
				particle.Scale = mFinalValue;
			}
			else {
				float interpolaterdValue = mInterpolator.GetInterpolation((miliseconds- mStartTime)*1f/mDuration);
				float newScale = mInitialValue + mValueIncrement*interpolaterdValue;
				particle.Scale = newScale;
			}
		}
	}
}

