using System;
using System.Collections.Generic;
using System.Globalization;

namespace NotAClue
{
    public class DaysOfTheYear : IEnumerable<DateTime>
    {
        private int m_Year;
        public DaysOfTheYear(int year)
        {
            m_Year = year;
        }

        public System.DateTime this[int index]
        {
            get { return new DateTime(m_Year, 1, 1).AddDays(index); }
        }

        #region IEnumerable<DateTime> Members
        public IEnumerator<DateTime> GetEnumerator()
        {
            for (int i = 0; i < CultureInfo.CurrentCulture.Calendar.GetDaysInYear(m_Year); i++)
            {
                yield return this[i];
            }
        }
        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

