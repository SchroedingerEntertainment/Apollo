// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    public static partial class FileDescriptorExtensions
    {
        /// <summary>
        /// Loads a package info from this file's content if possible
        /// </summary>
        /// <param name="logger">A logging interface to write parser errors to</param>
        /// <param name="result">The resulting package info if processing succeeded</param>
        /// <returns>True if the file has successfully been processed, false otherwise</returns>
        public static bool GetPackage(this FileDescriptor file, ILogSystem logger, out PackageMeta result)
        {
            try
            {
                JsonDocument document = new JsonDocument();
                using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    if (document.Load(fs))
                    {
                        result = new PackageMeta();
                        PropertyMapper.Assign(result, document, true, true);

                        return true;
                    }
                    else if (logger != null)
                    {
                        foreach (string error in document.Errors)
                            logger.Error(error);
                    }
            }
            catch (Exception er)
            {
                if (logger != null)
                    logger.Error(er.Message);
            }
            result = null;
            return false;
        }
        /// <summary>
        /// Loads a package info from this file's content if possible
        /// </summary>
        /// <param name="result">The resulting package info if processing succeeded</param>
        /// <returns>True if the file has successfully been processed, false otherwise</returns>
        public static bool GetPackage(this FileDescriptor file, out PackageMeta result)
        {
            return GetPackage(file, out result);
        }
    }
}
