﻿using Microsoft.Extensions.Configuration;
using System;

namespace Phnx.Configuration
{
    /// <summary>
    /// Loads the application configuration, including default values and converting the configuration to the needed types where necessary
    /// </summary>
    public class Config : IConfig
    {
        /// <summary>
        /// Configuration to read from
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Create a new <see cref="Config"/> using <paramref name="configuration"/> as a configuration source
        /// </summary>
        /// <param name="configuration">The configuration source</param>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null"/></exception>
        public Config(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Get a configuration by key as a string
        /// </summary>
        /// <param name="key">The key to the configuration to load</param>
        /// <returns>The value associated with the given configuration key</returns>
        public string this[string key] => Configuration[key];

        /// <summary>
        /// Get a configuration by key as <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the value for the key to load</typeparam>
        /// <param name="key">The key to the configuration to load</param>
        /// <returns>The value associated with the given configuration key, converted to <typeparamref name="T"/></returns>
        /// <exception cref="InvalidCastException">An error occured either getting the converter from <see cref="string"/> to <typeparamref name="T"/>, or an error occured running the converter. Check the inner exception for details on what went wrong in the converter</exception>
        public T Get<T>(string key)
        {
            try
            {
                var defaultConverter = ConverterHelpers.GetDefaultConverter<string, T>();
                return defaultConverter(this[key]);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Error casting from {typeof(string)} to {typeof(T)}", ex);
            }
        }

        /// <summary>
        /// Get a configuration by key as <typeparamref name="T"/>. If the configuration is missing, or the conversion fails, it will return <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">The type of the value for the key to load</typeparam>
        /// <param name="key">The key to the configuration to load</param>
        /// <param name="defaultValue">The default value to return if the value was not found, or could not be parsed</param>
        /// <returns>The value associated with the given configuration key, converted to <typeparamref name="T"/></returns>
        public T Get<T>(string key, T defaultValue)
        {
            if (TryGet(key, out T value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Try to get a configuration by key as <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the value for the key to load</typeparam>
        /// <param name="key">The key to the configuration to load</param>
        /// <param name="value">The value associated with the given configuration key, converted to <typeparamref name="T"/></param>
        /// <returns><see langword="true"/> if the value was successfully loaded and converted, otherwise <see langword="false"/></returns>
        public bool TryGet<T>(string key, out T value)
        {
            var keyValue = this[key];

            if (keyValue == null)
            {
                value = default(T);
                return false;
            }

            try
            {
                var defaultConverter = ConverterHelpers.GetDefaultConverter<string, T>();
                value = defaultConverter(keyValue);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// Get a connection string from the configuration, using the "ConnectionStrings" section of the configuration
        /// </summary>
        /// <param name="key">The key to the connection string to load</param>
        /// <returns>The connection string associated with the given key</returns>
        public string GetConnectionString(string key)
        {
            return Configuration.GetConnectionString(key);
        }
    }
}
