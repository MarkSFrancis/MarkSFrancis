﻿namespace Phnx.Security.Passwords
{
    /// <summary>
    /// A Password Hash ready for use by the <see cref="PasswordHashManager"/>
    /// </summary>
    public interface IPasswordHash
    {
        /// <summary>
        /// The length of the hash produced by this algorithm
        /// </summary>
        int HashBytesLength { get; }

        /// <summary>
        /// The length of the salt needed/ generated by this algorithm
        /// </summary>
        int SaltBytesLength { get; }

        /// <summary>
        /// Generate a hash using this algorithm
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt to use</param>
        /// <returns></returns>
        byte[] GenerateHash(byte[] password, byte[] salt);

        /// <summary>
        /// Generate a salt for use with this algorithm
        /// </summary>
        /// <returns></returns>
        byte[] GenerateSalt();
    }
}