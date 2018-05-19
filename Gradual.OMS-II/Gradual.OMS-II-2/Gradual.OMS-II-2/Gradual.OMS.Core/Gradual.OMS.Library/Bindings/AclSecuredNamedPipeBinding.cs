// AclSecuredNamedPipeBinding
// Create a pipe with security enhanced
// 2008 (C) by Chris Dickson
// http://blogs.charteris.com/blogs/chrisdi/archive/2008/06/23/exploring-the-wcf-named-pipe-binding-part-1.aspx

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace Gradual.OMS.Library.Bindings
{
    public class AclSecuredNamedPipeBinding : CustomBinding
    {
        public AclSecuredNamedPipeBinding(): base()
        {
            NetNamedPipeBinding standardBinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);

            foreach (BindingElement element in standardBinding.CreateBindingElements())
            {
                NamedPipeTransportBindingElement transportElement = element as NamedPipeTransportBindingElement;
                base.Elements.Add( null != transportElement ? new AclSecuredNamedPipeTransportBindingElement(transportElement) : element);
            }

            AddUserOrGroup(WindowsIdentity.GetCurrent().User);
        }

        public void AddUserOrGroup(SecurityIdentifier sid)
        {
            List<SecurityIdentifier> allowedUsers
                = Elements.Find<AclSecuredNamedPipeTransportBindingElement>().AllowedUsers;
            if (!allowedUsers.Contains(sid))
            {
                allowedUsers.Add(sid);
            }
        }
    }

    public class AclSecuredNamedPipeTransportBindingElement : NamedPipeTransportBindingElement
    {
        private static Type namedPipeChannelListenerType
            = Type.GetType("System.ServiceModel.Channels.NamedPipeChannelListener, System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false);

        internal List<SecurityIdentifier> AllowedUsers { get { return _allowedUsers; } }
        private List<SecurityIdentifier> _allowedUsers = new List<SecurityIdentifier>();

        public AclSecuredNamedPipeTransportBindingElement(NamedPipeTransportBindingElement inner): base(inner)
        {
            if (inner is AclSecuredNamedPipeTransportBindingElement)
            {
                _allowedUsers = new List<SecurityIdentifier>(((AclSecuredNamedPipeTransportBindingElement)inner)._allowedUsers);
            }
        }

        public override BindingElement Clone()
        {
            return new AclSecuredNamedPipeTransportBindingElement(this);
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            IChannelListener<TChannel> listener = base.BuildChannelListener<TChannel>(context);
            PropertyInfo p = namedPipeChannelListenerType.GetProperty("AllowedUsers", BindingFlags.Instance|BindingFlags.NonPublic);
            p.SetValue(listener, _allowedUsers, null);
            return listener;
        }
    }
} 