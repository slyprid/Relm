using Relm.Components.Physics.Colliders;

namespace Relm.Components.Physics
{
    public interface ITriggerListener
    {
        void OnTriggerEnter(Collider other, Collider local);
        void OnTriggerExit(Collider other, Collider local);
    }
}