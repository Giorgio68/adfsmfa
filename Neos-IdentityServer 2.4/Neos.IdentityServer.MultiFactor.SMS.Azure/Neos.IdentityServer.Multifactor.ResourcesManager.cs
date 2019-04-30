﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2019 Neos-Sdi (http://www.neos-sdi.com)                                                                                                                                    //                        
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
// https://adfsmfa.codeplex.com                                                                                                                                                             //
// https://github.com/neos-sdi/adfsmfa                                                                                                                                                      //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Neos.IdentityServer.Multifactor.SMS
{
    public class ResourcesLocale
    {
        private CultureInfo resourceCulture;
        private ResourceManager _SHtml;
        private ResourceManager _SAzure;

        /// <summary>
        /// ResourceManager constructor
        /// </summary>
        public ResourcesLocale(int lcid)
        {
            resourceCulture = new CultureInfo(lcid);
        }

        /// <summary>
        /// Culture property implmentation
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
        }

        private ResourceManager GetResourceManager(string resourcename)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)+@"\MFA\ResourceSet" + @"\" + resourcename + "." + Culture.TwoLetterISOLanguageName + ".resources"))
                return ResourceManager.CreateFileBasedResourceManager(resourcename, Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\MFA\ResourceSet", null);
            else
                return new ResourceManager(resourcename, typeof(ResourcesLocale).Assembly);
        }

        /// <summary>
        /// CSHhtml property
        /// </summary>
        private ResourceManager SHtml
        {
            get
            {
                if (_SHtml == null)
                {
                    _SHtml = GetResourceManager("Neos.IdentityServer.Multifactor.SMS.Resources.SHtml");
                }
                return _SHtml;
            }
        }

        /// <summary>
        /// CSErrors property 
        /// </summary>
        private ResourceManager SAzure
        {
            get
            {
                if (_SAzure == null)
                {
                    _SAzure = GetResourceManager("Neos.IdentityServer.Multifactor.SMS.Resources.SAzure");
                }
                return _SAzure;
            }
        }

        /// <summary>
        /// GetString method implementation
        /// </summary>
        public virtual string GetString(ResourcesLocaleKind kind, string name)
        {
            switch (kind)
            {
                case ResourcesLocaleKind.Html:
                    return SHtml.GetString(name, this.Culture);
                case ResourcesLocaleKind.Azure:
                    return SAzure.GetString(name, this.Culture);
                default:
                    return string.Empty;
            }
        }
    }

    public enum ResourcesLocaleKind
    {
        Html = 1,
        Azure = 2
    }
}
