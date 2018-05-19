This is a simple tool where you can cut and paste the MSDN WMI SDK properties into the textbox and it will reformat it so you can paste right into the settings.xml file. 

so if you were looking at the Win32_PortResource Class
http://msdn2.microsoft.com/en-us/library/aa394359.aspx

Then you cut this from the site:
boolean Alias;
  string Caption;
  string CreationClassName;
  string CSCreationClassName;
  string CSName;
  string Description;
  uint64 EndingAddress;
  datetime InstallDate;
  string Name;
  uint64 StartingAddress;
  string Status;

Paste it into the formatter and you get back:
<property>Alias</property>
<property>Caption</property>
<property>CreationClassName</property>
<property>CSCreationClassName</property>
<property>CSName</property>
<property>Description</property>
<property>EndingAddress</property>
<property>InstallDate</property>
<property>Name</property>
<property>StartingAddress</property>
<property>Status</property>

Which you can paste directly into the xml file after creating a new section called

<Win32_PortResource></Win32_PortResource>