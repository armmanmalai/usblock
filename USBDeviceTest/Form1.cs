using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Management; // need to add System.Management to your project references.

using System.Security.Claims;

using Jose;

namespace USBDeviceTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<USBDeviceInfo> usbs = GetUSBDevices();
            string x = "";
            textBox1.Text = "";
            foreach (USBDeviceInfo usb in usbs)
            {
                x = x + "\r\n" + usb.DeviceID + ":" + usb.Description + ":" + usb.PnpDeviceID;

            }
            textBox1.Text = x;

        }

        public static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var payload = new Dictionary<string, object>()
                {
                { "sub", "mr.x@contoso.com" },
                { "exp", 1300819380 }
                };

             byte[] array = Encoding.ASCII.GetBytes(textBox3.Text);
            string token = Jose.JWT.Encode(payload, array, JwsAlgorithm.HS256);
            textBox2.Text = token;
        }

        public class USBDeviceInfo
        {
            public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
            {
                this.DeviceID = deviceID;
                this.PnpDeviceID = pnpDeviceID;
                this.Description = description;
            }
            public string DeviceID { get; private set; }
            public string PnpDeviceID { get; private set; }
            public string Description { get; private set; }
        }



    }
}
