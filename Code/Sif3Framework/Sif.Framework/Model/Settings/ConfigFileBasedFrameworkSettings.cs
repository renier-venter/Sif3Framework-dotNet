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
using System;
using System.Configuration;

namespace Sif.Framework.Model.Settings
{

    /// <summary>
    /// This class represents settings that are stored in the SifFramework.config custom configuration file.
    /// </summary>
    abstract class ConfigFileBasedFrameworkSettings : IFrameworkSettings
    {
        private Configuration configuration;

        /// <summary>
        /// Retrieve the setting (boolean) value associated with the key.
        /// </summary>
        /// <param name="key">Key (identifier) of the setting.</param>
        /// <param name="defaultValue">Value returned if key not found.</param>
        /// <returns>Setting (boolean) value associated with the key if found; default value otherwise.</returns>
        private bool GetBooleanSetting(string key, bool defaultValue)
        {
            KeyValueConfigurationElement setting = configuration.AppSettings.Settings[key];
            bool value = defaultValue;

            if (setting != null)
            {

                try
                {
                    value = Boolean.Parse(setting.Value);
                }
                catch (FormatException)
                {
                    string message = String.Format("The valid values for the {0} setting are \"true\" or \"false\". The value \"{1}\" is not valid.", setting.Key, setting.Value);
                    throw new ConfigurationErrorsException(message);
                }

            }

            return value;
        }

        /// <summary>
        /// Retrieve the setting (int) value associated with the key.
        /// </summary>
        /// <param name="key">Key (identifier) of the setting.</param>
        /// <param name="defaultValue">Value returned if key not found.</param>
        /// <returns>Setting (int) value associated with the key if found; default value otherwise.</returns>
        private int GetIntegerSetting(string key, int defaultValue)
        {
            KeyValueConfigurationElement setting = configuration.AppSettings.Settings[key];
            int value = defaultValue;

            if (setting != null)
            {

                try
                {
                    value = Int32.Parse(setting.Value);
                }
                catch (FormatException)
                {
                    string message = String.Format("The value \"{1}\" is not a valid integer for the {0} setting.", setting.Key, setting.Value);
                    throw new ConfigurationErrorsException(message);
                }

            }

            return value;
        }

        /// <summary>
        /// Retrieve the setting (string) value associated with the key.
        /// </summary>
        /// <param name="key">Key (identifier) of the setting.</param>
        /// <returns>Setting (string) value associated with the key.</returns>
        private string GetStringSetting(string key)
        {
            KeyValueConfigurationElement setting = configuration.AppSettings.Settings[key];
            string value = null;

            if (setting != null)
            {
                value = setting.Value;
            }

            return value;
        }

        /// <summary>
        /// Prefix associated will all settings.
        /// </summary>
        protected abstract string SettingsPrefix { get; }

        /// <summary>
        /// Create an instance of this class based upon the values stored in the SifFramework.config custom
        /// configuration file.
        /// </summary>
        protected ConfigFileBasedFrameworkSettings()
        {
            string configurationFilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/SifFramework.config");

            if (configurationFilePath == null)
            {
                configurationFilePath = "SifFramework.config";
            }

            ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = configurationFilePath;
            configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);

            if (!configuration.HasFile)
            {
                string message = String.Format("Missing configuration file {0}.", configurationFilePath);
                throw new ConfigurationErrorsException(message);
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.ApplicationKey"/>
        /// </summary>
        public string ApplicationKey
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.applicationKey");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.AuthenticationMethod"/>
        /// </summary>
        public string AuthenticationMethod
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.authenticationMethod");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.ConsumerName"/>
        /// </summary>
        public string ConsumerName
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.consumerName");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.DataModelNamespace"/>
        /// </summary>
        public string DataModelNamespace
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.dataModelNamespace");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.DeleteOnUnregister"/>
        /// </summary>
        public bool DeleteOnUnregister
        {

            get
            {
                return GetBooleanSetting(SettingsPrefix + ".environment.deleteOnUnregister", false);
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.EnvironmentType"/>
        /// </summary>
        public EnvironmentType EnvironmentType
        {

            get
            {
                KeyValueConfigurationElement setting = configuration.AppSettings.Settings[SettingsPrefix + ".environmentType"];
                EnvironmentType value = EnvironmentType.DIRECT;

                if (setting != null)
                {

                    if (EnvironmentType.BROKERED.ToString().Equals(setting.Value.ToUpper()))
                    {
                        value = EnvironmentType.BROKERED;
                    }
                    else if (EnvironmentType.DIRECT.ToString().Equals(setting.Value.ToUpper()))
                    {
                        value = EnvironmentType.DIRECT;
                    }
                    else
                    {
                        string message = String.Format("The valid values for the {0} setting are \"BROKERED\" or \"DIRECT\". The value \"{1}\" is not valid.", setting.Key, setting.Value);
                        throw new ConfigurationErrorsException(message);
                    }

                }

                return value;
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.EnvironmentUrl"/>
        /// </summary>
        public string EnvironmentUrl
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.url");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.InstanceId"/>
        /// </summary>
        public string InstanceId
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.instanceId");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.NavigationPageSize"/>
        /// </summary>
        public int NavigationPageSize
        {

            get
            {
                return GetIntegerSetting(SettingsPrefix + ".paging.navigationPageSize", 100);
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.SharedSecret"/>
        /// </summary>
        public string SharedSecret
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.sharedSecret");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.SolutionId"/>
        /// </summary>
        public string SolutionId
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.solutionId");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.SupportedInfrastructureVersion"/>
        /// </summary>
        public string SupportedInfrastructureVersion
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.supportedInfrastructureVersion");
            }

        }

        /// <summary>
        /// <see cref="Sif.Framework.Model.Settings.IFrameworkSettings.UserToken"/>
        /// </summary>
        public string UserToken
        {

            get
            {
                return GetStringSetting(SettingsPrefix + ".environment.template.userToken");
            }

        }

    }

}
