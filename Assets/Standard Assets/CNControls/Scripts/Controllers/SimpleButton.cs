﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
    /// <summary>
    /// Simple button class
    /// Handles press, hold and release, just like a normal button
    /// </summary>
	public class SimpleButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler,IDragHandler
    {
        /// <summary>
        /// The name of the button
        /// </summary>
        public string ButtonName = "Jump";

        /// <summary>
        /// Utility object that is registered in the system
        /// </summary>
        private VirtualButton _virtualButton;
        
		public SimpleJoystick SimpleJoystick;
		public ToEnableDisable Function;

        /// <summary>
        /// It's pretty simple here
        /// When we enable, we register our button in the input system
        /// </summary>
        private void OnEnable()
        {
            _virtualButton = _virtualButton ?? new VirtualButton(ButtonName);
            CnInputManager.RegisterVirtualButton(_virtualButton);
        }

        /// <summary>
        /// When we disable, we unregister our button
        /// </summary>
        private void OnDisable()
        {
            CnInputManager.UnregisterVirtualButton(_virtualButton);
        }

        /// <summary>
        /// uGUI Event system stuff
        /// It's also utilised by the editor input helper
        /// </summary>
        /// <param name="eventData">Data of the passed event</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            _virtualButton.Release();
			if (SimpleJoystick)
				SimpleJoystick.OnPointerUp (eventData);
			if (Function)
				Function.OnPressed ();
			print ("Button called1");
        }

        /// <summary>
        /// uGUI Event system stuff
        /// It's also utilised by the editor input helper
        /// </summary>
        /// <param name="eventData">Data of the passed event</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            _virtualButton.Press();
			if (SimpleJoystick)
				SimpleJoystick.OnPointerDown (eventData);
        }

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (SimpleJoystick)
				SimpleJoystick.OnDrag (eventData);
		}
    }
}