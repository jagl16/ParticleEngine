using System;

namespace JG.ParticleEngine.Initializers
{
	/// <summary>
	/// Particle initializer for scaling behavior.
	/// </summary>
	public class ScaleInitializer : IParticleInitializer
	{
		readonly float mMaxScale;
		readonly float mMinScale;

		public ScaleInitializer(float minScale, float maxScale) {
			mMinScale = minScale;
			mMaxScale = maxScale;
		}

		void IParticleInitializer.InitParticle (Particle p, Random r)
		{
			p.Scale = (float)(r.NextDouble () * (mMaxScale - mMinScale) + mMinScale);
		}
	}
}

