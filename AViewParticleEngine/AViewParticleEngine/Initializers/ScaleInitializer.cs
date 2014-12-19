using Java.Util;

namespace AViewParticleEngine
{
	public class ScaleInitializer : IParticleInitializer
	{
		private readonly float mMaxScale;
		private readonly float mMinScale;

		public ScaleInitializer(float minScale, float maxScale) {
			mMinScale = minScale;
			mMaxScale = maxScale;
		}

		#region IParticleInitializer implementation
		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			float scale = r.NextFloat()*(mMaxScale-mMinScale) + mMinScale;
			p.Scale = scale;
		}
		#endregion
	}
}

