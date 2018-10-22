﻿using System.Security.Cryptography;

namespace Phnx.Security
{
    /// <summary>
    /// A 4096 bit RSA asymmetric encryption algorithm
    /// </summary>
    public class RsaEncryption : IAsymmetricEncryption
    {
        /// <summary>
        /// Create random secure keys for use by <see cref="Encrypt"/> and <see cref="Decrypt"/>
        /// </summary>
        /// <param name="keySize">The size of the asymetric key to generate. This does not guarantee the size of <paramref name="publicKey"/> or <paramref name="privateKey"/></param>
        /// <param name="publicKey">The generated public key</param>
        /// <param name="privateKey">The generated private key</param>
        public void CreateRandomKeys(int keySize, out byte[] publicKey, out byte[] privateKey)
        {
            var provider = new RSACryptoServiceProvider(keySize);

            publicKey = provider.ExportCspBlob(false);

            privateKey = provider.ExportCspBlob(true);
        }

        /// <summary>
        /// Encrypt data
        /// </summary>
        /// <param name="data">The data to encrypt</param>
        /// <param name="publicKey">The public key to use when encrypting the data</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data, byte[] publicKey)
        {
            var rsaServiceProvider = new RSACryptoServiceProvider();
            rsaServiceProvider.ImportCspBlob(publicKey);

            return rsaServiceProvider.Encrypt(data, true);
        }

        /// <summary>
        /// Decrypt data
        /// </summary>
        /// <param name="encryptedData">The data to decrypt</param>
        /// <param name="privateKey">The private key to use when decrypting the data</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] encryptedData, byte[] privateKey)
        {
            var rsaServiceProvider = new RSACryptoServiceProvider();
            rsaServiceProvider.ImportCspBlob(privateKey);

            return rsaServiceProvider.Decrypt(encryptedData, true);
        }
    }
}
