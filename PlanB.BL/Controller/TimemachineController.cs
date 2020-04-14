using PlanB.BL.Model;
using System;

namespace PlanB.BL.Controller
{
    /// <summary>
    /// Создаёт новый счётчик времени для заезда и переводит значения минут, секунд и сотых в сотые.
    /// </summary>
    public class TimemachineController
    {
        /// <summary>
        /// Создаёт новый счётчик времени.
        /// </summary>
        private Timemachine Timer { get; }

        /// <summary>
        /// Создаёт контроллер таймера.
        /// </summary>
        /// <param name="minutes"> Минуты. </param>
        /// <param name="seconds"> Секунды. </param>
        /// <param name="hundredths"> Сотые. </param>
        public TimemachineController(int minutes, int seconds, int hundredths)
        {
            Timer = new Timemachine(minutes, seconds, hundredths);
        }

        /// <summary>
        /// Вызов метода HundredthsValue переводит значения минут, секунд и сотых в сотые.
        /// </summary>
        public int HundredthsValue
        {
            get
            {
                return Timer.Minutes * 6000 + Timer.Seconds * 100 + Timer.Hundredths;
            }
        }

        /// <summary>
        /// Переводит время в сотых долях секунды в удобочитаемый человеком формат мм:сс:00.
        /// </summary>
        /// <param name="intTime"> Время в сотых долях секунды. </param>
        /// <returns> Время в формате мм:сс:00. </returns>
        public static string ToPrint(int intTime)
        {
            var min = intTime / 6000;
            var sec = (intTime - min * 6000) / 100;
            var hun = intTime - (min * 6000 + sec * 100);
            return string.Concat(min, " : ", sec, " : ", hun);
        }

        public override string ToString()
        {
            return Timer.ToString();
        }
    }
}
