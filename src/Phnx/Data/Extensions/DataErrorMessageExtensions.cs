﻿namespace Phnx
{
    /// <summary>
    /// Extends <see cref="ErrorMessage"/> with new messages related to <see cref="Data"/>
    /// </summary>
    public static class DataErrorMessageExtensions
    {
        /// <summary>
        /// Gets the default message for being unable to set a value
        /// </summary>
        /// <param name="_">The factory to extend</param>
        /// <returns>The default message for an empty collection</returns>
        public static string CannotSetValue(this ErrorMessage _)
        {
            return "Cannot set value. Operation not supported";
        }
    }
}
