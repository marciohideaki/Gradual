﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;

namespace Gradual.OMS.MarketData
{
    public class MessageComponent:QuickFix.FieldMap
    {
        protected abstract int[] getFields();
        protected abstract int[] getGroupFields();

        protected MessageComponent(): base() {}

        public void copyFrom(FieldMap fields)
        {
            try
            {
                int[] componentFields = getFields();
                for (int i = 0; i < componentFields.Length; i++)
                {
                    if (fields.isSetField(componentFields[i]))
                    {
                        setField(componentFields[i], fields.getField(componentFields[i]));
                    }

                }
                int[] groupFields = getGroupFields();
                for (int i = 0; i < groupFields.Length; i++)
                {
                    if (fields.isSetField(groupFields[i]))
                    {
                        setField(groupFields[i], fields.getField(groupFields[i]));
                        setGroups(groupFields[i], fields.getGroups(groupFields[i]));
                    }
                }
            }
            catch (FieldNotFound e)
            {
                // should not happen
            }
        }

        public void copyTo(FieldMap fields)
        {
            try
            {
                int[] componentFields = getFields();
                for (int i = 0; i < componentFields.length; i++)
                {
                    if (isSetField(componentFields[i]))
                    {
                        fields.setField(componentFields[i], getField(componentFields[i]));
                    }
                }
                int[] groupFields = getGroupFields();
                for (int i = 0; i < groupFields.length; i++)
                {
                    if (isSetField(groupFields[i]))
                    {
                        fields.setField(groupFields[i], getField(groupFields[i]));
                        fields.setGroups(groupFields[i], getGroups(groupFields[i]));
                    }
                }
            }
            catch (FieldNotFound e)
            {
                // should not happen
            }

        }
    }
}
