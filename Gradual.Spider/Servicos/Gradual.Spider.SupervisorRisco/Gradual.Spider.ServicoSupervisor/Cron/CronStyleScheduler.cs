using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gradual.OMS.Library;
using log4net;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
// using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;

namespace Gradual.Spider.ServicoSupervisor.Cron
{
    public class CronStyleScheduler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ArrayList crontab;
        private ArrayList processes;
        private bool _bKeepRunning = false;
        private Thread _me = null;
        private List<string> _cronlines = new List<string>();

        private int lastMinute;

        public void Start()
        {
            _bKeepRunning = true;

            _me = new Thread(new ThreadStart(_cronproc));
            _me.Name = "_cronproc";
            _me.Start();
        }

        public void AddCronLine(string cronentry)
        {
            try
            {
                logger.Debug("Adding cron line [" + cronentry.Trim() + "]");
                _cronlines.Add(cronentry.Trim());
            }
            catch (Exception ex)
            {
                logger.Error("AddCronLine() " + ex.Message, ex);
            }
        }


        private void _cronproc()
        {
            DateTime now;
            //lastMinute = DateTime.Now.Minute - 1;
            lastMinute = DateTime.Now.Minute;

            crontab = new ArrayList();
            processes = new ArrayList();

            DateTime lastCheck = DateTime.Now;


            try
            {
                readCrontab();

                while (_bKeepRunning)
                {
                    TimeSpan lastInterval = new TimeSpan(DateTime.Now.Ticks - lastCheck.Ticks);

                    // half a minute
                    if (lastInterval.TotalMilliseconds > 30000)
                    {
                        lastCheck = DateTime.Now;
                        now = DateTime.Now;

                        checkProcesses(now);
                        doCrontab(now);
                    }

                    Thread.Sleep(250);
                }
            }
            catch (Exception e)
            {
                logger.Error("start(): " + e.Message, e);
            }
        }

        public void Stop()
        {
            _bKeepRunning = false;

            while (_me.IsAlive)
            {
                Thread.Sleep(250);
            }
        }

