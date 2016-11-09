using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using JG.ParticleEngine.Initializers;
using JG.ParticleEngine.Modifiers;
using JG.ParticleEngine.Views;

namespace JG.ParticleEngine
{
	public class ParticleSystem
	{
		public static long TIMERTASK_INTERVAL = 50;
		ViewGroup mRootView;
		int mMaxParticles;
		Random mRandom;

		ParticleField mDrawingView;

		List<Particle> mParticles;
		List<Particle> mActiveParticles;
		long mTimeToLive;
		long mCurrentTime = 0;
		public long CurrentTime {
			set {
				mCurrentTime = value;
			}
			get {
				return mCurrentTime;
			}
		}


		public Action AnimationStarted;

		float mParticlesPerMilisecond;
		int mActivatedParticles;
		long mEmitingTime;

		List<IParticleModifier> mModifiers;
		List<IParticleInitializer> mInitializers;
		ValueAnimator mAnimator;
		Handler mHandler;

		float mDpToPxScale;
		int[] mParentLocation;

		int mEmiterXMin;
		int mEmiterXMax;
		int mEmiterYMin;

		int mEmiterYMax;

		public bool LoopAnimation {get;set;}

		ParticleSystem(ViewGroup view, int maxParticles, long timeToLive) {
			mRandom = new Random();
			mRootView = view;

			mModifiers = new List<IParticleModifier>();
			mInitializers = new List<IParticleInitializer>();

			mMaxParticles = maxParticles;
			// Create the particles
			mActiveParticles = new List<Particle>(); 
			mParticles = new List<Particle> ();
			mTimeToLive = timeToLive;

			mParentLocation = new int[2];		
			mRootView.GetLocationInWindow(mParentLocation);
		}
			
