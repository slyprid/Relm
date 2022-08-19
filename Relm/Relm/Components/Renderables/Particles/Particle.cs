using Microsoft.Xna.Framework;
using Relm.Core;
using Relm.Extensions;
using Relm.Math;
using Relm.Physics.Shapes;

namespace Relm.Components.Renderables.Particles
{
	public class Particle
	{
		static Circle _circleCollisionShape = new Circle(0);

		internal Vector2 position;
		internal Vector2 spawnPosition;
		Vector2 _direction;

		internal Color color;

		Color _startColor;

		Color _finishColor;
		internal float rotation;
		float _rotationDelta;
		float _radialAcceleration;
		float _tangentialAcceleration;
		float _radius;
		float _radiusDelta;
		float _angle;
		float _degreesPerSecond;
		internal float particleSize;
		float _particleSizeDelta;

		float _timeToLive;

		float _particleLifetime;

		bool _collided;

		Vector2 _velocity;


		public void Initialize(ParticleEmitterConfig emitterConfig, Vector2 spawnPosition)
		{
			_collided = false;

			position.X = emitterConfig.SourcePositionVariance.X * Random.MinusOneToOne();
			position.Y = emitterConfig.SourcePositionVariance.Y * Random.MinusOneToOne();

			this.spawnPosition = spawnPosition;

			var newAngle =
				MathHelper.ToRadians(emitterConfig.Angle + emitterConfig.AngleVariance * Random.MinusOneToOne());

			var vector = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));

			var vectorSpeed = emitterConfig.Speed + emitterConfig.SpeedVariance * Random.MinusOneToOne();

			_direction = vector * vectorSpeed;

			_timeToLive = MathHelper.Max(0,
				emitterConfig.ParticleLifespan + emitterConfig.ParticleLifespanVariance * Random.MinusOneToOne());
			_particleLifetime = _timeToLive;

			var startRadius = emitterConfig.MaxRadius + emitterConfig.MaxRadiusVariance * Random.MinusOneToOne();
			var endRadius = emitterConfig.MinRadius + emitterConfig.MinRadiusVariance * Random.MinusOneToOne();

			_radius = startRadius;
			_radiusDelta = (endRadius - startRadius) / _timeToLive;
			_angle = MathHelper.ToRadians(emitterConfig.Angle + emitterConfig.AngleVariance * Random.MinusOneToOne());
			_degreesPerSecond = MathHelper.ToRadians(emitterConfig.RotatePerSecond +
													 emitterConfig.RotatePerSecondVariance * Random.MinusOneToOne());

			_radialAcceleration = emitterConfig.RadialAcceleration +
								  emitterConfig.RadialAccelVariance * Random.MinusOneToOne();
			_tangentialAcceleration = emitterConfig.TangentialAcceleration +
									  emitterConfig.TangentialAccelVariance * Random.MinusOneToOne();

			var particleStartSize = emitterConfig.StartParticleSize +
									emitterConfig.StartParticleSizeVariance * Random.MinusOneToOne();
			var particleFinishSize = emitterConfig.FinishParticleSize +
									 emitterConfig.FinishParticleSizeVariance * Random.MinusOneToOne();
			_particleSizeDelta = (particleFinishSize - particleStartSize) / _timeToLive;
			particleSize = MathHelper.Max(0, particleStartSize);


			_startColor = new Color
			(
				(int)(emitterConfig.StartColor.R + emitterConfig.StartColorVariance.R * Random.MinusOneToOne()),
				(int)(emitterConfig.StartColor.G + emitterConfig.StartColorVariance.G * Random.MinusOneToOne()),
				(int)(emitterConfig.StartColor.B + emitterConfig.StartColorVariance.B * Random.MinusOneToOne()),
				(int)(emitterConfig.StartColor.A + emitterConfig.StartColorVariance.A * Random.MinusOneToOne())
			);
			color = _startColor;

			_finishColor = new Color
			(
				(int)(emitterConfig.FinishColor.R + emitterConfig.FinishColorVariance.R * Random.MinusOneToOne()),
				(int)(emitterConfig.FinishColor.G + emitterConfig.FinishColorVariance.G * Random.MinusOneToOne()),
				(int)(emitterConfig.FinishColor.B + emitterConfig.FinishColorVariance.B * Random.MinusOneToOne()),
				(int)(emitterConfig.FinishColor.A + emitterConfig.FinishColorVariance.A * Random.MinusOneToOne())
			);

