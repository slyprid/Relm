﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Assets.BitmapFonts;
using Relm.Core;
using Relm.Extensions;
using Relm.Graphics;
using Relm.Input;
using Relm.Math;
using Relm.Timers;
using Relm.UI.Base;
using Relm.UI.Widgets.Styles;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets
{
	public class TextField : Element, IInputListener, IKeyboardListener
	{
		public event Action<TextField, string> OnTextChanged;
		public event Action<TextField> OnEnterPressed = delegate { };
		public event Action<TextField> OnTabPressed = delegate { };

		public override float PreferredWidth => _preferredWidth;

		public override float PreferredHeight
		{
			get
			{
				var prefHeight = textHeight;
				if (style.Background != null)
					prefHeight = System.Math.Max(prefHeight + style.Background.BottomHeight + style.Background.TopHeight,
						style.Background.MinHeight);

				return prefHeight;
			}
		}

		/// <summary>
		/// the maximum distance outside the TextField the mouse can move when pressing it to cause it to be unfocused
		/// </summary>
		public float TextFieldBoundaryThreshold = 100f;

		/// <summary>
		/// if true and setText is called it will be ignored
		/// </summary>
		public bool ShouldIgnoreTextUpdatesWhileFocused = true;

		protected string text;
		protected int cursor, selectionStart;
		protected bool hasSelection;
		protected bool writeEnters;
		List<float> glyphPositions = new List<float>(15);

		float _preferredWidth = 150;
		TextFieldStyle style;
		string messageText;
		protected string displayText = string.Empty;
		ITextFieldFilter filter;
		bool focusTraversal = true, onlyFontChars = true, disabled;
		int textHAlign = AlignInternal.Left;
		float selectionX, selectionWidth;
		StringBuilder _textBuffer = new StringBuilder();

		bool passwordMode;
		StringBuilder passwordBuffer;
		char passwordCharacter = '*';

		protected float fontOffset, textHeight, textOffset;
		float renderOffset;
		int visibleTextStart, visibleTextEnd;
		int maxLength = 0;

		float blinkTime = 0.5f;
		bool cursorOn = true;
		float lastBlink;

		bool programmaticChangeEvents;

		protected bool _isOver, _isPressed, _isFocused;
		ITimer _keyRepeatTimer;
		float _keyRepeatTime = 0.2f;


		public TextField(string text, TextFieldStyle style)
		{
			SetStyle(style);
			SetText(text);
			SetSize(PreferredWidth, PreferredHeight);
		}


		public TextField(string text, Skin skin, string styleName = null) : this(text,
			skin.Get<TextFieldStyle>(styleName))
		{ }


		#region IInputListener

		float _clickCountInterval = 0.2f;
		int _clickCount;
		float _lastClickTime;

		void IInputListener.OnMouseEnter()
		{
			_isOver = true;
		}


		void IInputListener.OnMouseExit()
		{
			_isOver = _isPressed = false;
		}


		bool IInputListener.OnLeftMousePressed(Vector2 mousePos)
		{
			if (disabled)
				return false;

			_isPressed = true;
			SetCursorPosition(mousePos.X, mousePos.Y);
			selectionStart = cursor;
			hasSelection = true;
			var stage = GetStage();
			if (stage != null)
				stage.SetKeyboardFocus(this as IKeyboardListener);

			return true;
		}

		bool IInputListener.OnRightMousePressed(Vector2 mousePos)
		{
			return false;
		}


		void IInputListener.OnMouseMoved(Vector2 mousePos)
		{
			if (DistanceOutsideBoundsToPoint(mousePos) > TextFieldBoundaryThreshold)
			{
				_isPressed = _isOver = false;
				GetStage().RemoveInputFocusListener(this);
			}
			else
			{
				SetCursorPosition(mousePos.X, mousePos.Y);
			}
		}


		void IInputListener.OnLeftMouseUp(Vector2 mousePos)
		{
			if (selectionStart == cursor)
				hasSelection = false;

			if (Time.TotalTime - _lastClickTime > _clickCountInterval)
				_clickCount = 0;
			_clickCount++;
			_lastClickTime = Time.TotalTime;
			_isPressed = _isOver = false;
		}

		void IInputListener.OnRightMouseUp(Vector2 mousePos)
		{

		}

		bool IInputListener.OnMouseScrolled(int mouseWheelDelta)
		{
			return false;
		}

		#endregion


		#region IKeyboardListener

		void IKeyboardListener.KeyDown(Keys key)
		{
			if (disabled)
				return;

			lastBlink = 0;
			cursorOn = false;

			var isCtrlDown = InputUtils.IsControlDown();
			var jump = isCtrlDown && !passwordMode;
			var repeat = false;

			if (isCtrlDown)
			{
				if (key == Keys.V)
				{
					Paste(Clipboard.GetContents(), true);
				}
				else if (key == Keys.C || key == Keys.Insert)
				{
					Copy();
					return;
				}
				else if (key == Keys.X)
				{
					Cut(true);
					return;
				}
				else if (key == Keys.A)
				{
					SelectAll();
					return;
				}
			}

			if (InputUtils.IsShiftDown())
			{
				if (key == Keys.Insert)
					Paste(Clipboard.GetContents(), true);
				else if (key == Keys.Delete)
					Cut(true);

				// jumping around shortcuts
				var temp = cursor;
				var foundJumpKey = true;

				if (key == Keys.Left)
				{
					MoveCursor(false, jump);
					repeat = true;
				}
				else if (key == Keys.Right)
				{
					MoveCursor(true, jump);
					repeat = true;
				}
				else if (key == Keys.Home)
				{
					GoHome();
				}
				else if (key == Keys.End)
				{
					GoEnd();
				}
				else
				{
					foundJumpKey = false;
				}

				if (foundJumpKey && !hasSelection)
				{
					selectionStart = temp;
					hasSelection = true;
				}
			}
			else
			{
				// Cursor movement or other keys (kills selection)
				if (key == Keys.Left)
				{
					MoveCursor(false, jump);
					ClearSelection();
					repeat = true;
				}
				else if (key == Keys.Right)
				{
					MoveCursor(true, jump);
					ClearSelection();
					repeat = true;
				}
				else if (key == Keys.Home)
				{
					GoHome();
				}
				else if (key == Keys.End)
				{
					GoEnd();
				}
			}

			cursor = Mathf.Clamp(cursor, 0, text.Length);

			if (repeat)
			{
				if (_keyRepeatTimer != null)
					_keyRepeatTimer.Stop();
				_keyRepeatTimer = RelmGame.Schedule(_keyRepeatTime, true, this,
					t => (t.Context as IKeyboardListener).KeyDown(key));
			}
		}


		void IKeyboardListener.KeyPressed(Keys key, char character)
		{
			if (InputUtils.IsControlDown())
				return;

			// disallow typing most ASCII control characters, which would show up as a space
			switch (key)
			{
				case Keys.Back:
				case Keys.Delete:
				case Keys.Tab:
				case Keys.Enter:
					break;
				default:
					{
						if ((int)character < 32)
							return;

						break;
					}
			}

			if (key == Keys.Tab && focusTraversal)
			{
				Next(InputUtils.IsShiftDown());
			}
			else
			{
				var enterPressed = key == Keys.Enter;
				var backspacePressed = key == Keys.Back;
				var deletePressed = key == Keys.Delete;
				var tabPressed = key == Keys.Tab;
				var add = enterPressed ? writeEnters : (!onlyFontChars || style.Font.HasCharacter(character));
				var remove = backspacePressed || deletePressed;

				if (tabPressed)
					OnTabPressed(this);

				if (enterPressed)
					OnEnterPressed(this);

				if (add || remove)
				{
					var oldText = text;
					if (hasSelection)
					{
						cursor = Delete(false);
					}
					else
					{
						if (backspacePressed && cursor > 0)
						{
							text = text.Substring(0, cursor - 1) + text.Substring(cursor--);
							renderOffset = 0;
						}

						if (deletePressed && cursor < text.Length)
						{
							text = text.Substring(0, cursor) + text.Substring(cursor + 1);
						}
					}

					if (add && !remove)
					{
						// character may be added to the text.
						if (!enterPressed && filter != null && !filter.AcceptChar(this, character))
							return;

						if (!WithinMaxLength(text.Length))
							return;

						var insertion = enterPressed ? "\n" : character.ToString();
						text = Insert(cursor++, insertion, text);
					}

					ChangeText(oldText, text);
					UpdateDisplayText();
				}
			}
		}


		void IKeyboardListener.KeyReleased(Keys key)
		{
			if (_keyRepeatTimer != null)
			{
				_keyRepeatTimer.Stop();
				_keyRepeatTimer = null;
			}
		}


		void IKeyboardListener.GainedFocus()
		{
			hasSelection = _isFocused = true;
		}


		void IKeyboardListener.LostFocus()
		{
			hasSelection = _isFocused = false;
			if (_keyRepeatTimer != null)
			{
				_keyRepeatTimer.Stop();
				_keyRepeatTimer = null;
			}
		}

		#endregion


		protected int LetterUnderCursor(float x)
		{
			var halfSpaceSize = style.Font.DefaultCharacter.Bounds.Width + style.Font.DefaultCharacter.XAdvance;
			x -= textOffset + fontOffset + halfSpaceSize /*- style.font.getData().cursorX*/ -
				 glyphPositions[visibleTextStart];
			var n = glyphPositions.Count;
			for (var i = 0; i < n; i++)
			{
				if (glyphPositions[i] > x && i >= 1)
				{
					if (glyphPositions[i] - x <= x - glyphPositions[i - 1])
						return i;

					return i - 1;
				}
			}

			return n - 1;
		}


		protected bool IsWordCharacter(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
		}


		protected int[] WordUnderCursor(int at)
		{
			int start = at, right = text.Length, left = 0, index = start;
			for (; index < right; index++)
			{
				if (!IsWordCharacter(text[index]))
				{
					right = index;
					break;
				}
			}

			for (index = start - 1; index > -1; index--)
			{
				if (!IsWordCharacter(text[index]))
				{
					left = index + 1;
					break;
				}
			}

			return new int[] { left, right };
		}


		int[] WordUnderCursor(float x)
		{
			return WordUnderCursor(LetterUnderCursor(x));
		}


		bool WithinMaxLength(int size)
		{
			return maxLength <= 0 || size < maxLength;
		}


		public TextField SetMaxLength(int maxLength)
		{
			this.maxLength = maxLength;
			return this;
		}


		public int GetMaxLength()
		{
			return maxLength;
		}


		/// <summary>
		/// When false, text set by {@link #setText(String)} may contain characters not in the font, a space will be displayed instead.
		/// When true (the default), characters not in the font are stripped by setText. Characters not in the font are always stripped
		/// when typed or pasted.
		/// </summary>
		/// <param name="onlyFontChars">If set to <c>true</c> only font chars.</param>
		public TextField SetOnlyFontChars(bool onlyFontChars)
		{
			this.onlyFontChars = onlyFontChars;
			return this;
		}


		public TextField SetStyle(TextFieldStyle style)
		{
			this.style = style;
			textHeight = style.Font.LineHeight;
			InvalidateHierarchy();
			return this;
		}


		/// <summary>
		/// Returns the text field's style. Modifying the returned style may not have an effect until {@link #setStyle(TextFieldStyle)} is called
		/// </summary>
		/// <returns>The style.</returns>
		public TextFieldStyle GetStyle()
		{
			return style;
		}


		protected void CalculateOffsets()
		{
			float visibleWidth = GetWidth();
			if (style.Background != null)
				visibleWidth -= style.Background.LeftWidth + style.Background.RightWidth;

			var glyphCount = glyphPositions.Count;

			// Check if the cursor has gone out the left or right side of the visible area and adjust renderoffset.
			var distance = glyphPositions[System.Math.Max(0, cursor - 1)] + renderOffset;
			if (distance <= 0)
			{
				renderOffset -= distance;
			}
			else
			{
				var index = System.Math.Min(glyphCount - 1, cursor + 1);
				var minX = glyphPositions[index] - visibleWidth;
				if (-renderOffset < minX)
				{
					renderOffset = -minX;
				}
			}

			// calculate first visible char based on render offset
			visibleTextStart = 0;
			var startX = 0f;
			for (var i = 0; i < glyphCount; i++)
			{
				if (glyphPositions[i] >= -renderOffset)
				{
					visibleTextStart = System.Math.Max(0, i);
					startX = glyphPositions[i];
					break;
				}
			}

			// calculate last visible char based on visible width and render offset
			var length = displayText.Length;
			visibleTextEnd = System.Math.Min(length, cursor + 1);
			for (; visibleTextEnd <= length; visibleTextEnd++)
				if (glyphPositions[visibleTextEnd] > startX + visibleWidth)
					break;

			visibleTextEnd = System.Math.Max(0, visibleTextEnd - 1);

			if ((textHAlign & AlignInternal.Left) == 0)
			{
				textOffset = visibleWidth - (glyphPositions[visibleTextEnd] - startX);
				if ((textHAlign & AlignInternal.Center) != 0)
					textOffset = Mathf.Round(textOffset * 0.5f);
			}
			else
			{
				textOffset = startX + renderOffset;
			}

			// calculate selection x position and width
			if (hasSelection)
			{
				var minIndex = System.Math.Min(cursor, selectionStart);
				var maxIndex = System.Math.Max(cursor, selectionStart);
				var minX = System.Math.Max(glyphPositions[minIndex], -renderOffset);
				var maxX = System.Math.Min(glyphPositions[maxIndex], visibleWidth - renderOffset);
				selectionX = minX;

				if (renderOffset == 0)
					selectionX += textOffset;

				selectionWidth = maxX - minX;
			}
		}


		#region Drawing

		public override void Draw(SpriteBatch spriteBatch, float parentAlpha)
		{
			var font = style.Font;
			var fontColor = (disabled && style.DisabledFontColor.HasValue)
				? style.DisabledFontColor.Value
				: ((_isFocused && style.FocusedFontColor.HasValue) ? style.FocusedFontColor.Value : style.FontColor);
			IDrawable selection = style.Selection;
			IDrawable background = (disabled && style.DisabledBackground != null)
				? style.DisabledBackground
				: ((_isFocused && style.FocusedBackground != null) ? style.FocusedBackground : style.Background);

			var color = GetColor();
			var x = GetX();
			var y = GetY();
			var width = GetWidth();
			var height = GetHeight();

			float bgLeftWidth = 0, bgRightWidth = 0;
			if (background != null)
			{
				background.Draw(spriteBatch, x, y, width, height, ColorExtensions.Create(color, (int)(color.A * parentAlpha)));
				bgLeftWidth = background.LeftWidth;
				bgRightWidth = background.RightWidth;
			}

			var textY = GetTextY(font, background);
			var yOffset = (textY < 0) ? -textY - font.LineHeight / 2f + GetHeight() / 2 : 0;
			CalculateOffsets();

			if (_isFocused && hasSelection && selection != null)
				DrawSelection(selection, spriteBatch, font, x + bgLeftWidth, y + textY + yOffset);

			if (displayText.Length == 0)
			{
				if (!_isFocused && messageText != null)
				{
					var messageFontColor = style.MessageFontColor.HasValue
						? style.MessageFontColor.Value
						: new Color(180, 180, 180, (int)(color.A * parentAlpha));
					var messageFont = style.MessageFont != null ? style.MessageFont : font;
                    spriteBatch.DrawString(messageFont, messageText,
						new Vector2(x + bgLeftWidth, y + textY + yOffset), messageFontColor);

					//messageFont.draw( batcher.batcher, messageText, x + bgLeftWidth, y + textY + yOffset, 0, messageText.length(),
					//	width - bgLeftWidth - bgRightWidth, textHAlign, false, "..." );
				}
			}
			else
			{
				var col = ColorExtensions.Create(fontColor, (int)(fontColor.A * parentAlpha));
				var t = displayText.Substring(visibleTextStart, visibleTextEnd - visibleTextStart);
                spriteBatch.DrawString(font, t, new Vector2(x + bgLeftWidth + textOffset, y + textY + yOffset),
					col);
			}

			if (_isFocused && !disabled)
			{
				Blink();
				if (cursorOn && style.Cursor != null)
					DrawCursor(style.Cursor, spriteBatch, font, x + bgLeftWidth, y + textY + yOffset);
			}
		}


		protected float GetTextY(BitmapFont font, IDrawable background)
		{
			float height = GetHeight();
			float textY = textHeight / 2 + font.Padding.Bottom;
			if (background != null)
			{
				var bottom = background.BottomHeight;
				textY = textY - (height - background.TopHeight - bottom) / 2 + bottom;
			}
			else
			{
				textY = textY - height / 2;
			}

			return textY;
		}


		/// <summary>
		/// Draws selection rectangle
		/// </summary>
		/// <param name="selection">Selection.</param>
		/// <param name="batch">Batch.</param>
		/// <param name="font">Font.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		protected void DrawSelection(IDrawable selection, SpriteBatch spriteBatch, BitmapFont font, float x, float y)
		{
			selection.Draw(spriteBatch, x + selectionX + renderOffset + fontOffset, y - font.Padding.Bottom / 2,
				selectionWidth, textHeight, Color.White);
		}


		protected void DrawCursor(IDrawable cursorPatch, SpriteBatch spriteBatch, BitmapFont font, float x, float y)
		{
			cursorPatch.Draw(spriteBatch,
				x + textOffset + glyphPositions[cursor] - glyphPositions[visibleTextStart] + fontOffset -
				1 /*font.getData().cursorX*/,
				y - font.Padding.Bottom / 2, cursorPatch.MinWidth, textHeight, color);
		}

		#endregion


		void UpdateDisplayText()
		{
			var textLength = text.Length;

			_textBuffer.Clear();
			for (var i = 0; i < textLength; i++)
			{
				var c = text[i];
				_textBuffer.Append(style.Font.HasCharacter(c) ? c : ' ');
			}

			var newDisplayText = _textBuffer.ToString();

			if (passwordMode && style.Font.HasCharacter(passwordCharacter))
			{
				if (passwordBuffer == null)
					passwordBuffer = new StringBuilder(newDisplayText.Length);
				else if (passwordBuffer.Length > textLength)
					passwordBuffer.Clear();

				for (var i = passwordBuffer.Length; i < textLength; i++)
					passwordBuffer.Append(passwordCharacter);
				displayText = passwordBuffer.ToString();
			}
			else
			{
				displayText = newDisplayText;
			}

			//layout.setText( font, displayText );
			glyphPositions.Clear();
			float x = 0;
			if (displayText.Length > 0)
			{
				for (var i = 0; i < displayText.Length; i++)
				{
					var region = style.Font[displayText[i]];

					// we dont have fontOffset in BitmapFont, it is the first Glyph in a GlyphRun
					//if( i == 0 )
					//	fontOffset = region.xAdvance;
					glyphPositions.Add(x);
					x += region.XAdvance;
				}

				//GlyphRun run = layout.runs.first();
				//FloatArray xAdvances = run.xAdvances;
				//fontOffset = xAdvances.first();
				//for( int i = 1, n = xAdvances.size; i < n; i++ )
				//{
				//	glyphPositions.add( x );
				//	x += xAdvances.get( i );
				//}
			}
			else
			{
				fontOffset = 0;
			}

			glyphPositions.Add(x);

			if (selectionStart > newDisplayText.Length)
				selectionStart = textLength;
		}


		void Blink()
		{
			if ((Time.TotalTime - lastBlink) > blinkTime)
			{
				cursorOn = !cursorOn;
				lastBlink = Time.TotalTime;
			}
		}


		#region Text manipulation

		/// <summary>
		/// Copies the contents of this TextField to the {@link Clipboard} implementation set on this TextField
		/// </summary>
		public void Copy()
		{
			if (hasSelection && !passwordMode)
			{
				var start = System.Math.Min(cursor, selectionStart);
				var length = System.Math.Max(cursor, selectionStart) - start;
				Clipboard.SetContents(text.Substring(start, length));
			}
		}


		/// <summary>
		/// Copies the selected contents of this TextField to the {@link Clipboard} implementation set on this TextField, then removes it
		/// </summary>
		public void Cut()
		{
			Cut(programmaticChangeEvents);
		}


		void Cut(bool fireChangeEvent)
		{
			if (hasSelection && !passwordMode)
			{
				Copy();
				cursor = Delete(fireChangeEvent);
				UpdateDisplayText();
			}
		}


		void Paste(string content, bool fireChangeEvent)
		{
			if (content == null)
				return;

			_textBuffer.Clear();
			int textLength = text.Length;
			if (hasSelection)
				textLength -= System.Math.Abs(cursor - selectionStart);

			//var data = style.font.getData();
			for (int i = 0, n = content.Length; i < n; i++)
			{
				if (!WithinMaxLength(textLength + _textBuffer.Length))
					break;

				var c = content[i];
				if (!(writeEnters && c == '\r'))
				{
					if (onlyFontChars && !style.Font.HasCharacter(c))
						continue;

					if (filter != null && !filter.AcceptChar(this, c))
						continue;
				}

				_textBuffer.Append(c);
			}

			content = _textBuffer.ToString();

			if (hasSelection)
				cursor = Delete(fireChangeEvent);
			if (fireChangeEvent)
				ChangeText(text, Insert(cursor, content, text));
			else
				text = Insert(cursor, content, text);
			UpdateDisplayText();
			cursor += content.Length;
		}


		string Insert(int position, string text, string to)
		{
			if (to.Length == 0)
				return text;

			return to.Substring(0, position) + text + to.Substring(position, to.Length - position);
		}


		int Delete(bool fireChangeEvent)
		{
			var from = selectionStart;
			var to = cursor;
			var minIndex = System.Math.Min(from, to);
			var maxIndex = System.Math.Max(from, to);
			var newText = (minIndex > 0 ? text.Substring(0, minIndex) : "")
						  + (maxIndex < text.Length ? text.Substring(maxIndex, text.Length - maxIndex) : "");

			if (fireChangeEvent)
				ChangeText(text, newText);
			else
				text = newText;

			ClearSelection();
			return minIndex;
		}


		/// <summary>
		/// Focuses the next TextField. If none is found, the keyboard is hidden. Does nothing if the text field is not in a stage
		/// up: If true, the TextField with the same or next smallest y coordinate is found, else the next highest.
		/// </summary>
		/// <param name="up">Up.</param>
		public void Next(bool up)
		{
			var stage = GetStage();
			if (stage == null)
				return;

			var tmp2 = Vector2.Zero;
			var tmp1 = GetParent().LocalToStageCoordinates(new Vector2(GetX(), GetY()));
			var textField = FindNextTextField(stage.GetElements(), null, tmp2, tmp1, up);
			if (textField == null)
			{
				// Try to wrap around.
				if (up)
					tmp1 = new Vector2(float.MinValue, float.MinValue);
				else
					tmp1 = new Vector2(float.MaxValue, float.MaxValue);
				textField = FindNextTextField(GetStage().GetElements(), null, tmp2, tmp1, up);
			}

			if (textField != null)
				stage.SetKeyboardFocus(textField);
		}


		TextField FindNextTextField(List<Element> elements, TextField best, Vector2 bestCoords, Vector2 currentCoords,
									bool up)
		{
			bestCoords = Vector2.Zero;
			for (int i = 0, n = elements.Count; i < n; i++)
			{
				var element = elements[i];
				if (element == this)
					continue;

				if (element is TextField)
				{
					var textField = (TextField)element;
					if (textField.IsDisabled() || !textField.focusTraversal)
						continue;

					var elementCoords = element.GetParent()
						.LocalToStageCoordinates(new Vector2(element.GetX(), element.GetY()));
					if ((elementCoords.Y < currentCoords.Y ||
						 (elementCoords.Y == currentCoords.Y && elementCoords.X > currentCoords.X)) ^ up)
					{
						if (best == null
							|| (elementCoords.Y > bestCoords.Y ||
								(elementCoords.Y == bestCoords.Y && elementCoords.X < bestCoords.X)) ^ up)
						{
							best = (TextField)element;
							bestCoords = elementCoords;
						}
					}
				}
				else if (element is Group)
				{
					best = FindNextTextField(((Group)element).GetChildren(), best, bestCoords, currentCoords, up);
				}
			}

			return best;
		}

		#endregion


		/// <summary>
		/// if str is null, "" is used
		/// </summary>
		/// <param name="str">String.</param>
		public void AppendText(string str)
		{
			if (ShouldIgnoreTextUpdatesWhileFocused && _isFocused)
				return;

			if (str == null)
				str = "";

			ClearSelection();
			cursor = text.Length;
			Paste(str, programmaticChangeEvents);
		}


		/// <summary>
		/// str If null, "" is used
		/// </summary>
		/// <param name="str">String.</param>
		public TextField SetText(string str)
		{
			if (ShouldIgnoreTextUpdatesWhileFocused && _isFocused)
				return this;

			if (str == null)
				str = "";
			if (str == text)
				return this;

			ClearSelection();
			var oldText = text;
			text = "";
			Paste(str, false);
			if (programmaticChangeEvents)
				ChangeText(oldText, text);
			cursor = 0;

			return this;
		}


		/// <summary>
		/// force sets the text without validating or firing change events. Use at your own risk.
		/// </summary>
		/// <param name="str">String.</param>
		public TextField SetTextForced(string str)
		{
			text = str;
			UpdateDisplayText();

			// ensure our cursor is in bounds
			cursor = text.Length;

			return this;
		}


		/// <summary>
		/// Never null, might be an empty string
		/// </summary>
		/// <returns>The text.</returns>
		public string GetText()
		{
			return text;
		}


		/// <summary>
		/// oldText May be null
		/// </summary>
		/// <param name="oldText">Old text.</param>
		/// <param name="newText">New text.</param>
		void ChangeText(string oldText, string newText)
		{
			if (newText == oldText)
				return;

			text = newText;

			if (OnTextChanged != null)
				OnTextChanged(this, text);
		}


		/// <summary>
		/// If false, methods that change the text will not fire {@link onTextChanged}, the event will be fired only when user changes the text
		/// </summary>
		/// <param name="programmaticChangeEvents">If set to <c>true</c> programmatic change events.</param>
		public TextField SetProgrammaticChangeEvents(bool programmaticChangeEvents)
		{
			this.programmaticChangeEvents = programmaticChangeEvents;
			return this;
		}


		public int GetSelectionStart()
		{
			return selectionStart;
		}


		public string GetSelection()
		{
			return hasSelection
				? text.Substring(System.Math.Min(selectionStart, cursor), System.Math.Max(selectionStart, cursor))
				: "";
		}


		/// <summary>
		/// Sets the selected text
		/// </summary>
		/// <param name="selectionStart">Selection start.</param>
		/// <param name="selectionEnd">Selection end.</param>
		public TextField SetSelection(int selectionStart, int selectionEnd)
		{
			Assert.IsFalse(selectionStart < 0, "selectionStart must be >= 0");
            Assert.IsFalse(selectionEnd < 0, "selectionEnd must be >= 0");

			selectionStart = System.Math.Min(text.Length, selectionStart);
			selectionEnd = System.Math.Min(text.Length, selectionEnd);
			if (selectionEnd == selectionStart)
			{
				ClearSelection();
				return this;
			}

			if (selectionEnd < selectionStart)
			{
				int temp = selectionEnd;
				selectionEnd = selectionStart;
				selectionStart = temp;
			}

			hasSelection = true;
			this.selectionStart = selectionStart;
			cursor = selectionEnd;

			return this;
		}


		public void SelectAll()
		{
			SetSelection(0, text.Length);
		}


		public void ClearSelection()
		{
			hasSelection = false;
		}


		protected void SetCursorPosition(float x, float y)
		{
			lastBlink = 0;
			cursorOn = false;
			cursor = LetterUnderCursor(x);
		}


		/// <summary>
		/// Sets the cursor position and clears any selection
		/// </summary>
		/// <param name="cursorPosition">Cursor position.</param>
		public TextField SetCursorPosition(int cursorPosition)
		{
			Assert.IsFalse(cursorPosition < 0, "cursorPosition must be >= 0");
			ClearSelection();
			cursor = System.Math.Min(cursorPosition, text.Length);
			return this;
		}


		public int GetCursorPosition()
		{
			return cursor;
		}


		protected void GoHome()
		{
			cursor = 0;
		}


		protected void GoEnd()
		{
			cursor = text.Length;
		}


		protected void MoveCursor(bool forward, bool jump)
		{
			var limit = forward ? text.Length : 0;
			var charOffset = forward ? 0 : -1;

			if ((forward && cursor == limit) || (!forward && cursor == 0))
				return;

			while ((forward ? ++cursor < limit : --cursor > limit) && jump)
			{
				if (!ContinueCursor(cursor, charOffset))
					break;
			}
		}


		protected bool ContinueCursor(int index, int offset)
		{
			var c = text[index + offset];
			return IsWordCharacter(c);
		}


		#region Configuration

		public TextField SetPreferredWidth(float preferredWidth)
		{
			_preferredWidth = preferredWidth;
			return this;
		}


		/// <summary>
		/// filter May be null
		/// </summary>
		/// <param name="filter">Filter.</param>
		public TextField SetTextFieldFilter(ITextFieldFilter filter)
		{
			this.filter = filter;
			return this;
		}


		public ITextFieldFilter GetTextFieldFilter()
		{
			return filter;
		}


		/// <summary>
		/// If true (the default), tab/shift+tab will move to the next text field
		/// </summary>
		/// <param name="focusTraversal">If set to <c>true</c> focus traversal.</param>
		public TextField SetFocusTraversal(bool focusTraversal)
		{
			this.focusTraversal = focusTraversal;
			return this;
		}


		/// <summary>
		/// May be null
		/// </summary>
		/// <returns>The message text.</returns>
		public string GetMessageText()
		{
			return messageText;
		}


		/// <summary>
		/// Sets the text that will be drawn in the text field if no text has been entered.
		/// </summary>
		/// <param name="messageText">Message text.</param>
		public TextField SetMessageText(string messageText)
		{
			this.messageText = messageText;
			return this;
		}


		/// <summary>
		/// Sets text horizontal alignment (left, center or right).
		/// </summary>
		/// <param name="alignment">Alignment.</param>
		public TextField SetAlignment(Align alignment)
		{
			textHAlign = (int)alignment;
			return this;
		}


		/// <summary>
		/// If true, the text in this text field will be shown as bullet characters.
		/// </summary>
		/// <param name="passwordMode">Password mode.</param>
		public TextField SetPasswordMode(bool passwordMode)
		{
			this.passwordMode = passwordMode;
			UpdateDisplayText();
			return this;
		}


		public bool IsPasswordMode()
		{
			return passwordMode;
		}


		/// <summary>
		/// Sets the password character for the text field. The character must be present in the {@link BitmapFont}. Default is 149 (bullet)
		/// </summary>
		/// <param name="passwordCharacter">Password character.</param>
		public TextField SetPasswordCharacter(char passwordCharacter)
		{
			this.passwordCharacter = passwordCharacter;
			if (passwordMode)
				UpdateDisplayText();
			return this;
		}


		public TextField SetBlinkTime(float blinkTime)
		{
			this.blinkTime = blinkTime;
			return this;
		}


		public TextField SetDisabled(bool disabled)
		{
			this.disabled = disabled;
			return this;
		}


		public bool IsDisabled()
		{
			return disabled;
		}

		#endregion


		/// <summary>
		/// Interface for filtering characters entered into the text field.
		/// </summary>
		public interface ITextFieldFilter
		{
			bool AcceptChar(TextField textField, char c);
		}
	}
}