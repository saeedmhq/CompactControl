﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Compact_Control.Properties;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Compact_Control
{
    public partial class Form1 : Form
    {
        public static string portName = "Null";
        string gant_co;
        string gant_f1;
        string gant_f2;
        string gant_cofin;
        double gant_cofin_temp1;
        double gant_cofin_temp2;
        double gant_gain;
        double gant_offset;
        double gant_gain_temp;
        double gant_offset_temp;
        string gant_dv;
        string gnd;
        string gant_zpnt;
        string gant_length;
        string gant_fine_length;

        string collim_co;
        string collim_f1;
        string collim_f2;
        string collim_cofin;
        double collim_cofin_temp1;
        double collim_cofin_temp2;
        double collim_gain;
        double collim_offset;
        double collim_gain_temp;
        double collim_offset_temp;
        string collim_dv;
        string cld;
        string collim_zpnt;
        string collim_length;
        string collim_fine_length;

        string x1_co;
        double x1_co_temp1;
        double x1_co_temp2;
        double x1_gain;
        double x1_offset;
        double x1_gain_temp;
        double x1_offset_temp;
        string x1_dv;
        string x1d;

        string x2_co;
        double x2_co_temp1;
        double x2_co_temp2;
        double x2_gain;
        double x2_offset;
        double x2_gain_temp;
        double x2_offset_temp;
        string x2_dv;
        string x2d;

        string y1_co;
        double y1_co_temp1;
        double y1_co_temp2;
        double y1_gain;
        double y1_offset;
        double y1_gain_temp;
        double y1_offset_temp;
        string y1_dv;
        string y1d;


        string y2_co;
        double y2_co_temp1;
        double y2_co_temp2;
        double y2_gain;
        double y2_offset;
        double y2_gain_temp;
        double y2_offset_temp;
        string y2_dv;
        string y2d;

        string exp_perm;

        string gant_set;
        string collim_set;
        string x1_set;
        string x2_set;
        string y1_set;
        string y2_set;

        string adc;

        public static bool quit = false;

        Image errorImage = Resources.Error;
        Image requestImage = Resources.Request;

        public static bool isInServiceMode = false;
        public static bool isInPhysicMode = false;

        private static SerialPort GlobalSerialPort = new SerialPort();
        private ClientControls clientFrm = new ClientControls();

        public Form1()
        {
            InitializeComponent();

            if (isInServiceMode == false)
            {
                label_title.Text = "Clinical";
                if (panel_ClientControls.Controls.Count != 3)
                {
                    clientFrm = new ClientControls();
                    clientFrm.Dock = DockStyle.Top;
                    //GlobalSerialPort = clientFrm.serialPort1;
                    //GlobalSerialPort.DataReceived += clientFrm.serialPort1_DataReceived;
                    panel_ClientControls.Controls.Add(clientFrm);
                }
            }
            else
            {
                label_title.Text = "Service";
                //GlobalSerialPort = serialPort1;
                //GlobalSerialPort.DataReceived += serialPort1_DataReceived_1;
            }
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                label_ConnectStatus.Text = "No Serial port detected!";
                //MessageBox.Show("No Serial port detected!", "COM Port error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ports.Length >= 1)
            {
                if (GlobalSerialPort.IsOpen == false)
                {
                    if (portName != "Null" && portName != "")
                        GlobalSerialPort.PortName = portName;
                    else
                        GlobalSerialPort.PortName = ports[0];
                    int BaudRate = 0;
                    string filename = "portSettings.json";
                    try
                    {
                        if (System.IO.File.Exists(filename))
                        {
                            //string[] portSettings = readJson(filename);
                            HashPass.PortSettings pSettings = HashPass.readSettingsJson(filename);
                            HashPass.WriteBaudrateToReg(pSettings.Baudrate);
                            GlobalSerialPort.BaudRate = int.Parse(pSettings.Baudrate);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error reading Baudrate from file!");
                    }
                    if (int.TryParse(HashPass.ReadBaudRateFromReg(), out BaudRate) == true && BaudRate != 0)
                        GlobalSerialPort.BaudRate = BaudRate;
                    //ConnectToPort();
                }
            }
        }


        // Disable Alt+F4
        protected override System.Boolean ProcessCmdKey(ref System.Windows.Forms.Message
        msg, System.Windows.Forms.Keys keyData)
        {
            if (!isInServiceMode)
                if ((msg.Msg == 0x104) && (((int)msg.LParam) == 0x203e0001))
                    return true;
            return false;
        }


        public void KillCtrlAltDelete()
        {
            //RegistryKey regkey;
            //string keyValueInt = "1";
            //string subKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";

            //try
            //{
            //    regkey = Registry.CurrentUser.CreateSubKey(subKey);
            //    regkey.SetValue("DisableTaskMgr", keyValueInt);
            //    regkey.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }


        public static void EnableCTRLALTDEL()
        {
            try
            {
                string subKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void Board(bool isLock)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("a");
            GlobalSerialPort.Write(trackBar1.Value.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("b");
            GlobalSerialPort.Write(trackBar2.Value.ToString());
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("c");
            GlobalSerialPort.Write(trackBar3.Value.ToString());
        }


        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("d");
            GlobalSerialPort.Write(trackBar4.Value.ToString());
        }


        private void button10_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("e");
            GlobalSerialPort.Write(trackBar5.Value.ToString());
        }

        private void button11_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("f");
            GlobalSerialPort.Write(trackBar6.Value.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("g");
            GlobalSerialPort.Write(trackBar1.Value.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("h");
            GlobalSerialPort.Write(trackBar2.Value.ToString());
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("i");
            GlobalSerialPort.Write(trackBar3.Value.ToString());
        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("j");
            GlobalSerialPort.Write(trackBar4.Value.ToString());
        }

        private void button12_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("k");
            GlobalSerialPort.Write(trackBar5.Value.ToString());
        }

        private void button13_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("l");
            GlobalSerialPort.Write(trackBar6.Value.ToString());
        }


        private void button6_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button7_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button9_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button10_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button12_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button11_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button13_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("s");
        }


        private void button6_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();
            GlobalSerialPort.Write("s");
        }

        //private void button15_Click(object sender, EventArgs e)
        //{
        //    if (GlobalSerialPort.IsOpen == true)
        //        GlobalSerialPort.Close();
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            double m;
            double n;
            textBox1.Text = gant_co;
            textBox7.Text = gant_f1;
            textBox8.Text = gant_f2;
            textBox43.Text = gant_cofin;

            textBox2.Text = collim_co;
            textBox9.Text = collim_f1;
            textBox10.Text = collim_f2;
            textBox44.Text = collim_cofin;

            textBox3.Text = x1_co;
            textBox4.Text = x2_co;
            textBox5.Text = y1_co;
            textBox6.Text = y2_co;

            textBox19.Text = Math.Round(gant_gain, 7, MidpointRounding.ToEven).ToString();
            textBox20.Text = Math.Round(collim_gain, 7, MidpointRounding.ToEven).ToString();
            textBox21.Text = Math.Round(x1_gain, 7, MidpointRounding.ToEven).ToString();
            textBox22.Text = Math.Round(x2_gain, 7, MidpointRounding.ToEven).ToString();
            textBox23.Text = Math.Round(y1_gain, 7, MidpointRounding.ToEven).ToString();
            textBox24.Text = Math.Round(y2_gain, 7, MidpointRounding.ToEven).ToString();

            textBox25.Text = Math.Round(gant_offset, 3, MidpointRounding.ToEven).ToString();
            textBox26.Text = Math.Round(collim_offset, 3, MidpointRounding.ToEven).ToString();
            textBox27.Text = Math.Round(x1_offset, 3, MidpointRounding.ToEven).ToString();
            textBox28.Text = Math.Round(x2_offset, 3, MidpointRounding.ToEven).ToString();
            textBox29.Text = Math.Round(y1_offset, 3, MidpointRounding.ToEven).ToString();
            textBox30.Text = Math.Round(y2_offset, 3, MidpointRounding.ToEven).ToString();

            try
            {
                if (gant_cofin != null)
                {
                    gant_dv = Math.Round((gant_gain * double.Parse(gant_cofin) + gant_offset), 2, MidpointRounding.ToEven).ToString();
                }
                if (collim_cofin != null)
                {
                    collim_dv = Math.Round((collim_gain * double.Parse(collim_cofin) + collim_offset), 2, MidpointRounding.ToEven).ToString();
                }
                if (x1_co != null)
                    x1_dv = Math.Round(((x1_gain * double.Parse(x1_co)) + x1_offset), 1, MidpointRounding.ToEven).ToString();
                if (x2_co != null)
                    x2_dv = Math.Round(((x2_gain * double.Parse(x2_co)) + x2_offset), 1, MidpointRounding.ToEven).ToString();
                if (y1_co != null)
                    y1_dv = Math.Round(((y1_gain * double.Parse(y1_co)) + y1_offset), 1, MidpointRounding.ToEven).ToString();
                if (y2_co != null)
                    y2_dv = Math.Round(((y2_gain * double.Parse(y2_co)) + y2_offset), 1, MidpointRounding.ToEven).ToString();
            }
            catch
            {
            }

            textBox31.Text = gant_dv;
            textBox32.Text = collim_dv;
            textBox33.Text = x1_dv;
            textBox34.Text = x2_dv;
            textBox35.Text = y1_dv;
            textBox36.Text = y2_dv;
            adcheck.Text = adc;

            textBox42_TextChanged(sender, e);
            textBox41_TextChanged(sender, e);
            textBox40_TextChanged(sender, e);
            textBox39_TextChanged(sender, e);
            textBox38_TextChanged(sender, e);
            textBox37_TextChanged(sender, e);


            switch (comboBox1.Text)
            {
                case "Gantry":
                    //if (textBox13.Enabled & textBox13.ReadOnly == false & checkBox1.Checked == false)
                    textBox13.Text = gant_dv;
                    //if (textBox16.Enabled & textBox16.ReadOnly == false & checkBox2.Checked == false)
                    textBox16.Text = gant_dv;
                    break;
                case "Collimator":
                    // if (textBox13.Enabled & textBox13.ReadOnly == false & checkBox1.Checked == false)
                    textBox13.Text = collim_dv;
                    // if (textBox16.Enabled & textBox16.ReadOnly == false & checkBox2.Checked == false)
                    textBox16.Text = collim_dv;
                    break;
                case "X1":
                    //if (textBox13.Enabled & textBox13.ReadOnly==false & checkBox1.Checked == false)
                    textBox13.Text = x1_dv;
                    //if (textBox16.Enabled & textBox16.ReadOnly == false & checkBox2.Checked == false)
                    textBox16.Text = x1_dv;
                    break;
                case "X2":
                    //if (textBox13.Enabled & textBox13.ReadOnly == false & checkBox1.Checked == false)
                    textBox13.Text = x2_dv;
                    // if (textBox16.Enabled & textBox16.ReadOnly == false & checkBox2.Checked == false)
                    textBox16.Text = x2_dv;
                    break;
                case "Y1":
                    // if (textBox13.Enabled & textBox13.ReadOnly == false & checkBox1.Checked == false)
                    textBox13.Text = y1_dv;
                    //if (textBox16.Enabled & textBox16.ReadOnly == false & checkBox2.Checked == false)
                    textBox16.Text = y1_dv;
                    break;
                case "Y2":
                    //if (textBox13.Enabled & textBox13.ReadOnly == false & checkBox1.Checked == false)
                    textBox13.Text = y2_dv;
                    //if (textBox16.Enabled & textBox16.ReadOnly == false & checkBox2.Checked == false)
                    textBox16.Text = y2_dv;
                    break;
            }

            if (lbl_init.Visible == true)
            {
                if (initStatus == true)
                {
                    lbl_init.Text = "Initialized";
                    lbl_init.ForeColor = Color.Green;
                }
                else
                {
                    lbl_init.Text = "Initialization Failed";
                    lbl_init.ForeColor = Color.Red;
                }
            }
        }

        private void GlobalSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }


        private void button16_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked & checkBox2.Checked)
                try
                {
                    switch (comboBox1.Text)
                    {
                        case "Gantry":
                            gant_gain = gant_gain_temp;
                            gant_offset = gant_offset_temp;
                            gant_dv = (gant_gain * double.Parse(gant_cofin) + gant_offset).ToString();
                            textBox11.Text = gant_gain.ToString();
                            textBox12.Text = gant_offset.ToString();
                            break;
                        case "Collimator":
                            collim_gain = collim_gain_temp;
                            collim_offset = collim_offset_temp;
                            collim_dv = (collim_gain * double.Parse(collim_cofin) + collim_offset).ToString();
                            textBox11.Text = collim_gain.ToString();
                            textBox12.Text = collim_offset.ToString();
                            break;
                        case "X1":
                            x1_gain = x1_gain_temp;
                            x1_offset = x1_offset_temp;
                            x1_dv = (x1_gain * double.Parse(x1_co) + x1_offset).ToString();
                            textBox11.Text = x1_gain.ToString();
                            textBox12.Text = x1_offset.ToString();
                            break;
                        case "X2":
                            x2_gain = x2_gain_temp;
                            x2_offset = x2_offset_temp;
                            x2_dv = (x2_gain * double.Parse(x2_co) + x2_offset).ToString();
                            textBox11.Text = x2_gain.ToString();
                            textBox12.Text = x2_offset.ToString();
                            break;
                        case "Y1":
                            y1_gain = y1_gain_temp;
                            y1_offset = y1_offset_temp;
                            y1_dv = (y1_gain * double.Parse(y1_co) + y1_offset).ToString();
                            textBox11.Text = y1_gain.ToString();
                            textBox12.Text = y1_offset.ToString();
                            break;
                        case "Y2":
                            y2_gain = y2_gain_temp;
                            y2_offset = y2_offset_temp;
                            y2_dv = (y2_gain * double.Parse(y2_co) + y2_offset).ToString();
                            textBox11.Text = y2_gain.ToString();
                            textBox12.Text = y2_offset.ToString();
                            break;
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Wrong Nubmer!");
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (GlobalSerialPort.IsOpen == false)
                        GlobalSerialPort.Open();
                    if (comboBox1.Text == "Gantry")
                        GlobalSerialPort.Write("t");
                    if (comboBox1.Text == "Collimator")
                        GlobalSerialPort.Write("u");
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            double a;
            if (checkBox1.Checked)
            {
                try
                {
                    a = double.Parse(textBox14.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Wrong Number!");
                    checkBox1.Checked = false;
                    goto ff;
                }

                try
                {
                    gant_cofin_temp1 = double.Parse(gant_cofin);
                    collim_cofin_temp1 = double.Parse(collim_cofin);
                    x1_co_temp1 = double.Parse(x1_co);
                    x2_co_temp1 = double.Parse(x2_co);
                    y1_co_temp1 = double.Parse(y1_co);
                    y2_co_temp1 = double.Parse(y2_co);
                }
                catch
                {
                }

                textBox14.ReadOnly = true;
                if (checkBox2.Checked)
                {
                    try
                    {
                        switch (comboBox1.Text)
                        {
                            case "Gantry":
                                if (gant_cofin_temp1 == gant_cofin_temp2)
                                {
                                    MessageBox.Show("The Gantry angle muste be different in step1 and step2");
                                    checkBox1.Checked = false;
                                    goto ff;
                                }
                                gant_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (gant_cofin_temp2 - gant_cofin_temp1);
                                gant_offset_temp = double.Parse(textBox15.Text) - (gant_gain_temp * gant_cofin_temp2);
                                textBox17.Text = gant_gain_temp.ToString();
                                textBox18.Text = gant_offset_temp.ToString();
                                break;
                            case "Collimator":
                                if (collim_cofin_temp1 == collim_cofin_temp2)
                                {
                                    MessageBox.Show("The Collimator angle muste be different in step1 and step2");
                                    checkBox1.Checked = false;
                                    goto ff;
                                }
                                collim_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (collim_cofin_temp2 - collim_cofin_temp1);
                                collim_offset_temp = double.Parse(textBox15.Text) - (collim_gain_temp * collim_cofin_temp2);
                                textBox17.Text = collim_gain_temp.ToString();
                                textBox18.Text = collim_offset_temp.ToString();
                                break;
                            case "X1":
                                if (x1_co_temp1 == x1_co_temp2)
                                {
                                    MessageBox.Show("The X1 Position muste be different in step1 and step2");
                                    checkBox1.Checked = false;
                                    goto ff;
                                }
                                x1_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (x1_co_temp2 - x1_co_temp1);
                                x1_offset_temp = double.Parse(textBox15.Text) - (x1_gain_temp * x1_co_temp2);
                                textBox17.Text = x1_gain_temp.ToString();
                                textBox18.Text = x1_offset_temp.ToString();
                                break;
                            case "X2":
                                if (x2_co_temp1 == x2_co_temp2)
                                {
                                    MessageBox.Show("The X2 Position muste be different in step1 and step2");
                                    checkBox1.Checked = false;
                                    goto ff;
                                }
                                x2_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (x2_co_temp2 - x2_co_temp1);
                                x2_offset_temp = double.Parse(textBox15.Text) - (x2_gain_temp * x2_co_temp2);
                                textBox17.Text = x2_gain_temp.ToString();
                                textBox18.Text = x2_offset_temp.ToString();
                                break;
                            case "Y1":
                                if (y1_co_temp1 == y1_co_temp2)
                                {
                                    MessageBox.Show("The Y1 Position muste be different in step1 and step2");
                                    checkBox1.Checked = false;
                                    goto ff;
                                }
                                y1_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (y1_co_temp2 - y1_co_temp1);
                                y1_offset_temp = double.Parse(textBox15.Text) - (y1_gain_temp * y1_co_temp2);
                                textBox17.Text = y1_gain_temp.ToString();
                                textBox18.Text = y1_offset_temp.ToString();
                                break;
                            case "Y2":
                                if (y2_co_temp1 == y2_co_temp2)
                                {
                                    MessageBox.Show("The Y2 Position muste be different in step1 and step2");
                                    checkBox1.Checked = false;
                                    goto ff;
                                }
                                y2_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (y2_co_temp2 - y2_co_temp1);
                                y2_offset_temp = double.Parse(textBox15.Text) - (y2_gain_temp * y2_co_temp2);
                                textBox17.Text = y2_gain_temp.ToString();
                                textBox18.Text = y2_offset_temp.ToString();
                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Wrong Nubmer!");
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                    }
                    button16.Enabled = true;
                }
            ff:
                {
                }
            }
            else
            {
                textBox14.ReadOnly = false;
                button16.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            double a;
            if (checkBox2.Checked)
            {
                try
                {
                    a = double.Parse(textBox15.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Wrong Number!");
                    checkBox2.Checked = false;
                    goto gg;
                }

                try
                {
                    gant_cofin_temp2 = double.Parse(gant_cofin);
                    collim_cofin_temp2 = double.Parse(collim_cofin);
                    x1_co_temp2 = double.Parse(x1_co);
                    x2_co_temp2 = double.Parse(x2_co);
                    y1_co_temp2 = double.Parse(y1_co);
                    y2_co_temp2 = double.Parse(y2_co);
                }
                catch
                {
                }
                textBox15.ReadOnly = true;
                if (checkBox1.Checked)
                {
                    try
                    {
                        switch (comboBox1.Text)
                        {
                            case "Gantry":
                                if (gant_cofin_temp1 == gant_cofin_temp2)
                                {
                                    MessageBox.Show("The Gantry angle muste be different in step1 and step2");
                                    checkBox2.Checked = false;
                                    goto gg;
                                }
                                gant_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (gant_cofin_temp2 - gant_cofin_temp1);
                                gant_offset_temp = double.Parse(textBox15.Text) - (gant_gain_temp * gant_cofin_temp2);
                                textBox17.Text = gant_gain_temp.ToString();
                                textBox18.Text = gant_offset_temp.ToString();
                                break;
                            case "Collimator":
                                if (collim_cofin_temp1 == collim_cofin_temp2)
                                {
                                    MessageBox.Show("The Collimator angle muste be different in step1 and step2");
                                    checkBox2.Checked = false;
                                    goto gg;
                                }
                                collim_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (collim_cofin_temp2 - collim_cofin_temp1);
                                collim_offset_temp = double.Parse(textBox15.Text) - (collim_gain_temp * collim_cofin_temp2);
                                textBox17.Text = collim_gain_temp.ToString();
                                textBox18.Text = collim_offset_temp.ToString();
                                break;
                            case "X1":
                                if (x1_co_temp1 == x1_co_temp2)
                                {
                                    MessageBox.Show("The X1 Position muste be different in step1 and step2");
                                    checkBox2.Checked = false;
                                    goto gg;
                                }
                                x1_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (x1_co_temp2 - x1_co_temp1);
                                x1_offset_temp = double.Parse(textBox15.Text) - (x1_gain_temp * x1_co_temp2);
                                textBox17.Text = x1_gain_temp.ToString();
                                textBox18.Text = x1_offset_temp.ToString();
                                break;
                            case "X2":
                                if (x2_co_temp1 == x2_co_temp2)
                                {
                                    MessageBox.Show("The X2 Position muste be different in step1 and step2");
                                    checkBox2.Checked = false;
                                    goto gg;
                                }
                                x2_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (x2_co_temp2 - x2_co_temp1);
                                x2_offset_temp = double.Parse(textBox15.Text) - (x2_gain_temp * x2_co_temp2);
                                textBox17.Text = x2_gain_temp.ToString();
                                textBox18.Text = x2_offset_temp.ToString();
                                break;
                            case "Y1":
                                if (y1_co_temp1 == y1_co_temp2)
                                {
                                    MessageBox.Show("The Y1 Position muste be different in step1 and step2");
                                    checkBox2.Checked = false;
                                    goto gg;
                                }
                                y1_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (y1_co_temp2 - y1_co_temp1);
                                y1_offset_temp = double.Parse(textBox15.Text) - (y1_gain_temp * y1_co_temp2);
                                textBox17.Text = y1_gain_temp.ToString();
                                textBox18.Text = y1_offset_temp.ToString();
                                break;
                            case "Y2":
                                if (y2_co_temp1 == y2_co_temp2)
                                {
                                    MessageBox.Show("The Y2 Position muste be different in step1 and step2");
                                    checkBox2.Checked = false;
                                    goto gg;
                                }
                                y2_gain_temp = (double.Parse(textBox15.Text) - double.Parse(textBox14.Text)) / (y2_co_temp2 - y2_co_temp1);
                                y2_offset_temp = double.Parse(textBox15.Text) - (y2_gain_temp * y2_co_temp2);
                                textBox17.Text = y2_gain_temp.ToString();
                                textBox18.Text = y2_offset_temp.ToString();
                                break;
                        }
                    }

                    catch (FormatException)
                    {
                        MessageBox.Show("Wrong Nubmer!");
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                    }
                    button16.Enabled = true;
                }
            gg:
                {
                }
            }
            else
            {
                textBox15.ReadOnly = false;
                button16.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;

            textBox14.Enabled = true;
            textBox15.Enabled = true;
            textBox17.Enabled = true;
            textBox18.Enabled = true;

            textBox17.Text = "";
            textBox18.Text = "";

            label12.Enabled = true;
            label13.Enabled = true;
            label14.Enabled = true;
            label15.Enabled = true;
            label16.Enabled = true;
            label17.Enabled = true;
            label18.Enabled = true;
            label19.Enabled = true;
            label20.Enabled = true;
            label21.Enabled = true;

            switch (comboBox1.Text)
            {
                case "Gantry":
                    checkBox3.Enabled = true;
                    textBox11.Text = Math.Round(gant_gain, 7, MidpointRounding.ToEven).ToString();
                    textBox12.Text = Math.Round(gant_offset, 3, MidpointRounding.ToEven).ToString();
                    break;
                case "Collimator":
                    checkBox3.Enabled = true;
                    textBox11.Text = Math.Round(collim_gain, 7, MidpointRounding.ToEven).ToString();
                    textBox12.Text = Math.Round(collim_offset, 3, MidpointRounding.ToEven).ToString();
                    break;
                case "X1":
                    checkBox3.Enabled = false;
                    textBox11.Text = Math.Round(x1_gain, 7, MidpointRounding.ToEven).ToString();
                    textBox12.Text = Math.Round(x1_offset, 3, MidpointRounding.ToEven).ToString();
                    break;
                case "X2":
                    checkBox3.Enabled = false;
                    textBox11.Text = Math.Round(x2_gain, 7, MidpointRounding.ToEven).ToString();
                    textBox12.Text = Math.Round(x2_offset, 3, MidpointRounding.ToEven).ToString();
                    break;
                case "Y1":
                    checkBox3.Enabled = false;
                    textBox11.Text = Math.Round(y1_gain, 7, MidpointRounding.ToEven).ToString();
                    textBox12.Text = Math.Round(y1_offset, 3, MidpointRounding.ToEven).ToString();
                    break;
                case "Y2":
                    checkBox3.Enabled = false;
                    textBox11.Text = Math.Round(y2_gain, 7, MidpointRounding.ToEven).ToString();
                    textBox12.Text = Math.Round(y2_offset, 3, MidpointRounding.ToEven).ToString();
                    break;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            groupBox4.Enabled = true;
            comboBox1.Enabled = true;
            label11.Enabled = true;
            button18.Enabled = true;
        }

        private void textBox42_TextChanged(object sender, EventArgs e)
        {
            if (textBox42.Text == "")
            {
                gant_set = "0";
                pictureBox1.Hide();
                pictureBox1.BackgroundImage = requestImage;
                goto ff;
            }

            double a;
            try
            {
                a = double.Parse(textBox42.Text);
            }
            catch
            {
                gant_set = "0";
                pictureBox1.BackgroundImage = errorImage;
                pictureBox1.Show();
                goto ff;
            }
            if (a < -180 | a > 180)
            {
                gant_set = "0";
                pictureBox1.BackgroundImage = errorImage;
                pictureBox1.Show();
                goto ff;
            }
            if (gant_dv != null)
            {
                gant_set = ((int)((a - gant_offset) / gant_gain)).ToString();
                pictureBox1.BackgroundImage = requestImage;

                if (Math.Abs(double.Parse(textBox42.Text) - double.Parse(gant_dv)) > 1)
                {
                    pictureBox1.Show();
                }
                else
                {
                    pictureBox1.Hide();
                    pictureBox1.BackgroundImage = requestImage;
                }
            }

        ff:
            {
            }
        }

        private void textBox41_TextChanged(object sender, EventArgs e)
        {
            if (textBox41.Text == "")
            {
                collim_set = "0";
                pictureBox2.Hide();
                pictureBox2.BackgroundImage = requestImage;
                goto ff;
            }

            double a;
            try
            {
                a = double.Parse(textBox41.Text);
            }
            catch
            {
                collim_set = "0";
                pictureBox2.BackgroundImage = errorImage;
                pictureBox2.Show();
                goto ff;
            }
            if (a < -180 | a > 180)
            {
                collim_set = "0";
                pictureBox2.BackgroundImage = errorImage;
                pictureBox2.Show();
                goto ff;
            }
            if (collim_dv != null)
            {
                collim_set = ((int)((a - collim_offset) / collim_gain)).ToString();
                pictureBox2.BackgroundImage = requestImage;

                if (Math.Abs(double.Parse(textBox41.Text) - double.Parse(collim_dv)) > 1)
                {
                    pictureBox2.Show();
                }
                else
                {
                    pictureBox2.Hide();
                    pictureBox2.BackgroundImage = requestImage;
                }
            }
        ff:
            {
            }
        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {
            if (textBox40.Text == "")
            {
                x1_set = "0";
                pictureBox3.Hide();
                pictureBox3.BackgroundImage = requestImage;
                goto ff;
            }
            double a;
            try
            {
                a = double.Parse(textBox40.Text);
            }
            catch
            {
                x1_set = "0";
                pictureBox3.BackgroundImage = errorImage;
                pictureBox3.Show();
                goto ff;
            }
            if (a < 0 | a > 20)
            {
                x1_set = "0";
                pictureBox3.BackgroundImage = errorImage;
                pictureBox3.Show();
                goto ff;
            }
            if (-a > double.Parse(x2_dv) - 1)
            {
                x1_set = "0";
                pictureBox6.BackgroundImage = Resources.Error;
                pictureBox6.Show();
                goto ff;
            }
            if (x2_set != "0")
                if (-a > double.Parse(textBox39.Text) - 1)
                {
                    x1_set = "0";
                    pictureBox6.BackgroundImage = Resources.Error;
                    pictureBox6.Show();
                    goto ff;
                }
            if (x1_dv != null)
            {
                x1_set = ((int)((a - x1_offset) / x1_gain)).ToString();
                pictureBox3.BackgroundImage = requestImage;

                if (Math.Abs(double.Parse(textBox40.Text) - double.Parse(x1_dv)) > .1)
                {
                    pictureBox3.Show();
                }
                else
                {
                    pictureBox3.Hide();
                    pictureBox3.BackgroundImage = requestImage;
                }
            }
        ff:
            {
            }
        }

        private void textBox39_TextChanged(object sender, EventArgs e)
        {
            if (textBox39.Text == "")
            {
                x2_set = "0";
                pictureBox4.Hide();
                pictureBox4.BackgroundImage = requestImage;
                goto ff;
            }

            double a;
            try
            {
                a = double.Parse(textBox39.Text);
            }
            catch
            {
                x2_set = "0";
                pictureBox4.BackgroundImage = errorImage;
                pictureBox4.Show();
                goto ff;
            }
            if (a < 0 | a > 20)
            {
                x2_set = "0";
                pictureBox4.BackgroundImage = errorImage;
                pictureBox4.Show();
                goto ff;
            }
            double x1double;
            double.TryParse(x1_dv, out x1double);
            if (-a > x1double - 1)
            {
                x2_set = "0";
                pictureBox6.BackgroundImage = Resources.Error;
                pictureBox6.Show();
                goto ff;
            }
            if (x1_set != "0")
            {
                double textBox40double;
                double.TryParse(textBox40.Text, out textBox40double);
                if (-a > textBox40double - 1)
                {
                    x2_set = "0";
                    pictureBox6.BackgroundImage = Resources.Error;
                    pictureBox6.Show();
                    goto ff;
                }
            }
            if (x2_dv != null)
            {
                x2_set = ((int)((a - x2_offset) / x2_gain)).ToString();
                pictureBox4.BackgroundImage = requestImage;

                if (Math.Abs(double.Parse(textBox39.Text) - double.Parse(x2_dv)) > .1)
                {
                    pictureBox4.Show();
                }
                else
                {
                    pictureBox4.Hide();
                    pictureBox4.BackgroundImage = requestImage;
                }
            }
        ff:
            {
            }
        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {
            if (textBox38.Text == "")
            {
                y1_set = "0";
                pictureBox5.Hide();
                pictureBox5.BackgroundImage = requestImage;
                goto ff;
            }

            double a;
            try
            {
                a = double.Parse(textBox38.Text);
            }
            catch
            {
                y1_set = "0";
                pictureBox5.BackgroundImage = errorImage;
                pictureBox5.Show();
                goto ff;
            }
            if (a < -12.5 | a > 20)
            {
                y1_set = "0";
                pictureBox5.BackgroundImage = errorImage;
                pictureBox5.Show();
                goto ff;
            }
            if (-a > double.Parse(y2_dv) - 1)
            {
                y1_set = "0";
                pictureBox6.BackgroundImage = Resources.Error;
                pictureBox6.Show();
                goto ff;
            }
            if (y2_set != "0")
                if (-a > double.Parse(textBox37.Text) - 1)
                {
                    y1_set = "0";
                    pictureBox6.BackgroundImage = Resources.Error;
                    pictureBox6.Show();
                    goto ff;
                }
            if (y1_dv != null)
            {
                y1_set = ((int)((a - y1_offset) / y1_gain)).ToString();
                pictureBox5.BackgroundImage = requestImage;

                if (Math.Abs(double.Parse(textBox38.Text) - double.Parse(y1_dv)) > .1)
                {
                    pictureBox5.Show();
                }
                else
                {
                    pictureBox5.Hide();
                    pictureBox5.BackgroundImage = requestImage;
                }
            }
        ff:
            {
            }
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            if (textBox37.Text == "")
            {
                y2_set = "0";
                pictureBox6.Hide();
                pictureBox6.BackgroundImage = requestImage;
                goto ff;
            }

            double a;
            try
            {
                a = double.Parse(textBox37.Text);
            }
            catch
            {
                y2_set = "0";
                pictureBox6.BackgroundImage = errorImage;
                pictureBox6.Show();
                goto ff;
            }
            if (a < -12.5 | a > 20)
            {
                y2_set = "0";
                pictureBox6.BackgroundImage = errorImage;
                pictureBox6.Show();
                goto ff;
            }
            if (-a > double.Parse(y1_dv) - 1)
            {
                y2_set = "0";
                pictureBox6.BackgroundImage = Resources.Error;
                pictureBox6.Show();
                goto ff;
            }
            if (y1_set != "0")
                if (-a > double.Parse(textBox38.Text) - 1)
                {
                    y2_set = "0";
                    pictureBox6.BackgroundImage = Resources.Error;
                    pictureBox6.Show();
                    goto ff;
                }
            if (y2_dv != null)
            {
                y2_set = ((int)((a - y2_offset) / y2_gain)).ToString();
                pictureBox6.BackgroundImage = requestImage;

                if (Math.Abs(double.Parse(textBox37.Text) - double.Parse(y2_dv)) > .1)
                {
                    pictureBox6.Show();
                }
                else
                {
                    pictureBox6.Hide();
                    pictureBox6.BackgroundImage = requestImage;
                }
            }
        ff:
            {
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {

            button18.Enabled = false;

            if (GlobalSerialPort.IsOpen == false)
                GlobalSerialPort.Open();


            else
            {
                try
                {
                    string appPath = Application.StartupPath;
                    string dataPath = System.IO.Path.Combine(appPath, "Calib.dat");
                    string[] values = { gant_gain.ToString(), gant_offset.ToString(), collim_gain.ToString(), collim_offset.ToString(),
                                 x1_gain.ToString(), x1_offset.ToString(), x2_gain.ToString(), x2_offset.ToString(),
                                 y1_gain.ToString(), y1_offset.ToString(), y2_gain.ToString(), y2_offset.ToString()};
                    //System.IO.File.WriteAllLines(dataPath, lines);
                    HashPass.writeCalibJson(dataPath, values);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving to file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
                }
            }
            groupBox4.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClosePort();
            CloseProcess();
            Class_PatientData.cmd.Dispose();
            Application.Exit();
        }

        private static void CloseProcess()
        {
            foreach (Process p in System.Diagnostics.Process.GetProcessesByName("ProcessInfo"))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit(); // possibly with a timeout
                }
                catch (Win32Exception winException)
                {
                    // process was terminating or can't be terminated - deal with it
                }
                catch (InvalidOperationException invalidException)
                {
                    // process has already exited - might be able to let this one go
                }
            }
        }

        private void ClosePort()
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                serialPort1.Dispose();
                serialPort1.Close();
            }
            if (clientFrm.serialPort1.IsOpen == true)
            {
                clientFrm.serialPort1.DiscardOutBuffer();
                clientFrm.serialPort1.DiscardInBuffer();
                clientFrm.serialPort1.Dispose();
                clientFrm.serialPort1.Close();
            }
            if (GlobalSerialPort.IsOpen == true)
            {
                GlobalSerialPort.DiscardOutBuffer();
                GlobalSerialPort.DiscardInBuffer();
                GlobalSerialPort.Dispose();
                GlobalSerialPort.Close();
            }
        }

        private void serialPort1_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            string a = "";
            try
            {
                if (GlobalSerialPort.IsOpen)
                    a = GlobalSerialPort.ReadLine();
            }
            catch
            {
            }
            try
            {
                switch (a.Substring(0, 3))
                {
                    case "gco":
                        gant_co = a.Substring(3, a.Length - 3);
                        break;
                    case "gf1":
                        gant_f1 = a.Substring(3, a.Length - 3);
                        break;
                    case "gf2":
                        gant_f2 = a.Substring(3, a.Length - 3);
                        break;
                    case "gfn":
                        gant_cofin = a.Substring(3, a.Length - 3);
                        break;
                    case "cco":
                        collim_co = a.Substring(3, a.Length - 3);
                        break;
                    case "cf1":
                        collim_f1 = a.Substring(3, a.Length - 3);
                        break;
                    case "cf2":
                        collim_f2 = a.Substring(3, a.Length - 3);
                        break;
                    case "cfn":
                        collim_cofin = a.Substring(3, a.Length - 3);
                        break;
                    case "wco":
                        x1_co = a.Substring(3, a.Length - 3);
                        break;
                    case "xco":
                        x2_co = a.Substring(3, a.Length - 3);
                        break;
                    case "yco":
                        y1_co = a.Substring(3, a.Length - 3);
                        break;
                    case "zco":
                        y2_co = a.Substring(3, a.Length - 3);
                        break;
                    case "lok":
                        MessageBox.Show("Learning was succesfull");
                        break;
                    case "sok":
                        MessageBox.Show("Saving was succesfull!");
                        break;
                    case "snk":
                        MessageBox.Show("Error: Saving was not succesfull!");
                        break;

                    case "c43":
                        gant_zpnt = a.Substring(3, a.Length - 3);
                        break;
                    case "c44":
                        gant_length = a.Substring(3, a.Length - 3);
                        break;
                    case "c45":
                        gant_fine_length = a.Substring(3, a.Length - 3);
                        //GlobalSerialPort.Write(gant_zpnt + (gant_zpnt.Length + 1).ToString() + "/" + gant_length + (gant_length.Length + 1).ToString() + "/" + gant_fine_length + (gant_fine_length.Length + 1).ToString() + "/");
                        GlobalSerialPort.Write(gant_zpnt + "/" + gant_length + "/" + gant_fine_length + "/");
                        try
                        {
                            string appPath = Application.StartupPath;
                            string dataPath = System.IO.Path.Combine(appPath, "Learn.dat");
                            string[] values = { gant_zpnt, gant_length, gant_fine_length, collim_zpnt, collim_length, collim_fine_length };
                            //System.IO.File.WriteAllLines(dataPath, lines);
                            HashPass.writeLearnJson(dataPath, values);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error saving to file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
                        }
                        break;
                    case "c46":
                        collim_zpnt = a.Substring(3, a.Length - 3);
                        break;
                    case "c47":
                        collim_length = a.Substring(3, a.Length - 3);
                        break;
                    case "c48":
                        collim_fine_length = a.Substring(3, a.Length - 3);
                        GlobalSerialPort.Write(collim_zpnt + "/" + collim_length + "/" + collim_fine_length + "/");
                        //GlobalSerialPort.Write(collim_zpnt + (collim_zpnt.Length + 1).ToString() + "/");
                        //GlobalSerialPort.Write(collim_length + (collim_length.Length + 1).ToString() + "/");
                        //GlobalSerialPort.Write(collim_fine_length + (collim_fine_length.Length + 1).ToString() + "/");
                        try
                        {
                            string appPath = Application.StartupPath;
                            string dataPath = System.IO.Path.Combine(appPath, "Learn.dat");
                            string[] values = { gant_zpnt, gant_length, gant_fine_length, collim_zpnt, collim_length, collim_fine_length };
                            //System.IO.File.WriteAllLines(dataPath, lines);
                            HashPass.writeLearnJson(dataPath, values);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error saving to file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
                        }
                        break;
                    case "gnd":
                        gnd = a.Substring(3, a.Length - 3);
                        break;
                    case "cld":
                        cld = a.Substring(3, a.Length - 3);
                        break;
                    case "x1d":
                        x1d = a.Substring(3, a.Length - 3);
                        break;
                    case "x2d":
                        x2d = a.Substring(3, a.Length - 3);
                        break;
                    case "y1d":
                        y1d = a.Substring(3, a.Length - 3);
                        break;
                    case "y2d":
                        y2d = a.Substring(3, a.Length - 3);
                        break;
                    case "adc":
                        adc = a.Substring(3, a.Length - 3);
                        if (gant_set != gnd)
                            GlobalSerialPort.Write("m" + gant_set + (gant_set.Length + 1).ToString() + "/");
                        if (collim_set != cld)
                            GlobalSerialPort.Write("n" + collim_set + (collim_set.Length + 1).ToString() + "/");
                        if (x1_set != x1d)
                            GlobalSerialPort.Write("o" + x1_set + (x1_set.Length + 1).ToString() + "/");
                        if (x2_set != x2d)
                            GlobalSerialPort.Write("p" + x2_set + (x2_set.Length + 1).ToString() + "/");
                        if (y1_set != y1d)
                            GlobalSerialPort.Write("q" + y1_set + (y1_set.Length + 1).ToString() + "/");
                        if (y2_set != y2d)
                            GlobalSerialPort.Write("r" + y2_set + (y2_set.Length + 1).ToString() + "/");
                        if (quit == true)
                        {
                            ClosePort();
                            Application.Exit();
                            break;
                        }
                        break;
                }
            }
            catch
            {
            }
        }

        private void picBtn_Connect_MouseEnter(object sender, EventArgs e)
        {
            //picBtnToolTip.SetToolTip(picBtn_Connect, "Connect");
            picBtn_Connect.BackgroundImage = Resources.ConnectButton_MouseOver;
        }

        private void picBtn_Connect_MouseLeave(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen)
                picBtn_Connect.BackgroundImage = Resources.ConnectButton_Connected;
            else
                picBtn_Connect.BackgroundImage = Resources.ConnectButton;
        }

        private void picBtn_PatientList_MouseEnter(object sender, EventArgs e)
        {
            //picBtnToolTip.SetToolTip(picBtn_PatientList, "Show Patient List...");
            //picBtn_PatientList.BackgroundImage = Resources.btn_PatientList_MouseOver;
        }

        private void picBtn_PatientList_MouseLeave(object sender, EventArgs e)
        {
            //picBtn_PatientList.BackgroundImage = Resources.btn_PatientList;
        }

        private void picBtn_Setting_MouseEnter(object sender, EventArgs e)
        {
            picBtnToolTip.SetToolTip(picBtn_Setting, "Settings");
            picBtn_Setting.BackgroundImage = Resources.Setting_mouseOver;
        }

        private void picBtn_Setting_MouseLeave(object sender, EventArgs e)
        {
            picBtn_Setting.BackgroundImage = Resources.Setting;
        }

        private void picBtn_LogOff_MouseEnter(object sender, EventArgs e)
        {
            picBtnToolTip.SetToolTip(picBtn_LogOff, "Log off");
            picBtn_LogOff.BackgroundImage = Resources.Logout_mouseOver;
        }

        private void picBtn_LogOff_MouseLeave(object sender, EventArgs e)
        {
            picBtn_LogOff.BackgroundImage = Resources.Logout;
        }

        private void picBtn_Exit_MouseEnter(object sender, EventArgs e)
        {
            picBtnToolTip.SetToolTip(picBtn_Exit, "Exit");
            picBtn_Exit.BackgroundImage = Resources.Exit_mouseOver2;
        }

        private void picBtn_Exit_MouseLeave(object sender, EventArgs e)
        {
            picBtn_Exit.BackgroundImage = Resources.Exit2;
        }

        private void picBtn_LogOff_Click(object sender, EventArgs e)
        {
            string appPath = Application.StartupPath;
            string tempFile = System.IO.Path.Combine(appPath, Form_Login.tempFile);
            if (System.IO.File.Exists(tempFile))
                System.IO.File.Delete(tempFile);
            System.IO.File.Create(tempFile).Dispose();
            CloseProcess();
            this.Hide();
            try
            {
                DisconnectPort();
            }
            catch { }
            //Form_Login.ShowForm();
            Application.Restart();
            }

        private void picBtn_Exit_Click(object sender, EventArgs e)
        {
            quit = true;
            //ClosePort();
            if (GlobalSerialPort.IsOpen == true)
                MessageBox.Show("Please click on ''Disconnect'' button before exit!");
            else
                Application.Exit();
        }

        public static string curr_baudRate = "";
        private void picBtn_Connect_Click(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                SetConnection(true);
            else
                SetConnection(false);
        }

        private void SetConnection(bool connect)
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                MessageBox.Show("No Serial port detected!", "COM Port error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //else if (ports.Length > 1)
            //    MessageBox.Show("More than one Serial port detected!\n" + portName + " Selected to connect.", "Multiple COM Port", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (isInServiceMode == false)
            {
                GlobalSerialPort = clientFrm.serialPort1;
                GlobalSerialPort.DataReceived += clientFrm.serialPort1_DataReceived;
            }
            else
            {
                GlobalSerialPort = serialPort1;
                GlobalSerialPort.DataReceived += serialPort1_DataReceived_1;
            }
            if (connect)
            {
                if (portName != "Null" && portName != "")
                    GlobalSerialPort.PortName = portName;
                else
                    GlobalSerialPort.PortName = ports[0];

                int BaudRate = 0;
                if (int.TryParse(HashPass.ReadBaudRateFromReg(), out BaudRate) == true && BaudRate != 0)
                    GlobalSerialPort.BaudRate = BaudRate;

                DisconnectPort();
                Thread.Sleep(200);
                ConnectToPort();
                //Thread.Sleep(200);
                //DisconnectPort();
                //Thread.Sleep(200);
                //ConnectToPort();
                curr_baudRate = GlobalSerialPort.BaudRate.ToString();
            }
            else
            {
                DisconnectPort();
            }
        }

        private void DisconnectPort()
        {
            //GlobalSerialPort.DiscardInBuffer();
            //GlobalSerialPort.Close();
            panel_AdminControls.Enabled = false;
            panel_ClientControls.Enabled = false;
            picBtn_Connect.BackgroundImage = Resources.ConnectButton;
            picBtnToolTip.SetToolTip(picBtn_Connect, "Connect");
            label_ConnectStatus.ForeColor = Color.Red;
            label_ConnectStatus.Text = "Disconnected!";
            ClosePort();
        }

        private void ConnectToPort()
        {
            try
            {
                //ClosePort();
                //MessageBox.Show("Connection error!", "An error occured during connection!\n\n");
                if (!GlobalSerialPort.IsOpen)
                {
                    GlobalSerialPort.Open();
                    Thread.Sleep(500);
                }
                if (isInServiceMode == true)
                {
                    panel_AdminControls.Enabled = true;
                }
                else
                {
                    panel_ClientControls.Enabled = true;
                }
                try
                {
                    string appPath = Application.StartupPath;
                    string dataPath = System.IO.Path.Combine(appPath, "Calib.dat");
                    if (!System.IO.File.Exists(dataPath))
                    {
                        timer1.Stop();
                        timer1.Enabled = false;
                        MessageBox.Show("Can not connect to port!\n''Calib.dat'' file not found!", "Calibration file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    //string[] lines = System.IO.File.ReadAllLines(dataPath);
                    //gant_gain = double.Parse(lines[0]);
                    //gant_offset = double.Parse(lines[1]);
                    //collim_gain = double.Parse(lines[2]);
                    //collim_offset = double.Parse(lines[3]);
                    //x1_gain = double.Parse(lines[4]);
                    //x1_offset = double.Parse(lines[5]);
                    //x2_gain = double.Parse(lines[6]);
                    //x2_offset = double.Parse(lines[7]);
                    //y1_gain = double.Parse(lines[8]);
                    //y1_offset = double.Parse(lines[9]);
                    //y2_gain = double.Parse(lines[10]);
                    //y2_offset = double.Parse(lines[11]);

                    HashPass.CalibData values = HashPass.readCalibJson(dataPath);
                    gant_gain = double.Parse(values.gant_gain);
                    gant_offset = double.Parse(values.gant_offset);
                    collim_gain = double.Parse(values.collim_gain);
                    collim_offset = double.Parse(values.collim_offset);
                    x1_gain = double.Parse(values.x1_gain);
                    x1_offset = double.Parse(values.x1_offset);
                    x2_gain = double.Parse(values.x2_gain);
                    x2_offset = double.Parse(values.x2_offset);
                    y1_gain = double.Parse(values.y1_gain);
                    y1_offset = double.Parse(values.y1_offset);
                    y2_gain = double.Parse(values.y2_gain);
                    y2_offset = double.Parse(values.y2_offset);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading from file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
                }
                try
                {
                    string appPath = Application.StartupPath;
                    string dataPath = System.IO.Path.Combine(appPath, "Learn.dat");
                    if (!System.IO.File.Exists(dataPath))
                    {
                        timer1.Stop();
                        timer1.Enabled = false;
                        MessageBox.Show("Can not connect to port!\n''Learn.dat'' file not found!", "Learn file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }

                    //string[] lines = System.IO.File.ReadAllLines(dataPath);
                    //gant_zpnt = lines[0];
                    //gant_length = lines[2];
                    //gant_fine_length = lines[4];
                    //collim_zpnt = lines[6];
                    //collim_length = lines[8];
                    //collim_fine_length = lines[10];
                    
                    HashPass.LearnData values = HashPass.readLearnJson(dataPath);
                    gant_zpnt = values.gant_zpnt;
                    gant_length = values.gant_length;
                    gant_fine_length = values.gant_fine_length;
                    collim_zpnt = values.collim_zpnt;
                    collim_length = values.collim_length;
                    collim_fine_length = values.collim_fine_length;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading from file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
                }

                try
                {
                    string appPath = Application.StartupPath;
                    string dataPath = System.IO.Path.Combine(appPath, "Parameters.dat");
                    if (!System.IO.File.Exists(dataPath))
                    {
                        timer1.Stop();
                        timer1.Enabled = false;
                        MessageBox.Show("Can not connect to port!\n''Parameters.dat'' file not found!", "Parameters file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    HashPass.ParametersData values = HashPass.readParametersJson(dataPath);

                    string[] prms = new string[36];

                    prms[0]  = ClientControls.gant_tol0 = values.gant_tol0;
                    prms[1]  = ClientControls.gant_tol1 = values.gant_tol1;	
                    prms[2]  = ClientControls.gant_tol2 = values.gant_tol2;	
                    prms[3]  = ClientControls.gant_v1 = values.gant_v1;
                    prms[4]  = ClientControls.gant_v2 = values.gant_v2;
                    prms[5]  = ClientControls.gant_v3 = values.gant_v3;
                    prms[6]  = ClientControls.collim_tol0 = values.collim_tol0;
                    prms[7]  = ClientControls.collim_tol1 = values.collim_tol1;
                    prms[8]  = ClientControls.collim_tol2 = values.collim_tol2;  
                    prms[9]  = ClientControls.collim_v1 = values.collim_v1;
                    prms[10] = ClientControls.collim_v2 = values.collim_v2;
                    prms[11] = ClientControls.collim_v3 = values.collim_v3;
                    prms[12] = ClientControls.x1_tol0 = values.x1_tol0;
                    prms[13] = ClientControls.x1_tol1 = values.x1_tol1;
                    prms[14] = ClientControls.x1_tol2 = values.x1_tol2;
                    prms[15] = ClientControls.x1_v1 =  values.x1_v1;
                    prms[16] = ClientControls.x1_v2 =  values.x1_v2;
                    prms[17] = ClientControls.x1_v3 =  values.x1_v3;
                    prms[18] = ClientControls.x2_tol0 = values.x2_tol0;
                    prms[19] = ClientControls.x2_tol1 = values.x2_tol1;
                    prms[20] = ClientControls.x2_tol2 = values.x2_tol2;
                    prms[21] = ClientControls.x2_v1 =  values.x2_v1;
                    prms[22] = ClientControls.x2_v2 =  values.x2_v2;
                    prms[23] = ClientControls.x2_v3 =  values.x2_v3;
                    prms[24] = ClientControls.y1_tol0 = values.y1_tol0;
                    prms[25] = ClientControls.y1_tol1 = values.y1_tol1;
                    prms[26] = ClientControls.y1_tol2 = values.y1_tol2;
                    prms[27] = ClientControls.y1_v1 =  values.y1_v1;
                    prms[28] = ClientControls.y1_v2 =  values.y1_v2;
                    prms[29] = ClientControls.y1_v3 =  values.y1_v3;
                    prms[30] = ClientControls.y2_tol0 = values.y2_tol0;
                    prms[31] = ClientControls.y2_tol1 = values.y2_tol1;
                    prms[32] = ClientControls.y2_tol2 = values.y2_tol2;
                    prms[33] = ClientControls.y2_v1 =  values.y2_v1;
                    prms[34] = ClientControls.y2_v2 =  values.y2_v2;
                    prms[35] = ClientControls.y2_v3 =  values.y2_v3;

                    int i = 0;
                    foreach (Control tb in gb_parameters.Controls)
                    {
                        if (tb is TextBox)
                        {
                            tb.Text = prms[tb.TabIndex - 7];
                            i = i + 1;
                        }
                    }

                    string[] ourParams = new string[42];
                    ourParams[0] = gant_zpnt;
                    ourParams[1] = gant_length;
                    ourParams[2] = gant_fine_length;
                    ourParams[3] = collim_zpnt;
                    ourParams[4] = collim_length;
                    ourParams[5] = collim_fine_length;
                    Array.Copy(prms, 0, ourParams, 6, prms.Length);
                    ClientControls.ourParameters = ourParams;
                    lbl_init.Visible = true;
                    lbl_init.Text = "Initializing...";
                    lbl_init.ForeColor = Color.Green;
                    ClientControls.sendParametersFlag = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error reading from file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
                }

                panel_AdminControls.Enabled = true;
                panel_ClientControls.Enabled = true;
                picBtn_Connect.BackgroundImage = Resources.ConnectButton_Connected;
                picBtnToolTip.SetToolTip(picBtn_Connect, "Disconnect");
                label_ConnectStatus.ForeColor = Color.Green;
                label_ConnectStatus.Text = "Connected!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error!", "An error occured during connection!\n" + Environment.NewLine + ex.ToString().Split('\n')[0], MessageBoxButtons.OK, MessageBoxIcon.Error);
                ConnectToPort();
            }
        }

        private void picBtn_Setting_Click(object sender, EventArgs e)
        {
            Form_Settings frmSet = new Form_Settings();
            if (isInServiceMode == false)
            {
                PrepareSettingForPort(frmSet);
            }
            else if (this.Text.Contains("Service"))
            {
                curr_baudRate = GlobalSerialPort.BaudRate.ToString();
                PrepareSettingForAll(frmSet);
            }
            if (frmSet.ShowDialog() == DialogResult.OK)
            {
                if (portName == "Null")
                    return;
                if (isInServiceMode)
                {
                    DisconnectPort();
                    Thread.Sleep(200);
                    GlobalSerialPort.PortName = portName;
                    GlobalSerialPort.BaudRate = int.Parse(curr_baudRate);
                    HashPass.WriteBaudrateToReg(curr_baudRate);
                    ConnectToPort();
                }
            }
        }

        private static void PrepareSettingForAll(Form_Settings frmSet)
        {
            frmSet.checkBox_startup.Show();
            frmSet.groupBox_UserManagement.Show();
            frmSet.groupBox_portSetting.Show();
        }

        private static void PrepareSettingForPort(Form_Settings frmSet)
        {
            frmSet.checkBox_startup.Hide();
            frmSet.groupBox_UserManagement.Hide();
            frmSet.groupBox_portSetting.Hide();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            //ClientControls clientFrm = new ClientControls();
            if (this.Visible == true)
            {
                if (panel_ClientControls.Controls.Count != 3)
                {
                    if (isInServiceMode == false)
                    {
                        //GlobalSerialPort = clientFrm.serialPort1;
                        //GlobalSerialPort.DataReceived += clientFrm.serialPort1_DataReceived;
                        panel_ClientControls.Controls.Add(clientFrm);
                    }
                    else
                    {
                        //GlobalSerialPort = serialPort1;
                        //GlobalSerialPort.DataReceived += serialPort1_DataReceived_1;
                    }
                    string[] ports = SerialPort.GetPortNames();
                    if (ports.Length >= 1)
                    {
                        if (GlobalSerialPort.IsOpen == false)
                        {
                            if (portName != "Null" && portName != "")
                                GlobalSerialPort.PortName = portName;
                            else
                                GlobalSerialPort.PortName = ports[0];
                            //ConnectToPort();
                        }
                    }
                }
                else
                {
                    //GlobalSerialPort = serialPort1;
                    //GlobalSerialPort.DataReceived += serialPort1_DataReceived_1;
                }
            }
            else
            {
                panel_ClientControls.Controls.Clear();
            }
        }

        private void picBtn_PatientList_Click(object sender, EventArgs e)
        {
            HashPass.refreshLicInfo();
            if (HashPass.isExpired == true)
            {
                this.Hide();
                ClosePort();
                Form_Login.ShowForm();
                return;
            }

            MessageBox.Show("This is a demo version!\nThis item and another useful options will be available in full version!", "Limited version", MessageBoxButtons.OK, MessageBoxIcon.Information);
            /*
            Form_PatientList patientsFrm = new Form_PatientList();
            patientsFrm.ClearSelection();
            patientsFrm.ShowDialog();
             */
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (isInServiceMode)
            {
                this.TopMost = false;
                EnableCTRLALTDEL();
            }
            else
            {
                this.TopMost = false;
                KillCtrlAltDelete();
            }
            if (Class_PatientData.valuesChanged == true)
            {
                clientFrm.FillValues();
                //clientFrm.FillPatientData();
                //clientFrm.FillFieldData();
                Class_PatientData.valuesChanged = false;
            }
        }

        private void label40_Click(object sender, EventArgs e)
        {
            //Form_TrialReport trialFrm = new Form_TrialReport();
            //trialFrm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (isInServiceMode)
            {
                panel1.BackColor = Color.Turquoise;
                label_title.Text = "Service";
                picBtn_Exit.Show();
                picBtn_Close.Show();
                picBtn_Setting.Show();
            }
            else
            {
                panel1.BackColor = Color.LightPink;
                label_title.Text = "Clinical";
                picBtn_Exit.Hide();
                picBtn_Close.Hide();
                picBtn_Setting.Hide();
            }
            //Form_TrialReport trialFrm = new Form_TrialReport();
            //trialFrm.ShowDialog();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Class_PatientData.isBoardReadWrite = checkBox4.Checked;
        }

        private void txtBox_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void picBtn_Restart_MouseEnter(object sender, EventArgs e)
        {
            picBtnToolTip.SetToolTip(picBtn_Restart, "Restart Computer");
            picBtn_Restart.BackgroundImage = Resources.Restart_mouseOver;
        }

        private void picBtn_Restart_MouseLeave(object sender, EventArgs e)
        {
            picBtn_Restart.BackgroundImage = Resources.Restart;
        }

        private void picBtn_Shutdown_MouseEnter(object sender, EventArgs e)
        {
            picBtnToolTip.SetToolTip(picBtn_Shutdown, "Shutdown Computer");
            picBtn_Shutdown.BackgroundImage = Resources.Shutdown_mouseOver;
        }

        private void picBtn_Shutdown_MouseLeave(object sender, EventArgs e)
        {
            picBtn_Shutdown.BackgroundImage = Resources.Shutdown;
        }

        private void picBtn_Restart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Restart the computer?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Process.Start("Shutdown", "/r /t 1");
            }
        }

        private void picBtn_Shutdown_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Shutdown the computer?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Process.Start("Shutdown", "/s /t 1");
            }
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            string time = now.ToShortTimeString();
            string date = now.ToShortDateString();
            PersianCalendar pc = new PersianCalendar();
            string miladiDate = now.Year + "/" +
                now.Month + "/" + now.Day;
            string shamsiDate = pc.GetYear(now) + "/" +
                pc.GetMonth(now) + "/" + pc.GetDayOfMonth(now);
            label_time.Text = time;
            label_date.Text = miladiDate;
            label_shamsiDate.Text = shamsiDate;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void picBtn_Close_Click(object sender, EventArgs e)
        {
            quit = true;
            //ClosePort();
            if (GlobalSerialPort.IsOpen == true)
                MessageBox.Show("Please click on ''Disconnect'' button before exit!");
            else
                Application.Exit();

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (GlobalSerialPort.IsOpen == false)
                SetConnection(true);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Left)
            {
                Application.Exit();
            }
        }

        public static bool initStatus = false;
        private void btn_saveParameters_Click(object sender, EventArgs e)
        {
            lbl_init.Visible = true;
            lbl_init.Text = "Initializing...";
            lbl_init.ForeColor = Color.Green;
            string[] values = new string[36];
            int i = 0;
            foreach (Control tb in gb_parameters.Controls)
            {
                if (tb is TextBox)
                {
                    values[tb.TabIndex-7] = tb.Text;
                    i = i + 1;
                }
            }
            try
            {
                string appPath = Application.StartupPath;
                string dataPath = System.IO.Path.Combine(appPath, "Parameters.dat");
                HashPass.writeParametersJson(dataPath, values);

                string[] ourParams = new string[42];
                ourParams[0] = gant_zpnt;
                ourParams[1] = gant_length;
                ourParams[2] = gant_fine_length;
                ourParams[3] = collim_zpnt;
                ourParams[4] = collim_length;
                ourParams[5] = collim_fine_length;
                Array.Copy(values, 0, ourParams, 6, values.Length);
                ClientControls.ourParameters = ourParams;
                lbl_init.Visible = true;
                lbl_init.Text = "Initializing...";
                lbl_init.ForeColor = Color.Green;
                ClientControls.sendParametersFlag = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error saving parameters to file" + Environment.NewLine + ex.ToString().Split('\n')[0]);
            }
        }
    }
}