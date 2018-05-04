using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MailBee;
using MailBee.Mime;
using MailBee.ImapMail;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBoxTlsMode.SelectedIndex = 0;
        }

        Imap imp = null;                            // Imap object reference
        bool exiting = false;                       // Flag. If true, terminating the application is in progress and we should stop idle without attempt to download new messages
        DateTime startTime;                         // Time when idle started. Used for restarting idle by timeout
        TimeSpan timeout = new TimeSpan(0, 5, 0);   // 5 minutes timeout for idle

        /// <summary>
        /// Just displays text on the form
        /// </summary>
        void LogMessage(string msg)
        {
            textBox1.Text += msg + "\r\n";
            imp.Log.WriteLine(msg);
        }

        /// <summary>
        /// Start idle timer
        /// </summary>
        private void TimerStart()
        {
            // Store the current time
            startTime = DateTime.Now;
            LogMessage("TIMER started");
        }

        /// <summary>
        /// Stop idle timer
        /// </summary>
        private void TimerStop()
        {
            LogMessage("TIMER stopped");
        }

        /// <summary>
        /// Idling event handler. Ticks every 10 milliseconds while in idle state.
        /// </summary>
        private void imp_Idling(object sender, ImapIdlingEventArgs e)
        {
            // If the difference between start timer time and the current time >= timeout, initiate stopping idle
            if (DateTime.Now.Subtract(startTime) >= timeout)
            {
                imp.StopIdle();
                LogMessage("Initiated stopping idle by TIMER");
            }
        }

        /// <summary>
        /// MessageStatus event handler which receives notifications from IMAP server. Learns about new messages in idling state
        /// </summary>
        private void imp_MessageStatus(object sender, ImapMessageStatusEventArgs e)
        {
            LogMessage("Got " + e.StatusID + " status update");

            // RECENT status means new messages have just arrived to IMAP account. Initiate stopping idle and IdleCallback will download the messages
            if ("RECENT" == e.StatusID)
            {
                imp.StopIdle();
                LogMessage("Initiated stopping idle");
            }
        }

        /// <summary>
        /// Callback for BeginIdle. It'll be called after stopping idle and will download new messages
        /// </summary>
        private void IdleCallback(IAsyncResult result)
        {
            imp.EndIdle();

            // If not exiting, i.e. just stopping idle and we should try to download new messages
            // Exiting means the application is being terminated and we shouldn't try downloading new messages
            if (!exiting)
            {
                TimerStop();
                LogMessage("Stopped idling, will download messages");

                // Search for UNSEEN (i.e. new) messages
                UidCollection uids = (UidCollection)imp.Search(true, "UNSEEN", null);
                if (uids.Count > 0)
                {
                    // Download all the messages found by Search method
                    MailMessageCollection msgs = imp.DownloadEntireMessages(uids.ToString(), true);
                    imp.SetMessageFlags(uids.ToString(), true, SystemMessageFlags.Seen, MessageFlagAction.Replace);

                    // Iterate througn the messages collection and display info about them
                    foreach (MailMessage msg in msgs)
                    {
                        LogMessage("Recent message index: " + msg.IndexOnServer.ToString() + " Subject: " + msg.Subject);
                    }
                }
                else
                {
                    LogMessage("No messages to download");
                }

                // Messages have been downloaded and idling starts again
                TimerStart();
                imp.BeginIdle(new AsyncCallback(IdleCallback), null);
                LogMessage("Started idling again");
            }
            else
            {
                // If exiting is in progress, disconnect after stopping idle
                imp.Disconnect();
            }
        }

        /// <summary>
        /// "Start" button click handler
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // Licensing IMAP component. If the license key is invalid, it'll throw MailBeeLicenseException
            try
            {
                Imap.LicenseKey = textBoxLicenseKey.Text;
            }
            catch (MailBeeLicenseException ex)
            {
                MessageBox.Show("License key is invalid");
                return;
            }

            button1.Enabled = false;
            TextBox.CheckForIllegalCrossThreadCalls = false;

            imp = new Imap();

            // Enable logging
            imp.Log.Filename = @"log.txt";
            imp.Log.Enabled = true;
            imp.Log.Clear();

            // Enable SSL/TLS if necessary
            switch (comboBoxTlsMode.SelectedIndex)
            {
                case 1:
                    imp.SslMode = MailBee.Security.SslStartupMode.UseStartTls;
                    break;
                case 2:
                    imp.SslMode = MailBee.Security.SslStartupMode.OnConnect;
                    break;
            }

            // Assign port number
            int portNumber;
            if (!int.TryParse(textPort.Text, out portNumber))
            {
                portNumber = 143;
            }

            // Connect to IMAP server
            imp.Connect(textBoxServer.Text, portNumber);
            LogMessage("Connected to the server");

            // Check for IDLE support
            if (imp.GetExtension("IDLE") == null)
            {
                LogMessage("IDLE not supported");
                imp.Disconnect();
            }
            else
            {
                // Log into IMAP account
                imp.Login(textBoxLogin.Text, textBoxPassword.Text);
                LogMessage("Logged into the server");

                // Select Inbox folder
                imp.SelectFolder("Inbox");

                // Add message status event handler which will "listen to server" while idling
                imp.MessageStatus += new ImapMessageStatusEventHandler(imp_MessageStatus);

                // Add idling event handler which is used for timer
                imp.Idling += new ImapIdlingEventHandler(imp_Idling);

                // Start idle timer
                TimerStart();

                // Start idling
                imp.BeginIdle(new AsyncCallback(IdleCallback), null);
                LogMessage("Started idling");
            }

        }

        /// <summary>
        /// Form closing handler which correctly terminates IDLE and closes current connection
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (imp != null)
            {
                // If we're still idling, stop it and close the connection
                if (imp.IsIdle)
                {
                    exiting = true;
                    TimerStop();
                    imp.StopIdle();
                }
            }
        }

        /// <summary>
        /// Port number textbox validator
        /// </summary>
        private void textPort_TextChanged(object sender, EventArgs e)
        {
            int i;

            if (!int.TryParse(textPort.Text, out i))
            {
                textPort.Text = "143";
            }
        }

        /// <summary>
        /// Changes port number according to selected SSL/TLS mode
        /// </summary>
        private void comboBoxTlsMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxTlsMode.SelectedIndex)
            {
                case 0:
                case 1:
                    textPort.Text = "143";
                    break;
                case 2:
                    textPort.Text = "993";
                    break;
            }
        }

    }
}