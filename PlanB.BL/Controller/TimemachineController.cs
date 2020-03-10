using PlanB.BL.Model;
using System;

namespace PlanB.BL.Controller
{
    /// <summary>
    /// Создаёт новый счётчик времени для заезда и переводит значения минут, секунд и сотых в сотые.
    /// </summary>
    class TimemachineController
    {
        /// <summary>
        /// Создаёт новый счётчик времени.
        /// </summary>
        private Timemachine Timer { get; }

        /// <summary>
        /// Вызов метода Value переводит значения минут, секунд и сотых в сотые.
        /// </summary>
        public int Value
        {
            get
            {
                return Timer.Minutes * 6000 + Timer.Seconds * 100 + Timer.Hundredths;
            }
        }

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

        public override string ToString()
        {
            return Timer.ToString();
        }
    }
}
