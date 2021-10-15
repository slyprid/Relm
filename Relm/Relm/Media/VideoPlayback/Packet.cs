﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FFmpeg.AutoGen;
using Relm.Common;

namespace Relm.Media.VideoPlayback
{
    /// <summary>
    /// <see cref="AVPacket"/> with some custom properties.
    /// </summary>
    internal sealed unsafe class Packet : DisposableBase
    {

        /// <summary>
        /// Creates a new <see cref="Packet"/> instance.
        /// </summary>
        /// <param name="loopNumber">Loop number.</param>
        public Packet(int loopNumber)
        {
            _loopNumber = loopNumber;
            _rawPacket = ffmpeg.av_packet_alloc();
        }

        /// <summary>
        /// Loop number of the packet.
        /// </summary>
        public int LoopNumber
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get => _loopNumber;
        }

        /// <summary>
        /// Gets raw <see cref="AVPacket"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the packet was disposed.</exception>
        public AVPacket* RawPacket
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get
            {
                EnsureNotDisposed();

                Debug.Assert(_rawPacket != null);

                return _rawPacket;
            }
        }

        protected override void Dispose(bool disposing)
        {
            ffmpeg.av_packet_unref(_rawPacket);
            ffmpeg.av_free(_rawPacket);
            _rawPacket = null;
        }

        private readonly int _loopNumber;

        private AVPacket* _rawPacket;

    }
}