﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualBasic.PowerPacks;

namespace _0411
{
    public partial class Form1 : Form
    {
        UdpClient U;
        Thread Th;

        ShapeContainer C;
        ShapeContainer D;
        Point stP;
        string p;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            C = new ShapeContainer();
            this.Controls.Add(C);
            D = new ShapeContainer();
            this.Controls.Add(D);
        }
        private void Listen()
        {
            int Port = int.Parse(textBox_ListenPort.Text);
            U = new UdpClient(Port);

            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            while (true)
            {
                byte[] B = U.Receive(ref EP);
                string A = Encoding.Default.GetString(B);
                string[] Z = A.Split('_');
                string[] Q = Z[1].Split('/');
                Point[] R = new Point[Q.Length];
                for (int i = 0; i < Q.Length; i++)
                {
                    string[] K = Q[i].Split(',');
                    R[i].X = int.Parse(K[0]);
                    R[i].Y = int.Parse(K[1]);

                }
                for (int i = 0; i < Q.Length - 1; i++)
                {
                    LineShape L = new LineShape();
                    L.StartPoint = R[i];
                    L.EndPoint = R[i + 1];
                    

                    switch (Z[0])
                    {
                        case "1":
                            L.BorderColor = Color.Red;
                            break;
                        case "2":
                            L.BorderColor = Color.Lime;
                            break;
                        case "3":
                            L.BorderColor = Color.Blue;
                            break;
                        case "4":
                            L.BorderColor = Color.Black;
                            break;
                    }
                   L.Parent = D;
                }
            }

            
        
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Th.Abort();
                U.Close();
            }
            catch
            { 
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Th = new Thread(Listen);
            Th.Start();
            button1.Enabled = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            stP = e.Location;
            p = stP.X.ToString() + "," + stP.Y.ToString();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                LineShape L = new LineShape();
                L.StartPoint = stP;
                L.EndPoint = e.Location;

                if (radioButton_red.Checked) { L.BorderColor = Color.Red; }
                if (radioButton_green.Checked) { L.BorderColor = Color.Lime; }
                if (radioButton_blue.Checked) { L.BorderColor = Color.Blue; }
                if (radioButton_black.Checked) { L.BorderColor = Color.Black; }

                L.Parent = C;
                stP = e.Location;
                p += "/" + stP.X.ToString() + "," + stP.Y.ToString();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            int Port = int.Parse(textBox_port.Text);
            UdpClient S = new UdpClient(textBox_ip.Text, Port);

            if (radioButton_red.Checked) { p = "1_" + p; }
            if (radioButton_red.Checked) { p = "2_" + p; }
            if (radioButton_red.Checked) { p = "3_" + p; }
            if (radioButton_red.Checked) { p = "4_" + p; }

            byte[] B = Encoding.Default.GetBytes(p);
            S.Send(B, B.Length);
            S.Close();
        }
    }
}
