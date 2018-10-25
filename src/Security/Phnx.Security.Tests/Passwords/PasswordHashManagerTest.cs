﻿using NUnit.Framework;
using Phnx.Security.Passwords;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Phnx.Security.Tests.Passwords
{
    [TestFixture]
    public class PasswordHashManagerTest
    {
        public PasswordHashManager NewPasswordManager()
        {
            var mgr = new PasswordHashManager
            {
                { 0, new PasswordHashDefault() }
            };

            return mgr;
        }

        [Test]
        public void HashingAPassword_WithANewSalt_GeneratesHash()
        {
            // Arrange
            PasswordHashManager hashManager = NewPasswordManager();
            string password = "password";

            // Act
            byte[] securedPassword = hashManager.HashWithLatest(password);

            // Assert
            Assert.IsNotEmpty(securedPassword);
        }

        [Test]
        public void CheckingIfPasswordsMatch_WithMatchingPasswords_MatchesPassword()
        {
            PasswordHashManager hashManager = NewPasswordManager();
            string password = "password";

            byte[] securedPassword = hashManager.HashWithLatest(password);
            bool matchesPassword = hashManager.PasswordMatchesHash(password, securedPassword);

            Assert.True(matchesPassword);
        }

        [Test]
        public void CheckingIfPasswordIsLatest_WithOldHash_AsksForRehash()
        {
            PasswordHashManager hashManager = NewPasswordManager();

            string password = "password";

            byte[] securedPassword = hashManager.HashWithLatest(password);

            hashManager.Add(1, new PasswordHashVersionMock());
            bool needsUpdate = hashManager.ShouldUpdateHash(securedPassword);

            Assert.True(needsUpdate);
        }

        [Test]
        public void CheckingIfPasswordIsLatest_WithNewHash_DoesNotAskForRehash()
        {
            PasswordHashManager hashManager = NewPasswordManager();
            hashManager.Add(1, new PasswordHashVersionMock());

            string password = "password";

            byte[] securedPassword = hashManager.HashWithLatest(password);

            bool needsUpdate = hashManager.ShouldUpdateHash(securedPassword);

            Assert.False(needsUpdate);
        }

        [Test]
        public void LoggingIn_WithOldHash_StillLogsIn()
        {
            PasswordHashManager hashManager = NewPasswordManager();

            string password = "password";

            byte[] securedPassword = hashManager.HashWithLatest(password);

            hashManager.Add(1, new PasswordHashVersionMock());

            bool passwordsMatch = hashManager.PasswordMatchesHash(password, securedPassword);

            Assert.True(passwordsMatch);
        }

        [Test]
        public void HashingAPassword_WithBrokenHashGenerator_ThrowsArgumentException()
        {
            var hashManager = new PasswordHashManager
            {
                { 0, new PasswordHashVersionBroken() }
            };

            Assert.Throws<ArgumentException>(() => hashManager.HashWithLatest("testPassword"));
        }

        [Test]
        public void ShouldUpdateHash_WithNullHash_ThrowsArgumentNullException()
        {
            var hashManager = NewPasswordManager();

            Assert.Throws<ArgumentNullException>(() => hashManager.ShouldUpdateHash(null));
        }

        [Test]
        public void ShouldUpdateHash_WithHashMissingVersion_ThrowsArgumentException()
        {
            var hashManager = NewPasswordManager();

            Assert.Throws<ArgumentException>(() => hashManager.ShouldUpdateHash(new byte[0]));
        }

        [Test]
        public void PasswordMatchesHash_WithHashTooLong_ThrowsArgumentException()
        {
            var hashManager = NewPasswordManager();

            Assert.Throws<ArgumentException>(() => hashManager.PasswordMatchesHash("asdf", new byte[200]));
        }

        #region IDictionary Tests
        #endregion
    }
}
