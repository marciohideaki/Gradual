using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library
{
    public class ScheduleItem
    {
        [XmlAttribute("value")]
        public string value { get; set; }
    }

    public class ScheduleConfig
    {
        [XmlElement("ScheduleItem")]
        public List<ScheduleItem> ScheduleItem { get; set; }
    }
}
