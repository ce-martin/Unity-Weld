﻿using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityUI.Binding;

namespace UnityUI.Binding
{
    /// <summary>
    /// Class for binding Unity UI events to methods in a view model.
    /// </summary>
    public class EventBinding : AbstractMemberBinding
    {
        /// <summary>
        /// Name of the method in the view model to bind to.
        /// </summary>
        public string viewModelMethodName;

        /// <summary>
        /// Type of the component we're binding to.
        /// Must be a string so because Types can't be serialised in the scene.
        /// </summary>
        public string boundComponentType;

        /// <summary>
        /// Name of the event to bind to.
        /// </summary>
        public string uiEventName;

        /// <summary>
        /// Watches a Unity event for updates.
        /// </summary>
        private UnityEventWatcher eventWatcher;

        /// <summary>
        /// Cached view-model, after connection.
        /// </summary>
        private object viewModel;

        /// <summary>
        /// Cached method to call on the view-model.
        /// </summary>
        private MethodInfo viewModelMethod;

        public override void Connect()
        {
            viewModel = GetViewModelBinding().BoundViewModel;
            viewModelMethod = viewModel.GetType().GetMethod(viewModelMethodName, new Type[0]);

            eventWatcher = new UnityEventWatcher(GetComponent(boundComponentType), uiEventName, 
                () => viewModelMethod.Invoke(viewModel, new object[0])
            );
        }

        public override void Disconnect()
        {
            if (eventWatcher != null)
            {
                eventWatcher.Dispose();
                eventWatcher = null;
            }

            viewModel = null;
            viewModelMethod = null;
        }
    }
}
