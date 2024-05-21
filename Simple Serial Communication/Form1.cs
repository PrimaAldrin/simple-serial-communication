using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Serial_Communication{
    public partial class Form1 : Form{

        private bool connectStatus = false;
        private bool allRequest = false;
        private bool temperatureRequest = false;

        public Form1(){
            InitializeComponent();
            label1.Text = "Port";
            label2.Text = "Request Data";
            label3.Text = "Baud Rate";

            label4.Text = "Temperature";
            label5.Text = "Humidity";
            label6.Text = "Pressure";
            label7.Text = "UV Index";

            button1.Text = "Connect";
            button2.Text = "Close";
            button3.Text = "Send";

            groupBox1.Text = "Serial Port Settings";
            groupBox2.Text = "Send Custom Request";
            groupBox3.Text = "Received Data";
            groupBox4.Text = "Splitted Data";

            toolStripStatusLabel1.Text = "Message";

            button1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){
            button1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e){
            String[] portList = System.IO.Ports.SerialPort.GetPortNames();
            foreach(String portName in portList){
                comboBox1.Items.Add(portName);
                comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 1].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e){
            try{
                if (connectStatus == false){
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.Open();
                    connectStatus = !connectStatus;
                    toolStripStatusLabel1.Text = "Message";

                    serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                    serialPort1.NewLine = "\r\n";
                    Form1.ActiveForm.Text = serialPort1.PortName + " is connected.";
                }
                else{
                    Form1.ActiveForm.Text = "Already connected!";
                    toolStripStatusLabel1.Text = "Message";
                }
            }

            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error: " + ex.Message.ToString();
            }
            groupBox3.Text = "Received Data";
        }

        private void button2_Click(object sender, EventArgs e){
            serialPort1.Close();
            connectStatus = false;
            Form1.ActiveForm.Text = "Serial Communication";

            toolStripStatusLabel1.Text = serialPort1.PortName + " Connected";
        }

        private void button3_Click(object sender, EventArgs e){
            if(connectStatus == true){
                serialPort1.WriteLine(textBox1.Text);
                listBox1.Items.Add("Sent: " + textBox1.Text);
            }
            else{
                Form1.ActiveForm.Text = "Not connected yet, please connect to the correct Port first";
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e){
            Display(serialPort1.ReadLine());
        }

        private delegate void DisplayDelegate(object item);

        private void Display(object item){
            if (InvokeRequired){
                listBox1.Invoke(new DisplayDelegate(Display), item);
            }
            else{
                listBox1.Items.Add("Received: " + item);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                if (allRequest == true)
                {
                    splitData(item);
                }
            }
        }

        private void splitData(object item){
            String[] data = item.ToString().Split(',');
            textBox2.Text = data[1];
            textBox3.Text = data[2];
            textBox4.Text = data[3];
            textBox5.Text = data[4];
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (connectStatus == true)
            {
                serialPort1.WriteLine("A");
                listBox1.Items.Add("Sent: A");
                allRequest = true;
            }
            else
            {
                Form1.ActiveForm.Text = "Not connected yet, please connect to the correct Port first";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (connectStatus == true)
            {
                serialPort1.WriteLine("T");
                listBox1.Items.Add("Sent: T");
                temperatureRequest = true;
            }
            else
            {
                Form1.ActiveForm.Text = "Not connected yet, please connect to the correct Port first";
            }
        }
    }
}
