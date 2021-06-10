﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2021 @redhook62 (adfsmfa@gmail.com)                                                                                                                                    //                        
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
//                                                                                                                                                             //
// https://github.com/neos-sdi/adfsmfa                                                                                                                                                      //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//
using Microsoft.IdentityServer.Web.Authentication.External;
using Neos.IdentityServer.MultiFactor.Common;
using Neos.IdentityServer.MultiFactor.Resources;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace Neos.IdentityServer.MultiFactor
{
    public class AdapterPresentationDefault : BaseMFAPresentation
    {
        private const string CR = "\r\n";

        #region Constructors
        /// <summary>
        /// Constructor implementation
        /// </summary>
        public AdapterPresentationDefault(AuthenticationProvider provider, IAuthenticationContext context) : base(provider, context)
        {
        }

        /// <summary>
        /// Constructor overload implementation
        /// </summary>
        public AdapterPresentationDefault(AuthenticationProvider provider, IAuthenticationContext context, string message) : base(provider, context, message)
        {
        }

        /// <summary>
        /// Constructor overload implementation
        /// </summary>
        public AdapterPresentationDefault(AuthenticationProvider provider, IAuthenticationContext context, string message, bool ismessage) : base(provider, context, message, ismessage)
        {
        }

        /// <summary>
        /// Constructor overload implementation
        /// </summary>
        public AdapterPresentationDefault(AuthenticationProvider provider, IAuthenticationContext context, ProviderPageMode suite) : base(provider, context, suite)
        {
        }

        /// <summary>
        /// Constructor overload implementation
        /// </summary>
        public AdapterPresentationDefault(AuthenticationProvider provider, IAuthenticationContext context, string message, ProviderPageMode suite, bool disableoptions = false)
            : base(provider, context, message, suite, disableoptions)
        {
        }
        #endregion 

        #region CSS
        /// <summary>
        /// GetFormPreRenderHtmlCSS implementation
        /// </summary>
        protected override string GetFormPreRenderHtmlCSS(AuthenticationContext usercontext, bool removetag = false)
        {
            string result = string.Empty;

            result += "#buttonquit, #gotoinscription {";
            result += "border:none;";
            result += "background-color:rgb(232, 17, 35);";
            result += "min-width:120px;";
            result += "width:auto;";
            result += "height:30px;";
            result += "padding:4px 20px 6px 20px;";
            result += "border-style:solid;";
            result += "border-width:1px;";
            result += "transition:background 0s;";
            result += "color:rgb(255, 255, 255);";
            result += "cursor:pointer;";
            result += "margin-bottom:8px;";
            result += "-ms-user-select:none;";
            result += "-moz-transition:background 0s;";
            result += "-webkit-transition:background 0s;";
            result += "-o-transition:background 0s;";
            result += "-webkit-touch-callout:none;";
            result += "-webkit-user-select:none;";
            result += "-khtml-user-select:none;";
            result += "-moz-user-select: none;";
            result += "-o-user-select: none;";
            result += "user-select:none;";
            result += "}" + CR + CR;

            result += "#buttonquit:hover {";
            result += "background-color:rgb(170, 0, 0);";
            result += "}" + CR+ CR;

            string baseresult = base.GetFormPreRenderHtmlCSS(usercontext, true);
            if (!removetag)
                return "<style>" + baseresult + result + "</style>" + CR + CR;
            else
                return result ;
        }
        #endregion

        #region Identification
        /// <summary>
        /// GetFormPreRenderHtmlIdentification implementation
        /// </summary>
        public override string GetFormPreRenderHtmlIdentification(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function SetLinkData(frm, data)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   if (lnk)" + CR;
            result += "   {" + CR;
            result += "      lnk.value = data;" + CR;
            result += "      frm.submit();" + CR;
            result += "      return true;" + CR;
            result += "   }" + CR;
            result += "   return false;" + CR;
            result += "}" + CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlIdentification implementation
        /// </summary>
        public override string GetFormHtmlIdentification(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"IdentificationForm\" >";
            IExternalProvider prov = RuntimeAuthProvider.GetProvider(usercontext.PreferredMethod);
            if ((prov != null) && (prov.IsUIElementRequired(usercontext, RequiredMethodElements.CodeInputRequired)))
            {
                result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetUILabel(usercontext) + "</div>";
                result += "<input id=\"##ACCESSCODE##\" name=\"##ACCESSCODE##\" type=\"password\" placeholder=\"Code\" class=\"text fullWidth\" autofocus=\"autofocus\" autocomplete=\"one-time-code\" /><br/>";
                result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                if (!string.IsNullOrEmpty(prov.GetUIWarningThirdPartyLabel(usercontext)) && (usercontext.IsSendBack))
                    result += "<div class=\"error smallText\">" + prov.GetUIWarningThirdPartyLabel(usercontext) + "</div><br/>";
            }
            if ((prov != null) && (prov.IsUIElementRequired(usercontext, RequiredMethodElements.PinParameterRequired)))
            {
                result += "<div id=\"wizardMessage2\" class=\"groupMargin\">" + BaseExternalProvider.GetPINLabel(usercontext) + "</div>";
                result += "<input id=\"##PINCODE##\" name=\"##PINCODE##\" type=\"password\" placeholder=\"PIN\" autocomplete=\"one-time-code\" class=\"text fullWidth\" /><br/>";
                result += "<div class=\"fieldMargin smallText\">" + BaseExternalProvider.GetPINMessage(usercontext) + "</div><br/>";
            }
            bool soon = RuntimeRepository.MustChangePasswordSoon(Provider.Config, usercontext, out DateTime max);
            if (soon)
                result += "<div class=\"error smallText\">" + string.Format(Resources.GetString(ResourcesLocaleKind.Html, "HtmlMustChangePassword"), max.ToLocalTime().ToLongDateString()) + "</div><br/>";

            if (Provider.HasAccessToOptions(prov))
            {
                if ((soon) && (Provider.Config.UserFeatures.CanManagePassword()))
                    result += "<input id=\"##OPTIONS##\" type=\"checkbox\" name=\"##OPTIONS##\" checked=\"checked\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMAccessOptions");
                else
                    result += "<input id=\"##OPTIONS##\" type=\"checkbox\" name=\"##OPTIONS##\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMAccessOptions");
                result += "<br/><br/>";
            }
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\"/>";
            result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"continueButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMConnexion") + "\" /><br/><br/>";
            if (RuntimeAuthProvider.GetActiveProvidersCount() > 1)
                result += "<a class=\"actionLink\" href=\"#\" id=\"nocode\" name=\"nocode\" onclick=\"return SetLinkData(IdentificationForm, '3')\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMNoCode") + "</a>";
            result += GetFormHtmlMessageZone(usercontext);
            result += "</form>";
            return result;
        }
#endregion

#region ManageOptions
        /// <summary>
        /// GetFormPreRenderHtmlManageOptions implementation
        /// </summary>
        public override string GetFormPreRenderHtmlManageOptions(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function fnlinkclicked(frm, data)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = data;" + CR;
            result += "   frm.submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;

            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlManageOptions implementation
        /// </summary>
        public override string GetFormHtmlManageOptions(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"OptionsForm\" >";
            result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Titles, "ManageOptionsPageTitle") + "</div>";

            IExternalProvider prov1 = RuntimeAuthProvider.GetProvider(PreferredMethod.Code);
            if ((prov1 != null) && (prov1.Enabled) && prov1.IsUIElementRequired(usercontext, RequiredMethodElements.OTPLinkRequired))
            {
                result += "<a class=\"actionLink\" href=\"#\" id=\"enrollopt\" name=\"enrollopt\" onclick=\"fnlinkclicked(OptionsForm, 3)\" style=\"cursor: pointer;\">" + prov1.GetWizardLinkLabel(usercontext) + "</a>";
            }
            IExternalProvider prov4 = RuntimeAuthProvider.GetProvider(PreferredMethod.Biometrics);
            if ((prov4 != null) && (prov4.Enabled) && prov4.IsUIElementRequired(usercontext, RequiredMethodElements.BiometricParameterRequired))
            {
                result += "<a class=\"actionLink\" href=\"#\" id=\"enrollbio\" name=\"enrollbio\" onclick=\"fnlinkclicked(OptionsForm, 6)\" style=\"cursor: pointer;\">" + prov4.GetWizardLinkLabel(usercontext) + "</a>";
            }
            if ((!Provider.Config.IsPrimaryAuhentication) || (Provider.Config.PrimaryAuhenticationOptions.HasFlag(PrimaryAuthOptions.Externals)))
            {
                IExternalProvider prov2 = RuntimeAuthProvider.GetProvider(PreferredMethod.Email);
                if ((prov2 != null) && (prov2.Enabled) && prov2.IsUIElementRequired(usercontext, RequiredMethodElements.EmailLinkRequired))
                {
                    result += "<a class=\"actionLink\" href=\"#\" id=\"enrollemail\" name=\"enrollemail\" onclick=\"fnlinkclicked(OptionsForm, 4)\" style=\"cursor: pointer;\">" + prov2.GetWizardLinkLabel(usercontext) + "</a>";
                }
                IExternalProvider prov3 = RuntimeAuthProvider.GetProvider(PreferredMethod.External);
                if ((prov3 != null) && (prov3.Enabled) && (prov3.IsUIElementRequired(usercontext, RequiredMethodElements.PhoneLinkRequired) || prov3.IsUIElementRequired(usercontext, RequiredMethodElements.ExternalLinkRaquired)))
                {
                    result += "<a class=\"actionLink\" href=\"#\" id=\"enrollphone\" name=\"enrollphone\" onclick=\"fnlinkclicked(OptionsForm, 5)\" style=\"cursor: pointer;\">" + prov3.GetWizardLinkLabel(usercontext) + "</a>";
                }
                IExternalProvider prov5 = RuntimeAuthProvider.GetProvider(PreferredMethod.Azure);
                if (prov5 != null)
                {
                    if (!string.IsNullOrEmpty(prov5.GetAccountManagementUrl(usercontext)))
                        result += "<a class=\"actionLink\" href=\"" + prov5.GetAccountManagementUrl(usercontext) + "\" id=\"enrollazure\" name=\"enrollazure\" target=\"_blank\" style=\"cursor: pointer;\">" + prov5.GetUIAccountManagementLabel(usercontext) + "</a>";
                }
            }
            if (RuntimeAuthProvider.IsPinCodeRequired(usercontext))
            {
                result += "<a class=\"actionLink\" href=\"#\" id=\"enrollpin\" name=\"enrollpin\" onclick=\"fnlinkclicked(OptionsForm, 7)\" style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlEnrollPinCode") + "</a>";
            }
            result += "<br/>";

            if (Provider.Config.KeepMySelectedOptionOn)
            {
                result += GetPartHtmlSelectMethod(usercontext);
                result += "<br/>";
            }
            if ((!Provider.Config.IsPrimaryAuhentication) || (Provider.Config.PrimaryAuhenticationOptions.HasFlag(PrimaryAuthOptions.Register)))
            {

                if (!Provider.Config.UserFeatures.IsMFARequired() && !Provider.Config.UserFeatures.IsMFAMixed())
                {
                    result += "<input id=\"##DISABLEMFA##\" type=\"checkbox\" name=\"##DISABLEMFA##\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMDisableMFA");
                    result += "<br/>";
                }
            }
            result += "<br/>";
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";

            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\" />";

            result += "<table><tr>";
            result += "<td>";
            result += "<input id=\"saveButton\" type=\"submit\" class=\"submit\" name=\"saveButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDSave") + "\" onclick=\"fnbtnclicked(1)\" />";
            result += "</td>";
            if (!Provider.Config.UserFeatures.IsMFAMixed() || (usercontext.Enabled && usercontext.IsRegistered))
            {
                result += "<td style=\"width: 15px\" />";
                result += "<td>";
                result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(2)\" />";
                result += "</td>";
            }
            result += "</tr></table>";
            result += GetFormHtmlMessageZone(usercontext);
            result += "</form>";
            return result;
        }
#endregion

