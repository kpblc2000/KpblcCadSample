using System;
using System.Collections.Generic;
using System.Text;

namespace KpblcExtensions.Enums
{
    /// <summary>
    /// Результаты работы окна
    /// </summary>
    public enum WindowResult
    {
        /// <summary>
        /// Вообще непонятно что произошло
        /// </summary>
        Unknown,
        /// <summary>
        /// Была нажата кнопка ОК
        /// </summary>
        OK,
        /// <summary>
        /// Была нажата кнопка Отмена
        /// </summary>
        Cancel,
    }
}
