﻿/*
 * Copyright 2015 Systemic Pty Ltd
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Sif.Framework.Model.Infrastructure;
using Sif.Framework.Model.Settings;
using System;
using System.Configuration;
using Environment = Sif.Framework.Model.Infrastructure.Environment;

namespace Sif.Framework.Utils
{

    /// <summary>
    /// This is a utility class for operations on the Environment object.
    /// </summary>
    static class EnvironmentUtils
    {

        /// <summary>
        /// Create an Environment instance from properties defined in the framework settings.
        /// </summary>
        /// <param name="settings">Framework settings.</param>
        /// <returns>Environment instance.</returns>
        internal static Environment LoadFromSettings(IFrameworkSettings settings)
        {
            Environment merged = new Environment();

            if (merged.ApplicationInfo == null)
            {
                merged.ApplicationInfo = new ApplicationInfo();
            }

            if (String.IsNullOrWhiteSpace(settings.ApplicationKey))
            {
                throw new ConfigurationErrorsException("An applicationKey must be defined in the Provider Environment template.");
            }
            else
            {
                merged.ApplicationInfo.ApplicationKey = settings.ApplicationKey;
            }

            if (!String.IsNullOrWhiteSpace(settings.AuthenticationMethod))
            {
                merged.AuthenticationMethod = settings.AuthenticationMethod;
            }

            if (!String.IsNullOrWhiteSpace(settings.ConsumerName))
            {
                merged.ConsumerName = settings.ConsumerName;
            }

            if (!String.IsNullOrWhiteSpace(settings.DataModelNamespace))
            {
                merged.ApplicationInfo.DataModelNamespace = settings.DataModelNamespace;
            }

            if (!String.IsNullOrWhiteSpace(settings.SupportedInfrastructureVersion))
            {
                merged.ApplicationInfo.SupportedInfrastructureVersion = settings.SupportedInfrastructureVersion;
            }

            return merged;
        }

        /// <summary>
        /// Combine the passed in environment object with the equivalent properties defined in the framework settings
        /// (if present). Properties that already exist in the passed in the environment take precedence over the
        /// properties defined in the framework settings.
        /// </summary>
        /// <param name="environment">Environment object to check.</param>
        /// <param name="settings">Framework settings.</param>
        /// <returns>Environment object with merged properties.</returns>
        internal static Environment MergeWithSettings(Environment environment, IFrameworkSettings settings)
        {
            Environment merged;

            if (environment == null)
            {
                merged = new Environment();
            }
            else
            {
                merged = environment;
            }

            if (merged.ApplicationInfo == null)
            {
                merged.ApplicationInfo = new ApplicationInfo();
            }

            if (String.IsNullOrWhiteSpace(merged.ApplicationInfo.ApplicationKey) && settings.ApplicationKey != null)
            {
                merged.ApplicationInfo.ApplicationKey = settings.ApplicationKey;
            }

            if (String.IsNullOrWhiteSpace(merged.ApplicationInfo.ApplicationKey))
            {
                throw new ArgumentException("An applicationKey must either be provided or defined in the Consumer Environment template.", "applicationKey");
            }

            if (String.IsNullOrWhiteSpace(merged.AuthenticationMethod) && settings.AuthenticationMethod != null)
            {
                merged.AuthenticationMethod = settings.AuthenticationMethod;
            }

            if (String.IsNullOrWhiteSpace(merged.ConsumerName) && settings.ConsumerName != null)
            {
                merged.ConsumerName = settings.ConsumerName;
            }

            if (String.IsNullOrWhiteSpace(merged.ApplicationInfo.DataModelNamespace) && settings.DataModelNamespace != null)
            {
                merged.ApplicationInfo.DataModelNamespace = settings.DataModelNamespace;
            }

            if (String.IsNullOrWhiteSpace(merged.ApplicationInfo.SupportedInfrastructureVersion) && settings.SupportedInfrastructureVersion != null)
            {
                merged.ApplicationInfo.SupportedInfrastructureVersion = settings.SupportedInfrastructureVersion;
            }

            return merged;
        }

        /// <summary>
        /// Parse the Environment object for the service URL.
        /// </summary>
        /// <param name="environment">Environment object to parse.</param>
        /// <returns>Service URL.</returns>
        internal static string ParseServiceUrl(Environment environment)
        {
            string serviceUrl = null;

            if (environment != null &&
                environment.InfrastructureServices != null &&
                environment.InfrastructureServices[InfrastructureServiceNames.requestsConnector] != null &&
                environment.InfrastructureServices[InfrastructureServiceNames.requestsConnector].Value != null)
            {
                serviceUrl = environment.InfrastructureServices[InfrastructureServiceNames.requestsConnector].Value;
            }

            return serviceUrl;
        }

    }

}