#region Registration
        /// <summary>
        /// GetFormPreRenderHtmlRegistration implementation
        /// </summary>
        public override string GetFormPreRenderHtmlRegistration(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnRefreshPost(frm)" + CR;
            result += "{" + CR;
            result += "   document.getElementById('registrationForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;

            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "}" + CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlRegistration implementation
        /// </summary>
        public override string GetFormHtmlRegistration(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            bool IsRequired = true;
            PreferredMethod m = PreferredMethod.Choose;
            if (usercontext.EnrollPageStatus == EnrollPageStatus.Run)
            {
                m = Utilities.FindNextWizardToPlay(usercontext, ref IsRequired);
                if (m != PreferredMethod.None)
                    usercontext.EnrollPageStatus = EnrollPageStatus.NewStep;
                else
                {
                    result = "<script type='text/javascript'>" + CR;
                    result += "if (window.addEventListener)" + CR;
                    result += "{" + CR;
                    result += "   window.addEventListener('load', OnRefreshPost, false);" + CR;
                    result += "}" + CR;
                    result += "else if (window.attachEvent)" + CR;
                    result += "{" + CR;
                    result += "   window.attachEvent('onload', OnRefreshPost);" + CR;
                    result += "}" + CR;
                    result += CR;
                    result += "</script>" + CR;
                }
            }

            result += "<form method=\"post\" id=\"registrationForm\" >";
            switch (usercontext.EnrollPageStatus)
            {
                case EnrollPageStatus.Start:
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Titles, "RegistrationPageTitle") + "</div>";
                    result += "<div id=\"pwdMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Errors, "ErrorAccountAuthorized") + "</div><br/>";
                    List<IExternalProvider> lst = RuntimeAuthProvider.GeActiveProvidersList();
                    if (lst.Count > 0)
                    {
                        result += "<div id=\"xMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMustPrepareLabel") + "</div>";
                        foreach (IExternalProvider itm in lst)
                        {
                            if (itm.Kind != PreferredMethod.Azure)
                            {
                                if (itm.IsRequired)
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIEnrollmentTaskLabel(usercontext) + "</div>";
                                else
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIEnrollmentTaskLabel(usercontext) + " (*)" + "</div>";
                            }
                        }
                        if (RuntimeAuthProvider.IsPinCodeRequired(usercontext))
                            result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlEnrollPinTaskLabel") + "</div>";
                        result += "<br/>";
                    }

                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"gotoregistration\" type=\"submit\" class=\"submit\" name=\"gotoregistration\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMGoToRegistration") + "\" onclick=\"fnbtnclicked(2)\"/>";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    if (!Provider.Config.UserFeatures.IsRegistrationMixed())
                    {
                        result += "<td>";
                        result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"Continue\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMConnexion") + "\" onclick=\"fnbtnclicked(1)\" />";
                        result += "</td>";
                    }
                    result += "</tr></table>";
                    break;
                case EnrollPageStatus.NewStep:
                    if (m != PreferredMethod.Pin)
                    {
                        IExternalProvider prov = RuntimeAuthProvider.GetProviderInstance(m);
                        result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                        result += "<div id=\"pwdMessage\" class=\"fieldMargin smallText\">" + prov.GetUIEnrollmentTaskLabel(usercontext) + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIEnrollContinue") + "</div><br/>";
                        result += "<table><tr>";
                        result += "<td>";
                        result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"continueButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                        result += "</td>";
                        result += "<td style=\"width: 15px\" />";
                        if ((prov.Enabled) && (!prov.IsRequired))
                        {
                            result += "<td>";
                            result += "<input id=\"skipoption\" type=\"submit\" class=\"submit\" name=\"skipoption\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUINext") + "\" onclick=\"fnbtnclicked(6)\"/>";
                            result += "</td>";
                        }
                        result += "</tr></table>";
                    }
                    else
                    {   // Pin Code not provider
                        result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + BaseExternalProvider.GetPINWizardUILabel(usercontext) + "</div>";
                        result += "<div id=\"pwdMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlEnrollPinTaskLabel") + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIEnrollContinue") + "</div><br/>";
                        result += "<table><tr>";
                        result += "<td>";
                        result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"continueButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                        result += "</td>";
                        result += "</tr></table>";
                    }
                    break;
                case EnrollPageStatus.Stop:
                    result += "<br/>";
                    result += "<p style=\"text-align:center\"><img id=\"msgreen\" src=\"data:image/png;base64," + Convert.ToBase64String(images.cvert.ToByteArray(ImageFormat.Png)) + "\"/></p><br/><br/>";
                    result += "<div id=\"lbl\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPSuccess") + "</div><br/>";
                    List<IExternalProvider> lst2 = RuntimeAuthProvider.GeActiveProvidersList();
                    if (lst2.Count > 0)
                    {
                        result += "<div id=\"xMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIResultLabel") + "</div>";
                        foreach (IExternalProvider itm in lst2)
                        {
                            string value = string.Empty;
                            switch (itm.Kind)
                            {
                                default:
                                case PreferredMethod.Code:
                                    if (usercontext.KeyStatus == SecretKeyStatus.Success)
                                        value = "OK";
                                    else
                                        value = "<empty>";
                                    break;
                                case PreferredMethod.Email:
                                    value = Utilities.StripEmailAddress(usercontext.MailAddress);
                                    break;
                                case PreferredMethod.External:
                                    value = Utilities.StripPhoneNumber(usercontext.PhoneNumber);
                                    break;
                                case PreferredMethod.Biometrics:
                                    if (usercontext.KeyStatus == SecretKeyStatus.Success) 
                                        value = "OK";
                                    else
                                        value = "<empty>";
                                    break;
                            }
                            if (itm.Kind != PreferredMethod.Azure)
                            {
                                if (itm.IsRequired)
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIListOptionLabel(usercontext) + " : " + value + "</div>";
                                else
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIListOptionLabel(usercontext) + " (*) : " + value + "</div>";
                            }
                        }
                        if (RuntimeAuthProvider.IsPinCodeRequired(usercontext))
                            result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIPinOptionLabel") + " : " + Utilities.StripPinCode(usercontext.PinCode) + "</div>";
                        result += "<br/>";
                    }
                    result += "<input id=\"quitButton\" type=\"submit\" class=\"submit\" name=\"quitButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(5)\" />";
                    result += "<br/>";
                    break;
            }

            result += "<input id=\"##ISPROVIDER##\" type=\"hidden\" name=\"##ISPROVIDER##\" value=\"" + (int)m + "\" />";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\" />";
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";

            result += "</form>";
            return result;
        }
#endregion

#region Invitation
        /// <summary>
        /// GetFormPreRenderHtmlInvitation implementation
        /// </summary>
        public override string GetFormPreRenderHtmlInvitation(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnRefreshPost(frm)" + CR;
            result += "{" + CR;
            result += "   document.getElementById('invitationForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;

            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "}" + CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlInvitation implementation
        /// </summary>
        public override string GetFormHtmlInvitation(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            bool IsRequired = true;
            PreferredMethod m = PreferredMethod.Choose;
            if (usercontext.EnrollPageStatus == EnrollPageStatus.Run)
            {
                m = Utilities.FindNextWizardToPlay(usercontext, ref IsRequired);
                if (m != PreferredMethod.None)
                    usercontext.EnrollPageStatus = EnrollPageStatus.NewStep;
                else
                {
                    result = "<script type='text/javascript'>" + CR;
                    result += "if (window.addEventListener)" + CR;
                    result += "{" + CR;
                    result += "   window.addEventListener('load', OnRefreshPost, false);" + CR;
                    result += "}" + CR;
                    result += "else if (window.attachEvent)" + CR;
                    result += "{" + CR;
                    result += "   window.attachEvent('onload', OnRefreshPost);" + CR;
                    result += "}" + CR;
                    result += CR;
                    result += "</script>" + CR;
                }
            }
            result += "<form method=\"post\" id=\"invitationForm\" >";
            switch (usercontext.EnrollPageStatus)
            {
                case EnrollPageStatus.Start:
                    
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Titles, "InvitationPageTitle") + "</div>";
                    result += "<div id=\"pwdMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Errors, "ErrorAccountNotEnabled") + "</div><br/>";
                    List<IExternalProvider> lst = RuntimeAuthProvider.GeActiveProvidersList();
                    if (lst.Count > 0)
                    {
                        result += "<div id=\"xMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMustPrepareLabel") + "</div>";
                        foreach (IExternalProvider itm in lst)
                        {
                            if (itm.Kind != PreferredMethod.Azure)
                            {
                                if (itm.IsRequired)
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIEnrollmentTaskLabel(usercontext) + "</div>";
                                else
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIEnrollmentTaskLabel(usercontext) + " (*)" + "</div>";
                            }
                        }
                        if (RuntimeAuthProvider.IsPinCodeRequired(usercontext))
                            result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlEnrollPinTaskLabel") + "</div>";
                        result += "<br/>";
                    }
                    result += "<table><tr>";
                    if (Provider.Config.UserFeatures.IsManaged())
                    {
                        result += "<td>";
                        result += "<input id=\"gotoinscription\" type=\"submit\" class=\"submit\" name=\"gotoinscription\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMGotoInscription") + "\" onclick=\"fnbtnclicked(2)\"/>";
                        result += "</td>";
                        result += "<td style=\"width: 15px\" />";
                        result += "<td>";
                        result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"continueButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMConnexion") + "\" onclick=\"fnbtnclicked(1)\" />";
                        result += "</td>";
                    }
                    else
                    {
                        result += "<td>";
                        result += "<input id=\"gotoinscription\" type=\"submit\" class=\"submit\" name=\"gotoinscription\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMGotoInscription") + "\" onclick=\"fnbtnclicked(2)\"/>";
                        result += "</td>";
                    }
                    result += "</tr></table>";
                    break;
                case EnrollPageStatus.NewStep:
                    if (m != PreferredMethod.Pin)
                    {
                        IExternalProvider prov = RuntimeAuthProvider.GetProviderInstance(m);
                        result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                        result += "<div id=\"pwdMessage\" class=\"fieldMargin smallText\">" + prov.GetUIEnrollmentTaskLabel(usercontext) + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIEnrollContinue") + "</div><br/>";
                        result += "<table><tr>";
                        result += "<td>";
                        result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"Continue\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                        result += "</td>";
                        result += "<td style=\"width: 15px\" />";
                        if ((prov.Enabled) && (!prov.IsRequired))
                        {
                            result += "<td>";
                            result += "<input id=\"skipoption\" type=\"submit\" class=\"submit\" name=\"skipoption\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUINext") + "\" onclick=\"fnbtnclicked(6)\"/>";
                            result += "</td>";
                        }
                        result += "</tr></table>";
                    }
                    else
                    {   // Pin Code not provider
                        result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + BaseExternalProvider.GetPINWizardUILabel(usercontext) + "</div>";
                        result += "<div id=\"pwdMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlEnrollPinTaskLabel") + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIEnrollContinue") + "</div><br/>";
                        result += "<table><tr>";
                        result += "<td>";
                        result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"Continue\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                        result += "</td>";
                        result += "</tr></table>";
                    }
                    break;
                case EnrollPageStatus.Stop:
                    result += "<br/>";
                    result += "<p style=\"text-align:center\"><img id=\"msgreen\" src=\"data:image/png;base64," + Convert.ToBase64String(images.cvert.ToByteArray(ImageFormat.Png)) + "\"/></p><br/><br/>";
                    result += "<div id=\"lbl\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPSuccess") + "</div><br/>";
                    List<IExternalProvider> lst2 = RuntimeAuthProvider.GeActiveProvidersList();
                    if (lst2.Count > 0)
                    {
                        result += "<div id=\"xMessage\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIResultLabel") + "</div>";
                        foreach (IExternalProvider itm in lst2)
                        {
                            string value = string.Empty;
                            switch (itm.Kind)
                            {
                                default:
                                case PreferredMethod.Code:
                                    if (usercontext.KeyStatus == SecretKeyStatus.Success)
                                        value = "OK";
                                    else
                                        value = "<empty>";
                                    break;
                                case PreferredMethod.Email:
                                    value = Utilities.StripEmailAddress(usercontext.MailAddress);
                                    break;
                                case PreferredMethod.External:
                                    value = Utilities.StripPhoneNumber(usercontext.PhoneNumber);
                                    break;
                                case PreferredMethod.Biometrics:
                                    if (usercontext.KeyStatus == SecretKeyStatus.Success) 
                                        value = "OK";
                                    else
                                        value = "<empty>";
                                    break;
                            }
                            if (itm.Kind != PreferredMethod.Azure)
                            {
                                if (itm.IsRequired)
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIListOptionLabel(usercontext) + " : " + value + "</div>";
                                else
                                    result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + itm.GetUIListOptionLabel(usercontext) + " (*) : " + value + "</div>";
                            }
                        }
                        if (RuntimeAuthProvider.IsPinCodeRequired(usercontext))
                            result += "<div id=\"reqvalue\" class=\"fieldMargin smallText\">- " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIPinOptionLabel") + " : " + Utilities.StripPinCode(usercontext.PinCode) + "</div>";
                        result += "<br/>";
                    }
                    result += "<input id=\"quitButton\" type=\"submit\" class=\"submit\" name=\"quitButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(5)\" />";
                    result += "<br/>";
                    break;
            }

            result += "<input id=\"##ISPROVIDER##\" type=\"hidden\" name=\"##ISPROVIDER##\" value=\"" + (int)m + "\" />";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\" />";
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";

            result += "</form>";
            return result;
        }
