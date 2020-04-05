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

        public override string ToString()
        {
            return Timer.ToString();
        }
    }
}
