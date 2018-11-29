﻿using Microsoft.AspNetCore.Mvc;
using Phnx.AspNetCore.Rest.Models;
using Phnx.AspNetCore.Rest.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Phnx.AspNetCore.Rest.Services
{
    /// <summary>
    /// Formulates various REST compliant responses
    /// </summary>
    /// <typeparam name="TDataModel">The data model type</typeparam>
    /// <typeparam name="TDtoModel">The data transfer object type</typeparam>
    /// <typeparam name="TDtoLinksModel">The data transfer object type's HATEOAS links</typeparam>
    public class RestResponseFactory<TDataModel, TDtoModel, TDtoLinksModel> : IRestResponseService<TDataModel, TDtoModel, TDtoLinksModel>
        where TDataModel : IResourceDataModel
        where TDtoModel : IHateoasDtoModel<TDtoLinksModel>
        where TDtoLinksModel : ILinksDtoModel
    {
        /// <summary>
        /// The configured response mapper
        /// </summary>
        public IReadonlyResourceMapService<TDataModel, TDtoModel, TDtoLinksModel> Mapper { get; }

        /// <summary>
        /// The ETag reader service
        /// </summary>
        public IETagService ETagService { get; }

        /// <summary>
        /// Create a new REST response factory
        /// </summary>
        /// <param name="mapper">The response mapper to map from the <typeparamref name="TDataModel"/> to <typeparamref name="TDtoModel"/></param>
        /// <param name="eTagService">The E-Tag writer</param>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> or <paramref name="eTagService"/> is <see langword="null"/></exception>
        public RestResponseFactory(IReadonlyResourceMapService<TDataModel, TDtoModel, TDtoLinksModel> mapper, IETagService eTagService)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ETagService = eTagService ?? throw new ArgumentNullException(nameof(eTagService));
        }

        /// <summary>
        /// Create a response describing that the data could not be found
        /// </summary>
        /// <param name="resourceTypeFriendlyName">The resource type's friendly name</param>
        /// <param name="resourceIdentifier">The unique identifier for the resource</param>
        /// <returns>A response describing that the data could not be found</returns>
        public NotFoundObjectResult NotFound(string resourceTypeFriendlyName, string resourceIdentifier)
        {
            if (string.IsNullOrWhiteSpace(resourceTypeFriendlyName))
            {
                resourceTypeFriendlyName = "resource";
            }

            string message;
            if (string.IsNullOrWhiteSpace(resourceIdentifier))
            {
                message = $"The requested {resourceTypeFriendlyName} was not found";
            }
            else
            {
                message = $"The requested {resourceTypeFriendlyName} \"{resourceIdentifier}\" was not found";
            }

            return new NotFoundObjectResult(message);
        }

        /// <summary>
        /// Create a response describing that the data has not been changed (ETag)
        /// </summary>
        /// <returns>A response describing that the data has not been changed</returns>
        public StatusCodeResult DataHasNotChanged()
        {
            return ETagService.CreateMatchResponse();
        }

        /// <summary>
        /// Create a response with retrieved data in, mapped to a data transfer object
        /// </summary>
        /// <param name="data">The data that was retrieved</param>
        /// <returns>
        /// A response with retrieved data in, mapped to a data transfer object
        /// </returns>
        public OkObjectResult RetrievedData(TDataModel data)
        {
            ETagService.AddETagToResponse(data);

            var model = Mapper.MapToDto(data);

            return new OkObjectResult(model);
        }

        /// <summary>
        /// Create a response with a collection of retrieved data in, mapped to data transfer objects
        /// </summary>
        /// <param name="data">The data that was retrieved</param>
        /// <returns>
        /// A response with a collection of retrieved data in, mapped to data transfer objects
        /// </returns>
        public OkObjectResult RetrievedData(IEnumerable<TDataModel> data)
        {
            var model = Mapper.MapToDto(data);

            return new OkObjectResult(model);
        }

        /// <summary>
        /// Create a response describing that the data was successfully created
        /// </summary>
        /// <param name="data">The data that was created</param>
        /// <returns>A response describing that the data was successfully created</returns>
        public CreatedResult CreatedData(TDataModel data)
        {
            ETagService.AddETagToResponse(data);

            var createdModel = Mapper.MapToDto(data);

            return new CreatedResult(createdModel.Links.Self, createdModel);
        }

        /// <summary>
        /// Create a response describing that the data has been updated (ETag)
        /// </summary>
        /// <returns>A response describing that the data has been updated</returns>
        public StatusCodeResult DataHasChanged()
        {
            return ETagService.CreateDoNotMatchResponse();
        }

        /// <summary>
        /// Create a response with updated data in, mapped to a data transfer object
        /// </summary>
        /// <param name="data">The data that was updated</param>
        /// <returns>
        /// A response with updated data in, mapped to a data transfer object
        /// </returns>
        public OkObjectResult UpdatedData(TDataModel data)
        {
            ETagService.AddETagToResponse(data);

            var editedModel = Mapper.MapToDto(data);

            return new OkObjectResult(editedModel);
        }

        /// <summary>
        /// Create a response describing that the data has been deleted
        /// </summary>
        /// <returns>A response describing that the data has been deleted</returns>
        public NoContentResult DeletedData()
        {
            return new NoContentResult();
        }
    }
}