#endregion

#region Activation
        /// <summary>
        /// GetFormPreRenderHtmlActivation implementation
        /// </summary>
        public override string GetFormPreRenderHtmlActivation(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnRefreshPost(frm)" + CR;
            result += "{" + CR;
            result += "   document.getElementById('activationForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;

            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "}" + CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlActivation implementation
        /// </summary>
        public override string GetFormHtmlActivation(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"activationForm\" >";

            result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Titles, "ActivationPageTitle") + "</div>";
            result += "<div id=\"pwdMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Errors, "ErrorAccountActivate") + "</div><br/>";

            result += "<table><tr>";
            result += "<td>";
            result += "   <input id=\"activateButton\" type=\"submit\" class=\"submit\" name=\"activateButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIActivate") + "\" onclick=\"fnbtnclicked(2)\"/>";
            result += "</td>";
            result += "<td style=\"width: 15px\" />";
            result += "<td>";
            result += "   <input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"continueButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(1)\" />";
            result += "</td>";
            result += "</tr></table>";

            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\" />";
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";

            result += "</form>";
            return result;

        }
#endregion

#region SelectOptions
        /// <summary>
        /// GetFormPreRenderHtmlSelectOptions implementation
        /// </summary>
        public override string GetFormPreRenderHtmlSelectOptions(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;
            result += "function SetLinkTitle(frm, data)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTEDLINK##');" + CR;
            result += "   if (lnk)" + CR;
            result += "   {" + CR;
            result += "      lnk.value = data;" + CR;
            result += "   }" + CR;
            result += "   var donut = document.getElementById('cookiePullWaitingWheel');" + CR;
            result += "   if (donut)" + CR;
            result += "   {" + CR;
            result += "      donut.style.visibility = 'visible';" + CR;
            result += "   }" + CR;
            result += "   frm.submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;

            result += "function fnbtnclicked()" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = 1;" + CR;
            result += "}" + CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlSelectOptions implementation
        /// </summary>
        public override string GetFormHtmlSelectOptions(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"selectoptionsForm\" >";

            if (Provider.Config.UserFeatures.CanManageOptions())
            {
                result += "<a class=\"actionLink\" href=\"#\" id=\"chgopt\" name=\"chgopt\" onclick=\"return SetLinkTitle(selectoptionsForm, '1')\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlChangeConfiguration") + "</a>";
            }
            if (Provider.Config.UserFeatures.CanManagePassword() && RuntimeRepository.CanChangePassword(Provider.Config, usercontext))
            {
                if (!Provider.Config.CustomUpdatePassword)
                    result += "<a class=\"actionLink\" href=\"/adfs/portal/updatepassword?username=" + usercontext.UPN + "\" id=\"chgpwd\" name=\"chgpwd\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlChangePassword") + "</a>";
                else
                    result += "<a class=\"actionLink\" href=\"#\" id=\"chgpwd\" name=\"chgpwd\" onclick=\"return SetLinkTitle(selectoptionsForm, '2')\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlChangePassword") + "</a>";
            }
            result += "<br/>";
            if (Provider.Config.UserFeatures.CanEnrollDevices())
            {
                bool WantPin = false;
                bool SuperPin = false;
                IExternalProvider prov = null;
                if (RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.OTPLinkRequired))
                {
                    prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Code);
                    if (prov.PinRequired)
                        WantPin = true;
                    if (Provider.HasStrictAccessToOptions(prov))
                        result += "<a class=\"actionLink\" href=\"#\" id=\"enrollopt\" name=\"enrollopt\" onclick=\"return SetLinkTitle(selectoptionsForm, '3')\"; style=\"cursor: pointer;\">" + prov.GetWizardLinkLabel(usercontext) + "</a>";
                }
                if (RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.BiometricLinkRequired))
                {
                    prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Biometrics);
                    if (prov.PinRequired)
                        WantPin = true;
                    if (((IWebAuthNProvider)prov).PinRequirements != WebAuthNPinRequirements.Null)
                        SuperPin = true;
                    if (Provider.HasStrictAccessToOptions(prov))
                        result += "<a class=\"actionLink\" href=\"#\" id=\"enrollbio\" name=\"enrollbio\" onclick=\"return SetLinkTitle(selectoptionsForm, '4')\"; style=\"cursor: pointer;\">" + prov.GetWizardLinkLabel(usercontext) + "</a>";
                }
                if ((!Provider.Config.IsPrimaryAuhentication) || (Provider.Config.PrimaryAuhenticationOptions.HasFlag(PrimaryAuthOptions.Externals)))
                {

                    if (RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.EmailLinkRequired))
                    {
                        prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Email);
                        if (prov.PinRequired)
                            WantPin = true;
                        if (Provider.HasStrictAccessToOptions(prov))
                            result += "<a class=\"actionLink\" href=\"#\" id=\"enrollemail\" name=\"enrollemail\" onclick=\"return SetLinkTitle(selectoptionsForm, '5')\"; style=\"cursor: pointer;\">" + prov.GetWizardLinkLabel(usercontext) + "</a>";
                    }
                    if (RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.PhoneLinkRequired) || RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.ExternalLinkRaquired))
                    {
                        prov = RuntimeAuthProvider.GetProvider(PreferredMethod.External);
                        if (prov.PinRequired)
                            WantPin = true;
                        if (Provider.HasStrictAccessToOptions(prov))
                            result += "<a class=\"actionLink\" href=\"#\" id=\"enrollphone\" name=\"enrollphone\" onclick=\"return SetLinkTitle(selectoptionsForm, '6')\"; style=\"cursor: pointer;\">" + prov.GetWizardLinkLabel(usercontext) + "</a>";
                    }
                    if (RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.AzureInputRequired))
                    {
                        prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Azure);
                        if (prov != null)
                        {
                            if (prov.PinRequired)
                                WantPin = true;
                            if (Provider.HasStrictAccessToOptions(prov))
                            {
                                if (!string.IsNullOrEmpty(prov.GetAccountManagementUrl(usercontext)))
                                    result += "<a class=\"actionLink\" href=\"" + prov.GetAccountManagementUrl(usercontext) + "\" id=\"enrollazure\" name=\"enrollazure\" target=\"_blank\" style=\"cursor: pointer;\">" + prov.GetUIAccountManagementLabel(usercontext) + "</a>";
                            }
                        }
                    }
                }
                if (RuntimeAuthProvider.IsUIElementRequired(usercontext, RequiredMethodElements.PinLinkRequired) || SuperPin)
                {
                    if ((WantPin) || (SuperPin))
                        result += "<a class=\"actionLink\" href=\"#\" id=\"enrollpin\" name=\"enrollpin\"  onclick=\"return SetLinkTitle(selectoptionsForm, '7')\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlEnrollPinCode") + "</a>";
                }
            }

            result += "<script>";
            result += "   document.cookie = 'showoptions=;expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/adfs/;SameSite=Strict;';";
            result += "</script>";

            result += "<br/>";
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\" />";
            result += "<input id=\"##SELECTEDLINK##\" type=\"hidden\" name=\"##SELECTEDLINK##\" value=\"0\"/>";
            result += "<input id=\"saveButton\" type=\"submit\" class=\"submit\" name=\"saveButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMConnexion") + "\" onclick=\"fnbtnclicked()\"/>";
            result += "<br/>";
            result += GetFormHtmlMessageZone(usercontext);
            result += "</form>";
            return result;
        }
#endregion

#region ChooseMethod
        /// <summary>
        /// GetFormPreRenderHtmlChooseMethod implementation
        /// </summary>
        public override string GetFormPreRenderHtmlChooseMethod(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;
            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlChooseMethod implementation
        /// </summary>
        public override string GetFormHtmlChooseMethod(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"ChooseMethodForm\" >";
            result += "<b><div id=\"pwdTitle\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Titles, "MustUseCodePageTitle") + "</div></b>";
            PreferredMethod method = GetMethod4FBUsers(usercontext);
            if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Code))
                result += "<input id=\"opt1\" name=\"##SELECTEDRADIO##\" type=\"radio\" value=\"0\" " + (((method == PreferredMethod.Code) || (method == PreferredMethod.Choose)) ? "checked=\"checked\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.Code).GetUIChoiceLabel(usercontext) + "<br/><br/>";
            if ((!Provider.Config.IsPrimaryAuhentication) || (Provider.Config.PrimaryAuhenticationOptions.HasFlag(PrimaryAuthOptions.Externals)))
            {
                if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Email))
                    result += "<input id=\"opt2\" name=\"##SELECTEDRADIO##\" type=\"radio\" value=\"2\" " + ((method == PreferredMethod.Email) ? "checked=\"checked\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.Email).GetUIChoiceLabel(usercontext) + "<br/><br/>";
                if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.External))
                    result += "<input id=\"opt1\" name=\"##SELECTEDRADIO##\" type=\"radio\" value=\"1\" " + ((method == PreferredMethod.External) ? "checked=\"checked\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.External).GetUIChoiceLabel(usercontext) + "<br/><br/>";
                if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Azure))
                    result += "<input id=\"opt3\" name=\"##SELECTEDRADIO##\" type=\"radio\" value=\"3\" " + ((method == PreferredMethod.Azure) ? "checked=\"checked\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.Azure).GetUIChoiceLabel(usercontext) + "<br/><br/>";
            }
            if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Biometrics))
                result += "<input id=\"opt4\" name=\"##SELECTEDRADIO##\" type=\"radio\" value=\"4\" " + ((method == PreferredMethod.Biometrics) ? "checked=\"checked\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.Biometrics).GetUIChoiceLabel(usercontext) + "<br/><br/>";

            result += "<br/>";
            if (Provider.KeepMySelectedOptionOn())
                result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember") + "<br/>";
            result += "<br/>";
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\">";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\">";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"0\" />";

            result += "<table><tr>";
            result += "<td>";
            result += "<input id=\"saveButton\" type=\"submit\" class=\"submit\" name=\"Continue\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionSendCode") + "\" onclick=\"fnbtnclicked(0)\" />";
            result += "</td>";
            result += "<td style=\"width: 15px\" />";
            result += "<td>";
            result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
            result += "</td>";
            result += "</tr></table>";
            result += GetFormHtmlMessageZone(usercontext);
            result += "</form>";
            return result;
        }

        /// <summary>
        /// GetMethod4FBUsers method implementation
        /// </summary>
        private PreferredMethod GetMethod4FBUsers(AuthenticationContext usercontext)
        {
            PreferredMethod method = usercontext.PreferredMethod;
            PreferredMethod idMethod = usercontext.PreferredMethod;
            do
            {
                idMethod++;
                if (idMethod > PreferredMethod.Biometrics)
                    idMethod = PreferredMethod.Code;
                else if (idMethod == method)
                    return method;
            } while (!RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, idMethod));
            return idMethod;

        }
#endregion

