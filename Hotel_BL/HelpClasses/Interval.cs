using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.HelpClasses
{
    public class Interval
    {
        public Interval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public Interval()
        {

        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsInclude(Interval t)
        {
            return t.Start >= Start && t.Start < End ||
                t.End <= End && t.End > Start ||
                t.Start <= Start && t.End >= End;
        }

        public Interval IncludeDate(Interval interval)
        {
            if (interval.Start >= Start && interval.End >= End)
                return new Interval(interval.Start, End);

            if (interval.Start <= Start && interval.End <= End)
                return new Interval(Start, interval.End);

            if (interval.Start >= Start && interval.End <= End)
                return new Interval(interval.Start, interval.End);

            if (interval.Start <= Start && interval.End >= End)
                return new Interval(Start, End);

            return null;
        }

        public int DaysIncludes(Interval t)
        {
            int DaysInMonth = (End - Start).Days;

            if (!IsInclude(t))
                return 0;

            uint d1 = (t.Start - Start).Days < 0 ? 0 : (uint)(t.Start - Start).Days;
            uint d2 = (End - t.End).Days < 0 ? 0 : (uint)(End - t.End).Days;

            return DaysInMonth - (int)d1 - (int)d2;
        }

    }
}
