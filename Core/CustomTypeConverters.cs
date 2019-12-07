using System;
using System.ComponentModel;
using System.Globalization;

namespace Core
{
    public class CustomPopDensityTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                s = s.Replace(" per square km", "");
                return double.Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return $"{Convert.ToDouble(value).ToString("F2", culture)} per square km";

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class CustomPopulationTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                int mult = s.EndsWith(" thousand") ? 1000
                    : s.EndsWith(" million") ? 1000000 
                    : s.EndsWith(" billion") ? 1000000000 : 1;
                s = s.Substring(0, s.IndexOf(" ", StringComparison.Ordinal));

                return double.Parse(s, culture) * mult;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {

            if (destinationType == typeof (string))
            {
                string suffix = "";
                double population = Convert.ToDouble(value);
                if (population > 1000)
                {
                    population /= 1000.0;
                    suffix = " thousand";
                }
                if (population > 1000)
                {
                    population /= 1000.0;
                    suffix = " million";
                }
                if (population > 1000)
                {
                    population /= 1000.0;
                    suffix = " billion";
                }

                return $"{population:F2}{suffix}";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class CustomPerCapitaTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                s = s.Replace(" per capita", "");
                return double.Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            double val = Convert.ToDouble(value);
            if (destinationType == typeof (string))
            {
                if (val < 0.01)
                {
                    return $"{val.ToString("E2", culture)} per capita";
                }
                else
                {
                    return $"{val.ToString("F2", culture)} per capita";
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class CustomPercentTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                s = s.Replace("%", "");
                return double.Parse(s, culture)*0.01;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            double val = Convert.ToDouble(value)*100.0;
            if (destinationType == typeof(string))
                return $"{val.ToString("F1", culture)}%";

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class CustomBirthRateTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                s = s.Replace(" births per thou per day", "");
                return double.Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return $"{Convert.ToDouble(value).ToString("E2", culture)} births per day per capita";

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class CustomDeathRateTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }


        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                s = s.Replace(" deaths per thou per day", "");
                return double.Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return $"{Convert.ToDouble(value).ToString("E2", culture)} deaths per day per capita";

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class CustomDollarsPerCapitaTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            return sourceType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = (string)value;
                s = s.Replace(" per capita", "");
                s = s.Replace("$", "");
                return double.Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return $"${Convert.ToDouble(value).ToString("F2", culture)} per capita";

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