#region ChangePassword
        /// <summary>
        /// GetFormPreRenderHtmlChangePassword implementation
        /// </summary>
        public override string GetFormPreRenderHtmlChangePassword(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            if (Provider.Config.CustomUpdatePassword)
            {
                result += "<script type='text/javascript'>" + CR;

                result += "function ValidChangePwd(frm)" + CR;
                result += "{" + CR;
                result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
                result += "   if (lnk.value=='1')" + CR;
                result += "   {" + CR;
                result += "      var err = document.getElementById('errorText');" + CR;
                result += "      var oldpwd = document.getElementById('##OLDPASS##');" + CR;
                result += "      var newpwd = document.getElementById('##NEWPASS##');" + CR;
                result += "      var cnfpwd = document.getElementById('##CNFPASS##');" + CR;
                result += "      if (oldpwd.value == \"\")" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidPassActualError") + "\";" + CR;
                result += "         oldpwd.focus();" + CR;
                result += "         return false;" + CR;
                result += "      }" + CR;
                result += "      if (newpwd.value == \"\")" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidPassNewError") + "\";" + CR;
                result += "         newpwd.focus();" + CR;
                result += "        return false;" + CR;
                result += "      }" + CR;
                result += "      if (cnfpwd.value == \"\")" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidConfirmNewPassError") + "\";" + CR;
                result += "         cnfpwd.focus();" + CR;
                result += "         return false;" + CR;
                result += "      }" + CR;
                result += "      if (cnfpwd.value != newpwd.value)" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidNewPassError") + "\";" + CR;
                result += "         cnfpwd.focus();" + CR;
                result += "         return false;" + CR;
                result += "      }" + CR;
                result += "      err.innerHTML = \"\";" + CR;
                result += "   }" + CR;
                result += "   return true;" + CR;
                result += "}" + CR;
                result += CR;

                result += "function fnbtnclicked(id)";
                result += "{";
                result += "   var lnk = document.getElementById('##SELECTED##');";
                result += "   lnk.value = id;";
                result += "}";
                result += CR;

                result += "</script>" + CR;
            }
            return result;
        }

        /// <summary>
        /// GetFormHtmlChangePassword implementation
        /// </summary>
        public override string GetFormHtmlChangePassword(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            if (Provider.Config.CustomUpdatePassword)
            {
                result += "<form method=\"post\" id=\"passwordForm\" onsubmit=\"return ValidChangePwd(this)\" >";
                result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Titles, "PasswordPageTitle") + "</div>";

                result += "<div id=\"oldpwd\"  class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDLabelActual") + "</div>";
                result += "<input id=\"##OLDPASS##\" name=\"##OLDPASS##\" type=\"password\" class=\"text fullWidth\"/><br/><br/>";

                result += "<div id=\"newpwd\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDLabelNew") + "</div>";
                result += "<input id=\"##NEWPASS##\" name=\"##NEWPASS##\" type=\"password\" class=\"text fullWidth\"/><br/><br/>";

                result += "<div id=\"cnfpwd\" class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDLabelNewConfirmation") + "</div>";
                result += "<input id=\"##CNFPASS##\" name=\"##CNFPASS##\" type=\"password\" class=\"text fullWidth\"/><br/><br/>";

                result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
                result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
                result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";
                result += "<table><tr>";
                result += "<td>";
                result += "<input id=\"saveButton\" type=\"submit\" class=\"submit\" name=\"continue\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDSave") + "\" onClick=\"fnbtnclicked(1)\" />";
                result += "</td>";
                result += "<td style=\"width: 15px\" />";
                result += "<td>";
                result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onClick=\"fnbtnclicked(2)\"/>";
                result += "</td>";
                result += "</tr></table>";
                result += GetFormHtmlMessageZone(usercontext);
                result += "</form>";
            }
            return result;
        }
#endregion

#region Bypass
        /// <summary>
        /// GetFormPreRenderHtmlBypass implementation
        /// </summary>
        public override string GetFormPreRenderHtmlBypass(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnAutoPost(frm)" + CR;
            result += "{" + CR;
            result += "   var title = document.getElementById('mfaGreetingDescription');" + CR;
            result += "   if (title)" + CR;
            result += "   {" + CR;
            result += "      title.innerHTML = \"<b>" + Resources.GetString(ResourcesLocaleKind.Titles, "TitleRedirecting") + "</b>\";" + CR;
            result += "   }" + CR;
            if (!string.IsNullOrEmpty(usercontext.AccountManagementUrl))
            {
                result += "   var win = window.open('" + usercontext.AccountManagementUrl + "', 'configwindow', 'width=800, height=800');" + CR;
                result += "   win.focus();" + CR;
            }
            result += "   document.getElementById('bypassForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;
            return result;
        }
        /// <summary>
        /// GetFormHtmlBypass implementation
        /// </summary>
        public override string GetFormHtmlBypass(AuthenticationContext usercontext)
        {
            IExternalProvider prov = RuntimeAuthProvider.GetProvider(usercontext.PreferredMethod);
            bool needinput = ((usercontext.IsTwoWay) && (prov != null) && (prov.IsUIElementRequired(usercontext, RequiredMethodElements.PinParameterRequired)));
            if ((usercontext.WizContext == WizardContextMode.Registration) || (usercontext.WizContext == WizardContextMode.Invitation) ) // do not ask after registration/Invitation Enrollment process
                needinput = false;
            if (usercontext.PinDone)
                needinput = false;

            string result = string.Empty;
            result += "<script type='text/javascript'>" + CR;
            result += "if (window.addEventListener)" + CR;
            result += "{" + CR;
            if (!needinput)
                result += "   window.addEventListener('load', OnAutoPost, false);" + CR;
            result += "}" + CR;
            result += "else if (window.attachEvent)" + CR;
            result += "{";
            if (!needinput)
                result += "   window.attachEvent('onload', OnAutoPost);" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;

            if (needinput)
            {
                result += "<form method=\"post\" id=\"bypassForm\" title=\"PIN Confirmation\" >";
                result += "<div id=\"wizardMessage2\" class=\"groupMargin\">" + BaseExternalProvider.GetPINLabel(usercontext) + " : </div>";
                result += "<input id=\"##PINCODE##\" name=\"##PINCODE##\" type=\"password\" placeholder=\"PIN\" autocomplete=\"one-time-code\" class=\"text fullWidth\" /><br/>";
                result += "<div class=\"fieldMargin smallText\">" + BaseExternalProvider.GetPINMessage(usercontext) + "</div><br/>";
                result += "<input id=\"continueButton\" type=\"submit\" class=\"submit\" name=\"continueButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMConnexion") + "\" /><br/><br/>";

                result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
                result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";

            }
            else
            {
                result += "<form method=\"post\" id=\"bypassForm\" title=\""+ Resources.GetString(ResourcesLocaleKind.Titles, "TitleRedirecting") + "\" >";
                result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
                result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            }
            result += "</form>";
            return result;
        }
#endregion

#region Locking
        /// <summary>
        /// GetFormPreRenderHtmlLocking implementation
        /// </summary>
        public override string GetFormPreRenderHtmlLocking(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            result += "<script type=\"text/javascript\">" + CR;
            result += "function fnbtnclicked(id)" + CR; ;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlLocking implementation
        /// </summary>
        public override string GetFormHtmlLocking(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"lockingForm\" >";

            if (!usercontext.IsRegistered)
            {
                if (Provider.Config.UserFeatures.IsRegistrationRequired())
                {
                    result += "<div id=\"pwdMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Errors, "ErrorAccountAdminNotEnabled") + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"buttonquit\" type=\"submit\"  value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMQuit") + "\" onClick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                }
                else
                {
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"buttonquit\" type=\"submit\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMQuit") + "\" onClick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                }
            }
            else if (!usercontext.Enabled)
            {
                if (Provider.Config.UserFeatures.IsMFARequired())
                {
                    result += "<div id=\"pwdMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Errors, "ErrorAccountAdminNotEnabled") + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"buttonquit\" type=\"submit\"  value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMQuit") + "\" onClick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                }
                else
                {
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"buttonquit\" type=\"submit\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMQuit") + "\" onClick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                }
            }
            else
            {
                result += GetFormHtmlMessageZone(usercontext);
                result += "<br/>";
                result += "<table><tr>";
                result += "<td>";
                result += "<input id=\"buttonquit\" type=\"submit\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMQuit") + "\" onClick=\"fnbtnclicked(1)\" />";
                result += "</td>";
                result += "</tr></table>";
            }

            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";
            result += "</form>";
            return result;
        }
#endregion

