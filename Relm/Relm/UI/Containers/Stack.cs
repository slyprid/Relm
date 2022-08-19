using Relm.UI.Base;

namespace Relm.UI.Containers
{
	public class Stack : Group
	{
		#region ILayout

		public override float MinWidth
		{
			get
			{
				if (sizeInvalid)
					ComputeSize();
				return _minWidth;
			}
		}

		public override float MinHeight
		{
			get
			{
				if (sizeInvalid)
					ComputeSize();
				return _minHeight;
			}
		}

		public override float PreferredWidth
		{
			get
			{
				if (sizeInvalid)
					ComputeSize();
				return _prefWidth;
			}
		}

		public override float PreferredHeight
		{
			get
			{
				if (sizeInvalid)
					ComputeSize();
				return _prefHeight;
			}
		}

		public override float MaxWidth
		{
			get
			{
				if (sizeInvalid)
					ComputeSize();
				return _maxWidth;
			}
		}

		public override float MaxHeight
		{
			get
			{
				if (sizeInvalid)
					ComputeSize();
				return _maxHeight;
			}
		}

		float _prefWidth, _prefHeight, _minWidth, _minHeight, _maxWidth, _maxHeight;

		#endregion

		bool sizeInvalid = true;


		public Stack()
		{
			transform = false;
			SetSize(150, 150);
			touchable = Touchable.ChildrenOnly;
		}


		public override void Invalidate()
		{
			base.Invalidate();
			sizeInvalid = true;
		}


		private void ComputeSize()
		{
			sizeInvalid = false;
			_prefWidth = 0;
			_prefHeight = 0;
			_minWidth = 0;
			_minHeight = 0;
			_maxWidth = 0;
			_maxHeight = 0;

			for (int i = 0, n = children.Count; i < n; i++)
			{
				var child = children[i];
				float childMaxWidth, childMaxHeight;
				if (child is ILayout)
				{
					var layout = (ILayout)child;
					_prefWidth = System.Math.Max(_prefWidth, layout.PreferredWidth);
					_prefHeight = System.Math.Max(_prefHeight, layout.PreferredHeight);
					_minWidth = System.Math.Max(_minWidth, layout.MinWidth);
					_minHeight = System.Math.Max(_minHeight, layout.MinHeight);
					childMaxWidth = layout.MaxWidth;
					childMaxHeight = layout.MaxHeight;
				}
				else
				{
					_prefWidth = System.Math.Max(_prefWidth, child.width);
					_prefHeight = System.Math.Max(_prefHeight, child.height);
					_minWidth = System.Math.Max(_minWidth, child.width);
					_minHeight = System.Math.Max(_minHeight, child.height);
					childMaxWidth = 0;
					childMaxHeight = 0;
				}

				if (childMaxWidth > 0)
					_maxWidth = _maxWidth == 0 ? childMaxWidth : System.Math.Min(_maxWidth, childMaxWidth);
				if (childMaxHeight > 0)
					_maxHeight = _maxHeight == 0 ? childMaxHeight : System.Math.Min(_maxHeight, childMaxHeight);
			}
		}


		public T Add<T>(T element) where T : Element
		{
			return AddElement(element);
		}


		public override void Layout()
		{
			if (sizeInvalid)
				ComputeSize();

			for (int i = 0, n = children.Count; i < n; i++)
			{
				Element child = children[i];
				child.SetBounds(0, 0, width, height);
				if (child is ILayout)
					((ILayout)child).Validate();
			}
		}
	}
}