﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Media
{
    /// <summary>
    /// Represents a subtitle renderer interface.
    /// </summary>
    public interface ISubtitleRenderer : IDisposable
    {

        /// <summary>
        /// Gets or sets whether this renderer is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets or sets the dimensions (expected width and height) of this renderer.
        /// </summary>
        Point Dimensions { set; }

        /// <summary>
        /// Renders the subtitle to the specified texture.
        /// </summary>
        /// <param name="time">The time when the corresponding subtitle should be rendered.</param>
        /// <param name="texture">The texture to render to.</param>
        void Render(TimeSpan time, RenderTarget2D texture);

    }
}