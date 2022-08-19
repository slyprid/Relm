using System;
using Relm.Components;

namespace Relm.Renderers.Renderables
{
    public abstract class RenderableComponent
        : Component, IRenderable, IComparable<RenderableComponent>
    {
    }
}