#region CodeRequest
        /// <summary>
        /// GetFormPreRenderHtmlSendCodeRequest implementation
        /// </summary>
        public override string GetFormPreRenderHtmlSendCodeRequest(AuthenticationContext usercontext)
        {
            string dt = DateTime.Now.AddMilliseconds(Provider.Config.DeliveryWindow).ToString("R");
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnRefreshPost(frm)" + CR;
            result += "{" + CR;
            result += "   document.getElementById('refreshForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}";
            result += CR;

            result += "function SetLinkTitle(frm, data)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTEDLINK##');" + CR;
            result += "   if (lnk)" + CR;
            result += "   {" + CR;
            result += "      lnk.value = data;" + CR;
            result += "   }" + CR;
            result += "   frm.submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function SetOptions(frm)" + CR;
            result += "{" + CR;
            result += "   var opt = document.getElementById('##OPTIONS##');" + CR;
            result += "   if (opt)" + CR;
            result += "   {" + CR;
            result += "      if (opt.checked)" + CR;
            result += "      {" + CR;
            result += "         document.cookie = 'showoptions=1;expires=" + dt + ";path=/adfs/;SameSite=Strict;';" + CR;
            result += "      }" + CR;
            result += "      else" + CR;
            result += "      {" + CR;
            result += "         document.cookie = 'showoptions=;expires=Thu, 01 Jan 1970 00:00:01 GMT;SameSite=Strict;';" + CR;
            result += "      }" + CR;
            result += "   }" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlCodeRequest implementation
        /// </summary>
        public override string GetFormHtmlSendCodeRequest(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;
            result += "if (window.addEventListener)" + CR;
            result += "{" + CR;
            result += "   window.addEventListener('load', OnRefreshPost, false);" + CR;
            result += "}" + CR;
            result += "else if (window.attachEvent)" + CR;
            result += "{" + CR;
            result += "   window.attachEvent('onload', OnRefreshPost);" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;

            result += "<form method=\"post\" id=\"refreshForm\" >";
            bool soon = RuntimeRepository.MustChangePasswordSoon(Provider.Config, usercontext, out DateTime max);
            if (usercontext.IsRemote)
            {
                IExternalProvider prov = RuntimeAuthProvider.GetProvider(usercontext.PreferredMethod);
                result += GetPartHtmlDonut();
                result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                if (!string.IsNullOrEmpty(prov.GetUIWarningInternetLabel(usercontext)) && (usercontext.IsRemote))
                    result += "<div class=\"error smallText\">" + prov.GetUIWarningInternetLabel(usercontext) + "</div>";
                if (!string.IsNullOrEmpty(prov.GetUIWarningThirdPartyLabel(usercontext)) && (usercontext.IsTwoWay))
                    result += "<div class=\"error smallText\">" + prov.GetUIWarningThirdPartyLabel(usercontext) + "</div>";
                result += "<br />";

                if (soon)
                    result += "<div class=\"error smallText\">" + string.Format(Resources.GetString(ResourcesLocaleKind.Html, "HtmlMustChangePassword"), max.ToLocalTime().ToLongDateString()) + "</div><br/>";

                if (usercontext.IsTwoWay && (Provider.HasAccessToOptions(prov)))
                {
                    if ((soon) && (Provider.Config.UserFeatures.CanManagePassword()))
                        result += "<input id=\"##OPTIONS##\" type=\"checkbox\" name=\"##OPTIONS##\" checked=\"true\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMAccessOptions");
                    else
                        result += "<input id=\"##OPTIONS##\" type=\"checkbox\" name=\"##OPTIONS##\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMAccessOptions");
                    result += "<script>";
                    result += "   document.cookie = 'showoptions=;expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/adfs/;SameSite=Strict;';";
                    result += "</script>";
                    result += "<br/><br/>";
                }

                result += "<a class=\"actionLink\" href=\"#\" id=\"nocode\" name=\"nocode\" onclick=\"return SetLinkTitle(refreshForm, '3')\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMNoCode") + "</a>";
                result += "<input id=\"##SELECTEDLINK##\" type=\"hidden\" name=\"##SELECTEDLINK##\" value=\"0\"/>";
            }
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += GetFormHtmlMessageZone(usercontext);

            result += "</form>";
            return result;
        }

        /// <summary>
        /// GetFormPreRenderHtmlSendBiometricRequest implementation
        /// </summary>
        public override string GetFormPreRenderHtmlSendBiometricRequest(AuthenticationContext usercontext)
        {
            string dt = DateTime.Now.AddMilliseconds(Provider.Config.DeliveryWindow).ToString("R");
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnRefreshPost(response)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = 1;" + CR;
            result += "   var jsonCredentials = document.getElementById('assertionResponse');" + CR;
            result += "   jsonCredentials.value = JSON.stringify(response);" + CR;
            result += "   document.getElementById('refreshbiometricForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function OnAutoPost(frm)" + CR;
            result += "{" + CR;
            result += "   LoginWebAuthN(frm);" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function SetJsError(message)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = 2;" + CR;
            result += "   var err = document.getElementById('jserror');" + CR;
            result += "   err.value = message;" + CR;
            result += "   document.getElementById('refreshbiometricForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}";
            result += CR;

            result += "function SetLinkTitle(frm, data)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTEDLINK##');" + CR;
            result += "   if (lnk)" + CR;
            result += "   {";
            result += "      lnk.value = data;" + CR;
            result += "   }" + CR;
            result += "   frm.submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += GetWebAuthNAssertionScript(usercontext);
            result += GetFormHtmlWebAuthNSupport(usercontext);

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlSendBiometricRequest implementation
        /// </summary>
        public override string GetFormHtmlSendBiometricRequest(AuthenticationContext usercontext)
        {
            bool soon = RuntimeRepository.MustChangePasswordSoon(Provider.Config, usercontext, out DateTime max);
            string result = "<script type='text/javascript'>" + CR;
            result += "if (window.addEventListener)" + CR;
            result += "{" + CR;
            if ((usercontext.DirectLogin) && (!soon))
                result += "   window.addEventListener('load', OnAutoPost, false);" + CR;
            else
                result += "   window.addEventListener('submit', LoginWebAuthN, false);" + CR;
            result += "}" + CR;
            result += "else if (window.attachEvent)" + CR;
            result += "{" + CR;
            if ((usercontext.DirectLogin) && (!soon))
                result += "   window.attachEvent('load', OnAutoPost);" + CR;
            else
                result += "   window.attachEvent('submit', LoginWebAuthN);" + CR;
            result += "}" + CR;
            result += CR + CR;
            result += "</script>" + CR; 

            result += "<form method=\"post\" id=\"refreshbiometricForm\" >";
            IExternalProvider prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Biometrics);
            result += "<br/>";
            result += "<table>";
            result += "  <tr>";
            result += "    <td width=\"30px\">";
            result += "    </td>";
            result += "    <td width=\"100%\" align=\"center\" >";
            result += "      <img id=\"ms\" src=\"data:image/gif;base64," + Convert.ToBase64String(images.biometrics.ToByteArray(ImageFormat.Gif)) + "\"/>";
            result += "    </td>";
            result += "    <td width=\"30px\">";
            result += "    </td>";
            result += "  </tr>";
            result += "</table>";
            result += "<br/>";
            result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div>";
            result += "<br/>";

            if (soon)
                result += "<div class=\"error smallText\">" + string.Format(Resources.GetString(ResourcesLocaleKind.Html, "HtmlMustChangePassword"), max.ToLocalTime().ToLongDateString()) + "</div><br/>";

            if ((!usercontext.DirectLogin) || (soon))
            {
                if (Provider.HasAccessToOptions(prov))
                {
                    if ((soon) && (Provider.Config.UserFeatures.CanManagePassword()))
                        result += "<input id=\"##OPTIONS##\" type=\"checkbox\" name=\"##OPTIONS##\" checked=\"true\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMAccessOptions");
                    else
                        result += "<input id=\"##OPTIONS##\" type=\"checkbox\" name=\"##OPTIONS##\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMAccessOptions");
                    result += "<br/><br/>";
                    result += "<script>";
                    result += "   document.cookie = 'showoptions=;expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/adfs/;SameSite=Strict;';";
                    result += "</script>";
                    result += "<br/><br/>";
                }
                result += "<input id=\"signin\" type=\"submit\" class=\"submit\" name=\"signin\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMConnexion") + "\" /><br/><br/>";
                result += "<a class=\"actionLink\" href=\"#\" id=\"nocode\" name=\"nocode\" onclick=\"return SetLinkTitle(refreshbiometricForm, '3')\"; style=\"cursor: pointer;\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMNoCode") + "</a>";
                result += "<input id=\"##SELECTEDLINK##\" type=\"hidden\" name=\"##SELECTEDLINK##\" value=\"0\"/>";
            }
            result += GetFormHtmlMessageZone(usercontext);

            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";
            result += "<input id=\"jserror\" type=\"hidden\" name=\"jserror\" value=\"\" />";
            result += "<input id=\"assertionResponse\" type=\"hidden\" name=\"assertionResponse\" />";          
            result += "</form>";
            return result;
        }
#endregion

#region InvitationRequest
        /// <summary>
        /// GetFormPreRenderHtmlSendAdministrativeRequest implementation
        /// </summary>
        public override string GetFormPreRenderHtmlSendAdministrativeRequest(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "function OnRefreshPost(frm)" + CR;
            result += "{" + CR;
            result += "   document.getElementById('invitationReqForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlSendAdministrativeRequest implementation
        /// </summary>
        public override string GetFormHtmlSendAdministrativeRequest(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;

            result += "if (window.addEventListener)";
            result += "{";
            result += "window.addEventListener('load', OnRefreshPost, false);";
            result += "}";
            result += "else if (window.attachEvent)";
            result += "{";
            result += "window.attachEvent('onload', OnRefreshPost);";
            result += "}";
            result += CR;
            result += "</script>" + CR;

            result += "<form method=\"post\" id=\"invitationReqForm\" >";
            result += "<div class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlINSWaitRequest") + "</div>";

            IExternalAdminProvider admprov = RuntimeAuthProvider.GetAdministrativeProvider(Provider.Config);
            result += GetPartHtmlDonut();
            if (admprov != null)
            {
                result += "<div class=\"fieldMargin smallText\">" + admprov.GetUIInscriptionMessageLabel(usercontext) + "</div><br/>";
                if (!string.IsNullOrEmpty(admprov.GetUIWarningInternetLabel(usercontext)) && (usercontext.IsRemote))
                    result += "<div class=\"error smallText\">" + admprov.GetUIWarningInternetLabel(usercontext) + "</div>";
            }
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "</form>";
            return result;
        }
#endregion

#region SendKeyRequest
        /// <summary>
        /// GetFormPreRenderHtmlSendKeyRequest method implementation
        /// </summary>
        public override string GetFormPreRenderHtmlSendKeyRequest(AuthenticationContext usercontext)
        {
            string result = "<script type='text/javascript'>" + CR;
            result += "function OnAutoRefreshPost(frm)" + CR;
            result += "{" + CR;
            result += "   document.getElementById('sendkeyReqForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlSendKeyRequest method implementation
        /// </summary>
        public override string GetFormHtmlSendKeyRequest(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            result += "<script type=\"text/javascript\">" + CR;
            result += "if (window.addEventListener)" + CR;
            result += "{" + CR;
            result += "   window.addEventListener('load', OnAutoRefreshPost, false);" + CR;
            result += "}" + CR;
            result += "else if (window.attachEvent)" + CR;
            result += "{" + CR;
            result += "   window.attachEvent('onload', OnAutoRefreshPost);" + CR;
            result += "}" + CR;
            result += "</script>" + CR;

            result += "<form method=\"post\" id=\"sendkeyReqForm\" >";
            result += "<div class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlINSWaitRequest") + "</div>";

            IExternalAdminProvider admprov = RuntimeAuthProvider.GetAdministrativeProvider(Provider.Config);

            result += GetPartHtmlDonut();
            if (admprov != null)
            {
                result += "<div class=\"fieldMargin smallText\">" + admprov.GetUISecretKeyMessageLabel(usercontext) + "</div><br/>";
                if (!string.IsNullOrEmpty(admprov.GetUIWarningInternetLabel(usercontext)) && (usercontext.IsRemote))
                    result += "<div class=\"error smallText\">" + admprov.GetUIWarningInternetLabel(usercontext) + "</div>";
            }
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "</form>";
            return result;
        }
#endregion

#region EnrollOTP
        /// <summary>
        /// GetFormPreRenderHtmlEnrollOTP method implementation
        /// </summary>
        public override string GetFormPreRenderHtmlEnrollOTP(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            result += "<script type=\"text/javascript\">" + CR;
            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlEnrollOTP method implmentation
        /// </summary>
        public override string GetFormHtmlEnrollOTP(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            IExternalProvider prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Code);
            prov.GetAuthenticationContext(usercontext);
            result += "<form method=\"post\" id=\"enrollotpForm\" >";
            switch (usercontext.WizPageID)
            {
                case 0:
                    int pos = 0;
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetWizardUIComment(usercontext) + "</div><br/>";
                    result += "<table>";
                    result += "  <tr>";
                    result += "    <th width=\"60px\" />";
                    result += "    <th width=\"34px\" />";
                    result += "    <th width=\"82px\" />";
                    result += "    <th width=\"60px\" />";
                    result += "    <th width=\"34px\" />";
                    result += "    <th width=\"82px\" />";
                    result += "  </tr>";
                    if (!Provider.Config.OTPProvider.WizardOptions.HasFlag(OTPWizardOptions.NoMicrosoftAuthenticator))
                    {
                        pos++;
                        if (pos % 2 == 1)
                            result += " <tr>";
                        result += "  <td>";
                        result += "   <img id=\"ms\" src=\"data:image/png;base64," + Convert.ToBase64String(images.microsoft.ToByteArray(ImageFormat.Png)) + "\"/>";
                        result += "  </td>";
                        result += "  <td colspan=\"2\"> ";
                        result += "    <table>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td> ";
                        result += "          <a href=\"https://www.microsoft.com/store/p/microsoft-authenticator/9nblgggzmcj6\" target=\"blank\">Microsoft Store</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>";
                        result += "          <a href=\"https://play.google.com/store/apps/details?id=com.azure.authenticator\" target=\"blank\">Google Play</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>";
                        result += "          <a href=\"https://itunes.apple.com/app/microsoft-authenticator/id983156458\" target=\"blank\">iTunes</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "    </table";
                        result += "  </td>";
                        if (pos % 2 == 0)
                            result += " </tr>";
                    }
                    if (!Provider.Config.OTPProvider.WizardOptions.HasFlag(OTPWizardOptions.NoGoogleAuthenticator))
                    {
                        pos++;
                        if (pos % 2 == 1)
                            result += " <tr>";
                        result += "  <td>";
                        result += "    <img id=\"gl\" src=\"data:image/png;base64," + Convert.ToBase64String(images.google.ToByteArray(ImageFormat.Png)) + "\"/>";
                        result += "  </td>";
                        result += "  <td colspan=\"2\"> ";
                        result += "    <table>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>&nbsp</td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td >";
                        result += "          <a href=\"https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2\" target=\"blank\">Google Play</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>";
                        result += "          <a href=\"https://itunes.apple.com/app/google-authenticator/id388497605\" target=\"blank\">iTunes</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "    </table";
                        result += "  </td>";
                        if (pos % 2 == 0)
                            result += " </tr>";
                    }
                    if (!Provider.Config.OTPProvider.WizardOptions.HasFlag(OTPWizardOptions.NoAuthyAuthenticator))
                    {
                        pos++;
                        if (pos % 2 == 1)
                            result += " <tr>";
                        result += "  <td>";
                        result += "    <img id=\"at\" src=\"data:image/png;base64," + Convert.ToBase64String(images.authy2.ToByteArray(ImageFormat.Png)) + "\"/>";
                        result += "  </td>";
                        result += "  <td colspan=\"2\">";
                        result += "    <table";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>&nbsp</td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>";
                        result += "          <a href=\"https://play.google.com/store/apps/details?id=com.authy.authy\" target=\"blank\">Google Play</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>";
                        result += "          <a href=\"https://itunes.apple.com/app/authy/id494168017\" target=\"blank\">iTunes</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "    </table";
                        result += "  </td>";
                        if (pos % 2 == 0)
                            result += " </tr>";
                    }
                    if (!Provider.Config.OTPProvider.WizardOptions.HasFlag(OTPWizardOptions.NoGooglSearch))
                    {
                        pos++;
                        if (pos % 2 == 1)
                            result += " <tr>";
                        result += "  <td colspan=\"2\" >";
                        result += "    <img id=\"at\" src=\"data:image/png;base64," + Convert.ToBase64String(images.googlelogo.ToByteArray(ImageFormat.Png)) + "\"/>";
                        result += "  </td>";
                        result += "  <td> ";
                        result += "    <table";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>&nbsp</td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td colspan=\"2\">";
                        result += "          <a href=\"https://www.google.fr/search?q=Authenticator+apps&oq=Authenticator+apps\" target=\"blank\">Search...</a>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "      <tr>";
                        result += "        <td>&nbsp</td>";
                        result += "        <td>";
                        result += "        </td>";
                        result += "      </tr>";
                        result += "    </table";
                        result += "  </td>";
                        if (pos % 2 == 0)
                            result += " </tr>";
                    }
                    result += "</table>";

                    if (Provider.KeepMySelectedOptionOn())
                    {
                        MFAUser reg = RuntimeRepository.GetMFAUser(Provider.Config, usercontext.UPN);
                        result += "<br/>";
                        if (reg != null)
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\" " + ((reg.PreferredMethod == PreferredMethod.Code) ? "checked=\"checked\"> " : "> ") + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                        else
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\" checked=\"checked\"> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                    }
                    if (!string.IsNullOrEmpty(prov.GetAccountManagementUrl(usercontext)))
                    {
                        result += "<br/>";
                        result += "<input type=\"checkbox\" id=\"##MANAGEACCOUNT##\" name=\"##MANAGEACCOUNT##\" /> " + prov.GetUIAccountManagementLabel(usercontext) + "<br/>";
                    }
                    result += "<br/>";

                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMRecordNewKey") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollOTP))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    break;
                case 1: // Always Reset the Key
                    KeysManager.NewKey(usercontext.UPN);
                    string displaykey = KeysManager.EncodedKey(usercontext.UPN);
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                    result += "<div class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWRQRCode") + "</div><br/>";
                    if (prov.IsUIElementRequired(usercontext, RequiredMethodElements.KeyParameterRequired))
                        result += "<input id=\"secretkey\" name=\"secretkey\" type=\"text\" readonly=\"true\" placeholder=\"DisplayKey\" class=\"text fullWidth\" style=\"background-color: #F0F0F0\" value=\"" + Utilities.StripDisplayKey(displaykey) + "\"/><br/>";
                    result += "<p style=\"text-align:center\"><img id=\"qr\" src=\"data:image/png;base64," + Provider.GetQRCodeString(usercontext) + "\"/></p><br/>";
                    result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(3)\" />";
                    break;
                case 2: // Code verification
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetUILabel(usercontext) + "</div>";
                    result += "<input id=\"##ACCESSCODE##\" name=\"##ACCESSCODE##\" type=\"password\" placeholder=\"Verification Code\" class=\"text fullWidth\" autofocus=\"autofocus\" autocomplete=\"one-time-code\" /><br/>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"checkButton\" type=\"submit\" class=\"submit\" name=\"checkButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMCheck") + "\" onclick=\"fnbtnclicked(4)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollOTP))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    break;
                case 3: // Successfull test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<input id=\"finishButton\" type=\"submit\" class=\"submit\" name=\"finishButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "<br/>";
                    break;
                case 4: // Wrong result test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"priorButton\" type=\"submit\" class=\"submit\" name=\"priorButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPPRIOR") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollOTP))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += "<br/>";

                    break;
            }

            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";

            result += "</form>";
            return result;
        }
