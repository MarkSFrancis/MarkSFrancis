﻿namespace Phnx.Security
{
    /// <summary>
    /// An asymmetric encryption algorithm where seperate keys are used for encrypting and decrypting. Suitable for sending data to 3rd parties (such as sending messages), but not suited if the same user is encrypting and decrypting the data. Consider using <see cref="ISymmetricEncryption"/> if you don't need seperate keys for encrypting and decrypting data
    /// </summary>
    public interface IAsymmetricEncryption : IEncryption
    {
        /// <summary>
        /// Create a random pair of keys to use when encrypting (<paramref name="publicKey"/>) and decrypting (<paramref name="privateKey"/>)
        /// </summary>
        /// <param name="keySize">The total size of the asymetric key to generate. This does not guarantee the size of <paramref name="publicKey"/> or <paramref name="privateKey"/></param>
        /// <param name="publicKey">The key to use when encrypting data</param>
        /// <param name="privateKey">The key to use when decrypting data</param>
        void CreateRandomKeys(int keySize, out byte[] publicKey, out byte[] privateKey);
    }
}