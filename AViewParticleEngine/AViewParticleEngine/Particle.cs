using Android.Graphics;
using System.Collections.Generic;

namespace JG.ParticleEngine
{
	public class Particle
	{
		protected Bitmap mImage;

		public float CurrentX;
		public float CurrentY;

		public float Scale = 1f;
		public int Alpha = 255;

		public float InitialRotation = 0f;

		public float RotationSpeed = 0f;

		public float SpeedX = 0f;
		public float SpeedY = 0f;

		public float AccelerationX { set; get;}

		public float AccelerationY { set; get;}

		private Matrix mMatrix;
		private Paint mPaint;

		private float mInitialX;
		private float mInitialY;

		private float mRotation;

		private long mTimeToLive;

		protected long mStartingMilisecond;

		private int mBitmapHalfWidth;
		private int mBitmapHalfHeight;

		private IParticleModifier[]  mModifiers;

		protected Particle() {		
			mMatrix = new Matrix();
			mPaint = new Paint();
		}

		public Particle (Bitmap bitmap) :this(){
			mImage = bitmap;
		}

		public void Init() {
			Scale = 1;
			Alpha = 255;	
		}

		public void Configure(long timeToLive, float emiterX, float emiterY) {
			mBitmapHalfWidth = mImage.Width/2;
			mBitmapHalfHeight = mImage.Height/2;

			mInitialX = emiterX - mBitmapHalfWidth;
			mInitialY = emiterY - mBitmapHalfHeight;
			CurrentX = mInitialX;
			CurrentY = mInitialY;

			mTimeToLive = timeToLive;
		}

		public virtual bool Update (long miliseconds) {
			long realMiliseconds = miliseconds - mStartingMilisecond;
			if (realMiliseconds > mTimeToLive) {
				return false;
			}
			CurrentX = mInitialX+SpeedX*realMiliseconds+AccelerationX*realMiliseconds*realMiliseconds;
			CurrentY = mInitialY+SpeedY*realMiliseconds+AccelerationY*realMiliseconds*realMiliseconds;
			mRotation = InitialRotation + RotationSpeed*realMiliseconds/1000;
			for (int i=0; i<mModifiers.Length; i++) {
				mModifiers[i].Apply(this, realMiliseconds);
			}
			return true;
		}

		public void Draw (Canvas c) {
			mMatrix.Reset();
			mMatrix.PostRotate(mRotation, mBitmapHalfWidth, mBitmapHalfHeight);
			mMatrix.PostScale(Scale, Scale, mBitmapHalfWidth, mBitmapHalfHeight);
			mMatrix.PostTranslate(CurrentX, CurrentY);
			mPaint.Alpha = Alpha;		
			c.DrawBitmap(mImage, mMatrix, mPaint);
		}

		public Particle Activate(long startingMilisecond, List<IParticleModifier> modifiers) {
			mStartingMilisecond = startingMilisecond;
			// We do store a reference to the list, there is no need to copy, since the modifiers do not carte about states 
			mModifiers = modifiers.ToArray ();
			return this;
		}
	}
}

