﻿using System.Security.Cryptography;

namespace MarkSFrancis.Security
{
    public static class SecureRandomBytes
    {
        public static byte[] Generate(int numberOfBytesToGenerate)
        {
            var rnd = RandomNumberGenerator.Create();

            byte[] rndBytes = new byte[numberOfBytesToGenerate];
            rnd.GetBytes(rndBytes);

            return rndBytes;
        }
    }
}