        public void checkProcesses(DateTime now)
        {
            ArrayList toRemove = new ArrayList();

            for (int i = 0; i < processes.Count; i++)
            {
                Process proc = (Process)processes[i];

                if (proc.HasExited)
                {
                    toRemove.Add(proc);

                    if (proc.ExitCode != 0)
                    {
                        reportError("The process " + proc.StartInfo.FileName + " " +
                            proc.StartInfo.Arguments + " returned with error " + proc.ExitCode.ToString());
                    }
                }
                else if (DateTime.Compare(proc.StartTime, DateTime.Now.Subtract(new System.TimeSpan(0, 20, 0))) < 0)
                {
                    reportError(proc.StartInfo.FileName + " takes longer than 20 minutes and will be killed.");
                    proc.Kill();
                }
            }

            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                processes.Remove(toRemove[i]);
            }
        }

        public void doCrontab(DateTime now)
        {
            if (now.Minute.Equals(lastMinute))
                return;

            // for loop: deal with the highly unexpected eventuality of
            // having lost more than one minute to unavailable processor time
            for (int minute = (lastMinute == 59 ? 0 : lastMinute + 1); minute <= now.Minute; minute++)
            {
                int sunda = 0;
                foreach (ArrayList entry in crontab)
                {
                    if (contains(entry[0], now.Month) &&
                        contains(entry[1], getMDay(now)) &&
                        contains(entry[2], getWDay(now)) &&
                        contains(entry[3], now.Hour) &&
                        contains(entry[4], now.Minute))
                    {
                        // yay, we get to execute something!
                        string command = (String)entry[5];

                        string args = (string)entry[6];
                        string[] arr = null;
                        int qtdeParametros = 0;

                        if (args != null && args.Length > 0)
                        {
                            arr = args.Split(' ');
                            qtdeParametros = arr.Length;
                        }

                        object[] parameters = null;

                        if (qtdeParametros > 0)
                        {
                            parameters = new object[qtdeParametros];
                            for (int i = 0; i < qtdeParametros; i++)
                            {
                                parameters[i] = arr[i];
                                logger.Debug("parameters[" + i + "]: [" + parameters[i] + "]");
                            }
                        }

                        logger.Debug("Executing entry [" + sunda + "] [" + command + "] with [" + qtdeParametros + "] parameters");

                        logger.Info("Calling via reflection [" + command + "]");
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(
                             delegate(object required)
                             {
                                 InvocarMetodo(command, parameters);
                             }));
                        }

                    }
                    sunda++;
                }
            }

            lastMinute = now.Minute;
        }

        // sort of a macro to keep the if-statement above readable
        private bool contains(Object list, int val)
        {
            // -1 represents the star * from the crontab
            return ((ArrayList)list).Contains(val) || ((ArrayList)list).Contains(-1);
        }

        private int getMDay(DateTime date)
        {
            date.AddMonths(-(date.Month - 1));
            return date.DayOfYear;
        }

        private int getWDay(DateTime date)
        {
            if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                return 7;
            else
                return (int)date.DayOfWeek;
        }

        private void readCrontab()
        {
            ScheduleConfig _config = GerenciadorConfig.ReceberConfig<ScheduleConfig>();

            try
            {
                foreach (ScheduleItem item in _config.ScheduleItem)
                {
                    _cronlines.Add(item.value.Trim());
                    logger.Debug("Adding ScheduleItem [" + item.value.Trim() + "]");
                }
                String line;

                foreach (string linha in _cronlines)
                {
                    ArrayList minutes, hours, mDays, months, wDays;

                    line = linha;

                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    // re-escape space- and backslash-escapes in a cheap fashion
                    line = line.Replace("\\\\", "<BACKSLASH>");
                    line = line.Replace("\\ ", "<SPACE>");

                    // split string on whitespace
                    String[] cols = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < cols.Length; i++)
                    {
                        cols[i] = cols[i].Replace("<BACKSLASH>", "\\");
                        cols[i] = cols[i].Replace("<SPACE>", " ");
                    }

                    if (cols.Length < 6)
                    {
                        reportError("Parse error in crontab (line too short).");
                        crontab = new ArrayList();
                    }

                    minutes = parseTimes(cols[0], 0, 59);
                    hours = parseTimes(cols[1], 0, 23);
                    months = parseTimes(cols[3], 1, 12);

                    if (!cols[2].Equals("*") && cols[3].Equals("*"))
                    {
                        // every n monthdays, disregarding weekdays
                        mDays = parseTimes(cols[2], 1, 31);
                        wDays = new ArrayList();
                        wDays.Add(-1); // empty value
                    }
                    else if (cols[2].Equals("*") && !cols[3].Equals("*"))
                    {
                        // every n weekdays, disregarding monthdays
                        mDays = new ArrayList();
                        mDays.Add(-1); // empty value
                        wDays = parseTimes(cols[4], 1, 7); // 60 * 24 * 7
                    }
                    else
                    {
                        // every n weekdays, every m monthdays
                        mDays = parseTimes(cols[2], 1, 31);
                        wDays = parseTimes(cols[4], 1, 7); // 60 * 24 * 7
                    }

                    String args = "";

                    for (int i = 6; i < cols.Length; i++)
                        args += cols[i].Trim() + " ";

                    args = args.Trim();


                    // Prestar atencao aqui....inicializa com 6
                    ArrayList entry = new ArrayList(6);

                    entry.Add(months);
                    entry.Add(mDays);
                    entry.Add(wDays);
                    entry.Add(hours);
                    entry.Add(minutes);
                    entry.Add(cols[5]);
                    entry.Add(args);

                    crontab.Add(entry);

                    logger.Info("Command [" + cols[5] + "] scheduled to run");
                }
            }
            catch (Exception e)
            {
                reportError(e.ToString());
            }
        }

        public ArrayList parseTimes(String line, int startNr, int maxNr)
        {
            ArrayList vals = new ArrayList();
            String[] list, parts;

            list = line.Split(new char[] { ',' });

            foreach (String entry in list)
            {
                int start, end, interval;

                parts = entry.Split(new char[] { '-', '/' });

                if (parts[0].Equals("*"))
                {
                    if (parts.Length > 1)
                    {
                        start = startNr;
                        end = maxNr;

                        interval = int.Parse(parts[1]);
                    }
                    else
                    {
                        // put a -1 in place
                        start = -1;
                        end = -1;
                        interval = 1;
                    }
                }
                else
                {
                    // format is 0-8/2
                    start = int.Parse(parts[0]);
                    end = parts.Length > 1 ? int.Parse(parts[1]) : int.Parse(parts[0]);
                    interval = parts.Length > 2 ? int.Parse(parts[2]) : 1;
                }

                for (int i = start; i <= end; i += interval)
                {
                    vals.Add(i);
                }
            }
            return vals;
        }

        public void reportError(String error)
        {
            // Error reporting is left up to you; this is a case apart
            // (besides, my implementation was too specific to post here)
            logger.Error(error);
        }


        //public void Test(string methodName)
        //{
        //    Assembly assembly = Assembly.LoadFile("...Assembly1.dll");
        //    Type type = assembly.GetType("TestAssembly.Main");
        //    if (type != null)
        //    {
        //        MethodInfo methodInfo = type.GetMethod(methodName);
        //        if (methodInfo != null)
        //        {
        //            object result = null;
        //            ParameterInfo[] parameters = methodInfo.GetParameters();
        //            object classInstance = Activator.CreateInstance(type, null);
        //            if (parameters.Length == 0)
        //            {
        //                //This works fine
        //                result = methodInfo.Invoke(classInstance, null);
        //            }
        //            else
        //            {
        //                object[] parametersArray = new object[] { "Hello" };

        //                //The invoke does NOT work it throws "Object does not match target type"             
        //                result = methodInfo.Invoke(methodInfo, parametersArray);
        //            }
        //        }
        //    }
        //}


        public void InvocarMetodo(string classmethod, params object[] parameters)
        {
            if (classmethod.IndexOf('.') <= 0)
            {
                logger.Error("Invalid argument classmethod: [" + classmethod + "]");
                throw new ArgumentException("First parameter must be <classname.methodname>");
            }

            string[] strarr = classmethod.Split('.');

            string classname = string.Empty;
            for (int i = 0; i < strarr.Length - 1; i++)
            {
                classname = classname + strarr[i] + ".";
            }
            classname = classname.Substring(0, classname.Length - 1);
            //string classname = strarr[0];
            string methodname = strarr[strarr.Length - 1];

            var macroClasses = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.Name.Equals(classname)));

            foreach (var tempClass in macroClasses)
            {
                try
                {
                    object curInstance = null;
                    MethodInfo getinstance = tempClass.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);

                    if (getinstance != null)
                    {
                        curInstance = getinstance.Invoke(null, null);
                    }
                    else
                    {
                        curInstance = Activator.CreateInstance(tempClass);
                    }

                    tempClass.GetMethod(methodname).Invoke(curInstance, parameters);
                }
                catch (Exception ex)
                {
                    logger.Error("InvocarMetodo:" + ex.Message, ex);
                }
            }
        }
    }
}