			var startA = MathHelper.ToRadians(emitterConfig.RotationStart +
											  emitterConfig.RotationStartVariance * Random.MinusOneToOne());
			var endA = MathHelper.ToRadians(emitterConfig.RotationEnd +
											emitterConfig.RotationEndVariance * Random.MinusOneToOne());
			rotation = startA;
			_rotationDelta = (endA - startA) / _timeToLive;
		}


		public bool Update(ParticleEmitterConfig emitterConfig, ref ParticleCollisionConfig collisionConfig,
						   Vector2 rootPosition)
		{
			_timeToLive -= Time.DeltaTime;

			if (_timeToLive > 0)
			{
				if (!_collided)
				{
					if (emitterConfig.EmitterType == ParticleEmitterType.Radial)
					{
						_angle += _degreesPerSecond * Time.DeltaTime;
						_radius += _radiusDelta * Time.DeltaTime;

						Vector2 tmp;
						tmp.X = -Mathf.Cos(_angle) * _radius;
						tmp.Y = -Mathf.Sin(_angle) * _radius;

						_velocity = tmp - position;
						position = tmp;
					}
					else
					{
						Vector2 tmp, radial, tangential;
						radial = Vector2.Zero;

						if (position.X != 0 || position.Y != 0)
							Vector2.Normalize(ref position, out radial);

						tangential = radial;
						radial = radial * _radialAcceleration;

						var newy = tangential.X;
						tangential.X = -tangential.Y;
						tangential.Y = newy;
						tangential = tangential * _tangentialAcceleration;

						tmp = radial + tangential + emitterConfig.Gravity;
						tmp = tmp * Time.DeltaTime;
						_direction = _direction + tmp;
						tmp = _direction * Time.DeltaTime;

						_velocity = tmp / Time.DeltaTime;
						position = position + tmp;
					}
				}

				var t = (_particleLifetime - _timeToLive) / _particleLifetime;
				ColorExtensions.Lerp(ref _startColor, ref _finishColor, out color, t);

				particleSize += _particleSizeDelta * Time.DeltaTime;
				particleSize = MathHelper.Max(0, particleSize);

				rotation += _rotationDelta * Time.DeltaTime;


				if (collisionConfig.Enabled)
				{
					if (_collided)
					{
						_velocity += collisionConfig.Gravity * Time.DeltaTime;
						position += _velocity * Time.DeltaTime;

						if (_velocity.LengthSquared() < collisionConfig.MinKillSpeedSquared)
							return true;
					}

					var pos = emitterConfig.SimulateInWorldSpace ? spawnPosition : rootPosition;

					_circleCollisionShape.RecalculateBounds(particleSize * 0.5f * collisionConfig.RadiusScale,
						pos + position);
					var neighbors = RelmPhysics.BoxcastBroadphase(ref _circleCollisionShape.bounds,
						collisionConfig.CollidesWithLayers);
					foreach (var neighbor in neighbors)
					{
						CollisionResult result;
						if (_circleCollisionShape.CollidesWithShape(neighbor.Shape, out result))
						{
							position -= result.MinimumTranslationVector;
							CalculateCollisionResponseVelocity(collisionConfig.Friction, collisionConfig.Elasticity,
								ref result.MinimumTranslationVector);

							_timeToLive -= _timeToLive * collisionConfig.LifetimeLoss;
							_collided = true;
						}
					}
				}
			}
			else
			{
				return true;
			}

			return false;
		}


		void CalculateCollisionResponseVelocity(float friction, float elasticity, ref Vector2 minimumTranslationVector)
		{
			var inverseMTV = minimumTranslationVector * -1f;
			Vector2 normal;
			Vector2.Normalize(ref inverseMTV, out normal);

			float n;
			Vector2.Dot(ref _velocity, ref normal, out n);

			var normalVelocityComponent = normal * n;
			var tangentialVelocityComponent = _velocity - normalVelocityComponent;

			if (n > 0.0f)
				normalVelocityComponent = Vector2.Zero;

			var responseVelocity =
				-(1.0f + elasticity) * normalVelocityComponent - friction * tangentialVelocityComponent;
			_velocity += responseVelocity;
		}
	}
}