using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
	public class VirtualIntegerAxis : VirtualInput
	{
		public List<VirtualAxis.Node> Nodes = new List<VirtualAxis.Node>();

		public int Value
		{
			get
			{
				for (var i = 0; i < Nodes.Count; i++)
				{
					var val = Nodes[i].Value;
					if (val != 0)
						return System.Math.Sign(val);
				}

				return 0;
			}
		}


		public VirtualIntegerAxis() { }


		public VirtualIntegerAxis(params VirtualAxis.Node[] nodes)
		{
			Nodes.AddRange(nodes);
		}


		public override void Update()
		{
			for (var i = 0; i < Nodes.Count; i++)
				Nodes[i].Update();
		}


		#region Node management

		public VirtualIntegerAxis AddGamePadLeftStickX(int gamepadIndex = 0, float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			Nodes.Add(new VirtualAxis.GamePadLeftStickX(gamepadIndex, deadzone));
			return this;
		}


		public VirtualIntegerAxis AddGamePadLeftStickY(int gamepadIndex = 0, float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			Nodes.Add(new VirtualAxis.GamePadLeftStickY(gamepadIndex, deadzone));
			return this;
		}


		public VirtualIntegerAxis AddGamePadRightStickX(int gamepadIndex = 0, float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			Nodes.Add(new VirtualAxis.GamePadRightStickX(gamepadIndex, deadzone));
			return this;
		}


		public VirtualIntegerAxis AddGamePadRightStickY(int gamepadIndex = 0, float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			Nodes.Add(new VirtualAxis.GamePadRightStickY(gamepadIndex, deadzone));
			return this;
		}


		public VirtualIntegerAxis AddGamePadDPadUpDown(int gamepadIndex = 0)
		{
			Nodes.Add(new VirtualAxis.GamePadDpadUpDown(gamepadIndex));
			return this;
		}


		public VirtualIntegerAxis AddGamePadDPadLeftRight(int gamepadIndex = 0)
		{
			Nodes.Add(new VirtualAxis.GamePadDpadLeftRight(gamepadIndex));
			return this;
		}


		public VirtualIntegerAxis AddKeyboardKeys(OverlapBehavior overlapBehavior, Keys negative, Keys positive)
		{
			Nodes.Add(new VirtualAxis.KeyboardKeys(overlapBehavior, negative, positive));
			return this;
		}

		#endregion


		public static implicit operator int(VirtualIntegerAxis axis)
		{
			return axis.Value;
		}
	}
}