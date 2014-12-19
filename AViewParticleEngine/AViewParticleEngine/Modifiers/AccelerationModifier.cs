using Java.Lang;

namespace AViewParticleEngine
{
	public class AccelerationModifier : IParticleModifier
	{
		private readonly float mVelocityX;
		private readonly float mVelocityY;

		public AccelerationModifier(float velocity, float angle) {
			float velocityAngleInRads = (float) (angle*Math.Pi/180f);
			mVelocityX = (float) (velocity * Math.Cos(velocityAngleInRads));
			mVelocityY = (float) (velocity * Math.Sin(velocityAngleInRads));
		}

		public void Apply(Particle particle, long miliseconds) {
			particle.CurrentX += mVelocityX*miliseconds*miliseconds;
			particle.CurrentY += mVelocityY*miliseconds*miliseconds;
		}
	}
}

