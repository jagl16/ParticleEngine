using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace JG.ParticleEngine.Views
{
	[Register("jg.particleengine.views.ParticleField")]
	public class ParticleField : View
	{
		public ParticleField (Context context) :
			base (context)
		{

		}

		public ParticleField (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{

		}

		public ParticleField (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{

		}

		/// <summary>
		/// List of current particles the view emits.
		/// </summary>
		/// <value>The particles.</value>
		public List<Particle> Particles { get; private set; } = new List<Particle> ();
			
		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);
			Particles?.ForEach (p => p.Draw (canvas));
		}
	}
}