#endregion

#region EnrollEmail
        /// <summary>
        /// GetFormPreRenderHtmlEnrollEmail method implementation
        /// </summary>
        public override string GetFormPreRenderHtmlEnrollEmail(AuthenticationContext usercontext)
        {
            string result = string.Empty;

            string reg = @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,})+$/";

            result += "<script type=\"text/javascript\">" + CR;
            result += "function ValidateRegistration(frm)" + CR;
            result += "{" + CR;
            if (usercontext.WizPageID == 0)
            {
                result += "   var mailformat = " + reg + " ;" + CR;
                result += "   var email = document.getElementById('##EMAILADDRESS##');" + CR;
                result += "   var err = document.getElementById('errorText');" + CR;
                result += "   var canceled = document.getElementById('##SELECTED##');" + CR;
                result += "   if ((canceled) && (canceled.value==1))" + CR;
                result += "   {" + CR;
                result += "      return true;" + CR;
                result += "   }" + CR;
                result += "   if ((email) && (email.value=='') && (email.placeholder==''))" + CR;
                result += "   {" + CR;
                result += "      err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidIncorrectEmail") + "\";" + CR;
                result += "      return false;" + CR;
                result += "   }" + CR;
                result += "   if ((email) && (email.value!==''))" + CR;
                result += "   {" + CR;
                result += "      if (!email.value.match(mailformat))" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidIncorrectEmail") + "\";" + CR;
                result += "         return false;" + CR;
                result += "      }" + CR;
                result += "   }" + CR;
                result += "   err.innerHTML = \"\";" + CR;
            }
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function OnRefreshPost(frm)";
            result += "{";
            result += "fnbtnclicked(3);";
            result += "document.getElementById('enrollemailForm').submit();";
            result += "return true;";
            result += "}";
            result += CR;

            result += "function fnbtnclicked(id)";
            result += "{";
            result += "var lnk = document.getElementById('##SELECTED##');";
            result += "lnk.value = id;";
            result += "return true;";
            result += "}";
            result += CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlEnrollEmail method implmentation
        /// </summary>
        public override string GetFormHtmlEnrollEmail(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            if (usercontext.WizPageID == 1)
            {
                result += "<script type=\"text/javascript\">" + CR;
                result += "if (window.addEventListener)";
                result += "{";
                result += "window.addEventListener('load', OnRefreshPost, false);";
                result += "}";
                result += "else if (window.attachEvent)";
                result += "{";
                result += "window.attachEvent('onload', OnRefreshPost);";
                result += "}";
                result += "</script>" + CR;
            }           

            IExternalProvider prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Email);
            prov.GetAuthenticationContext(usercontext);
            result += "<form method=\"post\" id=\"enrollemailForm\" onsubmit=\"return ValidateRegistration(this)\" >";
            switch (usercontext.WizPageID)
            {
                case 0: // Get User email                    
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetWizardUIComment(usercontext) + "</div><br/>";
                    if (prov.IsUIElementRequired(usercontext, RequiredMethodElements.EmailParameterRequired))
                    {                       
                        result += "<input id=\"##EMAILADDRESS##\" name=\"##EMAILADDRESS##\" type=\"text\" placeholder=\"" + Utilities.StripEmailAddress(usercontext.MailAddress) + "\" class=\"text fullWidth\" autocomplete=\"email\" /><br/>";
                    }
                    List<AvailableAuthenticationMethod> lst = prov.GetAuthenticationMethods(usercontext);
                    if (lst.Count > 1)
                    {
                        AuthenticationResponseKind ov = AuthenticationResponseKind.Error;
                        if (prov.AllowOverride)
                        {
                            ov = prov.GetOverrideMethod(usercontext);
                            result += "<input id=\"optiongroup\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"Default\" checked=\"checked\" /> " + prov.GetUIDefaultChoiceLabel(usercontext) + "<br/>";
                        }
                        int i = 1;
                        foreach (AvailableAuthenticationMethod met in lst)
                        {
                            if (ov != AuthenticationResponseKind.Error)
                            {
                                if (met.Method == ov)
                                    result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + met.Method.ToString() + "\" checked=\"checked\" /> " + prov.GetUIChoiceLabel(usercontext, met) + "<br/>";
                                else
                                    result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + met.Method.ToString() + "\" /> " + prov.GetUIChoiceLabel(usercontext, met) + "<br/>";
                            }
                            else
                                result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + met.Method.ToString() + "\" /> " + prov.GetUIChoiceLabel(usercontext, met) + "<br/>";
                            i++;
                        }
                    }
                    if (Provider.KeepMySelectedOptionOn())
                    {
                        MFAUser reg = RuntimeRepository.GetMFAUser(Provider.Config, usercontext.UPN);
                        result += "<br/>";
                        if (reg != null)
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\"" + (reg.PreferredMethod == PreferredMethod.Email ? " checked=\"checked\"" : "\"\"") + "\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                        else
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                    }
                    if (!string.IsNullOrEmpty(prov.GetAccountManagementUrl(usercontext)))
                    {
                        result += "<br/>";
                        result += "<input type=\"checkbox\" id=\"##MANAGEACCOUNT##\" name=\"##MANAGEACCOUNT##\" /> " + prov.GetUIAccountManagementLabel(usercontext) + "<br/>";
                    }
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollEmail))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += GetFormHtmlMessageZone(usercontext);
                    break;
                case 1:
                    result += GetPartHtmlDonut();
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                    if (!string.IsNullOrEmpty(prov.GetUIWarningInternetLabel(usercontext)) && (usercontext.IsRemote))
                        result += "<div class=\"error smallText\">" + prov.GetUIWarningInternetLabel(usercontext) + "</div>";
                    if (!string.IsNullOrEmpty(prov.GetUIWarningThirdPartyLabel(usercontext)) && (usercontext.IsTwoWay))
                        result += "<div class=\"error smallText\">" + prov.GetUIWarningThirdPartyLabel(usercontext) + "</div>";
                    result += "<br />";
                    break;
                case 2: // Code verification
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetUILabel(usercontext) + "</div>";
                    result += "<input id=\"##ACCESSCODE##\" name=\"##ACCESSCODE##\" type=\"password\" placeholder=\"Verification Code\" class=\"text fullWidth\" autofocus=\"autofocus\" autocomplete=\"one-time-code\" /><br/>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"checkButton\" type=\"submit\" class=\"submit\" name=\"checkButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMCheck") + "\" onclick=\"fnbtnclicked(4)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollEmail))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    // result += "<br />";
                    break;
                case 3: // Successfull test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<input id=\"finishButton\" type=\"submit\" class=\"submit\" name=\"finishButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "<br/>";
                    break;
                case 4: // Wrong result test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"priorButton\" type=\"submit\" class=\"submit\" name=\"priorButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPPRIOR") + "\" onclick=\"fnbtnclicked(0)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollEmail))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += "<br/>";
                    break;
            }

            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";

            result += "</form>";
            return result;
        }
#endregion

