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
 using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Chummer
{
    public partial class frmSelectSkillGroup : Form
    {
        private string _strReturnValue = string.Empty;
        private string _strForceValue = string.Empty;
        private string _strExcludeCategory = string.Empty;

        private readonly XmlDocument _objXmlDocument;

        #region Control Events
        public frmSelectSkillGroup()
        {
            InitializeComponent();
            this.TranslateWinForm();
            _objXmlDocument = XmlManager.Load("skills.xml");
        }

        private void frmSelectSkillGroup_Load(object sender, EventArgs e)
        {
            List<ListItem> lstGroups = new List<ListItem>();

            if (string.IsNullOrEmpty(_strForceValue))
            {
                // Build the list of Skill Groups found in the Skills file.
                using (XmlNodeList objXmlSkillList = _objXmlDocument.SelectNodes("/chummer/skillgroups/name"))
                {
                    if (objXmlSkillList != null)
                    {
                        foreach (XmlNode objXmlSkill in objXmlSkillList)
                        {
                            if (!string.IsNullOrEmpty(_strExcludeCategory))
                            {
                                string strExclude = string.Empty;
                                foreach (string strCategory in _strExcludeCategory.SplitNoAlloc(',', StringSplitOptions.RemoveEmptyEntries))
                                    strExclude += "category != \"" + strCategory.Trim() + "\" and ";
                                // Remove the trailing " and ";
                                strExclude = strExclude.Substring(0, strExclude.Length - 5);

                                XmlNodeList objXmlNodeList = _objXmlDocument.SelectNodes("/chummer/skills/skill[" + strExclude + " and skillgroup = \"" + objXmlSkill.InnerText + "\"]");
                                if (objXmlNodeList == null || objXmlNodeList.Count == 0)
                                    continue;
                            }

                            string strInnerText = objXmlSkill.InnerText;
                            lstGroups.Add(new ListItem(strInnerText, objXmlSkill.Attributes?["translate"]?.InnerText ?? strInnerText));
                        }
                    }
                }
            }
            else
            {
                lstGroups.Add(new ListItem(_strForceValue, _strForceValue));
            }
            lstGroups.Sort(CompareListItems.CompareNames);
            cboSkillGroup.BeginUpdate();
            cboSkillGroup.ValueMember = nameof(ListItem.Value);
            cboSkillGroup.DisplayMember = nameof(ListItem.Name);
            cboSkillGroup.DataSource = lstGroups;
            cboSkillGroup.EndUpdate();

            // Select the first Skill in the list.
            cboSkillGroup.SelectedIndex = 0;

            if (cboSkillGroup.Items.Count == 1)
                cmdOK_Click(sender, e);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            _strReturnValue = cboSkillGroup.SelectedValue.ToString();
            DialogResult = DialogResult.OK;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Properties
        // Skill Group that was selected in the dialogue.
        public string SelectedSkillGroup => _strReturnValue;

        // Description to show in the window.
        public string Description
        {
            set => lblDescription.Text = value;
        }

        /// <summary>
        /// Force a specific SkillGroup to be selected.
        /// </summary>
        public string OnlyGroup
        {
            set => _strForceValue = value;
        }

        /// <summary>
        /// Only Skills not in the selected Category should be in the list.
        /// </summary>
        public string ExcludeCategory
        {
            set => _strExcludeCategory = value;
        }
        #endregion
    }
}
