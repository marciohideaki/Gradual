using System;
using System.Collections.Generic;
using System.Text;
using Gradual.Utils.Hardware;

namespace Gradual.Utils
{
    /// <summary>
    /// Example Application
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Local Connection
            Connection wmiConnection = new Connection();
            Win32_BaseBoard a = new Win32_BaseBoard(wmiConnection);
            Win32_Battery b = new Win32_Battery(wmiConnection);
            Win32_BIOS c = new Win32_BIOS(wmiConnection);
            Win32_Bus d = new Win32_Bus(wmiConnection);
            Win32_CDROMDrive e = new Win32_CDROMDrive(wmiConnection);
            Win32_DiskDrive f = new Win32_DiskDrive(wmiConnection);
            Win32_DMAChannel g = new Win32_DMAChannel(wmiConnection);
            Win32_Fan h = new Win32_Fan(wmiConnection);
            Win32_FloppyController i = new Win32_FloppyController(wmiConnection);
            Win32_FloppyDrive j = new Win32_FloppyDrive(wmiConnection);
            Win32_IDEController k = new Win32_IDEController(wmiConnection);
            Win32_IRQResource l = new Win32_IRQResource(wmiConnection);
            Win32_Keyboard m = new Win32_Keyboard(wmiConnection);
            Win32_MemoryDevice n = new Win32_MemoryDevice(wmiConnection);
            Win32_NetworkAdapter o = new Win32_NetworkAdapter(wmiConnection);
            Win32_NetworkAdapterConfiguration p = new Win32_NetworkAdapterConfiguration(wmiConnection);
            Win32_OnBoardDevice q = new Win32_OnBoardDevice(wmiConnection);
            Win32_ParallelPort r = new Win32_ParallelPort(wmiConnection);
            Win32_PCMCIController s = new Win32_PCMCIController(wmiConnection);
            Win32_PhysicalMedia t = new Win32_PhysicalMedia(wmiConnection);
            Win32_PhysicalMemory u = new Win32_PhysicalMemory(wmiConnection);
            Win32_PortConnector v = new Win32_PortConnector(wmiConnection);
            Win32_PortResource w = new Win32_PortResource(wmiConnection);
            Win32_POTSModem x = new Win32_POTSModem(wmiConnection);
            Win32_Processor y = new Win32_Processor(wmiConnection);
            Win32_SCSIController z = new Win32_SCSIController(wmiConnection);
            Win32_SerialPort aa = new Win32_SerialPort(wmiConnection);
            Win32_SerialPortConfiguration bb = new Win32_SerialPortConfiguration(wmiConnection);
            Win32_SoundDevice cc = new Win32_SoundDevice(wmiConnection);
            Win32_SystemEnclosure dd = new Win32_SystemEnclosure(wmiConnection);
            Win32_TapeDrive ee = new Win32_TapeDrive(wmiConnection);
            Win32_TemperatureProbe ff = new Win32_TemperatureProbe(wmiConnection);
            Win32_UninterruptiblePowerSupply gg = new Win32_UninterruptiblePowerSupply(wmiConnection);
            Win32_USBController hh = new Win32_USBController(wmiConnection);
            Win32_USBHub ii = new Win32_USBHub(wmiConnection);
            Win32_VideoController jj = new Win32_VideoController(wmiConnection);
            Win32_VoltageProbe kk = new Win32_VoltageProbe(wmiConnection);

            //Loop all the properties
            Console.WriteLine("");
            Console.WriteLine("------| " + a.GetType().ToString() + " |------");
            foreach (string property in a.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + b.GetType().ToString() + " |------");
            foreach (string property in b.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + c.GetType().ToString() + " |------");
            foreach (string property in c.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + d.GetType().ToString() + " |------");
            foreach (string property in d.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + e.GetType().ToString() + " |------");
            foreach (string property in e.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + f.GetType().ToString() + " |------");
            foreach (string property in f.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + g.GetType().ToString() + " |------");
            foreach (string property in g.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + h.GetType().ToString() + " |------");
            foreach (string property in h.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + i.GetType().ToString() + " |------");
            foreach (string property in i.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + j.GetType().ToString() + " |------");
            foreach (string property in j.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + k.GetType().ToString() + " |------");
            foreach (string property in k.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + l.GetType().ToString() + " |------");
            foreach (string property in l.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("------| " + m.GetType().ToString() + " |------");
            foreach (string property in m.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + n.GetType().ToString() + " |------");
            foreach (string property in n.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + o.GetType().ToString() + " |------");
            foreach (string property in o.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + p.GetType().ToString() + " |------");
            foreach (string property in p.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + q.GetType().ToString() + " |------");
            foreach (string property in q.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + r.GetType().ToString() + " |------");
            foreach (string property in r.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + s.GetType().ToString() + " |------");
            foreach (string property in s.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + t.GetType().ToString() + " |------");
            foreach (string property in t.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + u.GetType().ToString() + " |------");
            foreach (string property in u.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + v.GetType().ToString() + " |------");
            foreach (string property in v.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + w.GetType().ToString() + " |------");
            foreach (string property in w.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + x.GetType().ToString() + " |------");
            foreach (string property in x.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + y.GetType().ToString() + " |------");
            foreach (string property in y.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + z.GetType().ToString() + " |------");
            foreach (string property in z.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + aa.GetType().ToString() + " |------");
            foreach (string property in aa.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + bb.GetType().ToString() + " |------");
            foreach (string property in bb.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + cc.GetType().ToString() + " |------");
            foreach (string property in cc.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + dd.GetType().ToString() + " |------");
            foreach (string property in dd.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + ee.GetType().ToString() + " |------");
            foreach (string property in ee.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + ff.GetType().ToString() + " |------");
            foreach (string property in ff.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + gg.GetType().ToString() + " |------");
            foreach (string property in gg.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + hh.GetType().ToString() + " |------");
            foreach (string property in hh.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + ii.GetType().ToString() + " |------");
            foreach (string property in ii.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + jj.GetType().ToString() + " |------");
            foreach (string property in jj.GetPropertyValues())
            {
                Console.WriteLine(property);
            }
            Console.WriteLine("");
            Console.WriteLine("------| " + kk.GetType().ToString() + " |------");
            foreach (string property in kk.GetPropertyValues())
            {
                Console.WriteLine(property);
            }

            Console.ReadLine();
        }
    }
}