#region EnrollPhone
        /// <summary>
        /// GetFormPreRenderHtmlEnrollPhone method implementation
        /// </summary>
        public override string GetFormPreRenderHtmlEnrollPhone(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            string pho = @"/^\+(?:[0-9] ?){6,14}[0-9]$/";
            string pho10 = @"/^\d{10}$/";
            string phous = @"/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/";

            result += "<script type=\"text/javascript\">" + CR;

            result += "function ValidateRegistration(frm)" + CR;
            result += "{" + CR;
            if (usercontext.WizPageID == 0)
            {
                result += "   var phoneformat = " + pho + " ;" + CR;
                result += "   var phoneformat10 = " + pho10 + " ;" + CR;
                result += "   var phoneformatus = " + phous + " ;" + CR;
                result += "   var phone = document.getElementById('##PHONENUMBER##');" + CR;
                result += "   var err = document.getElementById('errorText');" + CR;
                result += "   var canceled = document.getElementById('##SELECTED##');" + CR;
                result += "   if ((canceled) && (canceled.value==1))" + CR;
                result += "   {";
                result += "      return true;" + CR;
                result += "   }";
                result += "   if ((phone) && (phone.value=='') && (phone.placeholder==''))" + CR;
                result += "   {" + CR;
                result += "      err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidIncorrectPhoneNumber") + "\";" + CR;
                result += "      return false;" + CR;
                result += "   }" + CR;
                result += "   if ((phone) && (phone.value!==''))" + CR;
                result += "   {" + CR;
                result += "      if (!phone.value.match(phoneformat) && !phone.value.match(phoneformat10) && !phone.value.match(phoneformatus) )" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidIncorrectPhoneNumber") + "\";" + CR;
                result += "         return false;" + CR;
                result += "      }" + CR;
                result += "   }" + CR;
                result += "   err.innerHTML = \"\";" + CR;
            }
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function OnRefreshPost(frm)";
            result += "{";
            result += "   fnbtnclicked(3);";
            result += "   document.getElementById('enrollphoneForm').submit();";
            result += "   return true;";
            result += "}";
            result += CR;

            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlEnrollPhone method implmentation
        /// </summary>
        public override string GetFormHtmlEnrollPhone(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            if (usercontext.WizPageID == 1)
            {
                result += "<script type=\"text/javascript\">" + CR;
                result += "if (window.addEventListener)" + CR;
                result += "{" + CR;
                result += "   window.addEventListener('load', OnRefreshPost, false);" + CR;
                result += "}" + CR;
                result += "else if (window.attachEvent)" + CR;
                result += "{" + CR;
                result += "   window.attachEvent('onload', OnRefreshPost);" + CR;
                result += "}" + CR;
                result += "</script>" + CR;
            }            

            IExternalProvider prov = RuntimeAuthProvider.GetProvider(PreferredMethod.External);
            prov.GetAuthenticationContext(usercontext);
            result += "<form method=\"post\" id=\"enrollphoneForm\" onsubmit=\"return ValidateRegistration(this)\" >";
            switch (usercontext.WizPageID)
            {
                case 0: // Get User Phone number                    
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetWizardUIComment(usercontext) + "</div><br/>";
                    if (prov.IsUIElementRequired(usercontext, RequiredMethodElements.PhoneParameterRequired))
                    {
                        result += "<input id=\"##PHONENUMBER##\" name=\"##PHONENUMBER##\" type=\"text\" placeholder=\"" + Utilities.StripPhoneNumber(usercontext.PhoneNumber) + "\" class=\"text fullWidth\" autocomplete=\"tel\" /><br/>";
                    }
                    else if (prov.IsUIElementRequired(usercontext, RequiredMethodElements.ExternalParameterRequired)) 
                    {
                        result += "<input id=\"##PHONENUMBER##\" name=\"##PHONENUMBER##\" type=\"text\" class=\"text fullWidth\" autocomplete=\"tel\" /><br/>";
                    }
                    List<AvailableAuthenticationMethod> lst = prov.GetAuthenticationMethods(usercontext);
                    if (lst.Count > 1)
                    {
                        AuthenticationResponseKind ov = AuthenticationResponseKind.Error;
                        if (prov.AllowOverride)
                        {
                            ov = prov.GetOverrideMethod(usercontext);
                            result += "<input id=\"optiongroup\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"Default\" checked=\"checked\" /> " + prov.GetUIDefaultChoiceLabel(usercontext) + "<br/>";
                        }
                        int i = 1;
                        foreach (AvailableAuthenticationMethod met in lst)
                        {
                            if (ov != AuthenticationResponseKind.Error)
                            {
                                if (met.Method == ov)
                                    result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + met.Method.ToString() + "\" checked=\"checked\" /> " + prov.GetUIChoiceLabel(usercontext, met) + "<br/>";
                                else
                                    result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + met.Method.ToString() + "\" /> " + prov.GetUIChoiceLabel(usercontext, met) + "<br/>";
                            }
                            else
                                result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + met.Method.ToString() + "\" /> " + prov.GetUIChoiceLabel(usercontext, met) + "<br/>";
                            i++;
                        }
                    }

                    if (Provider.KeepMySelectedOptionOn())
                    {
                        MFAUser reg = RuntimeRepository.GetMFAUser(Provider.Config, usercontext.UPN);
                        result += "<br/>";
                        if (reg != null)
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\"" + (reg.PreferredMethod == PreferredMethod.External ? " checked=\"checked\"" : "\"\"") + "\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                        else
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                    }
                    if (!string.IsNullOrEmpty(prov.GetAccountManagementUrl(usercontext)))
                    {
                        result += "<br/>";
                        result += "<input type=\"checkbox\" id=\"##MANAGEACCOUNT##\" name=\"##MANAGEACCOUNT##\" /> " + prov.GetUIAccountManagementLabel(usercontext) + "<br/>";
                    }

                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollPhone))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += GetFormHtmlMessageZone(usercontext);
                    break;
                case 1:
                    result += GetPartHtmlDonut();
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                    if (!string.IsNullOrEmpty(prov.GetUIWarningInternetLabel(usercontext)) && (usercontext.IsRemote))
                        result += "<div class=\"error smallText\">" + prov.GetUIWarningInternetLabel(usercontext) + "</div>";
                    if (!string.IsNullOrEmpty(prov.GetUIWarningThirdPartyLabel(usercontext)) && (usercontext.IsTwoWay))
                        result += "<div class=\"error smallText\">" + prov.GetUIWarningThirdPartyLabel(usercontext) + "</div>";
                    result += "<br />";
                    break;
                case 2: // Code verification If One-Way
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetUILabel(usercontext) + "</div>";
                    result += "<input id=\"##ACCESSCODE##\" name=\"##ACCESSCODE##\" type=\"password\" placeholder=\"Verification Code\" class=\"text fullWidth\" autofocus=\"autofocus\" autocomplete=\"one-time-code\" /><br/>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"checkButton\" type=\"submit\" class=\"submit\" name=\"checkButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMCheck") + "\" onclick=\"fnbtnclicked(4)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollPhone))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    break;
                case 3: // Successfull test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<input id=\"finishButton\" type=\"submit\" class=\"submit\" name=\"finishButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "<br/>";
                    break;
                case 4: // Wrong result test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"priorButton\" type=\"submit\" class=\"submit\" name=\"priorButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPPRIOR") + "\" onclick=\"fnbtnclicked(0)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollPhone))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += "<br/>";
                    break;
            }
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";

            result += "</form>";
            return result;
        }
#endregion

#region EnrollBiometrics
        /// <summary>
        /// GetFormPreRenderHtmlEnrollBio method implementation
        /// </summary>
        public override string GetFormPreRenderHtmlEnrollBio(AuthenticationContext usercontext)
        {
            string result = string.Empty;

            result += "<script type=\"text/javascript\">" + CR;

            result += "function OnRefreshPost(clicked, response)" + CR;
            result += "{" + CR;
            result += "   fnbtnclicked(clicked);" + CR;
            result += "   var jsonCredentials = document.getElementById('attestationResponse');" + CR;
            result += "   jsonCredentials.value = JSON.stringify(response);" + CR;
            result += "   document.getElementById('enrollbiometricsForm').submit();" + CR; 
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function fnbtnclicked(id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   return true;" + CR;
            result += "}";
            result += CR;

            result += "function fnlnkclicked(frm, id)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   frm.submit();" + CR;
            result += "   return true;" + CR;
            result += "}";
            result += CR;

            result += "function fn2lnkclicked(frm, id)" + CR;
            result += "{" + CR;
            result += "   var delbio = document.getElementById('delbio');" + CR;
            result += "   if (!delbio)" + CR;
            result += "   {" + CR;
            result += "      return false;" + CR;
            result += "   }";
            result += "   if (delbio.style.color===\"grey\")" + CR;
            result += "   {" + CR;
            result += "      return false;" + CR;
            result += "   }";
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   frm.submit();" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function SetLinkState(value, aaguid)" + CR;
            result += "{" + CR;
            result += "   var delbio = document.getElementById('delbio');" + CR;
            result += "   if (value)" + CR;
            result += "   {" + CR;
            result += "     delbio.style.color = \"blue\";" + CR;
            result += "     delbio.style.cursor = \"pointer\";" + CR;
            result += "   }";
            result += "   else" + CR;
            result += "   {" + CR;
            result += "     delbio.style.color = \"gray\";" + CR;
            result += "     delbio.style.cursor = \"default\";" + CR;
            result += "   }" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function SetJsError(message)" + CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = 5;" + CR;
            result += "   var err = document.getElementById('jserror');" + CR;
            result += "   err.value = message;" + CR;
            result += "   document.getElementById('enrollbiometricsForm').submit();" + CR;
            result += "   return true;" + CR;
            result += "}";
            result += CR;

            if (usercontext.WizPageID == 1)
            {
                result += GetWebAuthNAttestationScript(usercontext);
                result += GetFormHtmlWebAuthNSupport(usercontext);
            }
            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlEnrollBio method implementation
        /// </summary>
        public override string GetFormHtmlEnrollBio(AuthenticationContext usercontext)
        {
            string result = string.Empty;
            if (usercontext.WizPageID == 1)
            {
                result += "<script type=\"text/javascript\">" + CR;
                result += "if (window.addEventListener)" + CR;
                result += "{" + CR;
                result += "   window.addEventListener('load', RegisterWebAuthN, false);" + CR;
                result += "}" + CR;
                result += "else if (window.attachEvent)" + CR;
                result += "{" + CR;
                result += "   window.attachEvent('onload', RegisterWebAuthN);" + CR;
                result += "}" + CR;
                result += "</script>" + CR;
            }
            
            IExternalProvider prov = RuntimeAuthProvider.GetProvider(PreferredMethod.Biometrics);
            WebAuthNProvider auth = Provider.Config.WebAuthNProvider;
            IWebAuthNProvider web = prov as IWebAuthNProvider;
            prov.GetAuthenticationContext(usercontext);
            result += "<form method=\"post\" id=\"enrollbiometricsForm\" >";
            switch (usercontext.WizPageID)
            {
                case 0: // Get User input                    
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + prov.GetWizardUILabel(usercontext) + "</div>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetWizardUIComment(usercontext) + "</div><br/>";
                    if (prov.IsUIElementRequired(usercontext, RequiredMethodElements.BiometricParameterRequired))
                    {
                        List<WebAuthNCredentialInformation> creds = web.GetUserStoredCredentials(usercontext);
                        result += "<input id=\"optiongroup0\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + Guid.Empty.ToString() + "\" checked=\"checked\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWRAddBiometrics") + "<br/>";
                        int i = 1;
                        if (creds.Count > 0)
                        {
                            foreach (WebAuthNCredentialInformation cr in creds)
                            {
                                if (i <= 2)
                                {
                                    if ((auth != null) && auth.UseNickNames)
                                        result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + cr.CredentialID + "\" /> <b>" + cr.CredType + " " + cr.NickName + "</b> " + cr.RegDate.ToShortDateString() + " <b>(" + cr.SignatureCounter.ToString() + ")</b><br/>";
                                    else
                                        result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + cr.CredentialID + "\" /> <b>" + cr.CredType + "</b> " + cr.RegDate.ToShortDateString() + " <b>(" + cr.SignatureCounter.ToString() + ")</b><br/>";
                                }
                                if (i==3)
                                    result += "<p style = \"text-indent:20px;\" >More...</p>";
                                i++;
                            }
                        }
                        if (i > 1)
                        {
                            result += "<a class=\"actionLink\" href=\"#\" id=\"morebio\" name=\"morebio\" onclick=\"fnlnkclicked(enrollbiometricsForm, 7)\" >" + web.GetManageLinkLabel(usercontext) + "</a>";
                        }
                    }

                    if (Provider.KeepMySelectedOptionOn())
                    {
                        MFAUser reg = RuntimeRepository.GetMFAUser(Provider.Config, usercontext.UPN);
                        result += "<br/>";
                        if (reg != null)
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\"" + (reg.PreferredMethod == PreferredMethod.Biometrics ? " checked=\"checked\"" : "\"\"") + "\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                        else
                            result += "<input id=\"##REMEMBER##\" type=\"checkbox\" name=\"##REMEMBER##\" > " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlCHOOSEOptionRemember2") + "<br/>";
                    }
                    if (!string.IsNullOrEmpty(prov.GetAccountManagementUrl(usercontext)))
                    {
                        result += "<br/>";
                        result += "<input type=\"checkbox\" id=\"##MANAGEACCOUNT##\" name=\"##MANAGEACCOUNT##\" /> " + prov.GetUIAccountManagementLabel(usercontext) + "<br/>";
                    }

                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    if ((auth != null) && auth.UseNickNames)
                        result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(6)\" />";
                    else
                        result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollBiometrics))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += GetFormHtmlMessageZone(usercontext);
                    break;
                case 1:
                    result += "<br/>";
                    result += "<table>";
                    result += "  <tr>";
                    result += "    <td width=\"30px\">";
                    result += "    </td>";
                    result += "    <td width=\"100%\" align=\"center\" >";
                    result += "      <img id=\"ms\" src=\"data:image/gif;base64," + Convert.ToBase64String(images.biometrics.ToByteArray(ImageFormat.Gif)) + "\"/>";
                    result += "    </td>";
                    result += "    <td width=\"30px\">";
                    result += "    </td>";
                    result += "  </tr>";
                    result += "</table>";
                    result += "<br/>";
                    result += "<div class=\"fieldMargin smallText\">" + prov.GetUIMessage(usercontext) + "</div>";
                    result += "<br/>";
                    break;
                case 3: // Successfull test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<input id=\"finishButton\" type=\"submit\" class=\"submit\" name=\"finishButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "<br/>";
                    break;
                case 4: // Wrong result test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"priorButton\" type=\"submit\" class=\"submit\" name=\"priorButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPPRIOR") + "\" onclick=\"fnbtnclicked(0)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollBiometrics))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += "<br/>";
                    break;
                case 5: // Manage credentials list
                    if (prov.IsUIElementRequired(usercontext, RequiredMethodElements.BiometricParameterRequired))
                    {
                        List<WebAuthNCredentialInformation> creds = web.GetUserStoredCredentials(usercontext);
                        result += "<div class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWRManageBiometrics") + "</div><br/>";
                        if (creds.Count < 10)
                            result += "<input id=\"optiongroup0\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + Guid.Empty.ToString() + "\" checked=\"checked\" onchange=\"SetLinkState(false, '" + Guid.Empty.ToString() + "')\" /> " + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWRAddBiometrics") + "<br/>";
                        int i = 1;
                        if (creds.Count > 0)
                        {
                            foreach (WebAuthNCredentialInformation cr in creds)
                            {
                                if (i <= 10)
                                {
                                    if ((auth != null) && auth.UseNickNames)
                                        result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + cr.CredentialID + "\" onchange=\"SetLinkState(true, '" + cr.CredentialID + "')\" /> <b>" + cr.CredType + " " + cr.NickName + "</b> " + cr.RegDate.ToShortDateString() + " <b>(" + cr.SignatureCounter.ToString() + ")</b> <br/>";
                                    else
                                        result += "<input id=\"optiongroup" + i.ToString() + "\" name=\"##OPTIONITEM##\" type=\"radio\" value=\"" + cr.CredentialID + "\" onchange=\"SetLinkState(true, '" + cr.CredentialID + "')\" /> <b>" + cr.CredType + "</b> " + cr.RegDate.ToShortDateString() + " <b>(" + cr.SignatureCounter.ToString() + ")</b> <br/>";
                                }
                                i++;
                            }
                            result += "<a class=\"actionLink\" href=\"#\" id=\"delbio\" name=\"delbio\"  onclick=\"fn2lnkclicked(enrollbiometricsForm, 8)\" style=\"cursor: default; color:grey;\"> " + web.GetDeleteLinkLabel(usercontext) + "</a>";
                        }
                        result += "<br/>";
                        result += "<table><tr>";
                        result += "<td>";
                        if ((auth != null) && auth.UseNickNames)
                            result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(6)\" />";
                        else
                            result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                        result += "</td>";
                        result += "<td style=\"width: 15px\" />";
                        result += "<td>";
                        if (Utilities.CanCancelWizard(usercontext, prov, ProviderPageMode.EnrollBiometrics))
                            result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                        result += "</td>";
                        result += "</tr></table>";
                    }
                    break;
                case 6:
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMNickNamesLabel") + "</div>";
                    result += "<input id=\"AutenhticatorName\" name=\"AutenhticatorName\" type=\"input\" placeholder=\"No Name\" class=\"text fullWidth\" autofocus=\"autofocus\" /><br/>";
                    result += "<div class=\"fieldMargin smallText\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMNickNamesMessage") + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"checkButton\" type=\"submit\" class=\"submit\" name=\"checkButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    result += "</td>";
                    result += "</tr></table>";
                    break;
            }
            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";
            result += "<input id=\"jserror\" type=\"hidden\" name=\"jserror\" value=\"\" />";
            if (usercontext.WizPageID == 1)
            {
                result += "<input id=\"attestationResponse\" type=\"hidden\" name=\"attestationResponse\" />";
            }
            result += "</form>";
            return result;
        }
