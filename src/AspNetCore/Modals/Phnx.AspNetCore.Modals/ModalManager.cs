﻿using Microsoft.AspNetCore.Http;
using Phnx.Serialization;
using System.Collections.Generic;

namespace Phnx.AspNetCore.Modals
{
    /// <summary>
    /// A manager hosting and retrieving a list of all the modals to render
    /// </summary>
    /// <typeparam name="TModal">The type of modals that this manager hosts</typeparam>
    public class ModalManager<TModal> : IModalManager<TModal> where TModal : IModalViewModel
    {
        /// <summary>
        /// The current <see cref="ISession"/> to store and load modals
        /// </summary>
        public ISession Session { get; }

        /// <summary>
        /// The <see cref="IHttpContextAccessor"/> to load the current <see cref="Session"/> from
        /// </summary>
        public IHttpContextAccessor ContextAccessor { get; }

        /// <summary>
        /// The key to the part of the session which contains all the modal data
        /// </summary>
        public const string SessionModalsKey = "ModalMessage";

        /// <summary>
        /// Create a new <see cref="ModalManager{TModal}"/> using the <see cref="ISession"/> to hold the modal data
        /// </summary>
        /// <param name="contextAccessor">The session storage in which all modals are stored</param>
        public ModalManager(IHttpContextAccessor contextAccessor)
        {
            ContextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get all the modals currently stored in this session
        /// </summary>
        public List<TModal> Get()
        {
            if (!Session.TryGetValue(SessionModalsKey, out var valueBytes) || valueBytes is null)
            {
                return new List<TModal>();
            }

            var value = JsonSerializer.Deserialize<List<TModal>>(valueBytes);

            return value;
        }

        /// <summary>
        /// Set all the modals currently stored in this session
        /// </summary>
        public void Set(List<TModal> modals)
        {
            if (modals is null || modals.Count == 0)
            {
                Session.Remove(SessionModalsKey);
                return;
            }

            var valueBytes = JsonSerializer.Serialize(modals);

            Session.Set(SessionModalsKey, valueBytes);
        }

        /// <summary>
        /// Clear all the modals from the current session
        /// </summary>
        public void Clear()
        {
            Set(null);
        }
    }
}
