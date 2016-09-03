// <copyright file="MainForm.cs" company="www.PublicDomain.tech">All rights waived.</copyright>

// Programmed by Victor L. Senior (VLS) <support@publicdomain.tech>, 2016
//
// Web: http://publicdomain.tech
//
// Sources: http://github.com/publicdomaintech/
//
// This software and associated documentation files (the "Software") is
// released under the CC0 Public Domain Dedication, version 1.0, as
// published by Creative Commons. To the extent possible under law, the
// author(s) have dedicated all copyright and related and neighboring
// rights to the Software to the public domain worldwide. The Software is
// distributed WITHOUT ANY WARRANTY.
//
// If you did not receive a copy of the CC0 Public Domain Dedication
// along with the Software, see
// <http://creativecommons.org/publicdomain/zero/1.0/>

/// <summary>
/// Felt European Traditional
/// </summary>
namespace Felt_32_European_32_Traditional
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using System.Windows.Forms;
    using PdBets;

    /// <summary>
    /// Felt European Input Tracker 
    /// </summary>
    [Export(typeof(IPdBets))]
    public partial class MainForm : Form, IPdBets
    {
        /// <summary>
        /// The default number back color list.
        /// </summary>
        private List<Color> defaultNumberBackColorList = new List<Color>();

        /// <summary>
        /// The form buttons dictionary.
        /// </summary>
        private Dictionary<Button, Color> formButtonsDictionary = new Dictionary<Button, Color>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Felt_32_European_32_Traditional.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();

            // Set default number color list
            for (int i = 0; i < 37; i++)
            {
                // Set current number button
                Button numberButton = (Button)this.Controls.Find("button" + i.ToString(), true)[0];

                // Set number color
                Color numberColor = numberButton.BackColor;

                // Add to default number color list
                this.defaultNumberBackColorList.Add(numberColor);

                // Add to form buttons dictionary
                this.formButtonsDictionary.Add(numberButton, numberColor);
            }

            // Add undo button to form buttons dictionary
            this.formButtonsDictionary.Add(this.undoButton, Color.Navy);
        }

        /// <summary>
        /// Occurs when new input is sent.
        /// </summary>
        public event EventHandler<NewInputEventArgs> NewInput;

        /// <summary>
        /// Processes incoming input and bet strings.
        /// </summary>
        /// <param name="inputString">Input string.</param>
        /// <param name="betString">Bet string.</param>
        /// <returns>>The processed input string.</returns>
        public string Input(string inputString, string betString)
        {
            // Return passed bet string
            return betString;
        }

        /// <summary>
        /// Raises the number button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNumberButtonClick(object sender, EventArgs e)
        {
            // Hold new input string
            string newInputString;

            // Holds last number
            int lastNumber;

            // Try to parse last number
            if (!int.TryParse(((Button)sender).Name.Replace("button", string.Empty), out lastNumber))
            {
                // Set new input to UNDO
                newInputString = "-U";
            }
            else
            {
                // Set new input to clicked number
                newInputString = lastNumber.ToString();
            }

            // Send new input to pdBets
            this.NewInput(sender, new NewInputEventArgs(newInputString));
        }

        /// <summary>
        /// Raises the about tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            // About message
            MessageBox.Show("Programmed by Victor L. Senior (VLS)" + Environment.NewLine + "www.publicdomain.tech / support@publicdomain.tech" + Environment.NewLine + Environment.NewLine + "Version 0.1 - August 2016.");
        }

        /// <summary>
        /// Raises the undo tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnUndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Send undo message to pdBets
            this.NewInput(sender, new NewInputEventArgs("-U"));
        }

        /// <summary>
        /// Raises the button mouse enter event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnButtonMouseEnter(object sender, EventArgs e)
        {
            // Set sender button
            Button senderButton = (Button)sender;

            // Set back color to yellow
            senderButton.BackColor = Color.Yellow;

            // Set fore color to black
            senderButton.ForeColor = Color.Black;

            /* Handle sticky color */

            // Iterate buttons
            foreach (KeyValuePair<Button, Color> formButton in this.formButtonsDictionary)
            {
                // Test for reference equality
                if (!object.ReferenceEquals(senderButton, formButton.Key))
                {
                    // Set back color to default
                    formButton.Key.BackColor = formButton.Value;

                    // Set fore color to white
                    formButton.Key.ForeColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Raises the button mouse leave event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnButtonMouseLeave(object sender, EventArgs e)
        {
            // Set sender button
            Button senderButton = (Button)sender;

            // Holds last number
            int lastNumber;

            // Try to parse last number
            if (int.TryParse(senderButton.Name.Replace("button", string.Empty), out lastNumber))
            {
                // Set back color for current number
                senderButton.BackColor = this.defaultNumberBackColorList[lastNumber];
            }
            else
            {
                // Set back color for UNDO
                senderButton.BackColor = Color.Navy;
            }

            // Set button fore color to white
            senderButton.ForeColor = Color.White;
        }
    }
}
