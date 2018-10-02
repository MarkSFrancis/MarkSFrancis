﻿using MarkSFrancis;

namespace Phnx.Security.Passwords.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ErrorFactory"/>
    /// </summary>
    public static class ErrorFactoryExtensions
    {
        /// <summary>
        /// Create an error describing Duplicate Hash Versions in the <see cref="PasswordHashManager"/>
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="hashVersion">The hash version for which duplicates have been made</param>
        /// <returns></returns>
        public static DuplicateHashVersionException DuplicateHashVersion(this ErrorFactory factory, int hashVersion)
        {
            return new DuplicateHashVersionException($"{hashVersion} is already configured and cannot be re-added");
        }
    }
}
