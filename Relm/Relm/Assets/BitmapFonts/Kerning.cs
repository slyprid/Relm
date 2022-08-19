using System;

namespace Relm.Assets.BitmapFonts
{
	public struct Kerning 
        : IEquatable<Kerning>
    {
        public int Amount;

        public readonly char FirstCharacter;
        public readonly char SecondCharacter;

        public Kerning(char firstCharacter, char secondCharacter, int amount)
        {
            FirstCharacter = firstCharacter;
            SecondCharacter = secondCharacter;
            Amount = amount;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Kerning))
                return false;

            return Equals((Kerning)obj);
        }

        public bool Equals(Kerning other) => FirstCharacter == other.FirstCharacter && SecondCharacter == other.SecondCharacter;

        public override int GetHashCode() => (FirstCharacter << 16) | SecondCharacter;

        public override string ToString() => string.Format("{0} to {1} = {2}", FirstCharacter, SecondCharacter, Amount);
    }
}