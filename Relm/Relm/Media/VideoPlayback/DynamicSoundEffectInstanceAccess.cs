﻿using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

// ReSharper disable once CheckNamespace
namespace Relm.Media
{
    partial class VideoPlayer
    {

        internal struct DynamicSoundEffectInstanceAccess : IDisposable
        {

            internal DynamicSoundEffectInstanceAccess(VideoPlayer videoPlayer)
            {
                _videoPlayer = videoPlayer;

                Monitor.Enter(videoPlayer._soundEffectInstanceLock);
            }

            public DynamicSoundEffectInstance SoundEffect
            {
                [DebuggerStepThrough]
                get => _videoPlayer._soundEffectInstance;
            }

            public void Dispose()
            {
                Monitor.Exit(_videoPlayer._soundEffectInstanceLock);
            }

            private readonly VideoPlayer _videoPlayer;

        }

    }
}