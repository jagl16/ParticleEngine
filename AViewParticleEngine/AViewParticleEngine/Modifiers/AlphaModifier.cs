using Android.Views.Animations;

namespace AViewParticleEngine
{
	public class AlphaModifier : IParticleModifier
	{
		private int mInitialValue;
		private int mFinalValue;
		private long mStartTime;
		private long mEndTime;
		private float mDuration;
		private float mValueIncrement;
		private IInterpolator mInterpolator;

		public AlphaModifier(int initialValue, int finalValue, long startMilis, long endMilis, IInterpolator interpolator) {
			mInitialValue = initialValue;
			mFinalValue = finalValue;
			mStartTime = startMilis;		
			mEndTime = endMilis;
			mDuration = mEndTime - mStartTime;
			mValueIncrement = mFinalValue-mInitialValue;
			mInterpolator = interpolator;
		}

		public AlphaModifier (int initialValue, int finalValue, long startMilis, long endMilis) : this(initialValue, finalValue, startMilis, endMilis, new LinearInterpolator()){
		}


		public  void Apply(Particle particle, long miliseconds) {
			if (miliseconds < mStartTime) {
				particle.Alpha = mInitialValue;
			}
			else if (miliseconds > mEndTime) {
				particle.Alpha = mFinalValue;
			}
			else {	
				float interpolaterdValue = mInterpolator.GetInterpolation((miliseconds- mStartTime)*1f/mDuration);
				int newAlphaValue = (int) (mInitialValue + mValueIncrement*interpolaterdValue);
				particle.Alpha = newAlphaValue;
			}		
		}
	}
}

