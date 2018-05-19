/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>

*/
namespace OpenFAST.Session.Multicast
{
	public sealed class MulticastConnection : Connection
	{
        System.IO.StreamReader in_stream;

        public System.IO.StreamReader InputStream
		{
			get
			{
                return in_stream;
			}
			
		}
		public System.IO.StreamWriter OutputStream
		{
			get
			{
				throw new System.NotSupportedException("Multicast sending not currently supported.");
			}
			
		}
		private System.Net.Sockets.UdpClient socket;
		private System.Net.IPAddress group;
		
		public MulticastConnection(System.Net.Sockets.UdpClient socket, System.Net.IPAddress group)
		{
			this.socket = socket;
			this.group = group;
            in_stream = new System.IO.StreamReader(new MulticastInputStream(socket));
		}
		
		public void  Close()
		{
			try
			{
				socket.DropMulticastGroup((System.Net.IPAddress) group);
				socket.Close();
			}
			catch (System.IO.IOException)
			{
			}
		}
	}
}