#endregion

#region EnrollPINCode
        /// <summary>
        /// GetFormPreRenderHtmlEnrollPinCode method implementation
        /// </summary>
        public override string GetFormPreRenderHtmlEnrollPinCode(AuthenticationContext usercontext)
        {
            string result = string.Empty;

            string pl = Provider.Config.PinLength.ToString();
            string reg = @"/([0-9]{4," + pl + "})/";

            result += "<script type=\"text/javascript\">" + CR;

            result += "function ValidateRegistration(frm)" + CR;
            result += "{" + CR;
            if (usercontext.WizPageID == 0)
            {
                result += "   var pinformat = " + reg + " ;" + CR;
                result += "   var pincode = document.getElementById('##PINCODE##');" + CR;
                result += "   var err = document.getElementById('errorText');" + CR;
                result += "   var canceled = document.getElementById('##SELECTED##');" + CR;
                result += "   if ((canceled) && (canceled.value==1))" + CR;
                result += "   {" + CR;
                result += "      return true;" + CR;
                result += "   }" + CR;
                result += "   if ((pincode) && (pincode.value=='') && (pincode.placeholder==''))" + CR;
                result += "   {" + CR;
                result += "      err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidIncorrectPinCode") + "\";" + CR;
                result += "      return false;" + CR;
                result += "   }" + CR;
                result += "   if ((pincode) && (pincode.value!==''))" + CR;
                result += "   {" + CR;
                result += "      if (!pincode.value.match(pinformat))" + CR;
                result += "      {" + CR;
                result += "         err.innerHTML = \"" + Resources.GetString(ResourcesLocaleKind.Validation, "ValidIncorrectPinCode") + "\";" + CR;
                result += "         return false;" + CR;
                result += "      }" + CR;
                result += "   }" + CR;
                result += "   err.innerHTML = \"\";" + CR;
            }
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "function fnbtnclicked(id)" +CR;
            result += "{" + CR;
            result += "   var lnk = document.getElementById('##SELECTED##');" + CR;
            result += "   lnk.value = id;" + CR;
            result += "   return true;" + CR;
            result += "}" + CR;
            result += CR;

            result += "</script>" + CR;
            return result;
        }

        /// <summary>
        /// GetFormHtmlEnrollEmail method implmentation
        /// </summary>
        public override string GetFormHtmlEnrollPinCode(AuthenticationContext usercontext)
        {
            string result = "<form method=\"post\" id=\"enrollPinForm\" onsubmit=\"return ValidateRegistration(this)\" >";
            switch (usercontext.WizPageID)
            {
                case 0: // Get User Pin                    
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + BaseExternalProvider.GetPINWizardUILabel(usercontext) + "</div>";
                    result += "<div class=\"fieldMargin smallText\">" + string.Format(Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWRPinCode"), Provider.Config.PinLength.ToString()) + "</div><br/>";
                    if (usercontext.PinCode <= 0)
                        result += "<input id=\"##PINCODE##\" name=\"##PINCODE##\" type=\"text\" placeholder=\"" + Utilities.StripPinCode(Provider.Config.DefaultPin) + "\" autocomplete=\"one-time-code\" class=\"text fullWidth\" /><br/>";
                    else
                        result += "<input id=\"##PINCODE##\" name=\"##PINCODE##\" type=\"text\" placeholder=\"" + Utilities.StripPinCode(usercontext.PinCode) + "\" autocomplete=\"one-time-code\" class=\"text fullWidth\" /><br/>";
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"nextButton\" type=\"submit\" class=\"submit\" name=\"nextButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelWVERIFYOTP") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, null, ProviderPageMode.EnrollPin))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += GetFormHtmlMessageZone(usercontext);
                    break;
                case 2: // Code verification
                    result += "<div id=\"wizardMessage\" class=\"groupMargin\">" + BaseExternalProvider.GetPINLabel(usercontext) + "</div>";
                    result += "<input id=\"##PINCODE##\" name=\"##PINCODE##\" type=\"password\" placeholder=\"PIN\" class=\"text fullWidth\" autofocus=\"autofocus\" autocomplete=\"one-time-code\" /><br/>";
                    result += "<div class=\"fieldMargin smallText\">" + BaseExternalProvider.GetPINMessage(usercontext) + "</div><br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"checkButton\" type=\"submit\" class=\"submit\" name=\"checkButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlUIMCheck") + "\" onclick=\"fnbtnclicked(4)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, null, ProviderPageMode.EnrollPin))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    break;
                case 3: // Successfull test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<input id=\"finishButton\" type=\"submit\" class=\"submit\" name=\"finishButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPOK") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "<br/>";
                    break;
                case 4: // Wrong result test
                    result += "<br/>";
                    result += GetFormHtmlMessageZone(usercontext);
                    result += "<br/>";
                    result += "<table><tr>";
                    result += "<td>";
                    result += "<input id=\"priorButton\" type=\"submit\" class=\"submit\" name=\"priorButton\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlLabelVERIFYOTPPRIOR") + "\" onclick=\"fnbtnclicked(2)\" />";
                    result += "</td>";
                    result += "<td style=\"width: 15px\" />";
                    result += "<td>";
                    if (Utilities.CanCancelWizard(usercontext, null, ProviderPageMode.EnrollPin))
                        result += "<input id=\"mfa-cancelButton\" type=\"submit\" class=\"submit\" name=\"cancel\" value=\"" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlPWDCancel") + "\" onclick=\"fnbtnclicked(1)\" />";
                    result += "</td>";
                    result += "</tr></table>";
                    result += "<br/>";
                    break;
            }

            result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
            result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
            result += "<input id=\"##SELECTED##\" type=\"hidden\" name=\"##SELECTED##\" value=\"1\" />";

            result += "</form>";
            return result;
        }
#endregion

#region Private methods
        /// <summary>
        /// GetPartHtmlSelectMethod method ipmplmentation
        /// </summary>
        private string GetPartHtmlSelectMethod(AuthenticationContext usercontext)
        {
            MFAUser reg = RuntimeRepository.GetMFAUser(Provider.Config, usercontext.UPN);
            PreferredMethod method = PreferredMethod.None;
            if (reg == null)
                method = usercontext.PreferredMethod;
            else
                method = reg.PreferredMethod;
            string result = string.Empty;
            result += "<b><div class=\"fieldMargin\">" + Resources.GetString(ResourcesLocaleKind.Html, "HtmlREGAccessMethod") + "</div></b>";
            result += "<select id=\"##SELECTOPTIONS##\" name=\"##SELECTOPTIONS##\" role=\"combobox\"  required=\"required\" contenteditable=\"false\" class=\"text fullWidth\" >";

            result += "<option value=\"0\" " + ((method == PreferredMethod.Choose) ? "selected=\"true\"> " : "> ") + Resources.GetString(ResourcesLocaleKind.Html, "HtmlREGOptionChooseBest") + "</option>";

            if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Code))
                result += "<option value=\"1\" " + ((method == PreferredMethod.Code) ? "selected=\"true\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.Code).GetUIListChoiceLabel(usercontext) + "</option>";

            if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Biometrics))
                result += "<option value=\"5\" " + ((method == PreferredMethod.Biometrics) ? "selected=\"true\">" : ">") + RuntimeAuthProvider.GetProvider(PreferredMethod.Biometrics).GetUIListChoiceLabel(usercontext) + "</option>";
            if ((!Provider.Config.IsPrimaryAuhentication) || (Provider.Config.PrimaryAuhenticationOptions.HasFlag(PrimaryAuthOptions.Externals)))
            {
                if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Email))
                    result += "<option value=\"2\" " + ((method == PreferredMethod.Email) ? "selected=\"true\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.Email).GetUIListChoiceLabel(usercontext) + "</option>";

                if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.External))
                    result += "<option value=\"3\" " + ((method == PreferredMethod.External) ? "selected=\"true\"> " : "> ") + RuntimeAuthProvider.GetProvider(PreferredMethod.External).GetUIListChoiceLabel(usercontext) + "</option>";

                if (RuntimeAuthProvider.IsProviderAvailableForUser(usercontext, PreferredMethod.Azure))
                    result += "<option value=\"4\" " + ((method == PreferredMethod.Azure) ? "selected=\"true\">" : ">") + RuntimeAuthProvider.GetProvider(PreferredMethod.Azure).GetUIListChoiceLabel(usercontext) + "</option>";
            }
            result += "</select><br/>";
            return result;
        }
#endregion
    }
}