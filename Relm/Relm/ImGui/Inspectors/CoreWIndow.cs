﻿using System;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Relm.Gui.Utils;
using Num = System.Numerics;

namespace Relm.Gui.Inspectors
{
	class CoreWindow
    {
        string[] _textureFilters;
        float[] _frameRateArray = new float[100];
        int _frameRateArrayIndex = 0;

        public CoreWindow()
        {
            _textureFilters = Enum.GetNames(typeof(TextureFilter));
        }

        public void Show(ref bool isOpen)
        {
            if (!isOpen)
                return;

            ImGui.SetNextWindowPos(new Num.Vector2(Screen.Width - 300, Screen.Height - 240), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Num.Vector2(300, 240), ImGuiCond.FirstUseEver);
            ImGui.Begin("Nez Core", ref isOpen);
            DrawSettings();
            ImGui.End();
        }

        void DrawSettings()
        {
            _frameRateArray[_frameRateArrayIndex] = ImGui.GetIO().Framerate;
            _frameRateArrayIndex = (_frameRateArrayIndex + 1) % _frameRateArray.Length;

            ImGui.PlotLines("##hidelabel", ref _frameRateArray[0], _frameRateArray.Length, _frameRateArrayIndex,
                $"FPS: {ImGui.GetIO().Framerate:0}", 0, 60, new Num.Vector2(ImGui.GetContentRegionAvail().X, 50));

            RelmImGui.SmallVerticalSpace();

            if (ImGui.CollapsingHeader("Core Settings", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Checkbox("exitOnEscapeKeypress", ref RelmGame.ExitOnEscapeKeypress);
                ImGui.Checkbox("pauseOnFocusLost", ref RelmGame.PauseOnFocusLost);
                ImGui.Checkbox("debugRenderEnabled", ref RelmGame.DebugRenderEnabled);
            }

            if (ImGui.CollapsingHeader("Core.defaultSamplerState", ImGuiTreeNodeFlags.DefaultOpen))
            {
#if !FNA
                ImGui.PushStyleVar(ImGuiStyleVar.Alpha, ImGui.GetStyle().Alpha * 0.5f);
                RelmImGui.DisableNextWidget();
#endif

                var currentTextureFilter = (int)RelmGame.DefaultSamplerState.Filter;
                if (ImGui.Combo("Filter", ref currentTextureFilter, _textureFilters, _textureFilters.Length))
                    RelmGame.DefaultSamplerState.Filter = (TextureFilter)Enum.Parse(typeof(TextureFilter),
                        _textureFilters[currentTextureFilter]);

#if !FNA
                ImGui.PopStyleVar();
#endif
            }
        }
    }
}