namespace Relm.Core
{
    public class GlobalManager
    {
        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set => SetEnabled(value);
        }

        public void SetEnabled(bool isEnabled)
        {
            if (_enabled == isEnabled) return;
            _enabled = isEnabled;

            if (_enabled)
                OnEnabled();
            else
                OnDisabled();
        }
        
        public virtual void OnEnabled() { }
        public virtual void OnDisabled() { }
        public virtual void Update() { }
    }
}