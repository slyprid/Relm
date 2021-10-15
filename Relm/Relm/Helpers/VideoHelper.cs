﻿using System.IO;
using Relm.Media;

namespace Relm.Helpers
{
    /// <summary>
    /// Video helper class.
    /// </summary>
    public static class VideoHelper
    {

        /// <summary>
        /// Loads a video from file system using default decoding options.
        /// </summary>
        /// <param name="path">The path of the video file.</param>
        /// <returns>Loaded video.</returns>
        public static Video LoadFromFile(string path)
        {
            return LoadFromFile(path, DecodingOptions.Default);
        }

        /// <summary>
        /// Loads a video from file system using default decoding options.
        /// </summary>
        /// <param name="path">The path of the video file.</param>
        /// <returns>Loaded video.</returns>
        public static Video LoadFromContentFile(string path)
        {
            return LoadFromFile($"{ContentLibrary.Content.RootDirectory}/{path}", DecodingOptions.Default);
        }

        /// <summary>
        /// Loads a video from file system using specified decoding options.
        /// </summary>
        /// <param name="path">The path of the video file.</param>
        /// <param name="decodingOptions">The decoding options to use.</param>
        /// <returns>Loaded video.</returns>
        public static Video LoadFromFile(string path, DecodingOptions decodingOptions)
        {
            var fullPath = Path.GetFullPath(path);

            return LoadFromUrl(fullPath, decodingOptions);
        }

        /// <summary>
        /// Loads a video from a URL using default decoding options.
        /// </summary>
        /// <param name="url">The URL of the video source.</param>
        /// <returns>Loaded video.</returns>
        public static Video LoadFromUrl(string url)
        {
            return LoadFromUrl(url, DecodingOptions.Default);
        }

        /// <summary>
        /// Loads a video from a URL using specified decoding options.
        /// </summary>
        /// <param name="url">The URL of the video source.</param>
        /// <param name="decodingOptions">The decoding options to use.</param>
        /// <returns>Loaded video.</returns>
        public static Video LoadFromUrl(string url, DecodingOptions decodingOptions)
        {
            return new Video(url, decodingOptions);
        }

    }
}