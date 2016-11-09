This library git:
https://github.com/jagl16/AViewParticleEngine


ParticleEngine
==========================

ParticleEngine is a particle system library that works with the standard Android UI on Xamarin.
This library was based in [Leonids Library](https://github.com/plattysoft/Leonids) , but modified for Xamarin and enchanced a bit.


## Setup
Just put the build package file into the libs folder of your Xamarin project.
Or add a reference to the project on your Xamarin solution.

## Why this library?

Particle systems are often used in games for a wide range of purposes: Explosions, fire, smoke, etc. This effects can also be used on normal apps to add an element of "juiciness" or Playful Design.

Precisely because its main use is games, all engines have support for particle systems, but there is no such thing for standard Android UI.

This means that if you are building an Android app and you want a particle system, you have to include a graphics engine and use OpenGL -which is quite an overkill- or you have to implement it yourself.

Leonids is made to fill this gap, bringing particle sytems to developers that use the standard Android UI.

## Basic usage

Creating and firing a one-shot particle system is very easy, just 3 lines of code.

```C#
new ParticleSystem(this, numParticles, drawableResId, timeToLive)
.SetSpeedRange(0.2f, 0.5f)
.OneShot(anchorView, numParticles);
```

When you create the particle system, you tell how many particles will it use as a maximum, the resourceId of the drawable you want to use for the particles and for how long the particles will live.

Then you configure the particle system. In this case we specify that the particles will have a speed between 0.2 and 0.5 pixels per milisecond (support for dips will be included in the future). Since we did not provide an angle range, it will be considered as "any angle".

Finally, we call oneShot, passing the view from which the particles will be launched and saying how many particles we want to be shot.

![](https://raw.githubusercontent.com/plattysoft/Leonids/master/images/Leonids_one_shot.gif)

## Emitters

You can configure emitters, which have a constant ratio of particles being emited per second.
This is the code for the Confeti example:

```C#
new ParticleSystem(this, 80, R.drawable.confeti2, 10000)
.SetSpeedModuleAndAngleRange(0f, 0.3f, 180, 180)
.SetRotationSpeed(144)
.SetAcceleration(0.00005f, 90)
.Emit(findViewById(R.id.emiter_top_right), 8);

new ParticleSystem(this, 80, R.drawable.confeti3, 10000)
.SetSpeedModuleAndAngleRange(0f, 0.3f, 0, 0)
.SetRotationSpeed(144)
.SetAcceleration(0.00005f, 90)
.Emit(findViewById(R.id.emiter_top_left), 8);
```

It uses an initializer for the Speed as module and angle ranges, a fixed speed rotaion and extenal acceleration.

![](https://raw.githubusercontent.com/plattysoft/Leonids/master/images/leonids_confeti.gif)

## Available Methods

List of the methods available on the class ParticleSystem.

### Constructors

All constructors use the activity, the maximum number of particles and the time to live. The difference is in how the image for the particles is specified. 

Supported drawables are: BitmapDrawable and AnimationDrawable.

* _ParticleSystem(Activity a, int maxParticles, int drawableRedId, long timeToLive)_
* _ParticleSystem(Activity a, int maxParticles, Drawable drawable, long timeToLive)_
* _ParticleSystem(Activity a, int maxParticles, Bitmap bitmap, long timeToLive)_
* _ParticleSystem(Activity a, int maxParticles, AnimationDrawable animation, long timeToLive)_

### Configuration

Available methods on the Particle system for configuration are:

* _SetSpeedRange(float speedMin, float speedMax)_: Uses 0-360 as the angle range
* _SetSpeedModuleAndAngleRange(float speedMin, float speedMax, int minAngle, int maxAngle)_
* _SetSpeedByComponentsRange(float speedMinX, float speedMaxX, float speedMinY, float speedMaxY)_
* _SetInitialRotationRange (int minAngle, int maxAngle)_
* _SetScaleRange(float minScale, float maxScale)_
* _SetRotationSpeed(float rotationSpeed)_
* _SetRotationSpeedRange(float minRotationSpeed, float maxRotationSpeed)_
* _SetAcceleration(float acceleration, float angle)_
* _SetFadeOut(long milisecondsBeforeEnd, Interpolator interpolator)_: Utility method for a simple fade out effect using an interpolator
* _SetFadeOut(long duration)_:Utility method for a simple fade out

For more complex modifiers, you can use the method _addModifier(ParticleModifier modifier)_. Available modifiers are:

* _AlphaModifier (int initialValue, int finalValue, long startMilis, long endMilis)_
* _AlphaModifier (int initialValue, int finalValue, long startMilis, long endMilis, Interpolator interpolator)_
* _ScaleModifier (float initialValue, float finalValue, long startMilis, long endMilis)_
* _ScaleModifier (float initialValue, float finalValue, long startMilis, long endMilis, Interpolator interpolator)_

### One shot

Make one shot using from the anchor view using the number of particles specified, an interpolator is optional

* _OneShot(View anchor, int numParticles)_
* _OneShot(View anchor, int numParticles, Interpolator interpolator)_

### Emitters

Emits the number of particles per second from the emitter. If emittingTime is set, the emitter stops after that time, otherwise it is continuous.

####Basic emitters
* _Emit (View emitter, int particlesPerSecond)_
* _Emit (View emitter, int particlesPerSecond, int emittingTime)_

####Emit based on (x,y) coordinates
* _Emit (int emitterX, int emitterY, int particlesPerSecond)_
* _Emit (int emitterX, int emitterY, int particlesPerSecond, int emitingTime)_

####Emit with Gravity 
* _EmitWithGravity (View emiter, int gravity, int particlesPerSecond)_
* _EmitWithGravity (View emiter, int gravity, int particlesPerSecond, int emitingTime)_
 
####Update, stop, and cancel
* _UpdateEmitPoint (int emitterX, int emitterY)_ Updates dynamically the point of emission.
* _StopEmitting ()_ Stops the emission of new particles, but the active ones are updated.
* _Cancel ()_ Stops the emission of new particles and cancles the active ones.

## Other details

AViewParticleEngine requires minSDK 11 because it uses ValueAnimators. It should be very easy, however to use nineoldandroids and make it work on Gingerbread.

Each Particle System only uses one image for the particles. If you want different particles to be emitted, you need to create a Particle System for each one of them.
