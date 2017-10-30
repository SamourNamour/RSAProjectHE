
#region - Copyright Motive Television 2012 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: OptionsForm.cs
//
#endregion

#region - Using Directive(s) -
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MTV.Library.Core.Extensions;
#endregion 

namespace MTV.Scheduler.App.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OptionsForm : Form {
        #region - Field(s) -
        delegate void ProcessItemDelegate(IExtension extension, System.Windows.Forms.Control[] options);
        #endregion

        #region - Event(s) -

        #endregion

        #region - Class Member(s) -

        #endregion

        #region - Property(ies) -

        #endregion

        #region - Constructor(s) / Finalizer(s) -
        /// <summary>
        /// 
        /// </summary>
        public OptionsForm() {
            InitializeComponent();
        }
        #endregion

        #region - Callback(s) -

        #endregion

        #region - Delegate Invoker(s) -

        #endregion

        #region - Private Method(s) -
        void OptionsForm_Load(object sender, EventArgs e) {
            try {
                treeOptions.BeginUpdate();

                for (int i = 0; i < App.Instance.Extensions.Count; i++) {
                    IExtension extension = App.Instance.Extensions[i];
                    IUIExtension uiExtension = extension.UIExtension;

                    if (uiExtension == null) {
                        continue;
                    }

                    System.Windows.Forms.Control[] options = uiExtension.CreateSettingsView();

                    if (options == null || options.Length == 0) {
                        continue;
                    }

                    TreeNode node = new TreeNode(extension.Name);
                    node.Tag = extension;

                    for (int j = 0; j < options.Length; j++) {
                        TreeNode optioNd = new TreeNode(options[j].Text);
                        optioNd.Tag = options[j];
                        node.Nodes.Add(optioNd);
                    }
                    node.Expand();
                    treeOptions.Nodes.Add(node);
                }
            }
            catch (Exception ex) {
               
                MainForm.LogExceptionToFile(ex);
            }
            finally {
                treeOptions.EndUpdate();
            }
        }

        void ProcessSettings(ProcessItemDelegate process) {
            for (int i = 0; i < treeOptions.Nodes.Count; i++) {
                TreeNode node = treeOptions.Nodes[i];
                IExtension extension = (IExtension)node.Tag;
                System.Windows.Forms.Control[] options = new System.Windows.Forms.Control[node.Nodes.Count];
                for (int j = 0; j < node.Nodes.Count; j++) {
                    options[j] = (System.Windows.Forms.Control)node.Nodes[j].Tag;
                }

                process(extension, options);
            }

            treeOptions.Nodes.Clear();
        }

        void treeOptions_AfterSelect(object sender, TreeViewEventArgs e) {
            pnlExtension.Controls.Clear();

            if (e.Node.Parent != null) {
                ShowOptionFromNode(e.Node);
            }
            else {
                ShowOptionFromNode(e.Node.Nodes[0]);
            }
        }

        void ShowOptionFromNode(TreeNode node) {
            System.Windows.Forms.Control ctrl = (System.Windows.Forms.Control)node.Tag;
            ctrl.Visible = true;
            ctrl.Dock = DockStyle.Fill;
            pnlExtension.Controls.Add(ctrl);
        }

        void btnOK_Click(object sender, EventArgs e) {
            ProcessSettings(
                delegate(IExtension extension, System.Windows.Forms.Control[] options)
                {
                    extension.UIExtension.PersistSettings(options);

                    for (int i = 0; i < options.Length; i++) {
                        options[i].Dispose();
                    }
                }
                );

            DialogResult = DialogResult.OK;
        }

        void btnCancel_Click(object sender, EventArgs e) {
            ProcessSettings(
                delegate(IExtension extension, System.Windows.Forms.Control[] options)
                {
                    for (int i = 0; i < options.Length; i++) {
                        options[i].Dispose();
                    }
                }
            );

            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region - Internal Method(s) -

        #endregion

        #region - Public Method(s) -

        #endregion

        #region - IDisposable Member(s) -

        #endregion        
    }
}