using System;

namespace TsinghuaNet
{
    public struct ByteSize : IComparable, IComparable<ByteSize>, IEquatable<ByteSize>, IFormattable
    {
        private const long KILA = 1000;
        private const long MEGA = 1000_000;
        private const long GIGA = 1000_000_000;

        public long Bytes { get; set; }

        public double KilaBytes
        {
            get => Bytes / (double)KILA;
            set => Bytes = (long)(value * KILA);
        }

        public double MegaBytes
        {
            get => Bytes / (double)MEGA;
            set => Bytes = (long)(value * MEGA);
        }

        public double GigaBytes
        {
            get => Bytes / (double)GIGA;
            set => Bytes = (long)(value * GIGA);
        }

        public ByteSize(long bytes) => Bytes = bytes;

        public static ByteSize FromBytes(long bytes) => new ByteSize(bytes);

        public static ByteSize FromKilaBytes(double kb) => new ByteSize((long)(kb * KILA));

        public static ByteSize FromMegaBytes(double mb) => new ByteSize((long)(mb * MEGA));

        public static ByteSize FromGigaBytes(double gb) => new ByteSize((long)(gb * GIGA));

        /// <inheritdoc/>
        public int CompareTo(object obj) => CompareTo((ByteSize)obj);

        /// <inheritdoc/>
        public int CompareTo(ByteSize other) => Bytes.CompareTo(other.Bytes);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ByteSize s && Equals(s);

        /// <inheritdoc/>
        public bool Equals(ByteSize other) => Bytes == other.Bytes;

        /// <inheritdoc/>
        public override int GetHashCode() => Bytes.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => ToString(null, null);

        public string ToString(string format) => ToString(format, null);

        public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                format = "F2";
            var b = Math.Abs(Bytes);
            if (b < KILA)
                return $"{Bytes.ToString(format, formatProvider)} B";
            else if (b < MEGA)
                return $"{KilaBytes.ToString(format, formatProvider)} K";
            else if (b < GIGA)
                return $"{MegaBytes.ToString(format, formatProvider)} M";
            else
                return $"{GigaBytes.ToString(format, formatProvider)} G";
        }

        public static bool TryParse(string str, out ByteSize s)
        {
            if (str == null)
            {
                s = new ByteSize();
                return false;
            }
            int index = 0;
            while (index < str.Length)
            {
                if (!(char.IsDigit(str[index]) || str[index] == '.'))
                    break;
                index += 1;
            }
            var f = double.Parse(str.Substring(0, index));
            var id = str.Substring(index).Trim().ToUpper();
            switch (id)
            {
                case "B":
                    s = new ByteSize((long)f);
                    break;
                case "K":
                case "KB":
                    s = FromKilaBytes(f);
                    break;
                case "M":
                case "MB":
                    s = FromMegaBytes(f);
                    break;
                case "G":
                case "GB":
                    s = FromGigaBytes(f);
                    break;
                default:
                    s = new ByteSize();
                    return false;
            }
            return true;
        }

        public static ByteSize Parse(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            ByteSize result;
            if (TryParse(str, out result))
                return result;
            else
                throw new FormatException("不支持的单位。");
        }

        public static bool operator ==(ByteSize s1, ByteSize s2) => s1.Equals(s2);

        public static bool operator !=(ByteSize s1, ByteSize s2) => !(s1 == s2);

        public static bool operator <(ByteSize s1, ByteSize s2) => s1.Bytes < s2.Bytes;

        public static bool operator >(ByteSize s1, ByteSize s2) => s2 < s1;

        public static bool operator <=(ByteSize s1, ByteSize s2) => !(s2 < s1);

        public static bool operator >=(ByteSize s1, ByteSize s2) => !(s1 < s2);

        public static ByteSize operator +(ByteSize s) => s;

        public static ByteSize operator -(ByteSize s) => new ByteSize(-s.Bytes);

        public static ByteSize operator +(ByteSize s1, ByteSize s2) => new ByteSize(s1.Bytes + s2.Bytes);

        public static ByteSize operator -(ByteSize s1, ByteSize s2) => new ByteSize(s1.Bytes - s2.Bytes);

        public static ByteSize operator /(ByteSize s, double r) => new ByteSize((long)(s.Bytes / r));

        public static double operator /(ByteSize s1, ByteSize s2) => s1.Bytes / (double)s2.Bytes;
    }
}