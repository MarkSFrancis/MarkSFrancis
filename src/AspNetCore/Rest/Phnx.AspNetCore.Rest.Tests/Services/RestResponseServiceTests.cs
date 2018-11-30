﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Phnx.AspNetCore.Rest.Models;
using Phnx.AspNetCore.Rest.Services;
using Phnx.AspNetCore.Rest.Tests.Fakes;
using System;
using System.Net;

namespace Phnx.AspNetCore.Rest.Tests.Services
{
    public class RestResponseServiceTests
    {
        private RestResponseFactory<FakeResource, FakeDto> CreateFake(Mock<IETagService> eTagService = null)
        {
            var mapper = new FakeResourceMap();
            if (eTagService is null)
            {
                eTagService = new Mock<IETagService>();
            }

            return new RestResponseFactory<FakeResource, FakeDto>(mapper, eTagService.Object);
        }

        [Test]
        public void New_WithNullMapper_ThrowsArgumentNullException()
        {
            var eTagService = new Mock<IETagService>();

            Assert.Throws<ArgumentNullException>(() => new RestResponseFactory<FakeResource, object>(null, eTagService.Object));
        }

        [Test]
        public void New_WithNullETagService_ThrowsArgumentNullException()
        {
            var mapper = new FakeResourceMap();

            Assert.Throws<ArgumentNullException>(() => new RestResponseFactory<FakeResource, object>(mapper, null));
        }

        [Test]
        public void New_NonNullMapperAndETagService_SetsETagServiceAndMapper()
        {
            var mapper = new FakeResourceMap();
            var eTagService = new Mock<IETagService>();

            var responseService = new RestResponseFactory<FakeResource, object>(mapper, eTagService.Object);

            Assert.AreEqual(eTagService.Object, responseService.ETagService);
            Assert.AreEqual(mapper, responseService.Mapper);
        }

        [Test]
        public void NotFound_WithNullTypeNameAndIdentifier_DoesNotThrow()
        {
            var service = CreateFake();
            Assert.DoesNotThrow(() => service.NotFound(null, null));
        }

        [Test]
        public void NotFound_WithTypeNameAndIdentifier_DoesNotThrow()
        {
            var service = CreateFake();
            Assert.DoesNotThrow(() => service.NotFound("type", "id"));
        }

        [Test]
        public void DataHasNotChanged_ReturnsNotModified()
        {
            var eTagService = new Mock<IETagService>();
            eTagService.Setup(e =>
                e.CreateMatchResponse())
            .Returns(() =>
                new StatusCodeResult((int)HttpStatusCode.NotModified));

            var service = CreateFake(eTagService);

            var result = service.DataHasNotChanged();

            Assert.AreEqual((int)HttpStatusCode.NotModified, result.StatusCode);
        }

        [Test]
        public void RetrievedData_WithNullData_DoesNotThrow()
        {
            var mockETagService = new Mock<IETagService>();
            FakeResource fake = null;

            var requestService = CreateFake(mockETagService);

            Assert.DoesNotThrow(() => requestService.RetrievedData(fake));
        }

        [Test]
        public void RetrievedData_WithValidData_AddsStatusCodeOkay()
        {
            var resource = new FakeResource(12);

            var mockETagService = new Mock<IETagService>();
            mockETagService
                .Setup(e =>
                    e.AddETagToResponse(It.IsAny<IResourceDataModel>()));

            var requestService = CreateFake(mockETagService);

            var result = requestService.RetrievedData(resource);

            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void RetrievedData_WithValidData_AddsETagAndMaps()
        {
            var resource = new FakeResource(12);

            var mockETagService = new Mock<IETagService>();
            mockETagService
                .Setup(e =>
                    e.AddETagToResponse(It.IsAny<IResourceDataModel>()));

            var requestService = CreateFake(mockETagService);

            var result = requestService.RetrievedData(resource);

            var resultContent = (FakeDto)result.Value;

            mockETagService
                .Verify(e =>
                    e.AddETagToResponse(It.IsAny<IResourceDataModel>()), Times.Once);

            Assert.AreEqual(resource.Id, resultContent.Id);
        }
    }
}
