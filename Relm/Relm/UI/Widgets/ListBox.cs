﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using Relm.Extensions;
using Relm.Graphics;
using Relm.UI.Base;
using Relm.UI.Utils;
using Relm.UI.Widgets.Styles;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets
{
	public class ListBox<T> : Element, IInputListener where T : class
	{
		public event Action<T> OnChanged;

		ListBoxStyle _style;
		List<T> _items = new List<T>();
		ArraySelection<T> _selection;
		float _prefWidth, _prefHeight;
		float _itemHeight;
		float _textOffsetX, _textOffsetY;
		Rectangle? _cullingArea;
		int _hoveredItemIndex = -1;
		bool _isMouseOverList;


		public ListBox(Skin skin, string styleName = null) : this(skin.Get<ListBoxStyle>(styleName))
		{
		}


		public ListBox(ListBoxStyle style)
		{
			_selection = new ArraySelection<T>(_items);
			_selection.SetElement(this);
			_selection.SetRequired(true);

			SetStyle(style);
			SetSize(PreferredWidth, PreferredHeight);
		}


		#region ILayout

		public override float PreferredWidth
		{
			get
			{
				Validate();
				return _prefWidth;
			}
		}

		public override float PreferredHeight
		{
			get
			{
				Validate();
				return _prefHeight;
			}
		}

		#endregion


		#region IInputListener

		void IInputListener.OnMouseEnter()
		{
			_isMouseOverList = true;
		}


		void IInputListener.OnMouseExit()
		{
			_isMouseOverList = false;
			_hoveredItemIndex = -1;
		}


		bool IInputListener.OnLeftMousePressed(Vector2 mousePos)
		{
			if (_selection.IsDisabled() || _items.Count == 0)
				return false;

			var lastSelectedItem = _selection.GetLastSelected();
			var index = GetItemIndexUnderMousePosition(mousePos);
			index = System.Math.Max(0, index);
			index = System.Math.Min(_items.Count - 1, index);
			_selection.Choose(_items[index]);

			if (lastSelectedItem != _items[index] && OnChanged != null)
				OnChanged(_items[index]);

			return true;
		}

		bool IInputListener.OnRightMousePressed(Vector2 mousePos)
		{
			return false;
		}

		void IInputListener.OnMouseMoved(Vector2 mousePos)
		{
		}

		void IInputListener.OnLeftMouseUp(Vector2 mousePos)
		{
		}

		void IInputListener.OnRightMouseUp(Vector2 mousePos)
		{
		}

		bool IInputListener.OnMouseScrolled(int mouseWheelDelta)
		{
			return false;
		}


		int GetItemIndexUnderMousePosition(Vector2 mousePos)
		{
			if (_selection.IsDisabled() || _items.Count == 0)
				return -1;

			var top = 0f;
			if (_style.Background != null)
			{
				top += _style.Background.TopHeight + _style.Background.BottomHeight;
				mousePos.Y += _style.Background.BottomHeight;
			}

			var index = (int)((top + mousePos.Y) / _itemHeight);
			if (index < 0 || index > _items.Count - 1)
				return -1;

			return index;
		}

		#endregion


		public override void Layout()
		{
			var font = _style.Font;
			IDrawable selectedDrawable = _style.Selection;

			_itemHeight = /*font.getCapHeight()*/ font.LineHeight - font.Padding.Bottom * 2;
			_itemHeight += selectedDrawable.TopHeight + selectedDrawable.BottomHeight;

			_textOffsetX = selectedDrawable.LeftWidth;
			_textOffsetY = selectedDrawable.TopHeight - font.Padding.Bottom;

			_prefWidth = 0;
			for (var i = 0; i < _items.Count; i++)
				_prefWidth = System.Math.Max(font.MeasureString(_items[i].ToString()).X, _prefWidth);

			_prefWidth += selectedDrawable.LeftWidth + selectedDrawable.RightWidth;
			_prefHeight = _items.Count * _itemHeight;

			var background = _style.Background;
			if (background != null)
			{
				_prefWidth += background.LeftWidth + background.RightWidth;
				_prefHeight += background.TopHeight + background.BottomHeight;
			}
		}


		public override void Draw(SpriteBatch spriteBatch, float parentAlpha)
		{
			// update our hoved item if the mouse is over the list
			if (_isMouseOverList)
			{
				var mousePos = ScreenToLocalCoordinates(_stage.GetMousePosition());
				_hoveredItemIndex = GetItemIndexUnderMousePosition(mousePos);
			}

			Validate();

			var font = _style.Font;
			var selectedDrawable = _style.Selection;

			var color = GetColor();
			color = ColorExtensions.Create(color, (int)(color.A * parentAlpha));

			float x = GetX(), y = GetY(), width = GetWidth(), height = GetHeight();
			var itemY = 0f;

			var background = _style.Background;
			if (background != null)
			{
				background.Draw(spriteBatch, x, y, width, height, color);
				var leftWidth = background.LeftWidth;
				x += leftWidth;
				itemY += background.TopHeight;
				width -= leftWidth + background.RightWidth;
			}

			var unselectedFontColor =
                ColorExtensions.Create(_style.FontColorUnselected, (int)(_style.FontColorUnselected.A * parentAlpha));
			var selectedFontColor =
                ColorExtensions.Create(_style.FontColorSelected, (int)(_style.FontColorSelected.A * parentAlpha));
			var hoveredFontColor = ColorExtensions.Create(_style.FontColorHovered, (int)(_style.FontColorHovered.A * parentAlpha));
			Color fontColor;
			for (var i = 0; i < _items.Count; i++)
			{
				if (!_cullingArea.HasValue ||
					(itemY - _itemHeight <= _cullingArea.Value.Y + _cullingArea.Value.Height &&
					 itemY >= _cullingArea.Value.Y))
				{
					var item = _items[i];
					var selected = _selection.Contains(item);
					if (selected)
					{
						selectedDrawable.Draw(spriteBatch, x, y + itemY, width, _itemHeight, color);
						fontColor = selectedFontColor;
					}
					else if (i == _hoveredItemIndex && _style.HoverSelection != null)
					{
						_style.HoverSelection.Draw(spriteBatch, x, y + itemY, width, _itemHeight, color);
						fontColor = hoveredFontColor;
					}
					else
					{
						fontColor = unselectedFontColor;
					}

					var textPos = new Vector2(x + _textOffsetX, y + itemY + _textOffsetY);
					spriteBatch.DrawString(font, item.ToString(), textPos, fontColor);
				}
				else if (itemY < _cullingArea.Value.Y)
				{
					break;
				}

				itemY += _itemHeight;
			}
		}


		#region config

		public ListBox<T> SetStyle(ListBoxStyle style)
		{
			Assert.IsNotNull(style, "style cannot be null");
			_style = style;
			InvalidateHierarchy();
			return this;
		}


		/// <summary>
		/// Returns the list's style. Modifying the returned style may not have an effect until setStyle(ListStyle) is called
		/// </summary>
		/// <returns>The style.</returns>
		public ListBoxStyle GetStyle()
		{
			return _style;
		}


		public ArraySelection<T> GetSelection()
		{
			return _selection;
		}


		/// <summary>
		/// Returns the first selected item, or null
		/// </summary>
		/// <returns>The selected.</returns>
		public T GetSelected()
		{
			return _selection.First();
		}


		/// <summary>
		/// Sets the selection to only the passed item, if it is a possible choice.
		/// </summary>
		/// <param name="item">Item.</param>
		public ListBox<T> SetSelected(T item)
		{
			if (_items.Contains(item))
				_selection.Set(item);
			else if (_selection.GetRequired() && _items.Count > 0)
				_selection.Set(_items[0]);
			else
				_selection.Clear();

			return this;
		}


		/// <summary>
		/// gets the index of the first selected item. The top item has an index of 0. Nothing selected has an index of -1.
		/// </summary>
		/// <returns>The selected index.</returns>
		public int GetSelectedIndex()
		{
			var selected = _selection.Items();
			return selected.Count == 0 ? -1 : _items.IndexOf(selected[0]);
		}


		/// <summary>
		/// Sets the selection to only the selected index
		/// </summary>
		/// <param name="index">Index.</param>
		public ListBox<T> SetSelectedIndex(int index)
		{
			Assert.IsFalse(index < -1 || index >= _items.Count,
				"index must be >= -1 and < " + _items.Count + ": " + index);

			if (index == -1)
				_selection.Clear();
			else
				_selection.Set(_items[index]);

			return this;
		}


		public ListBox<T> SetItems(params T[] newItems)
		{
			SetItems(new List<T>(newItems));
			return this;
		}


		/// <summary>
		/// Sets the items visible in the list, clearing the selection if it is no longer valid. If a selection is
		/// ArraySelection#getRequired(), the first item is selected.
		/// </summary>
		/// <param name="newItems">New items.</param>
		public ListBox<T> SetItems(IList<T> newItems)
		{
			Assert.IsNotNull(newItems, "newItems cannot be null");
			float oldPrefWidth = _prefWidth, oldPrefHeight = _prefHeight;

			_items.Clear();
			_items.AddRange(newItems);
			_selection.Validate();

			Invalidate();
			Validate();
			if (oldPrefWidth != _prefWidth || oldPrefHeight != _prefHeight)
			{
				InvalidateHierarchy();
				SetSize(_prefWidth, _prefHeight);
			}

			return this;
		}


		public void ClearItems()
		{
			if (_items.Count == 0)
				return;

			_items.Clear();
			_selection.Clear();
			InvalidateHierarchy();
		}


		/// <summary>
		/// Returns the internal items array. If modified, {@link #setItems(Array)} must be called to reflect the changes.
		/// </summary>
		/// <returns>The items.</returns>
		public List<T> GetItems()
		{
			return _items;
		}


		public float GetItemHeight()
		{
			return _itemHeight;
		}


		public ListBox<T> SetCullingArea(Rectangle cullingArea)
		{
			_cullingArea = cullingArea;
			return this;
		}

		#endregion
	}
}