namespace Relm.Input
{
    internal class KeyboardSettings
    {
        public bool RepeatPress { get; set; }
        public int InitialDelay { get; set; }
        public int RepeatDelay { get; set; }

        public KeyboardSettings()
        {
            RepeatPress = true;
            InitialDelay = 500;
            RepeatDelay = 50;
        }
    }
}