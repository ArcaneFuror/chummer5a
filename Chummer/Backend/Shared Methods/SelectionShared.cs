/*  This file is part of Chummer5a.
 *
 *  Chummer5a is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Chummer5a is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Chummer5a.  If not, see <http://www.gnu.org/licenses/>.
 *
 *  You can obtain the full source code for Chummer5a at
 *  https://github.com/chummer5a/chummer5a
 */
using Chummer.Backend.Attributes;
using Chummer.Backend.Equipment;
using Chummer.Backend.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Chummer
{
    public static class SelectionShared
    {
        #region XmlNode overloads for selection methods.

        /// <summary>Evaluates requirements of a given node against a given Character object.</summary>
        /// <param name="xmlNode">XmlNode of the object.</param>
        /// <param name="objCharacter">Character object against which to check.</param>
        /// <param name="objParent">Parent object to be compared to.</param>
        /// <param name="strLocalName">Name of the type of item being checked for displaying messages. If empty or null, no message is displayed.</param>
        /// <param name="strIgnoreQuality">Name of a Quality that should be ignored. Typically used when swapping Qualities in career mode.</param>
        /// <param name="strSourceName">Name of the improvement that called this (if it was called by an improvement adding it)</param>
        /// <param name="strLocation">Limb side to use if we need a specific limb side (Left or Right)</param>
        /// <param name="blnIgnoreLimit">Whether to ignore checking for limits on the total amount of this item the character can have.</param>
        /// <returns></returns>
        [Obsolete("This method is a wrapper that calls XPathNavigator instead. Where possible, refactor the calling object to an XPathNavigator instead.", false)]
        public static bool RequirementsMet(this XmlNode xmlNode, Character objCharacter, object objParent = null, string strLocalName = "", string strIgnoreQuality = "", string strSourceName = "", string strLocation = "", bool blnIgnoreLimit = false)
        {
            if (xmlNode == null || objCharacter == null)
                return false;
            // Ignore the rules.
            return objCharacter.IgnoreRules || xmlNode.CreateNavigator().RequirementsMet(objCharacter, objParent, strLocalName, strIgnoreQuality, strSourceName, strLocation, blnIgnoreLimit);
        }

        /// <summary>
        ///     Evaluates the availability of a given node against Availability Limits in Create Mode
        /// </summary>
        /// <param name="objXmlGear"></param>
        /// <param name="objCharacter"></param>
        /// <param name="intRating"></param>
        /// <param name="intAvailModifier"></param>
        /// <returns></returns>
        public static bool CheckAvailRestriction(XmlNode objXmlGear, Character objCharacter, int intRating = 1, int intAvailModifier = 0)
        {
            return objXmlGear != null && objXmlGear.CreateNavigator().CheckAvailRestriction(objCharacter, intRating, intAvailModifier);
        }

        public static bool CheckNuyenRestriction(XmlNode objXmlGear, decimal decMaxNuyen, decimal decCostMultiplier = 1.0m, int intRating = 1)
        {
            return objXmlGear != null && objXmlGear.CreateNavigator().CheckNuyenRestriction(decMaxNuyen, decCostMultiplier, intRating);
        }
        #endregion

        //TODO: Might be a better location for this; Class names are screwy.
        /// <summary>Evaluates requirements of a given node against a given Character object.</summary>
        /// <param name="xmlNode">XmlNode of the object.</param>
        /// <param name="objCharacter">Character object against which to check.</param>
        /// <param name="objParent">Parent object against which to check.</param>
        /// <param name="strLocalName">Name of the type of item being checked for displaying messages. If empty or null, no message is displayed.</param>
        /// <param name="strIgnoreQuality">Name of a Quality that should be ignored. Typically used when swapping Qualities in career mode.</param>
        /// <param name="strSourceName">Name of the improvement that called this (if it was called by an improvement adding it)</param>
        /// <param name="strLocation">Limb side to use if we need a specific limb side (Left or Right)</param>
        /// <param name="blnIgnoreLimit">Whether to ignore checking for limits on the total amount of this item the character can have.</param>
        /// <returns></returns>
        public static bool RequirementsMet(this XPathNavigator xmlNode, Character objCharacter, object objParent = null, string strLocalName = "", string strIgnoreQuality = "", string strSourceName = "", string strLocation = "", bool blnIgnoreLimit = false)
        {
            if (xmlNode == null || objCharacter == null)
                return false;
            // Ignore the rules.
            if (objCharacter.IgnoreRules)
                return true;
            bool blnShowMessage = !string.IsNullOrEmpty(strLocalName);
            // See if the character is in career mode but would want to add a chargen-only Quality
            if (objCharacter.Created)
            {
                if (xmlNode.SelectSingleNode("chargenonly") != null)
                {
                    if (blnShowMessage)
                    {
                        Program.MainForm.ShowMessageBox(
                            string.Format(
                                GlobalOptions.CultureInfo,
                                LanguageManager.GetString("Message_SelectGeneric_ChargenRestriction"),
                                strLocalName),
                            string.Format(
                                GlobalOptions.CultureInfo,
                                LanguageManager.GetString("MessageTitle_SelectGeneric_Restriction"),
                                strLocalName),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }
            }
            // See if the character is using priority-based gen and is trying to add a Quality that can only be added through priorities
            else
            {
                if (xmlNode.SelectSingleNode("careeronly") != null)
                {
                    if (blnShowMessage)
                    {
                        Program.MainForm.ShowMessageBox(
                            string.Format(
                                GlobalOptions.CultureInfo,
                                LanguageManager.GetString("Message_SelectGeneric_CareerOnlyRestriction"),
                                strLocalName),
                            string.Format(
                                GlobalOptions.CultureInfo,
                                LanguageManager.GetString("MessageTitle_SelectGeneric_Restriction"),
                                strLocalName),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }
                if (objCharacter.BuildMethod == CharacterBuildMethod.Priority || objCharacter.BuildMethod == CharacterBuildMethod.SumtoTen)
                {
                    if (xmlNode.SelectSingleNode("onlyprioritygiven") != null)
                    {
                        if (blnShowMessage)
                        {
                            Program.MainForm.ShowMessageBox(
                                string.Format(
                                    GlobalOptions.CultureInfo,
                                    LanguageManager.GetString("Message_SelectGeneric_PriorityRestriction"),
                                    strLocalName),
                                string.Format(
                                    GlobalOptions.CultureInfo,
                                    LanguageManager.GetString("MessageTitle_SelectGeneric_Restriction"),
                                    strLocalName),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return false;
                    }
                }
            }
            if (!blnIgnoreLimit)
            {
                // See if the character already has this Quality and whether or not multiple copies are allowed.
                // If the limit at chargen is different from the actual limit, we need to make sure we fetch the former if the character is in Create mode
                string strLimitString = xmlNode.SelectSingleNode("chargenlimit")?.Value;
                if (string.IsNullOrWhiteSpace(strLimitString) || objCharacter.Created)
                {
                    strLimitString = xmlNode.SelectSingleNode("limit")?.Value;
                    // Default case is each quality can only be taken once
                    if (string.IsNullOrWhiteSpace(strLimitString))
                    {
                        if (xmlNode.Name == "quality" ||
                            xmlNode.Name == "martialart" ||
                            xmlNode.Name == "technique" ||
                            xmlNode.Name == "cyberware" ||
                            xmlNode.Name == "bioware")
                            strLimitString = "1";
                        else
                            strLimitString = bool.FalseString;
                    }
                }
                if (strLimitString != bool.FalseString)
                {
                    StringBuilder objLimitString = new StringBuilder(strLimitString);
                    foreach (string strAttribute in AttributeSection.AttributeStrings)
                    {
                        CharacterAttrib objLoopAttribute = objCharacter.GetAttribute(strAttribute);
                        objLimitString.CheapReplace(strLimitString, "{" + strAttribute + "}", () => objLoopAttribute.TotalValue.ToString(GlobalOptions.InvariantCultureInfo));
                        objLimitString.CheapReplace(strLimitString, "{" + strAttribute + "Base}", () => objLoopAttribute.TotalBase.ToString(GlobalOptions.InvariantCultureInfo));
                    }
                    foreach (string strLimb in Character.LimbStrings)
                    {
                        objLimitString.CheapReplace(strLimitString, "{" + strLimb + "}", () => (string.IsNullOrEmpty(strLocation) ? objCharacter.LimbCount(strLimb) : objCharacter.LimbCount(strLimb) / 2).ToString(GlobalOptions.InvariantCultureInfo));
                    }

                    object objProcess = CommonFunctions.EvaluateInvariantXPath(objLimitString.ToString(), out bool blnIsSuccess);
                    strLimitString = blnIsSuccess ? objProcess.ToString() : "1";

                    // We could set this to a list immediately, but I'd rather the pointer start at null so that no list ends up getting selected for the "default" case below
                    IEnumerable<IHasName> objListToCheck = null;
                    bool blnCheckCyberwareChildren = false;
                    switch (xmlNode.Name)
                    {
                        case "quality":
                            {
                                objListToCheck = objCharacter.Qualities.Where(objQuality => objQuality.SourceName == strSourceName && objQuality.Name != strIgnoreQuality);
                                break;
                            }
                        case "echo":
                        case "metamagic":
                            {
                                objListToCheck = objCharacter.Metamagics;
                                break;
                            }
                        case "art":
                            {
                                objListToCheck = objCharacter.Arts;
                                break;
                            }
                        case "enhancement":
                            {
                                objListToCheck = objCharacter.Enhancements;
                                break;
                            }
                        case "power":
                            {
                                objListToCheck = objCharacter.Powers;
                                break;
                            }
                        case "critterpower":
                            {
                                objListToCheck = objCharacter.CritterPowers;
                                break;
                            }
                        case "martialart":
                            {
                                objListToCheck = objCharacter.MartialArts;
                                break;
                            }
                        case "technique":
                            {
                                List<MartialArtTechnique> objTempList;
                                if (objParent is MartialArt objArt)
                                {
                                    objTempList = new List<MartialArtTechnique>(objArt.Techniques.Count);
                                    objTempList.AddRange(objArt.Techniques);
                                }
                                else
                                {
                                    objTempList = new List<MartialArtTechnique>(objCharacter.MartialArts.Count);
                                    foreach (MartialArt objMartialArt in objCharacter.MartialArts)
                                    {
                                        objTempList.AddRange(objMartialArt.Techniques);
                                    }
                                }
                                objListToCheck = objTempList;
                                break;
                            }
                        case "cyberware":
                        case "bioware":
                            {
                                blnCheckCyberwareChildren = true;
                                break;
                            }
                        default:
                            {
                                Utils.BreakIfDebug();
                                break;
                            }
                    }

                    int intLimit = Convert.ToInt32(strLimitString, GlobalOptions.InvariantCultureInfo);
                    int intExtendedLimit = intLimit;
                    string strLimitWithInclusions = xmlNode.SelectSingleNode("limitwithinclusions")?.Value;
                    if (!string.IsNullOrEmpty(strLimitWithInclusions))
                    {
                        intExtendedLimit = Convert.ToInt32(strLimitWithInclusions, GlobalOptions.InvariantCultureInfo);
                    }
                    int intCount = 0;
                    int intExtendedCount = 0;
                    if (objListToCheck != null || blnCheckCyberwareChildren)
                    {
                        var lstToCheck = objListToCheck?.ToList() ?? new List<IHasName>();
                        string strNameNode = xmlNode.SelectSingleNode("name")?.Value;
                        if (blnCheckCyberwareChildren)
                        {
                            intCount = string.IsNullOrEmpty(strLocation)
                                ? objCharacter.Cyberware.DeepCount(x => x.Children, x => string.IsNullOrEmpty(x.PlugsIntoModularMount) && strNameNode == x.Name)
                                : objCharacter.Cyberware.DeepCount(x => x.Children, x => string.IsNullOrEmpty(x.PlugsIntoModularMount) && x.Location == strLocation && strNameNode == x.Name);
                        }
                        else
                            intCount = lstToCheck.Count(objItem => strNameNode == objItem.Name);
                        intExtendedCount = intCount;
                        // In case one item is split up into multiple entries with different names, e.g. Indomitable quality, we need to be able to check all those entries against the limit
                        XPathNavigator xmlIncludeInLimit = xmlNode.SelectSingleNode("includeinlimit");
                        if (xmlIncludeInLimit != null)
                        {
                            List<string> lstNamesIncludedInLimit = new List<string>();
                            if (!string.IsNullOrEmpty(strNameNode))
                            {
                                lstNamesIncludedInLimit.Add(strNameNode);
                            }
                            foreach (XPathNavigator objChildXml in xmlIncludeInLimit.SelectChildren(XPathNodeType.Element))
                            {
                                lstNamesIncludedInLimit.Add(objChildXml.Value);
                            }

                            if (blnCheckCyberwareChildren)
                            {
                                intExtendedCount = string.IsNullOrEmpty(strLocation)
                                    ? objCharacter.Cyberware.DeepCount(x => x.Children, x => string.IsNullOrEmpty(x.PlugsIntoModularMount) && lstNamesIncludedInLimit.Any(objLimitName => objLimitName == x.Name))
                                    : objCharacter.Cyberware.DeepCount(x => x.Children, x => string.IsNullOrEmpty(x.PlugsIntoModularMount) && x.Location == strLocation && lstNamesIncludedInLimit.Any(strName => strName == x.Name));
                            }
                            else
                                intExtendedCount = lstToCheck.Count(objItem => lstNamesIncludedInLimit.Any(objLimitName => objLimitName == objItem.Name));
                        }
                    }
                    if (intCount >= intLimit || intExtendedCount >= intExtendedLimit)
                    {
                        if (blnShowMessage)
                        {
                            Program.MainForm.ShowMessageBox(
                                string.Format(
                                    GlobalOptions.CultureInfo,
                                    LanguageManager.GetString("Message_SelectGeneric_Limit"),
                                    strLocalName, intLimit == 0 ? "1" : intLimit.ToString(GlobalOptions.CultureInfo)),
                                string.Format(
                                    GlobalOptions.CultureInfo,
                                    LanguageManager.GetString("MessageTitle_SelectGeneric_Limit"),
                                    strLocalName),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return false;
                    }
                }
            }

            XPathNavigator xmlForbiddenNode = xmlNode.SelectSingleNode("forbidden");
            if (xmlForbiddenNode != null)
            {
                // Loop through the oneof requirements.
                foreach (XPathNavigator objXmlOneOf in xmlForbiddenNode.Select("oneof"))
                {
                    foreach (XPathNavigator xmlForbiddenItemNode in objXmlOneOf.SelectChildren(XPathNodeType.Element))
                    {
                        // The character is not allowed to take the Quality, so display a message and uncheck the item.
                        if (xmlForbiddenItemNode.TestNodeRequirements(objCharacter, objParent, out string strName, strIgnoreQuality, blnShowMessage))
                        {
                            if (blnShowMessage)
                            {
                                Program.MainForm.ShowMessageBox(
                                    string.Format(
                                        GlobalOptions.CultureInfo,
                                        LanguageManager.GetString("Message_SelectGeneric_Restriction"),
                                        strLocalName) + strName,
                                    string.Format(
                                        GlobalOptions.CultureInfo,
                                        LanguageManager.GetString("MessageTitle_SelectGeneric_Restriction"),
                                        strLocalName),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return false;
                        }
                    }
                }
            }

            XPathNavigator xmlRequiredNode = xmlNode.SelectSingleNode("required");
            if (xmlRequiredNode != null)
            {
                StringBuilder objRequirement = new StringBuilder();
                bool blnRequirementMet = true;

                // Loop through the oneof requirements.
                foreach (XPathNavigator objXmlOneOf in xmlRequiredNode.Select("oneof"))
                {
                    bool blnOneOfMet = false;
                    StringBuilder objThisRequirement = new StringBuilder(Environment.NewLine + LanguageManager.GetString("Message_SelectQuality_OneOf"));
                    foreach (XPathNavigator xmlRequiredItemNode in objXmlOneOf.SelectChildren(XPathNodeType.Element))
                    {
                        if (xmlRequiredItemNode.TestNodeRequirements(objCharacter, objParent, out string strName, strIgnoreQuality, blnShowMessage))
                        {
                            blnOneOfMet = true;
                            break;
                        }
                        if (blnShowMessage)
                            objThisRequirement.Append(strName);
                    }

                    // Update the flag for requirements met.
                    if (!blnOneOfMet)
                        blnRequirementMet = false;
                    if (blnShowMessage && !blnOneOfMet)
                        objRequirement.Append(objThisRequirement);
                    else if (!blnRequirementMet)
                        break;
                }

                if (blnRequirementMet || blnShowMessage)
                {
                    // Loop through the allof requirements.
                    foreach (XPathNavigator objXmlAllOf in xmlRequiredNode.Select("allof"))
                    {
                        bool blnAllOfMet = true;
                        StringBuilder objThisRequirement = new StringBuilder(Environment.NewLine + LanguageManager.GetString("Message_SelectQuality_AllOf"));
                        foreach (XPathNavigator xmlRequiredItemNode in objXmlAllOf.SelectChildren(XPathNodeType.Element))
                        {
                            // If this item was not found, fail the AllOfMet condition.
                            if (!xmlRequiredItemNode.TestNodeRequirements(objCharacter, objParent, out string strName, strIgnoreQuality, blnShowMessage))
                            {
                                blnAllOfMet = false;
                                if (blnShowMessage)
                                    objThisRequirement.Append(strName);
                                else
                                    break;
                            }
                        }

                        // Update the flag for requirements met.
                        if (!blnAllOfMet)
                            blnRequirementMet = false;
                        if (blnShowMessage)
                            objRequirement.Append(objThisRequirement);
                        else if (!blnRequirementMet)
                            break;
                    }
                }

                // The character has not met the requirements, so display a message and uncheck the item.
                if (!blnRequirementMet)
                {
                    if (blnShowMessage)
                    {
                        Program.MainForm.ShowMessageBox(
                            string.Format(
                                GlobalOptions.CultureInfo,
                                LanguageManager.GetString("Message_SelectGeneric_Requirement"),
                                strLocalName) + objRequirement,
                            string.Format(
                                GlobalOptions.CultureInfo,
                                LanguageManager.GetString("MessageTitle_SelectGeneric_Requirement"),
                                strLocalName),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }
            }
            return true;
        }

        public static bool TestNodeRequirements(this XPathNavigator xmlNode, Character objCharacter, object objParent, out string strName, string strIgnoreQuality = "", bool blnShowMessage = true)
        {
            strName = string.Empty;
            if (xmlNode == null || objCharacter == null)
            {
                return false;
            }
            string strNodeInnerText = xmlNode.Value;
            string strNodeName = xmlNode.SelectSingleNode("name")?.Value ?? string.Empty;
            switch (xmlNode.Name)
            {
                case "attribute":
                    {
                        // Check to see if an Attribute meets a requirement.
                        CharacterAttrib objAttribute = objCharacter.GetAttribute(strNodeName);
                        int intTargetValue = Convert.ToInt32(xmlNode.SelectSingleNode("total")?.Value, GlobalOptions.InvariantCultureInfo);
                        if (blnShowMessage)
                            strName = string.Format(GlobalOptions.CultureInfo, "{0}\t{1}{2}{3}", Environment.NewLine, objAttribute.DisplayAbbrev, LanguageManager.GetString("String_Space"), intTargetValue);
                        // Special cases for when we want to check if a special attribute is enabled
                        if (intTargetValue == 1)
                        {
                            switch (objAttribute.Abbrev)
                            {
                                case "MAG":
                                    return objCharacter.MAGEnabled;
                                case "MAGAdept":
                                    return objCharacter.MAGEnabled && objCharacter.IsMysticAdept;
                                case "RES":
                                    return objCharacter.RESEnabled;
                                case "DEP":
                                    return objCharacter.DEPEnabled;
                            }
                        }

                        if (xmlNode.SelectSingleNode("natural") != null)
                        {
                            return objAttribute.Value >= intTargetValue;
                        }
                        return objAttribute.TotalValue >= intTargetValue;
                    }
                case "attributetotal":
                    {
                        string strNodeAttributes = xmlNode.SelectSingleNode("attributes")?.Value ?? string.Empty;
                        int intNodeVal = Convert.ToInt32(xmlNode.SelectSingleNode("val")?.Value, GlobalOptions.InvariantCultureInfo);
                        // Check if the character's Attributes add up to a particular total.
                        string strAttributes = strNodeAttributes;
                        string strValue = strNodeAttributes;
                        foreach (string strAttribute in AttributeSection.AttributeStrings)
                        {
                            CharacterAttrib objLoopAttrib = objCharacter.GetAttribute(strAttribute);
                            if (strNodeAttributes.Contains(objLoopAttrib.Abbrev))
                            {
                                strAttributes = strAttributes.Replace(strAttribute, objLoopAttrib.DisplayAbbrev);
                                strValue = strValue.Replace(strAttribute, xmlNode.SelectSingleNode("natural") != null
                                    ? objLoopAttrib.Value.ToString(GlobalOptions.InvariantCultureInfo)
                                    : objLoopAttrib.TotalValue.ToString(GlobalOptions.InvariantCultureInfo));
                            }
                        }
                        if (blnShowMessage)
                            strName = string.Format("{0}\t{1} {2}", Environment.NewLine, strAttributes, intNodeVal);
                        object objProcess = CommonFunctions.EvaluateInvariantXPath(strValue, out bool blnIsSuccess);
                        return (blnIsSuccess ? Convert.ToInt32(objProcess, GlobalOptions.InvariantCultureInfo) : 0) >= intNodeVal;
                    }
                case "careerkarma":
                    {
                        // Check Career Karma requirement.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' + string.Format(GlobalOptions.CultureInfo, LanguageManager.GetString("Message_SelectQuality_RequireKarma"), strNodeInnerText);
                        return objCharacter.CareerKarma >= Convert.ToInt32(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                    }
                case "critterpower":
                    {
                        // Run through all of the Powers the character has and see if the current required item exists.
                        if (objCharacter.CritterEnabled)
                        {
                            CritterPower critterPower = objCharacter.CritterPowers.FirstOrDefault(p => p.Name == strNodeInnerText);
                            if (critterPower != null)
                            {
                                if (blnShowMessage)
                                    strName = critterPower.DisplayNameShort(GlobalOptions.Language);
                                return true;
                            }
                        }
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("critterpowers.xml").SelectSingleNode(string.Format("/chummer/powers/power[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("Tab_Critter"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("Tab_Critter"));
                        }
                        return false;
                    }
                case "bioware":
                    {
                        int count = Convert.ToInt32(xmlNode.SelectSingleNode("@count")?.Value ?? "1", GlobalOptions.InvariantCultureInfo);
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("bioware.xml").SelectSingleNode(string.Format("/chummer/biowares/bioware[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Bioware"), strTranslate)
                                : string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Bioware"), strNodeInnerText);
                        }
                        string strWareNodeSelectAttribute = xmlNode.SelectSingleNode("@select")?.Value ?? string.Empty;
                        return objCharacter.Cyberware.DeepCount(x => x.Children, objCyberware => objCyberware.Name == strNodeInnerText &&
                                objCyberware.SourceType == Improvement.ImprovementSource.Bioware && string.IsNullOrEmpty(objCyberware.PlugsIntoModularMount) &&
                               (string.IsNullOrEmpty(strWareNodeSelectAttribute) || strWareNodeSelectAttribute == objCyberware.Extra)) >= count;
                    }
                case "cyberware":
                    {
                        int count = Convert.ToInt32(xmlNode.SelectSingleNode("@count")?.Value ?? "1", GlobalOptions.InvariantCultureInfo);
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("cyberware.xml").SelectSingleNode(string.Format("/chummer/cyberwares/cyberware[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Cyberware"), strTranslate)
                                : string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Cyberware"), strNodeInnerText);
                        }
                        string strWareNodeSelectAttribute = xmlNode.SelectSingleNode("@select")?.Value ?? string.Empty;
                        return objCharacter.Cyberware.DeepCount(x => x.Children, objCyberware => objCyberware.Name == strNodeInnerText &&
                                objCyberware.SourceType == Improvement.ImprovementSource.Cyberware && string.IsNullOrEmpty(objCyberware.PlugsIntoModularMount) &&
                               (string.IsNullOrEmpty(strWareNodeSelectAttribute) || strWareNodeSelectAttribute == objCyberware.Extra)) >= count;
                    }
                case "biowarecontains":
                    {
                        int count = Convert.ToInt32(xmlNode.SelectSingleNode("@count")?.Value ?? "1", GlobalOptions.InvariantCultureInfo);
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("bioware.xml").SelectSingleNode(string.Format("/chummer/biowares/bioware[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Bioware"), strTranslate)
                                : string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Bioware"), strNodeInnerText);
                        }
                        string strWareNodeSelectAttribute = xmlNode.SelectSingleNode("@select")?.Value ?? string.Empty;
                        return objCharacter.Cyberware.DeepCount(x => x.Children, objCyberware => objCyberware.Name.Contains(strNodeInnerText) &&
                                objCyberware.SourceType == Improvement.ImprovementSource.Bioware && string.IsNullOrEmpty(objCyberware.PlugsIntoModularMount) &&
                               (string.IsNullOrEmpty(strWareNodeSelectAttribute) || strWareNodeSelectAttribute == objCyberware.Extra)) >= count;
                    }
                case "cyberwarecontains":
                    {
                        int count = Convert.ToInt32(xmlNode.SelectSingleNode("@count")?.Value ?? "1", GlobalOptions.InvariantCultureInfo);
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("cyberware.xml").SelectSingleNode(string.Format("/chummer/cyberwares/cyberware[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Cyberware"), strTranslate)
                                : string.Format("{0}\t{1} {2}", Environment.NewLine, LanguageManager.GetString("Label_Cyberware"), strNodeInnerText);
                        }
                        string strWareNodeSelectAttribute = xmlNode.SelectSingleNode("@select")?.Value ?? string.Empty;
                        return objCharacter.Cyberware.DeepCount(x => x.Children, objCyberware => objCyberware.Name.Contains(strNodeInnerText) &&
                                objCyberware.SourceType == Improvement.ImprovementSource.Cyberware && string.IsNullOrEmpty(objCyberware.PlugsIntoModularMount) &&
                               (string.IsNullOrEmpty(strWareNodeSelectAttribute) || strWareNodeSelectAttribute == objCyberware.Extra)) >= count;
                    }
                case "damageresistance":
                    {
                        // Damage Resistance must be a particular value.
                        if (blnShowMessage)
                            strName = string.Format("{0}\t{1}", Environment.NewLine, LanguageManager.GetString("String_DamageResistance"));
                        return objCharacter.BOD.TotalValue + ImprovementManager.ValueOf(objCharacter, Improvement.ImprovementType.DamageResistance) >= Convert.ToInt32(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                    }
                case "ess":
                    {
                        string strEssNodeGradeAttributeText = xmlNode.SelectSingleNode("@grade")?.Value ?? string.Empty;
                        if (!string.IsNullOrEmpty(strEssNodeGradeAttributeText))
                        {
                            HashSet<string> setEssNodeGradeAttributeText = new HashSet<string>(strEssNodeGradeAttributeText.Split(','));
                            decimal decGrade =
                                objCharacter.Cyberware.Where(
                                        objCyberware =>
                                            setEssNodeGradeAttributeText.Any(func => objCyberware.Grade.Name.Contains(func)))
                                    .AsParallel().Sum(objCyberware => objCyberware.CalculatedESS);
                            if (strNodeInnerText.StartsWith('-'))
                            {
                                // Essence must be less than the value.
                                if (blnShowMessage)
                                    strName = Environment.NewLine + '\t' +
                                              string.Format(GlobalOptions.CultureInfo
                                                  , LanguageManager.GetString("Message_SelectQuality_RequireESSGradeBelow")
                                                  , strNodeInnerText
                                                  , strEssNodeGradeAttributeText
                                                  , decGrade.ToString(GlobalOptions.CultureInfo));
                                return decGrade < Convert.ToDecimal(strNodeInnerText.TrimStart('-'), GlobalOptions.InvariantCultureInfo);
                            }
                            // Essence must be equal to or greater than the value.
                            if (blnShowMessage)
                                strName = Environment.NewLine + '\t' +
                                          string.Format(GlobalOptions.CultureInfo
                                              , LanguageManager.GetString("Message_SelectQuality_RequireESSAbove")
                                              , strNodeInnerText
                                              , strEssNodeGradeAttributeText
                                              , decGrade.ToString(GlobalOptions.CultureInfo));
                            return decGrade >= Convert.ToDecimal(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                        }
                        // Check Essence requirement.
                        if (strNodeInnerText.StartsWith('-'))
                        {
                            // Essence must be less than the value.
                            if (blnShowMessage)
                                strName = Environment.NewLine + '\t' +
                                          string.Format(GlobalOptions.CultureInfo
                                              , LanguageManager.GetString("Message_SelectQuality_RequireESSBelow")
                                              , strNodeInnerText
                                              , objCharacter.Essence().ToString(GlobalOptions.CultureInfo));
                            return objCharacter.Essence() < Convert.ToDecimal(strNodeInnerText.TrimStart('-'), GlobalOptions.InvariantCultureInfo);
                        }
                        // Essence must be equal to or greater than the value.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' +
                                      string.Format(GlobalOptions.CultureInfo
                                          , LanguageManager.GetString("Message_SelectQuality_RequireESSAbove")
                                          , strNodeInnerText
                                          , objCharacter.Essence().ToString(GlobalOptions.CultureInfo));
                        return objCharacter.Essence() >= Convert.ToDecimal(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                    }
                case "echo":
                    {
                        Metamagic objMetamagic = objCharacter.Metamagics.FirstOrDefault(x => x.Name == strNodeInnerText && x.SourceType == Improvement.ImprovementSource.Echo);
                        if (objMetamagic != null)
                        {
                            if (blnShowMessage)
                                strName = objMetamagic.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("echoes.xml").SelectSingleNode(string.Format("/chummer/echoes/echo[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1}{2}({3})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Space"), LanguageManager.GetString("String_Echo"))
                                : string.Format("{0}\t{1}{2}({3})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Space"), LanguageManager.GetString("String_Echo"));
                        }
                        return false;
                    }
                case "gameplayoption":
                {
                    // A particular gameplay option is required.
                    if (blnShowMessage)
                        strName = string.Format("{0}\t{1} = {2}", Environment.NewLine, LanguageManager.GetString("String_GameplayOption"), strNodeInnerText);
                    return objCharacter.GameplayOption == strNodeInnerText;
                }
                case "gear":
                    {
                        Gear objGear = objCharacter.Gear.FirstOrDefault(x => x.Name == strNodeInnerText);
                        //TODO: Probably a better way to handle minrating/rating/maxrating but eh, YAGNI.

                        if (xmlNode.SelectSingleNode("@minrating")?.Value != null)
                        {
                            int rating = Convert.ToInt32(xmlNode.SelectSingleNode("@minrating")?.Value, GlobalOptions.InvariantCultureInfo);
                            objGear = objCharacter.Gear.FirstOrDefault(x => x.Name == strNodeInnerText && x.Rating >= rating);
                        }
                        else if (xmlNode.SelectSingleNode("@rating")?.Value != null)
                        {
                            int rating = Convert.ToInt32(xmlNode.SelectSingleNode("@rating")?.Value, GlobalOptions.InvariantCultureInfo);
                            objGear = objCharacter.Gear.FirstOrDefault(x => x.Name == strNodeInnerText && x.Rating == rating);
                        }
                        else if (xmlNode.SelectSingleNode("@maxrating")?.Value != null)
                        {
                            int rating = Convert.ToInt32(xmlNode.SelectSingleNode("@maxrating")?.Value, GlobalOptions.InvariantCultureInfo);
                            objGear = objCharacter.Gear.FirstOrDefault(x => x.Name == strNodeInnerText && x.Rating <= rating);
                        }
                        if (objGear != null)
                        {
                            if (blnShowMessage)
                                strName = objGear.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (blnShowMessage)
                        {
                            // Character needs a specific Martial Art.
                            string strTranslate = XmlManager.Load("gear.xml").SelectSingleNode(string.Format("/chummer/gears/gear[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Gear"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Gear"));
                        }
                        return false;
                    }
                case "group":
                    {
                        // Check that clustered options are present (Magical Tradition + Skill 6, for example)
                        bool blnResult = true;
                        string strResultName = string.Empty;
                        foreach (XPathNavigator xmlChildNode in xmlNode.SelectChildren(XPathNodeType.Element))
                        {
                            blnResult = xmlChildNode.TestNodeRequirements(objCharacter, objParent, out strResultName, strIgnoreQuality, blnShowMessage);
                            if (!blnResult)
                            {
                                break;
                            }
                        }
                        if (blnShowMessage)
                            strName = strResultName;
                        return blnResult;
                    }
                case "grouponeof":
                {
                    // Check that one of the clustered options are present
                    bool blnResult = false;
                    string strResultName = LanguageManager.GetString("Message_SelectQuality_OneOf");
                    foreach (XPathNavigator xmlChildNode in xmlNode.SelectChildren(XPathNodeType.Element))
                    {
                        blnResult = xmlChildNode.TestNodeRequirements(objCharacter, objParent, out string strLoopResult, strIgnoreQuality, blnShowMessage);
                        if (blnResult)
                        {
                            break;
                        }

                        strResultName += strLoopResult;
                    }
                    if (blnShowMessage)
                        strName = strResultName;
                    return blnResult;
                }
                case "initiategrade":
                    {
                        // Character's initiate grade must be higher than or equal to the required value.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' + LanguageManager.GetString("String_InitiateGrade") + " >= " + strNodeInnerText;
                        return objCharacter.InitiateGrade >= Convert.ToInt32(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                    }
                case "martialart":
                    {
                        MartialArt objMartialArt = objCharacter.MartialArts.FirstOrDefault(x => x.Name == strNodeInnerText);
                        if (objMartialArt != null)
                        {
                            if (blnShowMessage)
                                strName = objMartialArt.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (blnShowMessage)
                        {
                            // Character needs a specific Martial Art.
                            string strTranslate = XmlManager.Load("martialarts.xml").SelectSingleNode(string.Format("/chummer/martialarts/martialart[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_MartialArt"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_MartialArt"));
                        }
                        return false;
                    }
                case "martialtechnique":
                    {
                        foreach (MartialArt objMartialArt in objCharacter.MartialArts)
                        {
                            MartialArtTechnique objMartialArtTechnique = objMartialArt.Techniques.FirstOrDefault(x => x.Name == strNodeInnerText);
                            if (objMartialArtTechnique != null)
                            {
                                if (blnShowMessage)
                                    strName = objMartialArtTechnique.CurrentDisplayName;
                                return true;
                            }
                        }
                        if (blnShowMessage)
                        {
                            // Character needs a specific Martial Arts technique.
                            string strTranslate = XmlManager.Load("martialarts.xml").SelectSingleNode(string.Format("/chummer/techniques/technique[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_MartialArt"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_MartialArt"));
                        }
                        return false;
                    }
                case "metamagic":
                    {
                        Metamagic objMetamagic = objCharacter.Metamagics.FirstOrDefault(x => x.Name == strNodeInnerText && x.SourceType == Improvement.ImprovementSource.Metamagic);
                        if (objMetamagic != null)
                        {
                            if (blnShowMessage)
                                strName = objMetamagic.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("metamagic.xml").SelectSingleNode(string.Format("/chummer/metamagics/metamagic[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Metamagic"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Metamagic"));
                        }
                        return false;
                    }
                case "metamagicart":
                case "art":
                    {
                        // Street Grimoire adds High Arts, which group metamagics and such together. If we're ignoring this requirement
                        if (objCharacter.Options.IgnoreArt)
                        {
                            // If we're looking for an art, return true.
                            if (xmlNode.Name == "art")
                            {
                                return true;
                            }

                            XPathNavigator xmlMetamagicDoc = XmlManager.Load("metamagic.xml").GetFastNavigator()
                                .SelectSingleNode("/chummer");
                            if (blnShowMessage)
                            {
                                string strTranslateArt = xmlMetamagicDoc
                                    ?.SelectSingleNode(string.Format("arts/art[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.Value;
                                strName = !string.IsNullOrEmpty(strTranslateArt)
                                    ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslateArt, LanguageManager.GetString("String_Art"))
                                    : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Art"));
                            }

                            if (xmlMetamagicDoc == null) return true;
                            // Loop through the data file for each metamagic to find the Required and Forbidden nodes.
                            foreach (Metamagic metamagic in objCharacter.Metamagics)
                            {
                                XPathNavigator xmlMetamagicNode =
                                    xmlMetamagicDoc.SelectSingleNode(
                                        string.Format("metamagics/metamagic[name = {0}]", metamagic.Name.CleanXPath()));
                                if (xmlMetamagicNode != null)
                                {
                                    if (xmlMetamagicNode.SelectSingleNode(
                                        string.Format("required/art[text() = {0}]", strNodeInnerText.CleanXPath())) != null)
                                    {
                                        return true;
                                    }

                                    if (xmlMetamagicNode.SelectSingleNode(
                                        string.Format("forbidden/art[text() = {0}]", strNodeInnerText.CleanXPath())) != null)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    // We couldn't find a metamagic with this name, so it's probably an art. Try and find the node.
                                    // If we can't, it's probably a data entry error.
                                    xmlMetamagicNode =
                                        xmlMetamagicDoc.SelectSingleNode(string.Format("arts/art[name = {0}]", metamagic.Name.CleanXPath()));
                                    if (xmlMetamagicNode == null)
                                        Utils.BreakIfDebug();
                                    else
                                        return true;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            Art objArt = objCharacter.Arts.FirstOrDefault(x => x.Name == strNodeInnerText);
                            if (objArt != null)
                            {
                                if (blnShowMessage)
                                    strName = objArt.DisplayNameShort(GlobalOptions.Language);
                                return true;
                            }

                            // In some cases, we want to proxy metamagics for arts. If we haven't found a match yet, check it here.
                            if (xmlNode.Name == "metamagicart")
                            {
                                Metamagic objMetamagic =
                                    objCharacter.Metamagics.FirstOrDefault(x => x.Name == strNodeInnerText);
                                if (objMetamagic != null)
                                {
                                    if (blnShowMessage)
                                        strName = objMetamagic.DisplayNameShort(GlobalOptions.Language);
                                    return true;
                                }
                            }

                            if (!blnShowMessage) return false;
                            string strTranslate = XmlManager.Load("metamagic.xml")
                                .SelectSingleNode(string.Format("/chummer/arts/art[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Art"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Art"));
                            return false;
                        }
                    }
                case "magenabled":
                    {
                        // Character must be Awakened.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' +
                                      LanguageManager.GetString("String_AttributeMAGLong") +
                                      " >= 1";
                        return objCharacter.MAGEnabled;
                    }
                case "metatype":
                    {
                        if (blnShowMessage)
                        {
                            string strXPathFilter = string.Format("/chummer/metatypes/metatype[name = {0}]/translate", strNodeInnerText.CleanXPath());
                            // Check the Metatype restriction.
                            string strTranslate = XmlManager.Load("metatypes.xml").SelectSingleNode(strXPathFilter)?.InnerText ??
                                                    XmlManager.Load("critters.xml").SelectSingleNode(strXPathFilter)?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Metatype"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Metatype"));
                        }
                        return strNodeInnerText == objCharacter.Metatype;
                    }
                case "metatypecategory":
                    {
                        if (blnShowMessage)
                        {
                            string strXPathFilter = string.Format("/chummer/categories/category[text() = {0}]/@translate", strNodeInnerText.CleanXPath());
                            // Check the Metatype Category restriction.
                            string strTranslate = XmlManager.Load("metatypes.xml").SelectSingleNode(strXPathFilter)?.InnerText ??
                                                    XmlManager.Load("critters.xml").SelectSingleNode(strXPathFilter)?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_MetatypeCategory"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_MetatypeCategory"));
                        }
                        return strNodeInnerText == objCharacter.MetatypeCategory;
                    }
                case "metavariant":
                    {
                        if (blnShowMessage)
                        {
                            string strXPathFilter = string.Format("/chummer/metatypes/metatype/metavariants/metavariant[name = {0}]/translate", strNodeInnerText.CleanXPath());
                            // Check the Metavariant restriction.
                            string strTranslate = XmlManager.Load("metatypes.xml").SelectSingleNode(strXPathFilter)?.InnerText ??
                                                    XmlManager.Load("critters.xml").SelectSingleNode(strXPathFilter)?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Metavariant"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Metavariant"));
                        }
                        return strNodeInnerText == objCharacter.Metavariant;
                    }
                case "nuyen":
                    {
                        // Character's nuyen must be higher than or equal to the required value.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' + LanguageManager.GetString("String_Nuyen") + " >= " + strNodeInnerText;
                        return objCharacter.Nuyen >= Convert.ToInt32(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                    }
                case "power":
                    {
                        // Run through all of the Powers the character has and see if the current required item exists.
                        Power power = objCharacter.Powers.FirstOrDefault(p => p.Name == strNodeInnerText);
                        if (power != null)
                        {
                            if (blnShowMessage)
                                strName = power.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("powers.xml").SelectSingleNode(string.Format("/chummer/powers/power[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("Tab_Adept"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("Tab_Adept"));
                        }
                        return false;
                    }
                case "program":
                    {
                        // Character needs a specific Program.
                        if (!blnShowMessage) return objCharacter.AIPrograms.Any(p => p.Name == strNodeInnerText);
                        string strTranslate = XmlManager.Load("programs.xml").SelectSingleNode(string.Format("/chummer/programs/program[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                        strName = !string.IsNullOrEmpty(strTranslate)
                            ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Program"))
                            : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Program"));
                        return objCharacter.AIPrograms.Any(p => p.Name == strNodeInnerText);
                    }
                case "quality":
                    {
                        string strExtra = xmlNode.SelectSingleNode("@extra")?.Value;
                        Quality quality = !string.IsNullOrEmpty(strExtra)
                            ? objCharacter.Qualities.FirstOrDefault(q => q.Name == strNodeInnerText && q.Extra == strExtra && q.Name != strIgnoreQuality)
                            : objCharacter.Qualities.FirstOrDefault(q => q.Name == strNodeInnerText && q.Name != strIgnoreQuality);
                        if (quality != null)
                        {
                            if (blnShowMessage)
                                strName = quality.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (!blnShowMessage) return false;
                        string strTranslate = XmlManager.Load("qualities.xml").SelectSingleNode(string.Format("/chummer/qualities/quality[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                        strName = !string.IsNullOrEmpty(strTranslate)
                            ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Quality"))
                            : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Quality"));
                        return false;
                    }
                case "resenabled":
                    // Character must be Emerged.
                    if (blnShowMessage)
                        strName = Environment.NewLine + '\t' + LanguageManager.GetString("String_AttributeRESLong") + " >= 1";
                    return objCharacter.RESEnabled;
                case "skill":
                    {
                        string strSpec = xmlNode.SelectSingleNode("spec")?.Value;
                        string strValue = xmlNode.SelectSingleNode("val")?.Value;
                        int intValue = Convert.ToInt32(strValue, GlobalOptions.InvariantCultureInfo);
                        // Check if the character has the required Skill.
                        if (xmlNode.SelectSingleNode("type") != null)
                        {
                            KnowledgeSkill objKnowledgeSkill = objCharacter.SkillsSection.KnowledgeSkills
                                .FirstOrDefault(objSkill => objSkill.Name == strNodeName &&
                                                   (string.IsNullOrEmpty(strSpec) ||
                                                    objSkill.Specializations.Any(objSpec => objSpec.Name == strSpec) &&
                                                    objSkill.TotalBaseRating >= intValue));

                            if (objKnowledgeSkill != null)
                            {
                                if (blnShowMessage)
                                {
                                    strName = objKnowledgeSkill.CurrentDisplayName;
                                    if (!string.IsNullOrEmpty(strSpec) && !objCharacter.Improvements.Any(objImprovement => objImprovement.ImproveType == Improvement.ImprovementType.DisableSpecializationEffects && objImprovement.UniqueName == objKnowledgeSkill.Name && string.IsNullOrEmpty(objImprovement.Condition) && objImprovement.Enabled))
                                    {
                                        strName += LanguageManager.GetString("String_Space") + '(' + strSpec + ')';
                                    }
                                    if (!string.IsNullOrEmpty(strValue))
                                    {
                                        strName += LanguageManager.GetString("String_Space") + strValue;
                                    }
                                }
                                return true;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(strNodeName))
                            {
                                Skill objSkill = objCharacter.SkillsSection.GetActiveSkill(strNodeName);
                                // Exotic Skill
                                if (objSkill == null && !string.IsNullOrEmpty(strSpec))
                                    objSkill = objCharacter.SkillsSection.GetActiveSkill(strNodeName + LanguageManager.GetString("String_Space") + '(' + strSpec + ')');
                                if (objSkill != null && (xmlNode.SelectSingleNode("spec") == null || objSkill.Specializations.Any(objSpec => objSpec.Name == strSpec)) && objSkill.TotalBaseRating >= intValue)
                                {
                                    if (blnShowMessage)
                                    {
                                        strName = objSkill.CurrentDisplayName;
                                        if (!string.IsNullOrEmpty(strSpec) && !objCharacter.Improvements.Any(objImprovement => objImprovement.ImproveType == Improvement.ImprovementType.DisableSpecializationEffects && objImprovement.UniqueName == objSkill.Name && string.IsNullOrEmpty(objImprovement.Condition) && objImprovement.Enabled))
                                        {
                                            strName += LanguageManager.GetString("String_Space") + '(' + strSpec + ')';
                                        }
                                        if (!string.IsNullOrEmpty(strValue))
                                        {
                                            strName += LanguageManager.GetString("String_Space") + strValue;
                                        }
                                    }
                                    return true;
                                }
                            }
                        }
                        if (blnShowMessage)
                        {
                            XmlDocument xmlSkillDoc = XmlManager.Load("skills.xml");
                            string strSkillName = xmlNode.SelectSingleNode("name")?.Value;
                            string strTranslate = xmlSkillDoc.SelectSingleNode(string.Format("/chummer/skills/skill[name = {0}]/translate", strSkillName.CleanXPath()))?.InnerText
                                                  ?? xmlSkillDoc.SelectSingleNode(string.Format("/chummer/knowledgeskills/skill[name = {0}]/translate", strSkillName.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate) ? string.Format("{0}\t{1}", Environment.NewLine, strTranslate) : string.Format("{0}\t{1}", Environment.NewLine, xmlNode.SelectSingleNode("name")?.Value);
                            if (!string.IsNullOrEmpty(strSpec))
                            {
                                strName += LanguageManager.GetString("String_Space") + '(' + strSpec + ')';
                            }
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                strName += LanguageManager.GetString("String_Space") + strValue;
                            }
                            strName += string.Format(" ({0})", LanguageManager.GetString("Tab_Skills"));
                        }
                        return false;
                    }
                case "skillgrouptotal":
                    {
                        // Check if the total combined Ratings of Skill Groups adds up to a particular total.
                        int intTotal = 0;
                        string[] strGroups = xmlNode.SelectSingleNode("skillgroups")?.Value.Split('+');
                        StringBuilder objOutputString = new StringBuilder(Environment.NewLine + '\t');
                        if (strGroups != null)
                        {
                            for (int i = 0; i <= strGroups.Length - 1; ++i)
                            {
                                foreach (SkillGroup objGroup in objCharacter.SkillsSection.SkillGroups)
                                {
                                    if (objGroup.Name == strGroups[i])
                                    {
                                        if (blnShowMessage)
                                            objOutputString.Append(objGroup.CurrentDisplayName + ',' + LanguageManager.GetString("String_Space"));
                                        intTotal += objGroup.Rating;
                                        break;
                                    }
                                }
                            }
                        }

                        if (blnShowMessage)
                        {
                            if (objOutputString.Length > 0)
                                objOutputString.Length -= 2;
                            strName = objOutputString + LanguageManager.GetString("String_Space") + '(' + LanguageManager.GetString("String_ExpenseSkillGroup") + ')';
                        }
                        return intTotal >= Convert.ToInt32(xmlNode.SelectSingleNode("val")?.Value, GlobalOptions.InvariantCultureInfo);
                    }
                case "specialmodificationlimit":
                {
                    // Add in the cost of all child components.
                    int intMods = 0;
                    object intLock = new object();
                    Parallel.ForEach(objCharacter.Weapons, objChild =>
                    {
                        int i = objChild.WeaponAccessories.Count(y => y.SpecialModification);
                        lock (intLock)
                            intMods += i;
                    });
                    Parallel.ForEach(objCharacter.Vehicles, objVehicle =>
                    {
                        int i = objVehicle.Weapons.SelectMany(x => x.WeaponAccessories).Count(y => y.SpecialModification);
                        lock (intLock)
                            intMods += i;

                        Parallel.ForEach(objVehicle.WeaponMounts, objMount =>
                        {
                            int j = objMount.Weapons.SelectMany(x => x.WeaponAccessories).Count(y => y.SpecialModification);
                            lock (intLock)
                                intMods += j;
                        });
                    });
                    if (blnShowMessage)
                    {
                        strName =
                            string.Format("{0}{1}{2} >= {3}", Environment.NewLine, '\t', LanguageManager.GetString("String_SpecialModificationLimit"), strNodeInnerText);
                    }

                    return (intMods + Convert.ToInt32(strNodeInnerText, GlobalOptions.InvariantCultureInfo)) <= objCharacter.SpecialModificationLimit;
                }
                case "spell":
                    {
                        Spell objSpell = objCharacter.Spells.FirstOrDefault(x => x.Name == strNodeInnerText);
                        if (objSpell != null)
                        {
                            if (blnShowMessage)
                                strName = objSpell.DisplayNameShort(GlobalOptions.Language);
                            return true;
                        }
                        if (blnShowMessage)
                        {
                            // Check for a specific Spell.
                            string strTranslate = XmlManager.Load("spells.xml").SelectSingleNode(string.Format("/chummer/spells/spell[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_DescSpell"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_DescSpell"));
                        }
                        return false;
                    }
                case "spellcategory":
                    {
                        // Check for a specified amount of a particular Spell category.
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("spells.xml").SelectSingleNode(string.Format("/chummer/categories/category[. = \"{0}\"]/@translate", strNodeName))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_SpellCategory"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_SpellCategory"));
                        }
                        return objCharacter.Spells.Count(objSpell => objSpell.Category == strNodeName) >= Convert.ToInt32(xmlNode.SelectSingleNode("count")?.Value, GlobalOptions.InvariantCultureInfo);
                    }
                case "spelldescriptor":
                    {
                        string strCount = xmlNode.SelectSingleNode("count")?.Value ?? string.Empty;
                        // Check for a specified amount of a particular Spell Descriptor.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' + LanguageManager.GetString("Label_Descriptors") + " >= " + strCount;
                        return objCharacter.Spells.Count(objSpell => objSpell.Descriptors.Contains(strNodeName)) >= Convert.ToInt32(strCount, GlobalOptions.InvariantCultureInfo);
                    }
                case "streetcredvsnotoriety":
                    {
                        // Street Cred must be higher than Notoriety.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' + LanguageManager.GetString("String_StreetCred") + " >= " + LanguageManager.GetString("String_Notoriety");
                        return objCharacter.StreetCred >= objCharacter.Notoriety;
                    }
                case "submersiongrade":
                    {
                        // Character's initiate grade must be higher than or equal to the required value.
                        if (blnShowMessage)
                            strName = Environment.NewLine + '\t' + LanguageManager.GetString("String_SubmersionGrade") + " >= " + strNodeInnerText;
                        return objCharacter.SubmersionGrade >= Convert.ToInt32(strNodeInnerText, GlobalOptions.InvariantCultureInfo);
                    }
                case "tradition":
                    {
                        // Character needs a specific Tradition.
                        if (blnShowMessage)
                        {
                            string strTranslate = XmlManager.Load("traditions.xml").SelectSingleNode(string.Format("/chummer/traditions/tradition[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                            strName = !string.IsNullOrEmpty(strTranslate)
                                ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Tradition"))
                                : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Tradition"));
                        }
                        return objCharacter.MagicTradition.Name == strNodeInnerText;
                    }
                case "traditionspiritform":
                {
                    // Character needs a specific spirit form provided by their Tradition.
                    if (blnShowMessage)
                    {
                        string strTranslate = XmlManager.Load("critterpowers.xml").SelectSingleNode(string.Format("/chummer/powers/power[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                        strName = !string.IsNullOrEmpty(strTranslate)
                            ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Tradition"))
                            : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Tradition"));
                    }
                    return objCharacter.MagicTradition.SpiritForm == strNodeInnerText;
                }
                case "weapon":
                {
                    // Character needs a specific Weapon.
                    if (!blnShowMessage) return objCharacter.Weapons.Any(w => w.Name == strNodeInnerText);
                    string strTranslate = XmlManager.Load("weapons.xml").SelectSingleNode(string.Format("/chummer/weapons/weapon[name = {0}]/translate", strNodeInnerText.CleanXPath()))?.InnerText;
                    strName = !string.IsNullOrEmpty(strTranslate)
                        ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_Weapon"))
                        : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_Weapon"));
                    return objCharacter.Weapons.Any(w => w.Name == strNodeInnerText);
                }
                case "accessory" when objParent is Weapon objWeapon:
                {
                    if (!blnShowMessage)
                        return objWeapon.WeaponAccessories.Any(objAccessory => objAccessory.Name == strNodeInnerText);
                    string strTranslate = XmlManager.Load("weapons.xml")
                        .SelectSingleNode(
                            string.Format("/chummer/accessories/accessory[name = {0}]/translate", strNodeInnerText.CleanXPath()))
                        ?.InnerText;
                    strName = !string.IsNullOrEmpty(strTranslate)
                        ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_WeaponAccessory"))
                        : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_WeaponAccessory"));
                    return objWeapon.WeaponAccessories.Any(objAccessory => objAccessory.Name == strNodeInnerText);
                }
                case "armormod":
                {
                    if (blnShowMessage)
                    {
                        string strTranslate = XmlManager.Load("armor.xml")
                            .SelectSingleNode(
                                string.Format("/chummer/armormods/armormod[name = {0}]/translate", strNodeInnerText.CleanXPath()))
                            ?.InnerText;
                        strName = !string.IsNullOrEmpty(strTranslate)
                            ? string.Format("{0}\t{1} ({2})", Environment.NewLine, strTranslate, LanguageManager.GetString("String_ArmorMod"))
                            : string.Format("{0}\t{1} ({2})", Environment.NewLine, strNodeInnerText, LanguageManager.GetString("String_ArmorMod"));
                    }

                    if (xmlNode.GetAttribute("sameparent", "") == bool.TrueString)
                    {
                        if (objParent is Armor objArmor)
                            return objArmor.ArmorMods.Any(mod => mod.Name == strNodeInnerText);
                        return false;
                    }
                    return objCharacter.Armor.Any(armor => armor.ArmorMods.Any(mod => mod.Name == strNodeInnerText));
                }
                default:
                    Utils.BreakIfDebug();
                    break;
            }
            if (blnShowMessage)
                strName = strNodeInnerText;
            return false;
        }

        public static bool CheckAvailRestriction(this XPathNavigator objXmlGear, Character objCharacter, int intRating = 1, int intAvailModifier = 0)
        {
            if (objXmlGear == null)
                return false;
            //TODO: Better handler for restricted gear
            if (objCharacter == null || objCharacter.Created || objCharacter.RestrictedGear > 0 || objCharacter.IgnoreRules)
                return true;
            // Avail.

            XPathNavigator objAvailNode = objXmlGear.SelectSingleNode("avail");
            if (objAvailNode == null)
            {
                int intHighestAvailNode = 0;
                foreach (XPathNavigator objLoopNode in objXmlGear.SelectChildren(XPathNodeType.Element))
                {
                    if (objLoopNode.Name.StartsWith("avail", StringComparison.Ordinal))
                    {
                        string strLoopCostString = objLoopNode.Name.Substring(5);
                        if (int.TryParse(strLoopCostString, out int intTmp))
                        {
                            intHighestAvailNode = Math.Max(intHighestAvailNode, intTmp);
                        }
                    }
                }
                objAvailNode = objXmlGear.SelectSingleNode("avail" + intHighestAvailNode);
                for (int i = intRating; i <= intHighestAvailNode; ++i)
                {
                    XPathNavigator objLoopNode = objXmlGear.SelectSingleNode("avail" + i.ToString(GlobalOptions.InvariantCultureInfo));
                    if (objLoopNode != null)
                    {
                        objAvailNode = objLoopNode;
                        break;
                    }
                }
            }

            // If avail contains "F" or "R", remove it from the string so we can use the expression.
            string strAvailExpr = objAvailNode?.Value ?? string.Empty;
            if (strAvailExpr.StartsWith("FixedValues(", StringComparison.Ordinal))
            {
                string[] strValues = strAvailExpr.TrimStartOnce("FixedValues(", true).TrimEndOnce(')').Split(',');
                strAvailExpr = strValues[Math.Max(Math.Min(intRating - 1, strValues.Length - 1), 0)];
            }

            if (string.IsNullOrEmpty(strAvailExpr))
                return true;
            char chrFirstAvailChar = strAvailExpr[0];
            if (chrFirstAvailChar == '+' || chrFirstAvailChar == '-')
                return true;

            strAvailExpr = strAvailExpr.TrimEndOnce(" or Gear").TrimEndOnce('F', 'R');
            int intAvail = intAvailModifier;
            object objProcess = CommonFunctions.EvaluateInvariantXPath(strAvailExpr.Replace("Rating", intRating.ToString(GlobalOptions.InvariantCultureInfo)), out bool blnIsSuccess);
            if (blnIsSuccess)
                intAvail += Convert.ToInt32(objProcess, GlobalOptions.InvariantCultureInfo);
            return intAvail <= objCharacter.MaximumAvailability;
        }

        public static bool CheckNuyenRestriction(this XPathNavigator objXmlGear, decimal decMaxNuyen, decimal decCostMultiplier = 1.0m, int intRating = 1)
        {
            if (objXmlGear == null)
                return false;
            // Cost.
            decimal decCost = 0.0m;
            XPathNavigator objCostNode = objXmlGear.SelectSingleNode("cost");
            if (objCostNode == null)
            {
                int intCostRating = 1;
                foreach (XmlNode objLoopNode in objXmlGear.SelectChildren(XPathNodeType.Element))
                {
                    if (objLoopNode.Name.StartsWith("cost", StringComparison.Ordinal))
                    {
                        string strLoopCostString = objLoopNode.Name.Substring(4);
                        if (int.TryParse(strLoopCostString, out int intTmp) && intTmp <= intRating)
                        {
                            intCostRating = Math.Max(intCostRating, intTmp);
                        }
                    }
                }

                objCostNode = objXmlGear.SelectSingleNode("cost" + intCostRating.ToString(GlobalOptions.InvariantCultureInfo));
            }
            string strCost = objCostNode?.Value;
            if (!string.IsNullOrEmpty(strCost))
            {
                if (strCost.StartsWith("FixedValues(", StringComparison.Ordinal))
                {
                    string[] strValues = strCost.TrimStartOnce("FixedValues(", true).TrimEndOnce(')').Split(',');
                    strCost = strValues[Math.Max(Math.Min(intRating, strValues.Length) - 1, 0)];
                }
                else if (strCost.StartsWith("Variable", StringComparison.Ordinal))
                {
                    strCost = strCost.TrimStartOnce("Variable(", true).TrimEndOnce(')');
                    int intHyphenIndex = strCost.IndexOf('-');
                    strCost = intHyphenIndex != -1 ? strCost.Substring(0, intHyphenIndex) : strCost.FastEscape('+');
                }

                object objProcess = CommonFunctions.EvaluateInvariantXPath(strCost.Replace("Rating", intRating.ToString(GlobalOptions.InvariantCultureInfo)), out bool blnIsSuccess);
                if (blnIsSuccess)
                    decCost = Convert.ToDecimal(objProcess, GlobalOptions.InvariantCultureInfo);
            }
            return decMaxNuyen >= decCost * decCostMultiplier;
        }
    }
}
