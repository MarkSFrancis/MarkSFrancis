﻿using System.Collections.Generic;

namespace Phnx.AspNetCore.Rest.Services
{
    /// <summary>
    /// Provides an interface for mapping from a data model to a data transfer object
    /// </summary>
    /// <typeparam name="TDataModel">The type of data model</typeparam>
    /// <typeparam name="TDtoModel">The type of data transfer object</typeparam>
    public interface IReadonlyResourceMapService<in TDataModel, out TDtoModel>
    {
        /// <summary>
        /// Maps from the data model to a data transfer object
        /// </summary>
        /// <param name="data">The data model to map</param>
        /// <returns><paramref name="data"/> mapped to a data transfer object</returns>
        TDtoModel MapToDto(TDataModel data);

        /// <summary>
        /// Maps from a collection of data models to a collection of data transfer objects
        /// </summary>
        /// <param name="data">The data models to map</param>
        /// <returns><paramref name="data"/> mapped to a collection of data transfer objects</returns>
        IEnumerable<TDtoModel> MapToDto(IEnumerable<TDataModel> data);
    }
}
