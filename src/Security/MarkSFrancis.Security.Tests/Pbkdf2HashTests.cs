﻿using System.Text;
using NUnit.Framework;

namespace MarkSFrancis.Security.Tests
{
    public class Pbkdf2HashTests
    {
        public Pbkdf2HashTests()
        {
            Pbkdf2Hash = new Pbkdf2Hash();
        }

        private Pbkdf2Hash Pbkdf2Hash { get; }

        [Test]
        public void EncryptingText_WithMessage_CreatesUnreadableBytes()
        {
            // Arrange
            byte[] salt = Pbkdf2Hash.GenerateSalt();

            string plainText = "This is an awkwardly long message that must be secured in a meaningful way";
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // Act
            var result = Pbkdf2Hash.Hash(plainBytes, salt);

            // Assert
            Assert.AreNotEqual(plainBytes, result);
        }
    }
}
