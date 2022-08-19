using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Entities;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Components
{
	public class Transform
	{
		[Flags]
		enum DirtyType
		{
			Clean = 0,
			PositionDirty = 1,
			ScaleDirty = 2,
			RotationDirty = 4
		}

		public enum Component
		{
			Position,
			Scale,
			Rotation
		}

        private Transform _parent;
        private DirtyType hierarchyDirty;
        private bool _localDirty;
        private bool _localPositionDirty;
        private bool _localScaleDirty;
        private bool _localRotationDirty;
        private bool _positionDirty;
        private bool _worldToLocalDirty;
        private bool _worldInverseDirty;

        private Matrix2D _localTransform;

        private Matrix2D _worldTransform = Matrix2D.Identity;
        private Matrix2D _worldToLocalTransform = Matrix2D.Identity;
        private Matrix2D _worldInverseTransform = Matrix2D.Identity;

        private Matrix2D _rotationMatrix;
        private Matrix2D _translationMatrix;
        private Matrix2D _scaleMatrix;

        private Vector2 _position;
        private Vector2 _scale;
        private float _rotation;

        private Vector2 _localPosition;
        private Vector2 _localScale;
        private float _localRotation;

        private List<Transform> _children = new List<Transform>();

		public readonly Entity Entity;
        public int ChildCount => _children.Count;

		public Transform Parent
		{
			get => _parent;
			set => SetParent(value);
		}
		
		public Vector2 Position
		{
			get
			{
				UpdateTransform();
				if (_positionDirty)
				{
					if (Parent == null)
					{
						_position = _localPosition;
					}
					else
					{
						Parent.UpdateTransform();
                        Vector2Extensions.Transform(ref _localPosition, ref Parent._worldTransform, out _position);
					}

					_positionDirty = false;
				}

				return _position;
			}
			set => SetPosition(value);
		}
		
		public Vector2 LocalPosition
		{
			get
			{
				UpdateTransform();
				return _localPosition;
			}
			set => SetLocalPosition(value);
		}
		
		public float Rotation
		{
			get
			{
				UpdateTransform();
				return _rotation;
			}
			set => SetRotation(value);
		}
		
		public float RotationDegrees
		{
			get => MathHelper.ToDegrees(_rotation);
			set => SetRotation(MathHelper.ToRadians(value));
		}
		
		public float LocalRotation
		{
			get
			{
				UpdateTransform();
				return _localRotation;
			}
			set => SetLocalRotation(value);
		}
		
		public float LocalRotationDegrees
		{
			get => MathHelper.ToDegrees(_localRotation);
			set => LocalRotation = MathHelper.ToRadians(value);
		}
		
		public Vector2 Scale
		{
			get
			{
				UpdateTransform();
				return _scale;
			}
			set => SetScale(value);
		}
		
		public Vector2 LocalScale
		{
			get
			{
				UpdateTransform();
				return _localScale;
			}
			set => SetLocalScale(value);
		}
		
		public Matrix2D WorldInverseTransform
		{
			get
			{
				UpdateTransform();
				if (_worldInverseDirty)
				{
					Matrix2D.Invert(ref _worldTransform, out _worldInverseTransform);
					_worldInverseDirty = false;
				}

				return _worldInverseTransform;
			}
		}
		
		public Matrix2D LocalToWorldTransform
		{
			get
			{
				UpdateTransform();
				return _worldTransform;
			}
		}
		
		public Matrix2D WorldToLocalTransform
		{
			get
			{
				if (_worldToLocalDirty)
				{
					if (Parent == null)
					{
						_worldToLocalTransform = Matrix2D.Identity;
					}
					else
					{
						Parent.UpdateTransform();
						Matrix2D.Invert(ref Parent._worldTransform, out _worldToLocalTransform);
					}

					_worldToLocalDirty = false;
				}

				return _worldToLocalTransform;
			}
		}
		
		public Transform(Entity entity)
		{
			Entity = entity;
			_scale = _localScale = Vector2.One;
		}
		
		public Transform GetChild(int index)
		{
			return _children[index];
		}
		
		public Transform SetParent(Transform parent)
		{
			if (_parent == parent)
				return this;

			if (_parent != null)
				_parent._children.Remove(this);

			if (parent != null)
				parent._children.Add(this);

			_parent = parent;
			SetDirty(DirtyType.PositionDirty);

			return this;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetPosition(Vector2 position)
		{
			if (position == _position)
				return this;

			_position = position;
			if (Parent != null)
				LocalPosition = Vector2.Transform(_position, WorldToLocalTransform);
			else
				LocalPosition = position;

			_positionDirty = false;

			return this;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetPosition(float x, float y)
		{
			return SetPosition(new Vector2(x, y));
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetLocalPosition(Vector2 localPosition)
		{
			if (localPosition == _localPosition)
				return this;

			_localPosition = localPosition;
			_localDirty = _positionDirty = _localPositionDirty = _localRotationDirty = _localScaleDirty = true;
			SetDirty(DirtyType.PositionDirty);

			return this;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetRotation(float radians)
		{
			_rotation = radians;
			if (Parent != null)
				LocalRotation = Parent.Rotation + radians;
			else
				LocalRotation = radians;

			return this;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetRotationDegrees(float degrees)
		{
			return SetRotation(MathHelper.ToRadians(degrees));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetLocalRotation(float radians)
		{
			_localRotation = radians;
			_localDirty = _positionDirty = _localPositionDirty = _localRotationDirty = _localScaleDirty = true;
			SetDirty(DirtyType.RotationDirty);

			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetLocalRotationDegrees(float degrees)
		{
			return SetLocalRotation(MathHelper.ToRadians(degrees));
		}

		public void LookAt(Vector2 pos)
		{
			var sign = _position.X > pos.X ? -1 : 1;
			var vectorToAlignTo = Vector2.Normalize(_position - pos);
			Rotation = sign * Mathf.Acos(Vector2.Dot(vectorToAlignTo, Vector2.UnitY));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetScale(Vector2 scale)
		{
			_scale = scale;
			if (Parent != null)
				LocalScale = scale / Parent._scale;
			else
				LocalScale = scale;

			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetScale(float scale)
		{
			return SetScale(new Vector2(scale));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetLocalScale(Vector2 scale)
		{
			_localScale = scale;
			_localDirty = _positionDirty = _localScaleDirty = true;
			SetDirty(DirtyType.ScaleDirty);

			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform SetLocalScale(float scale)
		{
			return SetLocalScale(new Vector2(scale));
		}

        public void RoundPosition()
		{
			Position = Vector2Extensions.Round(_position);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void UpdateTransform()
		{
			if (hierarchyDirty != DirtyType.Clean)
			{
				if (Parent != null)
					Parent.UpdateTransform();

				if (_localDirty)
				{
					if (_localPositionDirty)
					{
						Matrix2D.CreateTranslation(_localPosition.X, _localPosition.Y, out _translationMatrix);
						_localPositionDirty = false;
					}

					if (_localRotationDirty)
					{
						Matrix2D.CreateRotation(_localRotation, out _rotationMatrix);
						_localRotationDirty = false;
					}

					if (_localScaleDirty)
					{
						Matrix2D.CreateScale(_localScale.X, _localScale.Y, out _scaleMatrix);
						_localScaleDirty = false;
					}

					Matrix2D.Multiply(ref _scaleMatrix, ref _rotationMatrix, out _localTransform);
					Matrix2D.Multiply(ref _localTransform, ref _translationMatrix, out _localTransform);

					if (Parent == null)
					{
						_worldTransform = _localTransform;
						_rotation = _localRotation;
						_scale = _localScale;
						_worldInverseDirty = true;
					}

					_localDirty = false;
				}

				if (Parent != null)
				{
					Matrix2D.Multiply(ref _localTransform, ref Parent._worldTransform, out _worldTransform);

					_rotation = _localRotation + Parent._rotation;
					_scale = Parent._scale * _localScale;
					_worldInverseDirty = true;
				}

				_worldToLocalDirty = true;
				_positionDirty = true;
				hierarchyDirty = DirtyType.Clean;
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void SetDirty(DirtyType dirtyFlagType)
		{
			if ((hierarchyDirty & dirtyFlagType) == 0)
			{
				hierarchyDirty |= dirtyFlagType;

				switch (dirtyFlagType)
				{
					case DirtyType.PositionDirty:
						Entity.OnTransformChanged(Component.Position);
						break;
					case DirtyType.RotationDirty:
						Entity.OnTransformChanged(Component.Rotation);
						break;
					case DirtyType.ScaleDirty:
						Entity.OnTransformChanged(Component.Scale);
						break;
				}

				for (var i = 0; i < _children.Count; i++) _children[i].SetDirty(dirtyFlagType);
			}
		}
		
		public void CopyFrom(Transform transform)
		{
			_position = transform.Position;
			_localPosition = transform._localPosition;
			_rotation = transform._rotation;
			_localRotation = transform._localRotation;
			_scale = transform._scale;
			_localScale = transform._localScale;

			SetDirty(DirtyType.PositionDirty);
			SetDirty(DirtyType.RotationDirty);
			SetDirty(DirtyType.ScaleDirty);
		}
		
		public override string ToString()
		{
			return $"[Transform: parent: {Parent != null}, position: {Position}, rotation: {Rotation}, scale: {Scale}, localPosition: {LocalPosition}, localRotation: {LocalRotation}, localScale: {LocalScale}]";
		}
	}
}