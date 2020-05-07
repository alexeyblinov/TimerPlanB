using System;

namespace PlanB.BL.Model
{
    [Serializable]
    public class Gender
    {
        /// <summary>
        /// Gender name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Создать новый экземпляр пола участника. Возможно, когда-нибудь, когда мы будем юридически ближе к Европе,
        /// придётся сделать более сложную систему определения пола, может даже с возможностью динамической смены. 
        /// Поэтому здесь класс.
        /// </summary>
        /// <param name="name"> Буква, обозначающая пол. M - мужской, F - женский. </param>
        public Gender(string name)
        {
            if (name == "M" || name == "F")
            {
                Name = name;
            }
            else
            {
                throw new ArgumentException("Пол должен быть мужским либо женским, по крайней мере пока (M или F).", nameof(name));
            }    
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
