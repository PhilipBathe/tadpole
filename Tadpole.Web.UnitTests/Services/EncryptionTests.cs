﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Tadpole.Web.Services;

namespace Tadpole.Web.UnitTests.Services
{
    [TestFixture]
    public class EncryptionTests
    {
        private Encryption sut;
        private const string constantPassword = "password";

        [SetUp]
        public void SetUp()
        {
            sut = new Encryption();
        }

        [Test]
        public void EncryptShouldReturnSaltAndHash()
        {
            //Act
            var result = sut.Encrypt(constantPassword);

            //Assert
            result.Salt.ShouldNotBeNullOrEmpty();
            result.Hash.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void EncryptSaltShouldAlwaysBeDifferent()
        {
            //Act
            var firstResult = sut.Encrypt(constantPassword);
            var secondResult = sut.Encrypt(constantPassword);

            //Assert
            firstResult.Salt.ShouldNotMatch(secondResult.Salt);
        }

        [Test]
        public void EncryptHashShouldAlwaysBeDifferent()
        {
            //Act
            var firstResult = sut.Encrypt(constantPassword);
            var secondResult = sut.Encrypt(constantPassword);

            //Assert
            firstResult.Hash.ShouldNotMatch(secondResult.Hash);
        }


    }
}
