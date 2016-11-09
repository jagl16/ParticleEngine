using Android.Content;
using Android.Util;
using Android.Views;
using System.Collections.Generic;

namespace JG.ParticleEngine
{
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

		public List<Particle> Particles {
			set;
			get;
		}
			

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);
			Particles?.ForEach (p => p.Draw (canvas));
		}
	}
}
