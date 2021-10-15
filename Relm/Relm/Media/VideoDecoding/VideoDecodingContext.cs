﻿using System.Diagnostics;
using FFmpeg.AutoGen;
using Relm.Common;

namespace Relm.Media.VideoDecoding
{
    /// <inheritdoc />
    /// <summary>
    /// Video decoding context.
    /// </summary>
    internal sealed unsafe class VideoDecodingContext : DisposableBase
    {

        /// <summary>
        /// Creates a new <see cref="VideoDecodingContext"/> instance.
        /// </summary>
        /// <param name="videoStream">The video stream.</param>
        /// <param name="decodingOptions">Decoding options.</param>
        internal VideoDecodingContext(AVStream* videoStream, DecodingOptions decodingOptions)
        {
            _codecContext = videoStream->codec;
            _videoStream = videoStream;
            _decodingOptions = decodingOptions;
        }

        /// <summary>
        /// The underlying <see cref="AVCodecContext"/>.
        /// </summary>
        internal AVCodecContext* CodecContext
        {
            get
            {
                EnsureNotDisposed();

                return _codecContext;
            }
        }

        /// <summary>
        /// The underlying <see cref="AVStream"/>.
        /// </summary>
        internal AVStream* VideoStream
        {
            get
            {
                EnsureNotDisposed();

                return _videoStream;
            }
        }

        /// <summary>
        /// Returns a suitable audio rescale context according to output width and height.
        /// The context returned will be cached until this function is called again with different arguments.
        /// </summary>
        /// <param name="width">Output width, in pixels.</param>
        /// <param name="height">Output height, in pixels.</param>
        /// <returns>Cached or created rescale context.</returns>
        internal SwsContext* GetSuitableScaleContext(int width, int height)
        {
            EnsureNotDisposed();

            Trace.Assert(width > 0 && height > 0);

            var scaleContext = _scaleContext;

            if (scaleContext == null || width != _lastScaledWidth || height != _lastScaledHeight)
            {
                if (scaleContext != null)
                {
                    ffmpeg.sws_freeContext(scaleContext);
                    _scaleContext = null;
                }

                var codec = CodecContext;
                const AVPixelFormat destPixelFormat = FFmpegHelper.RequiredPixelFormat;
                var frameScaling = (int)_decodingOptions.FrameScalingMethod;

                // Unlike SWR context, SWS context can be allocated and options set in one function.
                scaleContext = ffmpeg.sws_getContext(codec->width, codec->height, codec->pix_fmt, width, height, destPixelFormat, frameScaling, null, null, null);

                if (scaleContext == null)
                {
                    Dispose();

                    throw new FFmpegException("Failed to get video frame conversion context.");
                }

                _lastScaledWidth = width;
                _lastScaledHeight = height;

                _scaleContext = scaleContext;
            }

            Trace.Assert(_scaleContext != null, nameof(_scaleContext) + " != null");

            return _scaleContext;
        }

        /// <summary>
        /// Gets the number of average frames per second (FPS) of the video stream.
        /// </summary>
        /// <returns>Average FPS of the video stream.</returns>
        internal float GetFramesPerSecond()
        {
            EnsureNotDisposed();

            if (_videoStream == null)
            {
                return 0;
            }
            else
            {
                var frameRate = _videoStream->avg_frame_rate;

                return (float)frameRate.num / frameRate.den;
            }
        }

        /// <summary>
        /// Gets the width of the video stream, in pixels.
        /// </summary>
        /// <returns>The width of the video stream.</returns>
        internal int GetWidth()
        {
            EnsureNotDisposed();

            return _codecContext == null ? 0 : _codecContext->width;
        }

        /// <summary>
        /// Gets the height of the video stream, in pixels.
        /// </summary>
        /// <returns>The height of the video stream.</returns>
        internal int GetHeight()
        {
            EnsureNotDisposed();

            return _codecContext == null ? 0 : _codecContext->height;
        }

        protected override void Dispose(bool disposing)
        {
            if (_codecContext != null)
            {
                ffmpeg.avcodec_close(_codecContext);
                _codecContext = null;
            }

            if (_scaleContext != null)
            {
                ffmpeg.sws_freeContext(_scaleContext);
                _scaleContext = null;
            }

            // Will be freed by AVFormatContext
            _videoStream = null;
        }

        
        private AVCodecContext* _codecContext;

        
        private AVStream* _videoStream;

        private int _lastScaledWidth = -1;
        private int _lastScaledHeight = -1;

        
        private SwsContext* _scaleContext;

        private readonly DecodingOptions _decodingOptions;

    }
}