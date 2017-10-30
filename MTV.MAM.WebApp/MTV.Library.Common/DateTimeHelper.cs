using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MTV.Library.Common
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Generate a TimeSpan of minutes
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan GenerateTimeSpan(int minutes)
        {
            TimeSpan NewTimeSpan = new TimeSpan(0, minutes, 0);
            return NewTimeSpan;
        }

        /// <summary>
        /// Convertir un DateTime au Format ('dd/MM/yyyy HH:mm:ss') - 24 Heures.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToString(DateTime dt)
        {
            return dt.ToString(string.Format("{0} HH:mm:ss", DefaultValue.DateTimeFormat));
        }

        /// <summary>
        /// Get Total Minutes From Time DD:HH:MM:SS
        /// </summary>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="secondes"></param>
        /// <returns></returns>
        public static int TimeToTotalMinute(int days, int hours, int minutes, int secondes)
        {
            TimeSpan ts = new TimeSpan(days, hours, minutes, secondes);
            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// DateTime to String format HH:MM 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GenerateStringTime(DateTime dt)
        {
            return dt.ToShortTimeString();
        }


        /// <summary>
        /// (1971, 11, 6, 23, 59, 59)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime DateTimeMineValue()
        {
            return new DateTime(1971, 11, 6, 23, 59, 59);
        }

        /// <summary>
        /// Convert string (hh:mm) to TimeSpan
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static TimeSpan ConvertStingToTimeSpan(string strTime)
        {
            string[] _time = strTime.Split(':');
            TimeSpan ts = new TimeSpan(Convert.ToInt32(_time[0]), Convert.ToInt32(_time[1]), 0);
            return ts;
        }

        /// <summary>
        /// Convertir  DateTime au EDMFormat
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToEDMFormat(DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// Convertir une chaine de caratère au Date Time.
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string strDate)
        {
            try
            {
                return Convert.ToDateTime(strDate);
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Retourner le Date & temps courrant
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDateTime()
        {
            DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("en-GE").DateTimeFormat;
            return DateTime.UtcNow.ToString("dddd, MMMM dd, yyyy HH:mm:ss", dtfi);
        }

        public static string ConvertDateTimeToMySQLString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// convert the seconds to the time format (HH:MM)
        /// </summary>
        /// <param name="nSegonsTotal"></param>
        /// <returns></returns>
        public static string ConvertSecondsToHours(int seconds)
        {
            string strOut = "";
            int nHores = 0;
            int nMinuts = 0;
            int nSegons = 0;

            nHores = seconds / 3600;
            seconds = seconds - nHores * 3600;
            nMinuts = seconds / 60;
            seconds = seconds - nMinuts * 60;
            nSegons = seconds;

            strOut = string.Format("{0}:{1:00}", nHores, nMinuts);
            return strOut;
        }

        /// <summary>
        /// Description : Convert Houres and Minutes to total Seconds
        /// </summary>
        /// <param name="nHora"></param>
        /// <param name="nMinuts"></param>
        /// <returns></returns>
        public static int ConvertHoursToSeconds(int nHora, int nMinuts)
        {
            int nSegons = 0;

            nSegons = nHora * 3600 + nMinuts * 60;

            return nSegons;
        }

        /// <summary>
        /// Convert Hours:Minutes:Seconds to Total Seconds
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static double HMS2Seconds(string strTime)
        {
            string[] TableTime = strTime.Split(':');
            TimeSpan ts = new TimeSpan(Convert.ToInt32(TableTime[0]), Convert.ToInt32(TableTime[1]), Convert.ToInt32(TableTime[2]));
            return ts.TotalSeconds;
        }

        /// <summary>
        ///  If the days (Hours or Minute) less than 10 , the format change from X to 0X
        /// </summary>
        /// <param name="Valeur"></param>
        /// <returns></returns>
        public static string DisplayValueInDateFormat(int Valeur)
        {
            string Valeur_Resultat = string.Empty;
            if (Valeur < 10)
            {
                Valeur_Resultat = string.Format("0{0}", Valeur);
            }
            else
            {
                Valeur_Resultat = Valeur.ToString();
            }

            return Valeur_Resultat;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nBitsTimeInterval"></param>
        /// <param name="m_strTimeInterval"></param>
        /// <param name="m_nHoraIniTimeInterval"></param>
        /// <param name="m_nHoraFiTimeInterval"></param>
        public static void CalculaHoraIniHoraFiTimeInterval(int nBitsTimeInterval, out string m_strTimeInterval, out int m_nHoraIniTimeInterval, out int m_nHoraFiTimeInterval)
        {
            m_nHoraIniTimeInterval = 0;
            m_nHoraFiTimeInterval = 24;//23(ultima hora) + 1
            int nInterval1 = 0;
            int nInterval2 = 0;
            int nInterval1Iniciat = 0;
            int nInterval2Iniciat = 0;
            int nInterval3Iniciat = 0;
            bool bPrimeraEntrada1 = true;
            bool bPrimeraEntrada2 = true;
            bool bPrimeraEntrada3 = true;

            for (int i = 0; i < 24; i++)
            {
                if ((((nBitsTimeInterval >> i) & 0x01) == 0) && (nInterval1 == 0))//el bit en la posició 'i' val 0 (i no hi ha cap interval definit)
                {
                    if ((nInterval1Iniciat == 0) & (nInterval1Iniciat == 0))
                    {
                        m_nHoraIniTimeInterval++;
                    }
                }
                else if (((nBitsTimeInterval >> i) & 0x01) == 1)//el bit en la posició 'i' val 1
                {
                    if (nInterval2Iniciat == 0)
                    {
                        nInterval1++;
                        if (bPrimeraEntrada1)
                        {
                            bPrimeraEntrada1 = false;
                            nInterval1Iniciat = i;
                        }
                    }
                    else
                    {
                        nInterval2++;
                        if (bPrimeraEntrada3)
                        {
                            bPrimeraEntrada3 = false;
                            nInterval3Iniciat = i;
                        }
                    }
                }
                else if (((nBitsTimeInterval >> i) & 0x01) == 0)//el bit en la posició 'i' val 0 (i l'interval ja ha estat iniciat)
                {
                    nInterval2++;
                    if (bPrimeraEntrada2)
                    {
                        bPrimeraEntrada2 = false;
                        nInterval2Iniciat = i;
                    }
                }
            }
            if (nInterval2Iniciat == 0)
            {
                nInterval2Iniciat = 24;//només hi ha un interval que acaba a les 24hPM
            }

            //Càlcul hora inici i hora final del Time Interval global (conjunt de tots els blocs)
            if (nInterval3Iniciat != 0)
            {
                m_nHoraFiTimeInterval = m_nHoraIniTimeInterval + nInterval1 + nInterval2;
            }
            else
            {
                m_nHoraFiTimeInterval = m_nHoraIniTimeInterval + nInterval1;
            }

            //Càlcul dels marges de cada bloc
            m_strTimeInterval = string.Format("{0}_{1}", nInterval1Iniciat, nInterval2Iniciat);
            if (nInterval3Iniciat != 0)
            {
                m_strTimeInterval += string.Format("_{0}_24", nInterval3Iniciat);
            }

        }

        /// <summary>
        /// Desciption : Verify the existence of only one or more blocks
        /// </summary>
        /// <param name="nHoraIniBloc1"></param>
        /// <param name="nHoraIniBloc2"></param>
        /// <param name="nHoraFiBloc1"></param>
        /// <param name="nHoraFiBloc2"></param>
        /// <param name="m_strTimeInterval"></param>
        /// <returns></returns>
        public static bool GetSiTimeInterval2Blocs(out int nHoraIniBloc1, out int nHoraIniBloc2, out int nHoraFiBloc1, out int nHoraFiBloc2, string m_strTimeInterval)
        {
            nHoraIniBloc1 = 0;
            nHoraIniBloc2 = 0;
            nHoraFiBloc1 = 0;
            nHoraFiBloc2 = 0;

            string[] strHores = m_strTimeInterval.Split('_');

            if (strHores.Length == 2)//hi ha un bloc d'hores
            {
                nHoraIniBloc1 = int.Parse(strHores[0]);
                nHoraFiBloc1 = int.Parse(strHores[1]);
            }
            else if (strHores.Length == 4)//hi ha 2 blocs d'hores
            {
                nHoraIniBloc1 = int.Parse(strHores[0]);
                nHoraFiBloc1 = int.Parse(strHores[1]);
                nHoraIniBloc2 = int.Parse(strHores[2]);
                nHoraFiBloc2 = int.Parse(strHores[3]);
            }

            if (nHoraIniBloc2 == nHoraFiBloc2)//si només hi ha un bloc (bloc1)
            {
                return false;
            }
            else//hi ha 2 blocs (bloc1 i bloc2)
            {
                return true;
            }
        }

        /// <summary>
        /// Description : Calculate the end of free Space
        /// </summary>
        /// <param name="nSegonsIni"></param>
        /// <param name="bTimeInterval2Blocs"></param>
        /// <param name="nHoraIniBloc1"></param>
        /// <param name="nHoraIniBloc2"></param>
        /// <param name="nHoraFiBloc1"></param>
        /// <param name="nHoraFiBloc2"></param>
        /// <returns></returns>
        public static int CalculaSegonsEndHmax_EspaiLLiure(int nSegonsIni, bool bTimeInterval2Blocs, int nHoraIniBloc1, int nHoraIniBloc2, int nHoraFiBloc1, int nHoraFiBloc2)
        {
            //retorna nSegonsEndHmax de la cela
            int nSegonsEndHmax = 0;

            int nSegonsIniBloc1 = ConvertHoursToSeconds(nHoraIniBloc1, 0);
            int nSegonsIniBloc2 = ConvertHoursToSeconds(nHoraIniBloc2, 0);
            int nSegonsFiBloc1 = ConvertHoursToSeconds(nHoraFiBloc1, 0);
            int nSegonsFiBloc2 = ConvertHoursToSeconds(nHoraFiBloc2, 0);

            if (!bTimeInterval2Blocs)//Només hi ha un bloc de TimeInterval
            {
                nSegonsEndHmax = nSegonsFiBloc1;
            }
            else//hi ha 2 blocs de TimeInterval
            {
                if ((nSegonsIni >= nSegonsIniBloc2) && (nSegonsIni <= nSegonsFiBloc2))//estem a una cela del bloc2
                {
                    nSegonsEndHmax = nSegonsFiBloc2;
                    //nSegonsEndHmax = nSegonsFiBloc1;
                }
                else if ((nSegonsIni >= nSegonsIniBloc1) && (nSegonsIni < nSegonsFiBloc1))//estem a una cela del bloc1
                {
                    nSegonsEndHmax = nSegonsFiBloc1;
                }
                else//altre cas: bloc no schedulable
                {
                    nSegonsEndHmax = nSegonsFiBloc2;
                }
            }

            return nSegonsEndHmax;
        }

        /// <summary>
        /// Description : Calculate the start of free Space
        /// </summary>
        /// <param name="bSegonsLimit"></param>
        /// <param name="nSegonsIni"></param>
        /// <param name="nSegonsLimit"></param>
        /// <param name="bTimeInterval2Blocs"></param>
        /// <param name="nHoraIniBloc1"></param>
        /// <param name="nHoraIniBloc2"></param>
        /// <param name="nHoraFiBloc1"></param>
        /// <param name="nHoraFiBloc2"></param>
        /// <returns></returns>
        public static int CalculaSegonsStartHmax_EspaiLliure(bool bSegonsLimit, int nSegonsIni, int nSegonsLimit, bool bTimeInterval2Blocs, int nHoraIniBloc1, int nHoraIniBloc2, int nHoraFiBloc1, int nHoraFiBloc2)
        {
            //retorna nSegonsStartHmax de la cela
            int nSegonsStartHmax = 0;

            int nSegonsIniBloc1 = ConvertHoursToSeconds (nHoraIniBloc1, 0);
            int nSegonsIniBloc2 = ConvertHoursToSeconds(nHoraIniBloc2, 0);
            int nSegonsFiBloc1 = ConvertHoursToSeconds(nHoraFiBloc1, 0);
            int nSegonsFiBloc2 = ConvertHoursToSeconds(nHoraFiBloc2, 0);

            if (!bTimeInterval2Blocs)//Només hi ha un bloc de TimeInterval
            {
                nSegonsStartHmax = nSegonsIniBloc1;
            }
            else//hi ha 2 blocs de TimeInterval
            {
                if ((nSegonsIni >= nSegonsIniBloc1) && (nSegonsIni < nSegonsFiBloc1))//estem a una cela del bloc1
                {
                    if (bSegonsLimit)
                    {
                        nSegonsStartHmax = nSegonsLimit;
                    }
                    else
                    {
                        nSegonsStartHmax = nSegonsIniBloc1;
                    }
                }
                else if ((nSegonsIni >= nSegonsIniBloc2) && (nSegonsIni <= nSegonsFiBloc2))//estem a una cela del bloc2
                {
                    nSegonsStartHmax = nSegonsIniBloc2;
                }
                else//altre cas: bloc no schedulable
                {
                    nSegonsStartHmax = nSegonsIniBloc1;
                }
            }

            return nSegonsStartHmax;
        }

    }
}
