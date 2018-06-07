﻿using MarkSFrancis.AspNet.Core.Rest.Models;

namespace MarkSFrancis.AspNet.Core.Rest.Services.Interfaces
{
    /// <summary>
    /// Provides an interface for mapping between a data model and a data transfer object
    /// </summary>
    /// <typeparam name="TDataModel">The type of data model</typeparam>
    /// <typeparam name="TDtoModel">The type of data transfer object</typeparam>
    /// <typeparam name="TDtoLinksModel">The type of links contained within the data transfer object</typeparam>
    /// <typeparam name="TPatchDtoModel">The type of data transfer object used when patching</typeparam>
    public interface IResourceMapService<TDataModel, TDtoModel, TDtoLinksModel, in TPatchDtoModel>
        : IReadonlyResourceMapService<TDataModel, TDtoModel, TDtoLinksModel>
        where TDtoModel : IHateoasDtoModel<TDtoLinksModel>
        where TDtoLinksModel : ILinksDtoModel
    {
        /// <summary>
        /// Maps from the data transfer object to a data model
        /// </summary>
        /// <param name="dto">The data transfer object to map</param>
        /// <returns><paramref name="dto"/> mapped to a data model</returns>
        TDataModel MapToData(TDtoModel dto);

        /// <summary>
        /// Patches an existing data model using a patch data transfer object
        /// </summary>
        /// <param name="patch">The data transfer object to patch onto <paramref name="data"/></param>
        /// <param name="data">The data model to patch</param>
        void PatchToData(TPatchDtoModel patch, TDataModel data);
    }
}