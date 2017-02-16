﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;


// TODO: THIS IS AN OLD CLASS.  GET A "CLEAN" VERSION OF THIS THAT DOES NOT HAVE VARIOUS NAMING CONVENTIONS BASED ON LEGACY ISSUES.
namespace Quest.Util.Buffer
{
    public static class BufferMgr
    {
        #region Public Methods
        //================================================================================
        // Public Methods
        //================================================================================
        public static void InitializeReferenceTypes(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (pi.PropertyType.IsValueType)
                {
                    continue;
                }
                // TODO: ALL TYPES...FOR NOW, WHAT MATTERS.
                if (pi.PropertyType.FullName == "System.String")
                {
                    string _s = "";
                    pi.SetValue(obj, _s);
                }
            }
        }


        #region Static Public Methods
        //--------------------------------------------------------------------------------
        // Static Public Methods
        //--------------------------------------------------------------------------------
        public static void TransferBufferAndHTMLEncode(object source, object destination)
        {
            TransferBuffer(source, destination);
            if (destination.GetType().GetProperties().Length > 0)
            {
                propertyEncode(destination);
            }
            else
            {
                fieldEncode(destination);
            }
        }
        public static void TransferBufferAndHTMLDecode(object source, object destination)
        {
            TransferBuffer(source, destination);
            if (destination.GetType().GetProperties().Length > 0)
            {
                propertyDecode(destination);
            }
            else
            {
                fieldDecode(destination);
            }
        }
        public static void TransferBuffer(object source, object destination)
        {
            TransferBuffer(source, destination, false);
        }
        public static void TransferBufferCaseInsensitive(object source, object destination)
        {
            TransferBufferCaseInsensitive(source, destination, false);
        }
        public static void TransferBuffer(object source, object destination, bool bIgnoreExceptions)
        {
            PropertyInfo[] piSource = source.GetType().GetProperties();
            if (piSource.Length > 0)
            {
                propertyTransfer(piSource, source, destination, bIgnoreExceptions);
            }
            else
            {
                FieldInfo[] fiSource = source.GetType().GetFields();
                fieldTransfer(fiSource, source, destination);
            }
        }
        public static void TransferBufferCaseInsensitive(object source, object destination, bool bIgnoreExceptions)
        {
            PropertyInfo[] piSource = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (piSource.Length > 0)
            {
                propertyTransferCaseInsensitive(piSource, source, destination, bIgnoreExceptions);
            }
            else
            {
                FieldInfo[] fiSource = source.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);
                fieldTransferCaseInsensitive(fiSource, source, destination);
            }
        }
        #endregion


        #region Instance Public Methods
        //--------------------------------------------------------------------------------
        // Instance Public Methods
        //--------------------------------------------------------------------------------
        public static void DecodeProperties(object buffer, string fieldName)
        {

        }
        public static object GetPropertyValue(object buffer, string fieldName)
        {
            return (GetPropertyValue(buffer, fieldName, null));
        }
        public static object GetPropertyValue(object buffer, string fieldName, Type propertyType)
        {
            PropertyInfo pi = buffer.GetType().GetProperty(fieldName);
            object objPropertyValue = null;
            if (pi != null)
            {
                // If propertyType specified, it must be that type.
                if (propertyType != null)
                {
                    if (pi.PropertyType != propertyType)
                    {
                        return (null);
                    }
                }

                // Get value of property.
                try
                {
                    objPropertyValue = pi.GetValue(buffer, null);
                }
                catch (System.Exception ex)
                {
                    throw new ApplicationException(String.Format("BufferMgr.GetPropertyValue: error getting property {0} for recordInstance type {1}: {2}",
                            fieldName, buffer.GetType(), ex.ToString()));
                }
            }
            return (objPropertyValue);
        }
        public static object GetFieldValue(object buffer, string fieldName)
        {
            return (GetFieldValue(buffer, fieldName, null));
        }
        public static object GetFieldValue(object buffer, string fieldName, Type fieldType)
        {
            FieldInfo fi = buffer.GetType().GetField(fieldName);
            object objFieldValue = null;
            if (fi != null)
            {
                // If fieldType specified, fi must be that type.
                if (fieldType != null)
                {
                    if (fi.FieldType != fieldType)
                    {
                        return (null);
                    }
                }

                // Get value of field.
                try
                {
                    objFieldValue = fi.GetValue(buffer);
                }
                catch (System.Exception ex)
                {
                    throw new ApplicationException(String.Format("BufferMgr.GetFieldValue: error getting field {0} for recordInstance type {1}: {2}",
                            fieldName, buffer.GetType(), ex.ToString()));
                }
            }
            return (objFieldValue);
        }
        public static bool IsValueSpecified(object objValue, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsValueType)
            {
                if (bIsNullable(propertyInfo.PropertyType))
                {
                    return (objValue != null);
                }
                else
                {
                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        return (true);  // Always has a value, true or false.
                    }
                    else if (propertyInfo.PropertyType == typeof(Int32))
                    {
                        return ((Int32)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(UInt32))
                    {
                        return ((UInt32)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime))
                    {
                        return ((DateTime)objValue != DateTime.MinValue);
                    }
                    else if (propertyInfo.PropertyType == typeof(byte))
                    {
                        return ((byte)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(sbyte))
                    {
                        return ((sbyte)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(Char))
                    {
                        return ((Char)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(Int64))
                    {
                        return ((Int64)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(UInt64))
                    {
                        return ((UInt64)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(Int16))
                    {
                        return ((Int16)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(UInt16))
                    {
                        return ((UInt16)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(Double))
                    {
                        return ((Double)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(Single))
                    {
                        return ((Single)objValue != 0);
                    }
                    else if (propertyInfo.PropertyType == typeof(Guid))
                    {
                        return ((Guid)objValue != Guid.Empty);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("BufferMgr.bValueSpecified: Unsupported property value type: {0}",
                                objValue.GetType()));
                    }
                }
            }
            else  // Reference types.
            {
                return (objValue != null);
            }
        }
        public static bool IsValueSpecified(object objValue, FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType.IsValueType)
            {
                if (bIsNullable(fieldInfo.FieldType))
                {
                    return (objValue != null);
                }
                else
                {
                    if (fieldInfo.FieldType == typeof(bool))
                    {
                        return (true);  // Always has a value, true or false.
                    }
                    else if (fieldInfo.FieldType == typeof(Int32))
                    {
                        return ((Int32)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(UInt32))
                    {
                        return ((UInt32)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(DateTime))
                    {
                        return ((DateTime)objValue != DateTime.MinValue);
                    }
                    else if (fieldInfo.FieldType == typeof(byte))
                    {
                        return ((byte)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(sbyte))
                    {
                        return ((sbyte)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(Char))
                    {
                        return ((Char)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(Int64))
                    {
                        return ((Int64)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(UInt64))
                    {
                        return ((UInt64)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(Int16))
                    {
                        return ((Int16)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(UInt16))
                    {
                        return ((UInt16)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(Double))
                    {
                        return ((Double)objValue != 0);
                    }
                    else if (fieldInfo.FieldType == typeof(Single))
                    {
                        return ((Single)objValue != 0);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("BufferMgr.bValueSpecified: Unsupported field value type: {0}",
                                objValue.GetType()));
                    }
                }
            }
            else  // Reference types.
            {
                return (objValue != null);
            }
        }
        public static bool bIsNullable(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        public static void TrimRequiredFields(object source)
        {
            string className = source.GetType().Name;
            PropertyInfo[] piSource = source.GetType().GetProperties();
            foreach (PropertyInfo piS in piSource)
            {
                string fieldType = piS.PropertyType.Name;

                if (piS.PropertyType.FullName == "System.String")
                {
                    string sValue = (string)piS.GetValue(source, null);
                    if (sValue != null)
                    {
                        try
                        {
                            piS.SetValue(source, sValue.Trim());
                        }
                        catch (System.Exception ex)
                        {
                            throw new ApplicationException(String.Format("BufferMgr.TrimRequiredFields: error trimming field {0}.{1}: {2}",
                                   className, piS.Name, ex.ToString()));
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        private static void propertyEncode(object destination)
        {
            PropertyInfo[] piDestination = destination.GetType().GetProperties();
            foreach (PropertyInfo piD in piDestination)
            {
                if (piD.PropertyType == typeof(System.String))
                {
                    object o = piD.GetValue(destination, null);
                    piD.SetValue(destination, (object)HttpUtility.HtmlEncode((string)o), null);
                }
            }
        }
        private static void propertyDecode(object destination)
        {
            PropertyInfo[] piDestination = destination.GetType().GetProperties();
            foreach (PropertyInfo piD in piDestination)
            {
                if (piD.PropertyType == typeof(System.String))
                {
                    object o = piD.GetValue(destination, null);
                    piD.SetValue(destination, (object)HttpUtility.HtmlDecode((string)o), null);
                }
            }
        }
        private static void fieldEncode(object destination)
        {
            FieldInfo[] fiDestination = destination.GetType().GetFields();
            foreach (FieldInfo fiD in fiDestination)
            {
                if (fiD.GetType() == typeof(System.String))
                {
                    object o = fiD.GetValue(destination);
                    fiD.SetValue(destination, (object)HttpUtility.HtmlEncode((string)o));
                }
            }
        }
        private static void fieldDecode(object destination)
        {
            FieldInfo[] fiDestination = destination.GetType().GetFields();
            foreach (FieldInfo fiD in fiDestination)
            {
                if (fiD.GetType() == typeof(System.String))
                {
                    object o = fiD.GetValue(destination);
                    fiD.SetValue(destination, (object)HttpUtility.HtmlDecode((string)o));
                }
            }
        }
        private static void propertyTransfer(PropertyInfo[] piSource, object source, object destination, bool bIgnoreExceptions = false)
        {
            foreach (PropertyInfo piS in piSource)
            {
                PropertyInfo piD = destination.GetType().GetProperty(piS.Name);
                if (piD == null)
                {
                    FieldInfo fi = destination.GetType().GetField(piS.Name);
                    if (fi != null)
                    {
                        try
                        {
                            fi.SetValue(destination, piS.GetValue(source, null));
                        }
                        catch (System.Exception ex)
                        {
                            throw new ApplicationException(String.Format("BufferMgr: error setting field {0} with {1}: {2}",
                                    fi.Name, piS.Name, ex.ToString()));
                        }
                    }
                    // else - support for nested types.
                }
                else
                {
                    try
                    {
                        piD.SetValue(destination, piS.GetValue(source, null), null);
                    }
                    catch (System.Exception ex)
                    {
                        // Some typical cases of different data types still being default buffer transfers.
                        if (piS.PropertyType.FullName == "System.String")
                        {
                            if (piD.PropertyType.FullName == "System.DateTime")
                            {
                                string _dateTime = piS.GetValue(source, null) == null ? null : piS.GetValue(source, null).ToString();
                                if (string.IsNullOrEmpty(_dateTime))
                                {
                                    _dateTime = DateTime.MinValue.ToString();
                                }
                                try
                                {
                                    DateTime dateTime = Convert.ToDateTime(_dateTime);
                                    piD.SetValue(destination, dateTime, null);
                                }
                                catch (System.Exception)
                                {
                                    if (! bIgnoreExceptions)
                                    {
                                        throw;
                                    }
                                }
                            }
                            else if (piD.PropertyType.FullName == "System.Decimal")
                            {
                                string _stringDecimal = piS.GetValue(source, null).ToString().Replace("%", "").Replace("$", "").Replace(",", "").Trim();
                                decimal _decimal = Decimal.Parse(_stringDecimal,
                                      NumberStyles.None |
                                      NumberStyles.AllowCurrencySymbol |
                                      NumberStyles.AllowDecimalPoint | 
                                      NumberStyles.AllowThousands);
                                piD.SetValue(destination, _decimal, null);
                            }
                            else if (piD.PropertyType.FullName == "System.Int32")
                            {
                                try
                                {
                                    int _int = Convert.ToInt32(piS.GetValue(source, null).ToString().Replace("%", "").Replace("$", "").Replace(",", ""));
                                    piD.SetValue(destination, _int, null);
                                }
                                catch (System.Exception e)
                                {
                                    if (!bIgnoreExceptions)
                                    {
                                        throw e;
                                    }
                                }
                            }
                            else if (piD.PropertyType.FullName == "System.Int16")
                            {
                                try
                                {
                                    short _short = Convert.ToInt16(piS.GetValue(source, null));
                                    piD.SetValue(destination, _short, null);
                                }
                                catch (System.Exception e)
                                {
                                    if (!bIgnoreExceptions)
                                    {
                                        throw e;
                                    }
                                }
                            }
                            else if (piD.PropertyType.Name == "Nullable`1")
                            {
                                try
                                {
                                    if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Decimal")
                                    {
                                        decimal? _decimal = null;
                                        if (piS.GetValue(source, null) != null)
                                        {
                                            _decimal = Convert.ToDecimal(piS.GetValue(source, null).ToString().Replace("$", "").Replace("%", "").Replace(",", ""));
                                        }
                                        piD.SetValue(destination, _decimal);
                                    }
                                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Int32")
                                    {
                                        Int32? _int = null;
                                        if (piS.GetValue(source, null) != null)
                                        {
                                            _int = Convert.ToInt32(piS.GetValue(source, null).ToString().Replace("$", "").Replace("%", "").Replace(",", ""));
                                        }
                                        piD.SetValue(destination, _int);
                                    }
                                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.DateTime")
                                    {
                                        DateTime? _dateTime = null;
                                        if (piS.GetValue(source, null) != null)
                                        {
                                            _dateTime = Convert.ToDateTime(piS.GetValue(source, null));
                                        }
                                        piD.SetValue(destination, _dateTime);
                                    }
                                }
                                catch (System.Exception e)
                                {
                                    if (!bIgnoreExceptions)
                                    {
                                        throw e;
                                    }
                                }
                            }
                            else
                            {
                                if (!bIgnoreExceptions)
                                {
                                    throw new ApplicationException(String.Format("BufferMgr: error setting property {0} with {1} after trying expected implicit conversion: {2}",
                                            piD.Name, piS.Name, ex.ToString()));
                                }
                            }
                        }
                        else
                        {
                            if (piD.PropertyType.FullName == "System.String")
                            {
                                try
                                {
                                    string _string = null;
                                    if (piS.GetValue(source, null) != null)
                                    {
                                        _string = piS.GetValue(source, null).ToString();
                                    }
                                    piD.SetValue(destination, _string);
                                }
                                catch (System.Exception e)
                                {
                                    if (!bIgnoreExceptions)
                                    {
                                        throw e;
                                    }
                                }
                            }
                            else if (!bIgnoreExceptions)
                            {
                                if (!bIgnoreExceptions)
                                {
                                    throw new ApplicationException(String.Format("BufferMgr: error setting property {0} with {1}: {2}",
                                            piD.Name, piS.Name, ex.ToString()));
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void propertyTransferCaseInsensitive(PropertyInfo[] piSource, object source, object destination, bool bIgnoreExceptions = false)
        {
            foreach (PropertyInfo piS in piSource)
            {
                PropertyInfo piD = destination.GetType().GetProperty(piS.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);
                if (piD == null)
                {
                    FieldInfo fi = destination.GetType().GetField(piS.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);
                    if (fi != null)
                    {
                        try
                        {
                            fi.SetValue(destination, piS.GetValue(source, null));
                        }
                        catch (System.Exception ex)
                        {
                            throw new ApplicationException(String.Format("BufferMgr: error setting field {0} with {1}: {2}",
                                    fi.Name, piS.Name, ex.ToString()));
                        }
                    }
                    // else - support for nested types.
                }
                else
                {
                    try
                    {
                        piD.SetValue(destination, piS.GetValue(source, null), null);
                    }
                    catch (System.Exception ex)
                    {
                        if (!bIgnoreExceptions)
                        {
                            throw new ApplicationException(String.Format("BufferMgr: error setting property {0} with {1}: {2}",
                                    piD.Name, piS.Name, ex.ToString()));
                        }
                    }
                }
            }
        }
        private static void fieldTransfer(FieldInfo[] fiSource, object source, object destination)
        {
            foreach (FieldInfo fiS in fiSource)
            {
                PropertyInfo piD = destination.GetType().GetProperty(fiS.Name);
                if (piD == null)
                {
                    FieldInfo fi = destination.GetType().GetField(fiS.Name);
                    if (fi != null)
                    {
                        fi.SetValue(destination, fiS.GetValue(source));
                    }
                }
                else
                {
                    piD.SetValue(destination, fiS.GetValue(source), null);
                }
            }
        }
        private static void fieldTransferCaseInsensitive(FieldInfo[] fiSource, object source, object destination)
        {
            foreach (FieldInfo fiS in fiSource)
            {
                PropertyInfo piD = destination.GetType().GetProperty(fiS.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);
                if (piD == null)
                {
                    FieldInfo fi = destination.GetType().GetField(fiS.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);
                    if (fi != null)
                    {
                        fi.SetValue(destination, fiS.GetValue(source));
                    }
                }
                else
                {
                    piD.SetValue(destination, fiS.GetValue(source), null);
                }
            }
        }
    }
}