		/// <summary>
		/// Initializes a new instance of the <see cref="ParticleSystem"/> class.
		/// </summary>
		/// <param name="rootView">The parent activity.</param>
		/// <param name="maxParticles">Max particles.</param>
		/// <param name="drawableRedId">The drawable resource to use as particle (supports Bitmaps and Animations)</param>
		/// <param name="timeToLive">Time to live.</param>
		public ParticleSystem(ViewGroup rootView, int maxParticles, int drawableRedId, long timeToLive) : this(rootView, maxParticles, ContextCompat.GetDrawable(rootView.Context,drawableRedId), timeToLive) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AViewParticleEngine.ParticleSystem"/> class with a Drawable.
		/// </summary>
		/// <param name="a">The parent activity.</param>
		/// <param name="maxParticles">Max particles.</param>
		/// <param name="drawable">Drawable.</param>
		/// <param name="timeToLive">Time to live.</param>
		public ParticleSystem(ViewGroup a, int maxParticles, Drawable drawable, long timeToLive) :this(a, maxParticles, timeToLive) {
			var bitmapDrawable = drawable as BitmapDrawable;
			if (bitmapDrawable != null) {
				Bitmap bitmap = bitmapDrawable.Bitmap;
				for (int i = 0; i < mMaxParticles; i++) {
					mParticles.Add (new Particle (bitmap));
				}
			} else {
				var animationDrawable = drawable as AnimationDrawable;
				if (animationDrawable != null) {
					AnimationDrawable animation = animationDrawable;
					for (int i = 0; i < mMaxParticles; i++) {
						mParticles.Add (new AnimatedParticle (animation));
					}
				} 
				// Not supported, no particles are being created
			}
			var displayMetrics = a.Resources.DisplayMetrics;
			mDpToPxScale = (displayMetrics.Xdpi / (int) DisplayMetricsDensity.Default);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AViewParticleEngine.ParticleSystem"/> class with a Bitmap.
		/// </summary>
		/// <param name="a">The parent activity.</param>
		/// <param name="maxParticles">Max particles.</param>
		/// <param name="bitmap">Bitmap.</param>
		/// <param name="timeToLive">Time to live.</param>
		public ParticleSystem(ViewGroup rootView, int maxParticles, Bitmap bitmap, long timeToLive) : this(rootView, maxParticles, timeToLive){		
			for (int i=0; i<mMaxParticles; i++) {
				mParticles.Add (new Particle (bitmap));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AViewParticleEngine.ParticleSystem"/> class with a AnimationDrawable.
		/// </summary>
		/// <param name="a">The parent activity.</param>
		/// <param name="maxParticles">Max particles.</param>
		/// <param name="animation">AnimationDrawable to display.</param>
		/// <param name="timeToLive">Time to live.</param>
		public ParticleSystem(ViewGroup rootView, int maxParticles, AnimationDrawable animation, long timeToLive) : this(rootView, maxParticles, timeToLive){
			// Create the particles
			for (int i=0; i<mMaxParticles; i++) {
				mParticles.Add (new AnimatedParticle (animation));
			}
		}

		public float DpToPx(float dp) {
			return dp * mDpToPxScale;
		}

		public ParticleSystem AddModifier(IParticleModifier modifier) {
			mModifiers.Add(modifier);
			return this;
		}

		public ParticleSystem SetSpeedRange(float speedMin, float speedMax) { 
			mInitializers.Add(new SpeedModuleAndRangeInitializer(DpToPx(speedMin), DpToPx(speedMax), 0, 360));		
			return this;
		}

		public ParticleSystem SetSpeedModuleAndAngleRange(float speedMin, float speedMax, int minAngle, int maxAngle) {
			mInitializers.Add(new SpeedModuleAndRangeInitializer(DpToPx(speedMin), DpToPx(speedMax), minAngle, maxAngle));		
			return this;
		}

		public ParticleSystem SetSpeedByComponentsRange(float speedMinX, float speedMaxX, float speedMinY, float speedMaxY) {
			mInitializers.Add(new SpeeddByComponentsInitializer(DpToPx(speedMinX), DpToPx(speedMaxX), 
				DpToPx(speedMinY), DpToPx(speedMaxY)));		
			return this;
		}

		public ParticleSystem SetInitialRotationRange (int minAngle, int maxAngle) {
			mInitializers.Add(new RotationInitiazer(minAngle, maxAngle));
			return this;
		}

		public ParticleSystem SetScaleRange(float minScale, float maxScale) {
			mInitializers.Add(new ScaleInitializer(minScale, maxScale));
			return this;
		}

		public ParticleSystem SetRotationSpeed(float rotationSpeed) {
			mInitializers.Add(new RotationSpeedInitializer(rotationSpeed, rotationSpeed));
			return this;
		}

		public ParticleSystem SetRotationSpeedRange(float minRotationSpeed, float maxRotationSpeed) {
			mInitializers.Add(new RotationSpeedInitializer(minRotationSpeed, maxRotationSpeed));
			return this;
		}

		public ParticleSystem SetAccelerationModuleAndAndAngleRange(float minAcceleration, float maxAcceleration, int minAngle, int maxAngle) {
			mInitializers.Add(new AccelerationInitializer(DpToPx(minAcceleration), DpToPx(maxAcceleration), 
				minAngle, maxAngle));
			return this;
		}

		public ParticleSystem SetAcceleration(float acceleration, int angle) {
			mInitializers.Add(new AccelerationInitializer(acceleration, acceleration, angle, angle));
			return this;
		}

		public ParticleSystem SetParentViewGroup(ViewGroup viewGroup) {
			mRootView = viewGroup;
			return this;
		}

		public ParticleSystem SetStartTime(int time) {
			mCurrentTime = time;
			return this;
		}
			
		/// <summary>
		/// Configures a fade out for the particles when they disappear
		/// </summary>
		/// <param name="milisecondsBeforeEnd">duration fade out duration in miliseconds.</param>
		/// <param name="interpolator">the interpolator for the fade out (default is linear).</param>
		public ParticleSystem SetFadeOut(long milisecondsBeforeEnd, IInterpolator interpolator) {
			mModifiers.Add(new AlphaModifier(255, 0, mTimeToLive-milisecondsBeforeEnd, mTimeToLive, interpolator));
			return this;
		}

		/// <summary>
		/// Configures a fade out for the particles when they disappear
		/// </summary>
		/// <param name="duration">fade out duration in miliseconds</param>
		public ParticleSystem SetFadeOut(long duration) {
			return SetFadeOut(duration, new LinearInterpolator());
		}


		/// <summary>
		/// Starts emiting particles from a specific view. If at some point the number goes over the amount of particles availabe on create
		/// no new particles will be created
		/// </summary>
		/// <param name="emiter">View from which center the particles will be emited.</param>
		/// <param name="gravity">Which position among the view the emission takes place.</param>
		/// <param name="particlesPerSecond">Number of particles per second that will be emited (evenly distributed).</param>
		/// <param name="emitingTime">time the emiter will be emiting particles.</param>
		public void EmitWithGravity (View emiter, GravityFlags gravity, int particlesPerSecond, int emitingTime) {
			// Setup emiter
			ConfigureEmiter(emiter, gravity);
			StartEmiting(particlesPerSecond, emitingTime);
		}


		/// <summary>
		/// Starts emiting particles from a specific view. If at some point the number goes over the amount of particles availabe on create
		/// no new particles will be created
		/// </summary>
		/// <param name="emiter">View from which center the particles will be emited</param>
		/// <param name="gravity">Which position among the view the emission takes place.</param>
		/// <param name="particlesPerSecond">Number of particles per second that will be emited (evenly distributed)</param>
		public void EmitWithGravity (View emiter, GravityFlags gravity, int particlesPerSecond) {
			// Setup emiter
			ConfigureEmiter(emiter, gravity);
			StartEmiting(particlesPerSecond);
		}


		/// <summary>
		/// Starts emiting particles from a specific view. If at some point the number goes over the amount of particles availabe on create
		/// no new particles will be created
		/// </summary>
		/// <param name="emiter">View from which center the particles will be emited.</param>
		/// <param name="particlesPerSecond">Particles per second.</param>
		/// <param name="emitingTime">Emiting time.</param>
		public void Emit (View emiter, int particlesPerSecond, int emitingTime) {
			EmitWithGravity(emiter, GravityFlags.Center, particlesPerSecond, emitingTime);
		}
			
		public void Emit (View emiter, int particlesPerSecond) {
			// Setup emiter
			EmitWithGravity(emiter, GravityFlags.Center, particlesPerSecond);
		}



		void StartEmiting(int particlesPerSecond) {
			mActivatedParticles = 0;
			mParticlesPerMilisecond = particlesPerSecond/1000f;
			// Add a full size view to the parent view		
			mDrawingView = new ParticleField(mRootView.Context);
			mRootView.AddView(mDrawingView);
			mEmitingTime = -1; // Meaning infinite
			mDrawingView.Particles = mActiveParticles;
			UpdateParticlesBeforeStartTime(particlesPerSecond);
			//mTimer = new Timer();
			mHandler = new Handler ();
			mHandler.PostDelayed (() => {
				OnUpdate(CurrentTime);
				CurrentTime += TIMERTASK_INTERVAL;
			}, TIMERTASK_INTERVAL);
		}

		public void Emit (int emitterX, int emitterY, int particlesPerSecond, int emitingTime) {
			ConfigureEmiter(emitterX, emitterY);
			StartEmiting(particlesPerSecond, emitingTime);
		}	
			
		private void StartEmiting(int particlesPerSecond, int emitingTime) {
			mActivatedParticles = 0;
			mParticlesPerMilisecond = particlesPerSecond/1000f;
			// Add a full size view to the parent view		
			mDrawingView = new ParticleField(mRootView.Context);
			mRootView.AddView(mDrawingView);

			mDrawingView.Particles =  mActiveParticles;
			UpdateParticlesBeforeStartTime(particlesPerSecond);
			mEmitingTime = emitingTime;
			StartAnimator(new LinearInterpolator(), emitingTime+mTimeToLive);
		}

		public void Emit (int emitterX, int emitterY, int particlesPerSecond) {
			ConfigureEmiter(emitterX, emitterY);
			StartEmiting(particlesPerSecond);
		}


		public void UpdateEmitPoint (int emitterX, int emitterY) {
			ConfigureEmiter(emitterX, emitterY);
		}

		public void OneShot(View emiter, int numParticles) {
			OneShot(emiter, numParticles, new LinearInterpolator());
		}

		public void OneShot(View emiter, int numParticles, IInterpolator interpolator) {
			ConfigureEmiter(emiter, GravityFlags.Center);
			mActivatedParticles = 0;
			mEmitingTime = mTimeToLive;
			// We create particles based in the parameters
			for (int i=0; i<numParticles && i<mMaxParticles; i++) {
				ActivateParticle(0);
			}
			// Add a full size view to the parent view		
			mDrawingView = new ParticleField(mRootView.Context);
			mRootView.AddView(mDrawingView);
			mDrawingView.Particles = mActiveParticles;
			// We start a property animator that will call us to do the update
			// Animate from 0 to timeToLiveMax
			StartAnimator(interpolator, mTimeToLive);
		}

		void ConfigureEmiter(int emitterX, int emitterY) {
			// We configure the emiter based on the window location to fix the offset of action bar if present		
			mEmiterXMin = emitterX - mParentLocation[0];
			mEmiterXMax = mEmiterXMin;
			mEmiterYMin = emitterY - mParentLocation[1];
			mEmiterYMax = mEmiterYMin;
		}

		private void ConfigureEmiter(View emiter, GravityFlags gravity) {
			// It works with an emision range
			var location = new int[2];
			emiter.GetLocationInWindow(location);

			// Check horizontal gravity and set range
			if (HasGravity(gravity, GravityFlags.Left)) {
				mEmiterXMin = location[0] - mParentLocation[0];
				mEmiterXMax = mEmiterXMin;
			}
			else if (HasGravity(gravity, GravityFlags.Right)) {
				mEmiterXMin = location[0] + emiter.Width - mParentLocation[0];
				mEmiterXMax = mEmiterXMin;
			}
			else if (HasGravity(gravity, GravityFlags.CenterHorizontal)){
				mEmiterXMin = location[0] + emiter.Width/2 - mParentLocation[0];
				mEmiterXMax = mEmiterXMin;
			}
			else {
				// All the range
				mEmiterXMin = location[0] - mParentLocation[0];
				mEmiterXMax = location[0] + emiter.Width - mParentLocation[0];
			}

			// Now, vertical gravity and range
			if (HasGravity(gravity, GravityFlags.Top)) {
				mEmiterYMin = location[1] - mParentLocation[1];
				mEmiterYMax = mEmiterYMin;
			}
			else if (HasGravity(gravity, GravityFlags.Bottom)) {
				mEmiterYMin = location[1] + emiter.Height - mParentLocation[1];
				mEmiterYMax = mEmiterYMin;
			}
			else if (HasGravity(gravity, GravityFlags.CenterVertical)){
				mEmiterYMin = location[1] + emiter.Height/2 - mParentLocation[1];
				mEmiterYMax = mEmiterYMin;
			}
			else {
				// All the range
				mEmiterYMin = location[1] - mParentLocation[1];
				mEmiterYMax = location[1] + emiter.Width - mParentLocation[1];
			}
		}

		static bool HasGravity(GravityFlags gravity, GravityFlags gravityToCheck) {
			return (gravity & gravityToCheck) == gravityToCheck;
		}

		void ActivateParticle(long delay) {
			Particle p = mParticles.ElementAt (0);
			mParticles.RemoveAt(0);
			p.Init();
			// Initialization goes before configuration, scale is required before can be configured properly
			mInitializers.ForEach(particle => particle.InitParticle (p, mRandom));

			int particleX = GetFromRange (mEmiterXMin, mEmiterXMax);
			int particleY = GetFromRange (mEmiterYMin, mEmiterYMax);
			p.Configure(mTimeToLive, particleX, particleY);
			p.Activate(delay, mModifiers);
			mActiveParticles.Add(p);
			mActivatedParticles++;
		}



		int GetFromRange(int minValue, int maxValue) {
			if (minValue == maxValue) {
				return minValue;
			}
			return mRandom.Next(maxValue-minValue) + minValue;
		}

		public void OnUpdate(long miliseconds) {
			while (((mEmitingTime > 0 && miliseconds < mEmitingTime)|| mEmitingTime == -1) && // This point should emit
				mParticles.Count > 0 && // We have particles in the pool 
				mActivatedParticles < mParticlesPerMilisecond*miliseconds) { // and we are under the number of particles that should be launched
				// Activate a new particle
				ActivateParticle(miliseconds);			
			}
			for (int i=0; i<mActiveParticles.Count; i++) {
				bool active = mActiveParticles.ElementAt(i).Update(miliseconds);
				if (!active) {
					Particle p = mActiveParticles.ElementAt (i);
					mActiveParticles.RemoveAt(i);
					i--; // Needed to keep the index at the right position
					mParticles.Add(p);
				}
			}
			mDrawingView.PostInvalidate();
		}


		void CleanupAnimation() {
			mRootView.RemoveView(mDrawingView);
			mDrawingView = null;
			mRootView.PostInvalidate();
			mParticles.AddRange(mActiveParticles);
		}

		public void StopEmitting () {
			// The time to be emiting is the current time (as if it was a time-limited emiter
			mEmitingTime = mCurrentTime;
		}


		/// <summary>
		/// Cancels the particle system and all the animations.
		/// To stop emitting but animate until the end, use stopEmitting instead.
		/// </summary>
		public void Cancel() {
			if (mAnimator != null && mAnimator.IsRunning) {
				mAnimator.Cancel();
			}
			if (mHandler != null) {
				mHandler.Dispose ();
				CleanupAnimation();
			}
		}

		void UpdateParticlesBeforeStartTime(int particlesPerSecond) {
			if (particlesPerSecond == 0) {
				return;
			}
			long currentTimeInMs = mCurrentTime / 1000;
			long framesCount = currentTimeInMs / particlesPerSecond;
			if (framesCount == 0) {
				return;
			}
			long frameTimeInMs = mCurrentTime / framesCount;
			for (int i = 1; i <= framesCount; i++) {
				OnUpdate(frameTimeInMs * i + 1);
			}
		}

		void StartAnimator(IInterpolator interpolator, long animnationTime) {
			mAnimator = ValueAnimator.OfInt(new int[] {0, (int) animnationTime});
			mAnimator.SetDuration(animnationTime);
			mAnimator.Update+=((sender, e) => {
				int miliseconds = (int)(e.Animation.AnimatedValue);
				OnUpdate (miliseconds);
			});

			mAnimator.AnimationCancel+= (sender, e) => CleanupAnimation ();
			mAnimator.AnimationEnd += (sender, e) => CleanupAnimation ();
			mAnimator.AnimationStart+=((sender, e) => { AnimationStarted.Invoke(); });
			mAnimator.SetInterpolator(interpolator);
			mAnimator.Start();
		}
			
	}
}

