using Android.Graphics.Drawables;
using Android.Runtime;

namespace JG.ParticleEngine.Views
{
	[Register("jg.particleengine.views.AnimatedParticle")]
	public class AnimatedParticle : Particle
	{
		readonly AnimationDrawable mAnimationDrawable;
		readonly int mTotalTime;

		public AnimatedParticle(AnimationDrawable animationDrawable) {
			mAnimationDrawable = animationDrawable;
			mImage = ((BitmapDrawable) mAnimationDrawable.GetFrame(0)).Bitmap;
			// If it is a repeating animation, calculate the time
			mTotalTime = 0;
			for (int i=0; i<mAnimationDrawable.NumberOfFrames; i++) {
				mTotalTime += mAnimationDrawable.GetDuration(i);
			}
		}

		public override bool Update(long miliseconds) {
			bool active = base.Update(miliseconds);
			if (active) {
				long animationElapsedTime = 0;
				long realMiliseconds = miliseconds - mStartingMilisecond;
				if (realMiliseconds > mTotalTime) {
					if (mAnimationDrawable.OneShot) {
						return false;
					}
					realMiliseconds = realMiliseconds % mTotalTime;
				}
				for (int i=0; i<mAnimationDrawable.NumberOfFrames; i++) {
					animationElapsedTime += mAnimationDrawable.GetDuration(i);
					if (animationElapsedTime > realMiliseconds) {
						mImage = ((BitmapDrawable) mAnimationDrawable.GetFrame(i)).Bitmap;
						break;
					}
				}
			}
			return active;
		}

	}
}
